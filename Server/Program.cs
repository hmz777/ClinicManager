using ClinicProject.Server.Data;
using ClinicProject.Server.Data.DBModels.AppUsers;
using ClinicProject.Server.Data.DummyDataSeed;
using ClinicProject.Server.Helpers;
using ClinicProject.Server.OData;
using ClinicProject.Server.Resources;
using ClinicProject.Shared.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region DBContext base configuration

if (builder.Configuration.GetSection("ConnectionStrings:Active").Value == "Postgres")
{
    var connectionString = builder.Configuration.GetConnectionString("Postgres");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("SQLServer");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#endregion

#region ASP.NET Identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

#endregion

#region Identity Server

builder.Services.AddIdentityServer(options =>
{
    options.UserInteraction.LoginUrl = "/login";
    options.UserInteraction.LogoutUrl = "/logout";
})
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

#endregion

#region Localization

builder.Services.AddLocalization();

#endregion

#region MVC Config, OData

builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    })
.AddOData(opt =>
{
    var batchHandler = new DefaultODataBatchHandler();
    batchHandler.MessageQuotas.MaxNestingDepth = 2;
    batchHandler.MessageQuotas.MaxReceivedMessageSize = 100;
    batchHandler.MessageQuotas.MaxOperationsPerChangeset = 10;

    opt.AddRouteComponents("oapi",
        new OdataModelBuilder().GetEDM(),
       services => services.AddSingleton<ISearchBinder, ODataSearch>());

    opt.AddRouteComponents("oapi_b",
       new OdataModelBuilder().GetEDM(),
       batchHandler);

    opt.EnableQueryFeatures();
})
.AddDataAnnotationsLocalization(options =>
{
    options.DataAnnotationLocalizerProvider = (type, factory) =>
        factory.Create(typeof(SharedResources));
})
.AddRazorRuntimeCompilation()
.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<PatientValidator>());

builder.Services.AddRazorPages();

#endregion

#region Auto Mapper

builder.Services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());

#endregion

#region HTTPContext

builder.Services.AddHttpContextAccessor();

#endregion

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<DummySeedService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
    app.UseODataRouteDebug();

    using (var scope = app.Services.CreateScope())
    {
        var seedService = scope.ServiceProvider.GetRequiredService<DummySeedService>();
        await seedService.Seed();
    }
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseODataBatching();
app.UseRouting();

app.UseMiddleware<ODataHttpContextFixer>();

var supportedCultures = new[] { "en-US", "ar-SY" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
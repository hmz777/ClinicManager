using ClinicProject.Server.Data;
using ClinicProject.Server.Data.DBModels.AppUsers;
using ClinicProject.Server.Data.DummyDataSeed;
using ClinicProject.Server.Helpers;
using ClinicProject.Server.OData;
using ClinicProject.Server.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
      {
          options.SignIn.RequireConfirmedAccount = true;
          options.Password.RequiredLength = 8;
      })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer(options =>
{
    options.UserInteraction.LoginUrl = "/login";
    options.UserInteraction.LogoutUrl = "/logout";
})
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddLocalization();
builder.Services.AddControllersWithViews(options =>
    {
        options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
    })
    .AddOData(opt =>
    {
        opt.AddRouteComponents("oapi",
            new OdataModelBuilder().GetEDM(),
            services => services.AddSingleton<ISearchBinder, ODataSearch>());

        opt.AddRouteComponents("oapi_b",
           new OdataModelBuilder().GetEDM(),
           new DefaultODataBatchHandler());

        opt.EnableQueryFeatures();
    })
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResources));
    })
    .AddRazorRuntimeCompilation();

builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());

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

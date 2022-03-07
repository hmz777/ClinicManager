using ClinicProject.Client;
using ClinicProject.Client.Services;
using ClinicProject.Shared.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ClinicProject.ServerAPI", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
})
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient("NonTokenClient", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ClinicProject.ServerAPI"));

builder.Services.AddApiAuthorization();
builder.Services.AddScoped<APIDiscoveryService>();

builder.Services.AddMudServices();

builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.Converters.Add(new JsonStringEnumConverter());
    options.PropertyNameCaseInsensitive = true;
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(PatientValidator)));

builder.Services.AddScoped(typeof(ODataCRUDHandler<>));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var apiDiscoveryService = scope.ServiceProvider.GetRequiredService<APIDiscoveryService>();
    await apiDiscoveryService.Discover();

    if (APIDiscoveryService.APIDefinitionDocument.IsValid() == false)
        throw new Exception("Failed to obtain api endpoints");

}

await app.RunAsync();
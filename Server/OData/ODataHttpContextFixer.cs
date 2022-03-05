namespace ClinicProject.Server.OData
{
    public class ODataHttpContextFixer
    {
        private readonly RequestDelegate requestDelegate;

        public ODataHttpContextFixer(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext, IHttpContextAccessor contextAccessor)
        {
            contextAccessor.HttpContext ??= httpContext;

            await requestDelegate(httpContext);
        }
    }
}

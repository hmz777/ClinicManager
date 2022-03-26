using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ClinicProject.Server.Helpers
{
    public class DTOModelBinder<T> : IModelBinder
    {
        private readonly IOptions<JsonOptions> jsonOptions;

        public DTOModelBinder(IOptions<JsonOptions> jsonOptions)
        {
            this.jsonOptions = jsonOptions;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            try
            {
                bindingContext.HttpContext.Request.EnableBuffering();

                var bodyStream = bindingContext.HttpContext.Request.Body;

                string bodyJson;

                using (StreamReader sr = new(bodyStream))
                {
                    bodyJson = await sr.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(bodyJson))
                {
                    return;
                }

                var model = JsonSerializer.Deserialize<T>(bodyJson, jsonOptions.Value.JsonSerializerOptions);

                if (model == null)
                {
                    return;
                }

                bindingContext.Result = ModelBindingResult.Success(model);

                return;
            }
            catch
            {
                return;
            }
        }
    }

    public class DTOModelBinder : IModelBinder
    {
        private readonly Type type;
        private readonly IOptions<JsonOptions> jsonOptions;

        public DTOModelBinder(Type type, IOptions<JsonOptions> jsonOptions)
        {
            this.type = type;
            this.jsonOptions = jsonOptions;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            try
            {
                bindingContext.HttpContext.Request.EnableBuffering();

                var bodyStream = bindingContext.HttpContext.Request.Body;

                string bodyJson;

                using (StreamReader sr = new(bodyStream))
                {
                    bodyJson = await sr.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(bodyJson))
                {
                    return;
                }

                var model = JsonSerializer.Deserialize(bodyJson, type, jsonOptions.Value.JsonSerializerOptions);

                if (model == null)
                {
                    return;
                }

                bindingContext.Result = ModelBindingResult.Success(model);

                return;
            }
            catch
            {
                return;
            }
        }
    }
}

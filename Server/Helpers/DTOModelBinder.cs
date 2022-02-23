using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace ClinicProject.Server.Helpers
{
    public class DTOModelBinder<T> : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            try
            {
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

                var model = JsonSerializer.Deserialize<T>(bodyJson);

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

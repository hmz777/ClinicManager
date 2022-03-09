using ClinicProject.Shared.Attributes;
using ClinicProject.Shared.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ClinicProject.Client.Shared.Dialogs
{
    public partial class AddDialog<T> where T : DTOBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] IValidator<T> ModelValidator { get; set; }

        MudForm Form;
        Dictionary<PropertyInfo, (DisplayAttribute, DataFieldAttribute)> PropertyAttributes = new();

        public T Model { get; set; } = Activator.CreateInstance<T>();

        protected override void OnInitialized()
        {
            foreach (var property in Model.GetType().GetProperties())
            {
                var attributes = property.GetCustomAttributes();

                var dataField = attributes
                    .Where(a => a.GetType() == typeof(DataFieldAttribute))
                    .Cast<DataFieldAttribute>()
                    .FirstOrDefault();

                var disp = attributes
                    .Where(a => a.GetType() == typeof(DisplayAttribute))
                    .Cast<DisplayAttribute>()
                    .FirstOrDefault();

                PropertyAttributes[property] = (disp, dataField);
            }

            PropertyAttributes = PropertyAttributes
              .OrderByDescending(p => p.Key.Name.ToLower().StartsWith('i'))
              .ThenByDescending(p => p.Value.Item2?.DataField != DataField.Navigation)
              .ToDictionary(p => p.Key, p => p.Value);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ModelValidator.ValidateAsync(ValidationContext<T>
                .CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));

            if (result.IsValid)
                return Array.Empty<string>();

            return result.Errors.Select(e => e.ErrorMessage);
        };

        void Submit()
        {
            if (Form.IsValid)
            {
                MudDialog.Close(DialogResult.Ok(Model));
            }
        }

        void Cancel() => MudDialog.Cancel();
    }
}

using ClinicProject.Shared.DTOs.Treatments;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicProject.Client.Shared.Dialogs.Treatments
{
    public partial class AddTreatment
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] IValidator<TreatmentDTO> ModelValidator { get; set; }

        MudForm Form;

        public TreatmentDTO Model { get; set; } = new();

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ModelValidator.ValidateAsync(ValidationContext<TreatmentDTO>
                .CreateWithOptions((TreatmentDTO)model, x => x.IncludeProperties(propertyName)));

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

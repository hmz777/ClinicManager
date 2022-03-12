using ClinicProject.Shared.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicProject.Client.Shared.Dialogs.Patients
{
    public partial class AddPatient
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] IValidator<PatientDTO> ModelValidator { get; set; }

        MudForm Form;

        public PatientDTO Model { get; set; } = new();

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ModelValidator.ValidateAsync(ValidationContext<PatientDTO>
                .CreateWithOptions((PatientDTO)model, x => x.IncludeProperties(propertyName)));

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

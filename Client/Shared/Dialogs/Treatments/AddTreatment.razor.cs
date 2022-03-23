using ClinicProject.Shared.DTOs;
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

        public int SelectedPatient { get; set; }

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
                Model.PatientId = SelectedPatient;
                MudDialog.Close(DialogResult.Ok(Model));
            }
        }

        void Cancel() => MudDialog.Cancel();
    }
}

using ClinicProject.Shared.DTOs.Appointments;
using ClinicProject.Shared.DTOs.Patients;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicProject.Client.Shared.Dialogs.Appointments
{
    public partial class AddAppointment
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        MudForm Form;

        public int SelectedPatient { get; set; }

        public AppointmentDTO Model { get; set; } = new();

        void Submit()
        {
            if (Form.IsValid)
            {
                Model.Patient = new PatientDTO { Id = SelectedPatient };
                MudDialog.Close(DialogResult.Ok(Model));
            }
        }

        void Cancel() => MudDialog.Cancel();
    }
}

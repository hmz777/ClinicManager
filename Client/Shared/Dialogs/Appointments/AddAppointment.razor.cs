using ClinicProject.Shared.DTOs.Appointments;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicProject.Client.Shared.Dialogs.Appointments
{
    public partial class AddAppointment
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        MudForm Form;

        public AppointmentDTO Model { get; set; } = new();

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

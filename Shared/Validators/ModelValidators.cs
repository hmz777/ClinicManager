using ClinicProject.Shared.DTOs;
using ClinicProject.Shared.DTOs.Appointments;
using ClinicProject.Shared.DTOs.Patients;
using FluentValidation;

namespace ClinicProject.Shared.Validators
{
    public class PatientValidator : AbstractValidator<PatientDTO>
    {
        public PatientValidator()
        {
            RuleFor(patient => patient.FirstName)
                .NotEmpty()
                .Length(2, 40)
                .WithName("First Name");

            RuleFor(patient => patient.MiddleName)
                .NotEmpty()
                .Length(2, 40)
                .WithName("Middle Name");

            RuleFor(patient => patient.LastName)
                .NotEmpty()
                .Length(2, 40)
                .WithName("Last Name");

            RuleFor(patient => patient.Age)
                .NotEmpty()
                .InclusiveBetween(1, 110);

            RuleFor(patient => patient.Gender)
               .NotEmpty()
               .IsInEnum();

            RuleFor(patient => patient.PhoneNumber)
               .NotEmpty()
               .Length(4, 15)
               .WithName("Phone Number");
        }
    }

    public class AppointmentValidator : AbstractValidator<AppointmentDTO>
    {
        public AppointmentValidator()
        {
            RuleFor(appointment => appointment.AppointmentType)
                .NotEmpty()
                .IsInEnum();

            RuleFor(appointment => appointment.Date)
                .NotEmpty();

            RuleFor(appointment => appointment.PatientId)
               .NotEmpty();
        }
    }

    public class TreatmentValidator : AbstractValidator<TreatmentDTO>
    {
        public TreatmentValidator()
        {
            RuleFor(treatment => treatment.TreatmentType)
                .NotEmpty()
                .Length(2, 40);

            RuleFor(treatment => treatment.TotalCost)
                .NotEmpty();

            RuleFor(treatment => treatment.PaymentType)
                .NotEmpty()
                .IsInEnum();

            RuleFor(treatment => treatment.PaymentStatus)
                .NotEmpty()
                .IsInEnum();

            RuleFor(treatment => treatment.PatientId)
                .NotEmpty();
        }
    }
}
using ClinicProject.Shared.DTOs;
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
}
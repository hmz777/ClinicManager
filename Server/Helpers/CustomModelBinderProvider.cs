using ClinicProject.Shared.DTOs.Appointments;
using ClinicProject.Shared.DTOs.Patients;
using ClinicProject.Shared.DTOs.Treatments;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace ClinicProject.Server.Helpers
{
    public class CustomModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(PatientDTO))
            {
                return new BinderTypeModelBinder(typeof(DTOModelBinder<PatientDTO>));
            }

            if (context.Metadata.ModelType == typeof(AppointmentDTO))
            {
                return new BinderTypeModelBinder(typeof(DTOModelBinder<AppointmentDTO>));
            }

            if (context.Metadata.ModelType == typeof(TreatmentDTO))
            {
                return new BinderTypeModelBinder(typeof(DTOModelBinder<TreatmentDTO>));
            }

            return null;
        }
    }
}
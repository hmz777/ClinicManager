using ClinicProject.Shared.Models.Error;

namespace ClinicProject.Client.Helpers
{
    public class ModelRestoreHelper
    {
        public static void Restore<T>(ModelValidationResult validationResult, T backupModel, T newModel)
        {
            foreach (var result in validationResult.Results)
            {
                var prop = newModel.GetType().GetProperty(result.Key);

                if (prop != null)
                {
                    prop.SetValue(newModel, prop.GetValue(backupModel));
                }
            }
        }
    }
}
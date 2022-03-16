using System.Runtime.CompilerServices;

namespace ClinicProject.Client.Helpers
{
    public static class Extensions
    {
        public static bool HasNull(this ITuple tuple)
        {
            for (int i = 0; i < tuple.Length; i++)
            {
                if (tuple[i] == null)
                    return true;
            }

            return false;
        }
    }
}

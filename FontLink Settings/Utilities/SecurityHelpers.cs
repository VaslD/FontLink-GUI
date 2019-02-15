using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Windows;

namespace FontLinkSettings.Utilities
{
    public static class SecurityHelpers
    {
        public static Boolean IsRunningAsAdmin()
        {
            try
            {
                using (var identity = WindowsIdentity.GetCurrent())
                {
                    var principal = new WindowsPrincipal(identity);
                    return principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        public static void RestartAsAdmin()
        {
            var executable = Assembly.GetExecutingAssembly().Location;

            var request = new ProcessStartInfo(executable)
                          {
                              Verb = "runas"
                          };
            Process.Start(request);
            Application.Current.Shutdown(0);
        }
    }
}

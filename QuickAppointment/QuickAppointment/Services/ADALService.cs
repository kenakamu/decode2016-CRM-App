using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using QuickAppointment.DependencyService;

namespace QuickAppointment.Services
{
    static public class ADALService
    {
       
        static public AuthenticationContext AuthContext = new AuthenticationContext(SettingsService.AuthUri, false);
        static public bool SignedIn = false;

        static public async Task<string> GetAccessToken(string resource = "")
        {
            AuthenticationResult result = null;

            while (true)
            {
                try
                {
                    result = await AuthContext.AcquireTokenSilentAsync(resource, SettingsService.ClientId);
                    break;
                }
                catch
                {
                    try
                    {
                        result = await Xamarin.Forms.DependencyService.Get<IAuthenticator>().AcquireTokenAsync(resource, SettingsService.ClientId, SettingsService.RedirectUri);
                        break;
                    }
                    catch (Exception ex)
                    {
                        // continue authentication
                    }
                }
            }

            return result.AccessToken;            
        }
    }
}

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using QuickAppointment.DependencyService;
using QuickAppointment.Services;
using QuickAppointment.UWP.DependencyService;
using System;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(Authenticator))]

namespace QuickAppointment.UWP.DependencyService
{
    public class Authenticator : IAuthenticator
    {
        public async Task<AuthenticationResult> AcquireTokenAsync(string resource, string clientId, string redirectUri)
        {
            return await ADALService.AuthContext.AcquireTokenAsync(resource, clientId, new Uri(redirectUri), new PlatformParameters(PromptBehavior.Always, false));
        }
    }
}

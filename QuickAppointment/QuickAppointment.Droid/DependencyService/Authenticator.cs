using Android.App;

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using QuickAppointment.DependencyService;
using QuickAppointment.Droid.DependencyService;
using QuickAppointment.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Authenticator))]

namespace QuickAppointment.Droid.DependencyService
{
    public class Authenticator : IAuthenticator
    {
        public async Task<AuthenticationResult> AcquireTokenAsync(string resource, string clientId, string redirectUri)
        {
            return await ADALService.AuthContext.AcquireTokenAsync(resource, clientId,
                            new Uri(redirectUri), new PlatformParameters((Activity)Forms.Context));
        }
    }
}
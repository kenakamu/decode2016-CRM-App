﻿using Microsoft.IdentityModel.Clients.ActiveDirectory;
using QuickAppointment.DependencyService;
using QuickAppointment.iOS.DependencyService;
using QuickAppointment.Services;
using System;
using System.Threading.Tasks;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Authenticator))]

namespace QuickAppointment.iOS.DependencyService
{
    public class Authenticator : IAuthenticator
    {
        public async Task<AuthenticationResult> AcquireTokenAsync(string resource, string clientId, string redirectUri)
        {
            return await ADALService.AuthContext.AcquireTokenAsync(resource, clientId,
                            new Uri(redirectUri), new PlatformParameters(UIApplication.SharedApplication.KeyWindow.RootViewController));
        }
    }
}

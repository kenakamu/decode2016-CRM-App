using QuickAppointment.DependencyService;
using QuickAppointment.UWP.DependencyService;
using System;
using Windows.Storage;
using Windows.System;

[assembly: Xamarin.Forms.Dependency(typeof(OpenUriService))]

namespace QuickAppointment.UWP.DependencyService
{
    public class OpenUriService : IOpenUriService
    {
        public async void OpenMap(string address)
        {
            if (string.IsNullOrEmpty(address))
                return;
            await Launcher.LaunchUriAsync(new Uri(string.Format("bingmaps:?where={0}", address)));
        }

        public async void OpenFile(string path, string type = "")
        {
            if (string.IsNullOrEmpty(path))
                return;
            StorageFile file = await StorageFile.GetFileFromPathAsync(path);
            await Launcher.LaunchFileAsync(file);
        }
    }
}

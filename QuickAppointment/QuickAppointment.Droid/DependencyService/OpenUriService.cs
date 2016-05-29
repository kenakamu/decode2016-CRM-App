using Android.Content;
using Java.IO;
using QuickAppointment.DependencyService;
using QuickAppointment.Droid.DependencyService;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(OpenUriService))]

namespace QuickAppointment.Droid.DependencyService
{
    public class OpenUriService : IOpenUriService
    {

        public void OpenMap(string address)
        {
            if (string.IsNullOrEmpty(address))
                return;

            address = System.Net.WebUtility.UrlEncode(address);

            Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(string.Format("geo:0,0?q={0}", address)));
            Forms.Context.StartActivity(intent);
        }

        public void OpenFile(string path, string type = "")
        {
            if (string.IsNullOrEmpty(path))
                return;

            File f = new File(path);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(Android.Net.Uri.FromFile(f), type);
            intent.SetFlags(ActivityFlags.NewTask);
            Forms.Context.StartActivity(Intent.CreateChooser(intent, ""));
        }
    }
}
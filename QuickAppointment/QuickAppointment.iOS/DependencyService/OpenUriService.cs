using QuickAppointment.DependencyService;
using QuickAppointment.iOS.DependencyService;
using Foundation;
using System.Drawing;
using System.Linq;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(OpenUriService))]

namespace QuickAppointment.iOS.DependencyService
{
    public class OpenUriService : IOpenUriService
    {
        public void OpenMap(string address)
        {
            if (string.IsNullOrEmpty(address))
                return;
            address = System.Net.WebUtility.UrlEncode(address);
            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(string.Format("http://maps.apple.com/?q={0}", address)));
        }

        public void OpenFile(string path, string type = "")
        {
            if (string.IsNullOrEmpty(path))
                return;
            var window = UIApplication.SharedApplication.KeyWindow;
            var subviews = window.Subviews;
            var view = subviews.Last();
            var frame = view.Frame;
            frame = new RectangleF(10, 500, 0, 0);
            var viewer = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(path));
            viewer.PresentOpenInMenu(frame, view, true);
        }
    }
}

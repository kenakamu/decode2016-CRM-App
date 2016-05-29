using QuickAppointment.DependencyService;
using QuickAppointment.Droid.DependencyService;
using System.IO;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(FileService))]

namespace QuickAppointment.Droid.DependencyService
{
    public class FileService : IFileService
    {
        public async Task<string> SaveFile(string filename, byte[] data)
        {
            var rootFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);

            string filepath = Path.Combine(rootFolder.Path, filename);
            using (var resultStream = new MemoryStream(data))
            {
                using (var sw = new StreamWriter(filepath))
                {
                    await resultStream.CopyToAsync(sw.BaseStream);
                }
            }

            return filepath;
        }

        public async Task<Stream> OpenFile(string filename)
        {
            var rootFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            string filepath = Path.Combine(rootFolder.Path, filename);
            Stream fs = File.OpenRead(filepath);
            return fs;
        }
    }
}
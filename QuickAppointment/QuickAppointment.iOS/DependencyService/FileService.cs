using QuickAppointment.DependencyService;
using QuickAppointment.iOS.DependencyService;
using System;
using System.IO;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(FileService))]

namespace QuickAppointment.iOS.DependencyService
{

    public class FileService : IFileService
    {
        public async Task<string> SaveFile(string filename, byte[] data)
        {
            var rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string filepath = Path.Combine(rootFolder, filename);
            File.WriteAllBytes(filepath, data);

            return filepath;
        }

        public async Task<Stream> OpenFile(string filename)
        {
            var rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filepath = Path.Combine(rootFolder, filename);
            Stream fs = File.OpenRead(filepath);
            return fs;
        }
    }
}

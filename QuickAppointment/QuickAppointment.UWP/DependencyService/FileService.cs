using QuickAppointment.DependencyService;
using QuickAppointment.UWP.DependencyService;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(FileService))]

namespace QuickAppointment.UWP.DependencyService
{
    public class FileService : IFileService
    {
        public async Task<string> SaveFile(string filename, byte[] data)
        {
            var rootFolder = ApplicationData.Current.LocalFolder;

            var file = await rootFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            
            await FileIO.WriteBytesAsync(file, data);

            return file.Path;
        }

        public async Task<Stream> OpenFile(string filename)
        {
            var rootFolder = ApplicationData.Current.LocalFolder;
            return await rootFolder.OpenStreamForReadAsync(filename);
        }
    }
}

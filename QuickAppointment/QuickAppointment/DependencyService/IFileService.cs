using System.IO;
using System.Threading.Tasks;

namespace QuickAppointment.DependencyService
{
    public interface IFileService
    {
        /// <summary>
        /// ファイルを指定したファイル名で保存
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<string> SaveFile(string filename, byte[] data);

        /// <summary>
        /// 指定したファイルを開く
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        Task<Stream> OpenFile(string filepath);
    }
}

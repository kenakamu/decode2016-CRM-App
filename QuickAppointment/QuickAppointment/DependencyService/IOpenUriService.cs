using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickAppointment.DependencyService
{
    public interface IOpenUriService
    {
        /// <summary>
        /// ファイルをネイティブアプリ開く
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <param name="type">ファイルのタイプ</param>
        void OpenFile(string path, string type = "");

        /// <summary>
        /// 地図をネイティブアプリで開く
        /// </summary>
        /// <param name="address">住所</param>
        void OpenMap(string address);
    }
}

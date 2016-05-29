using QuickAppointment.Model;
using System.Collections.Generic;
using Xamarin.Forms;

namespace QuickAppointment.CustomControl
{
    /// <summary>
    /// 予定の資料を表示するラベルコントロール
    /// </summary>
    public class FilesControl : StackLayout
    {
        #region プロパティ

        // ファイルのリスト
        public static BindableProperty FilesProperty =
            BindableProperty.Create(nameof(Files), typeof(List<File>), typeof(FilesControl), propertyChanged: CreateFileList);

        public List<File> Files
        {
            get { return (List<File>)GetValue(FilesProperty); }
            set { SetValue(FilesProperty, value); }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// ファイルリストの作成
        /// </summary>
        static private void CreateFileList(BindableObject bindable, object oldValue, object newValue)
        {
            FilesControl control = bindable as FilesControl;
            // 既存の情報を一旦クリア
            control.Children.Clear();

            foreach (File file in (newValue as List<File>))
            {
                // ファイルの種類によってアイコンを設定
                string icon = "";
                switch (file.Name.Split('.')[1])
                {
                    case "pptx":
                    case "ppt":
                        icon = "QuickAppointment.Assets.PowerPoint.png";
                        break;
                    case "doc":
                    case "docx":
                        icon = "QuickAppointment.Assets.Word.png";
                        break;
                    case "xls":
                    case "xlsx":
                        icon = "QuickAppointment.Assets.Excel.png";
                        break;
                    default:
                        icon = "QuickAppointment.Assets.Document.png";
                        break;
                }

                // ActionLabel を利用して詳細は表示
                ActionLabel label = new ActionLabel();
                label.Icon = icon;
                label.IconSize = 30;
                label.Text = file.Name;
                label.Type = "File";
                label.Data = file;
                control.Children.Add(label);
            }
        }

        #endregion
    }
}

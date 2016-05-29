using QuickAppointment.DependencyService;
using QuickAppointment.Model;
using QuickAppointment.Services;
using System;
using System.Linq;
using Xamarin.Forms;

namespace QuickAppointment.CustomControl
{
    /// <summary>
    /// アイコンと文字列を表示し、クリック時にアクションを実行するラベルコントロール
    /// </summary>
    public class ActionLabel : StackLayout
    {
        #region プロパティ

        // ラベルに表示する文字列
        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ActionLabel), propertyChanged: CreateLabel);

        // アクションで利用するデータ
        public static BindableProperty DataProperty =
           BindableProperty.Create(nameof(Data), typeof(object), typeof(ActionLabel));

        //　アクションのタイプ
        public static BindableProperty TypeProperty =
            BindableProperty.Create(nameof(Type), typeof(string), typeof(ActionLabel), propertyChanged: CreateLabel);

        //　ラベルに表示するアイコン
        public static BindableProperty IconProperty =
           BindableProperty.Create(nameof(Icon), typeof(string), typeof(ActionLabel), propertyChanged: CreateLabel);

        // アイコンのサイズ:既定 20
        public static BindableProperty IconSizeProperty =
           BindableProperty.Create(nameof(IconSize), typeof(int), typeof(ActionLabel), 20);

        SharePointService spservice;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public int IconSize
        {
            get { return (int)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        #endregion

        #region メソッド

        public ActionLabel()
        {
            this.Orientation = StackOrientation.Horizontal;

            // タップジェスチャーの追加
            TapGestureRecognizer recognizer = new TapGestureRecognizer();
            recognizer.Tapped += recognizer_Tapped;
            GestureRecognizers.Add(recognizer);
        }

        /// <summary>
        /// ラベルの作成
        /// </summary>
        static private void CreateLabel(BindableObject bindable, object oldValue, object newValue)
        {
            // ActionLabel インスタンスを取得し、必要な情報がそろっていることを確認
            ActionLabel control = bindable as ActionLabel;
            if (string.IsNullOrEmpty(control.Text) || string.IsNullOrEmpty(control.Type) || string.IsNullOrEmpty(control.Icon))
                return;

            // アイコンの作成
            Image icon = new Image();
            icon.WidthRequest = icon.HeightRequest = control.IconSize;
            icon.Source = ImageSource.FromResource(control.Icon);
            control.Children.Add(icon);

            // 文字列の作成
            Label label = new Label();
            label.Text = control.Text.TrimStart(' ');
            control.Children.Add(label);
        }

        /// <summary>
        /// タップした場合の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void recognizer_Tapped(object sender, EventArgs e)
        {
            if (Type == "Address")
                Xamarin.Forms.DependencyService.Get<IOpenUriService>().OpenMap(Text);

            else if (Type == "File")
            {
                File file = Data as File;
                HandleFile(file);
            }

            else if (Type == "account" || Type == "opportunity")
                Device.OpenUri(new Uri(string.Format("ms-dynamicsxrm://?pagetype=entity&etn={0}&id={1}", Type, Data.ToString())));
        }

        /// <summary>
        /// ファイルの処理
        /// </summary>
        /// <param name="file">ファイル</param>
        private async void HandleFile(File file)
        {
            spservice = new SharePointService();

            // ファイルを SharePointOnline より取得
            var filedata = await spservice.GetContent(file.ServerRelativeUrl);
            // ファイルが編集対象とマーク
            file.InEdit = true;
            // 後で比較するため、現在のファイルサイズを保存
            file.Size = filedata.Length;
            
            var path = await Xamarin.Forms.DependencyService.Get<IFileService>().SaveFile(file.Name, filedata);
            string type = "";

            // 拡張子からタイプを設定
            string extension = file.Name.Split('.').Count() > 1 ? file.Name.Split('.').Last() : "application/*";
            if (extension == "pptx")
                type = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            else if (extension == "ppt")
                type = "application/vnd.ms-powerpoint";
            else if (extension == "docx")
                type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            else if (extension == "xlsx")
                type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            else if (extension == "txt")
                type = "application/txt";

            Xamarin.Forms.DependencyService.Get<IOpenUriService>().OpenFile(path, type);
        }

        #endregion
    }
}

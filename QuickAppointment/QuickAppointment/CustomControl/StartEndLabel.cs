using System.Linq;
using Xamarin.Forms;

namespace QuickAppointment.CustomControl
{
    /// <summary>
    /// 開始時間と終了時間の時間だけを表示するラベルコントロール
    /// </summary>
    public class StartEndLabel : StackLayout
    {
        #region プロパティ

        // 開始時間
        public static BindableProperty StartTimeProperty =
            BindableProperty.Create(nameof(StartTime), typeof(string), typeof(StartEndLabel), propertyChanged: CreateStartEnd);

        // 終了時間
        public static BindableProperty EndTimeProperty =
            BindableProperty.Create(nameof(EndTime), typeof(string), typeof(StartEndLabel), propertyChanged: CreateStartEnd);

        public string StartTime
        {
            get { return (string)GetValue(StartTimeProperty); }
            set { SetValue(StartTimeProperty, value); }
        }

        public string EndTime
        {
            get { return (string)GetValue(EndTimeProperty); }
            set { SetValue(EndTimeProperty, value); }
        }

        #endregion

        public StartEndLabel()
        {
            this.Orientation = StackOrientation.Horizontal;
            Image timeicon = new Image();
            timeicon.WidthRequest = timeicon.HeightRequest = 20;
            timeicon.Source = ImageSource.FromResource("QuickAppointment.Assets.time_icon.png");
            this.Children.Add(timeicon);
            Label timelabel = new Label();
            this.Children.Add(timelabel);
        }

        /// <summary>
        /// ラベルの作成
        /// </summary>
        static private void CreateStartEnd(BindableObject bindable, object oldValue, object newValue)
        {
            StartEndLabel control = bindable as StartEndLabel;

            if (string.IsNullOrEmpty(control.StartTime) || string.IsNullOrEmpty(control.EndTime))
                return;

            // 時刻のみ取得
            var starttime = control.StartTime.Split(' ').Count() == 1 ? control.StartTime : control.StartTime.Split(' ')[1];
            var endtime = control.EndTime.Split(' ').Count() == 1 ? control.EndTime : control.EndTime.Split(' ')[1];

            string time = starttime + " - " + endtime;
            (control.Children[1] as Label).Text = time;
        }
    }
}

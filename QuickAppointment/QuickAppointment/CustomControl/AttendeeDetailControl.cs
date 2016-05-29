using QuickAppointment.Model;
using System;
using Xamarin.Forms;

namespace QuickAppointment.CustomControl
{
    /// <summary>
    /// 参加者をタップした場合に表示する Flyout 
    /// </summary>
    public class AttendeeDetailControl : StackLayout
    {
        #region プロパティ

        // 選択された参加者
        public static BindableProperty AttendeeProperty =
            BindableProperty.Create(nameof(Attendee), typeof(Contact), typeof(AttendeeDetailControl));

        // 予定
        public static BindableProperty AppointmentProperty =
           BindableProperty.Create(nameof(Appointment), typeof(Appointment), typeof(AttendeeDetailControl));

        public Contact Attendee
        {
            get { return (Contact)GetValue(AttendeeProperty); }
            set { SetValue(AttendeeProperty, value); }
        }

        public Appointment Appointment
        {
            get { return (Appointment)GetValue(AppointmentProperty); }
            set { SetValue(AppointmentProperty, value); }
        }

        #endregion

        public AttendeeDetailControl()
        {
            // 電話、メール、Dynamics CRM アイコンの設定
            this.Orientation = StackOrientation.Horizontal;
            this.Padding = new Thickness(4);
            Image phoneimg = GetImage("phone");
            this.Children.Add(phoneimg);
            Image mailimg = GetImage("mail");
            this.Children.Add(mailimg);
            Image crmimg = GetImage("crm");
            this.Children.Add(crmimg);
        }

        /// <summary>
        /// アイコンの作成
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Image GetImage(string type)
        {
            string iconpath = $"QuickAppointment.Assets.{type}_icon.png";
            Image img = new Image();
            img.Aspect = Aspect.AspectFill;
            img.WidthRequest = 40;
            img.HeightRequest = 40;
            img.Source = ImageSource.FromResource(iconpath);
            img.BindingContext = type;

            // タップジェスチャーを設定
            TapGestureRecognizer recognizer = new TapGestureRecognizer();
            recognizer.Tapped += img_Tapped;
            img.GestureRecognizers.Add(recognizer);

            return img;
        }

        /// <summary>
        /// アイコンをタップした場合のアクション
        /// </summary>
        void img_Tapped(object sender, EventArgs e)
        {
            string type = (sender as Image).BindingContext.ToString();
            if (type == "phone")
                Device.OpenUri(new Uri($"tel:{Attendee.Telephone}"));
            else if (type == "mail")
                Device.OpenUri(new Uri($"mailto:{Attendee.EmailAddress}?subject={Appointment.Subject}"));
            else if (type == "crm")
                Device.OpenUri(new Uri($"ms-dynamicsxrm://?pagetype=entity&etn=contact&id={Attendee.ContactId}"));
        }
    }
}

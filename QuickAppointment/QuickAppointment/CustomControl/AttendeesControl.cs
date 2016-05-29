using QuickAppointment.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace QuickAppointment.CustomControl
{
    public class AttendeesControl : StackLayout//, INotifyPropertyChanged
    {
        #region プロパティ

        // 参加者一覧
        public static BindableProperty AttendeesProperty =
            BindableProperty.Create(nameof(Attendees), typeof(List<Contact>), typeof(AttendeesControl), propertyChanged: CreateAttendees);

        // 選択した参加者
        public static BindableProperty SelectedAttendeeProperty =
            BindableProperty.Create(nameof(SelectedAttendee), typeof(Contact), typeof(AttendeesControl), defaultBindingMode: BindingMode.TwoWay);

        public List<Contact> Attendees
        {
            get { return (List<Contact>)GetValue(AttendeesProperty); }
            set { SetValue(AttendeesProperty, value); }
        }

        public Contact SelectedAttendee
        {
            get { return (Contact)GetValue(SelectedAttendeeProperty); }
            set { SetValue(SelectedAttendeeProperty, value); //NotifyPropertyChanged();
            }
        }

        // 選択された参加者を保持
        private StackLayout selectedImage;

        #endregion

        #region メソッド


        public AttendeesControl()
        {
            this.Orientation = StackOrientation.Horizontal;
        }

        /// <summary>
        /// 参加者一覧の作成
        /// </summary>
        static private void CreateAttendees(BindableObject bindable, object oldValue, object newValue)
        {
            AttendeesControl control = bindable as AttendeesControl;
            // 既存の一覧をクリア
            control.Children.Clear();
            foreach (Contact attendee in newValue as List<Contact>)
            {
                // 参加者の画像と名前を追加
                StackLayout stack = new StackLayout();
                stack.WidthRequest = 80;
                Grid grid = new Grid();
                grid.Padding = new Thickness(4);
                Image img = new Image();
                img.Aspect = Aspect.AspectFill;
                img.WidthRequest = 70;
                img.HeightRequest = 70;
                img.Source = ImageSource.FromStream(() =>
                { return new MemoryStream(Convert.FromBase64String(attendee.EntityImage)); });
                grid.Children.Add(img);

                Label name = new Label();
                name.Text = attendee.FullName;
                name.FontSize = 14;
                stack.Children.Add(grid);
                stack.Children.Add(name);
                stack.BindingContext = attendee;

                // タップジェスチャーの追加
                TapGestureRecognizer recognizer = new TapGestureRecognizer();
                recognizer.Tapped += recognizer_Tapped;
                stack.GestureRecognizers.Add(recognizer);

                control.Children.Add(stack);
            }
        }

        /// <summary>
        /// タップした場合、タップされた参加者のハイライトを変更
        /// </summary>
        static void recognizer_Tapped(object sender, EventArgs e)
        {
            StackLayout selected = sender as StackLayout;
            
            // ハイライトされた参加者がいない場合、タップした参加者をハイライト
            if ((selected.Parent as AttendeesControl).selectedImage == null)
            {
                (selected.Children[0] as Grid).BackgroundColor = Color.FromHex("3498db");
                (selected.Parent as AttendeesControl).SelectedAttendee = selected.BindingContext as Contact;
                (selected.Parent as AttendeesControl).selectedImage = selected;
            }
            // 同じ参加者をタップした場合、ハイライトを解除
            else if (selected == (selected.Parent as AttendeesControl).selectedImage)
            {
                (selected.Children[0] as Grid).BackgroundColor = Color.White;
                (selected.Parent as AttendeesControl).SelectedAttendee = null;
                (selected.Parent as AttendeesControl).selectedImage = null;
            }
            // 別の参加者がハイライトされた場合、ハイライトを移動
            else if ((selected.Parent as AttendeesControl).selectedImage != null)
            {
                (selected.Children[0] as Grid).BackgroundColor = Color.FromHex("3498db");
                ((selected.Parent as AttendeesControl).selectedImage.Children[0] as Grid).BackgroundColor = Color.White;
                (selected.Parent as AttendeesControl).SelectedAttendee = selected.BindingContext as Contact;
                (selected.Parent as AttendeesControl).selectedImage = selected;
            }
        }

        #endregion

        //#region INotifyPropertyChanged Members

        //public event PropertyChangedEventHandler PropertyChanged;

        //// Used to notify Silverlight that a property has changed.
        //internal void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberNameAttribute] string propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        //#endregion
    }
}

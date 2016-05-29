using QuickAppointment.Common;
using QuickAppointment.Model;
using QuickAppointment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QuickAppointment.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        #region プロパティ

        public TabbedPage Page { get; set; }

        // 予定の読み込みステータス
        private bool isLoaded;
        public bool IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; IsLoading = !value; NotifyPropertyChanged(); }
        }

        // 予定一覧の更新ボタン表示/非表示: Windows のみ PullToRefresh がないため更新ボタンを表示
        public bool IsRefreshVisible
        {
            get { return Device.OS == TargetPlatform.Windows; }
        }
        
        private bool isMemoVisible;
        public bool IsMemoVisible
        {
            get { return isMemoVisible; }
            set { isMemoVisible = value; NotifyPropertyChanged(); }
        }

        // 今日の予定一覧
        private List<Appointment> appointments;
        public List<Appointment> Appointments
        {
            get { return appointments; }
            set { appointments = value; NotifyPropertyChanged(); }
        }

        // 次の予定
        private Appointment appointment;
        public Appointment Appointment
        {
            get { return appointment; }
            set
            {
                if (value == null)
                    return;
                appointment = value;
                NotifyPropertyChanged();
                IsMemoVisible = DateTime.Parse(value.ScheduledStart) < DateTime.Now;
            }
        }

        // 一覧で選択されたアイテム
        private Appointment selectedItem;
        public Appointment SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (value == null)
                    NotifyPropertyChanged();
                else
                {
                    Appointment = value;
                    Page.CurrentPage = Page.Children[0];
                    SelectedItem = null;
                }
            }
        }

        // 選択された参加者
        private Contact selectedAttendee;
        public Contact SelectedAttendee
        {
            get { return selectedAttendee; }
            set
            {
                selectedAttendee = value;
                AttendeeDetailVisible = value != null ? true : false;
                NotifyPropertyChanged();
            }
        }

        // 参加者の詳細メニュー表示/非表示
        private bool attendeeDetailVisible;
        public bool AttendeeDetailVisible
        {
            get { return attendeeDetailVisible; }
            set { attendeeDetailVisible = value; NotifyPropertyChanged(); }
        }

        CrmService crmservice = new CrmService();

        #endregion

        #region メソッド

        /// <summary>
        /// アプリケーションの初期化
        /// </summary>
        public async void Initialize()
        {
            IsLoaded = false;
            
            // ユーザー情報の取得          
            await crmservice.GetUserId();
            // 予定の取得
            Appointments = await crmservice.GetAppointments();
            // 初めの予定を取得
            Appointment = Appointments.FirstOrDefault();

            IsLoaded = true;
        }

        /// <summary>
        /// 日報の保存
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    IsLoading = true;
                    await crmservice.SaveAppointment(Appointment);
                    IsLoading = false;
                });
            }
        }

        /// <summary>
        /// 予定の詳細表示
        /// </summary>
        public RelayCommand ShowAppointmentDetailCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Device.OpenUri(new Uri(string.Format("ms-dynamicsxrm://?pagetype=entity&etn=appointment&id={0}", Appointment.ActivityId)));
                });
            }
        }

        /// <summary>
        /// 予定の取得
        /// </summary>
        public RelayCommand GetAppointmentsCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (!IsLoaded)
                        return;

                    Appointments = await crmservice.GetAppointments();
                });
            }
        }

        #endregion
    }
}

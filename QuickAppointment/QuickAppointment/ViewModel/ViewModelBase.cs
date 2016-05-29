using System.ComponentModel;

namespace QuickAppointment.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region プロパティ

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { if (isLoading == value) return; isLoading = value; NotifyPropertyChanged(); }
        }

        #endregion

        #region INotifyPropertyChanged 関連

        public event PropertyChangedEventHandler PropertyChanged;
        
        internal void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberNameAttribute] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}

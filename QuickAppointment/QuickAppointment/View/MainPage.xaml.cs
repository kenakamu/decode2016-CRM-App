using QuickAppointment.ViewModel;
using Xamarin.Forms;

namespace QuickAppointment.View
{
    public partial class MainPage : TabbedPage
    {
        MainPageViewModel vm = new MainPageViewModel();

        public MainPage()
        {
            InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);
            vm.Page = this;
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            vm.Initialize();
        }
    }
}

using Sportics.Model;
using Sportics.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class MembershipsViewModel: BaseViewModel
    {
        public List<Membership> Memberships { get; set; }

        public Membership Membership { get; set; }

        public string ShortName { get; set; }

        public int Price { get; set; }

        public byte[] Photo { get; set; }

        public ICommand OpenAddMembershipCommand { get; }

        public ICommand OpenAboutMembershipCommand { get; }

        public ICommand OpenAdminCommand { get; }

        public ICommand DetailsCommand { get; }

        public MembershipsViewModel()
        {
            OpenAddMembershipCommand = new RelayCommand(obj => OpenAddMembership());
            OpenAdminCommand = new RelayCommand(obj => OpenAdmin());
            AllMemberships();
            DetailsCommand = new RelayCommand(obj => GetDetails((Membership)obj));
        }

        private void AllMemberships()
        {
            Memberships = DataWorker.GetAllMemberships();
            OnPropertyChanged(nameof(Memberships));
        }

        private void OpenAddMembership()
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Owner = Application.Current.MainWindow;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addWindow.ShowDialog();

            AllMemberships();
        }

        private void OpenAdmin()
        {
            AdminWindow adminWindow = new AdminWindow();
            Application.Current.MainWindow = adminWindow;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is MembershipsWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }

        private void GetDetails(Membership membership)
        {
            MembershipInfoWindow window = new MembershipInfoWindow();
            window.DataContext = new MembershipInfoViewModel(membership);
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
    }
}

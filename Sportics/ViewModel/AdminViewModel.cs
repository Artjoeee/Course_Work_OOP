using Sportics.Model;
using Sportics.View;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class AdminViewModel: BaseViewModel
    {
        public Membership SelectedItem { get; set; }

        public TabItem SelectedTab { get; set; }

        public List<User> Users { get; set; }

        public List<Membership> Memberships { get; set; }

        public ICommand OpenAccountCommand { get; }

        public ICommand OpenMembershipsCommand { get; }

        public ICommand DeleteMembershipCommand { get; }

        public ICommand EditorCommand { get; }

        public AdminViewModel() 
        {
            OpenAccountCommand = new RelayCommand(obj => OpenAccount());
            OpenMembershipsCommand = new RelayCommand(obj => OpenMemberships());
            DeleteMembershipCommand = new RelayCommand(obj => DeleteMembership());
            AllMemberships();
            AllUsers();
            EditorCommand = new RelayCommand(obj => OpenEditor(SelectedItem));
        }

        private void AllUsers()
        {
            Users = DataWorker.GetAllUsers();
            OnPropertyChanged(nameof(Users));
        }

        private void AllMemberships()
        {
            Memberships = DataWorker.GetAllMemberships();
            OnPropertyChanged(nameof(Memberships));
        }

        private void DeleteMembership()
        {
            if (SelectedTab.Name == "Memberships")
            {
                DataWorker.DeleteMembership(SelectedItem);
                AllMemberships();
            }
        }

        private void OpenAccount()
        {
            AccountWindow accountWindow = new AccountWindow();
            accountWindow.Owner = Application.Current.MainWindow;
            accountWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            accountWindow.ShowDialog();
        }

        private void OpenMemberships()
        {
            MembershipsWindow membershipsWindow = new MembershipsWindow();
            Application.Current.MainWindow = membershipsWindow;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is AdminWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }

        private void OpenEditor(Membership membership)
        {
            EditWindow window = new EditWindow();
            window.DataContext = new EditViewModel(membership);
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
    }
}

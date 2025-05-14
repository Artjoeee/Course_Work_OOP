using Sportics.Model;
using Sportics.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class MembershipInfoViewModel: BaseViewModel
    {
        public Membership Membership { get; set; }

        public ICommand DeleteMembershipCommand { get; }

        public ICommand EditorCommand { get; }

        public MembershipInfoViewModel(Membership membership)
        {
            Membership = membership;
            DeleteMembershipCommand = new RelayCommand(obj => DeleteMembership());
            EditorCommand = new RelayCommand(obj => OpenEditor(Membership));
        }

        public MembershipInfoViewModel() { }

        public event Action RequestClose;

        private void DeleteMembership()
        {
            DataWorker.DeleteMembership(Membership);
            RequestClose?.Invoke();
        }

        private void OpenEditor(Membership membership)
        {
            EditWindow window = new EditWindow();
            EditViewModel viewModel = new EditViewModel(membership);
            window.DataContext = viewModel;
            viewModel.RequestClose += () => window.Close();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is MembershipInfoWindow)?
            .Close();
        }
    }
}

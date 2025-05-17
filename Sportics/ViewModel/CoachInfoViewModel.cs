using Sportics.Model;
using Sportics.View;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class CoachInfoViewModel : BaseViewModel
    {
        public Coach Coach { get; set; }

        public ICommand DeleteCoachCommand { get; }
        public ICommand EditCoachCommand { get; }

        public CoachInfoViewModel(Coach coach)
        {
            Coach = coach;
            DeleteCoachCommand = new RelayCommand(obj => DeleteCoach());
            EditCoachCommand = new RelayCommand(obj => OpenEditor(Coach));
        }

        public CoachInfoViewModel() { }

        public event Action RequestClose;

        private void DeleteCoach()
        {
            DataWorker.DeleteCoach(Coach);
            RequestClose?.Invoke();
        }

        private void OpenEditor(Coach coach)
        {
            EditCoachWindow window = new EditCoachWindow();
            EditCoachViewModel viewModel = new EditCoachViewModel(coach);
            window.DataContext = viewModel;
            viewModel.RequestClose += () => window.Close();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is CoachInfoWindow)?
                .Close();
        }
    }
}

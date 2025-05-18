using Sportics.Model;
using System;
using System.Windows;
using System.Windows.Input;
using Sportics.View;

namespace Sportics.ViewModel
{
    public class ClientCoachInfoViewModel : BaseViewModel
    {
        public Coach Coach { get; set; }

        public ICommand OpenScheduleCommand { get; }
        public ICommand OpenReviewCommand { get; }

        public ClientCoachInfoViewModel(Coach coach)
        {
            Coach = coach;

            OpenScheduleCommand = new RelayCommand(obj => OpenSchedule(Coach));
            OpenReviewCommand = new RelayCommand(obj => OpenReview());
        }

        public ClientCoachInfoViewModel() { }

        public event Action RequestClose;

        private void OpenSchedule(Coach coach)
        {
            var window = new CoachScheduleWindow();
            var viewModel = new CoachScheduleViewModel(coach);
            window.DataContext = viewModel;
            viewModel.RequestClose += () => window.Close();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }


        private void OpenReview()
        {
            ReviewWindow reviewWindow = new ReviewWindow();
            reviewWindow.DataContext = new ReviewViewModel(Coach);
            reviewWindow.Owner = Application.Current.MainWindow;
            reviewWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            reviewWindow.ShowDialog();
        }
    }
}

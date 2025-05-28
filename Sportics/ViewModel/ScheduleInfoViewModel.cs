using Sportics.Model;
using Sportics.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class ScheduleInfoViewModel : BaseViewModel
    {
        public Schedule Schedule { get; set; }

        public ICommand DeleteScheduleCommand { get; }
        public ICommand EditScheduleCommand { get; }
        public ICommand ReviewScheduleCommand { get; }

        public ScheduleInfoViewModel(Schedule schedule)
        {
            Schedule = schedule;

            DeleteScheduleCommand = new RelayCommand(obj => DeleteSchedule());
            EditScheduleCommand = new RelayCommand(obj => OpenEditor(schedule));
            ReviewScheduleCommand = new RelayCommand(obj => LeaveReview());

            EvaluateAccess();
        }

        public ScheduleInfoViewModel() { }

        public event Action RequestClose;

        private void DeleteSchedule()
        {
            if ((Schedule.ClientSessionRecords?.Count ?? 0) == 0)
            {
                DataWorker.DeleteSchedule(Schedule);
                RequestClose?.Invoke();
            }
            else
            {
                ShowMessage("На занятие ещё записаны люди");
            }
        }

        private void EvaluateAccess()
        {
            DateTime dateTime = Schedule.Date.Date + Schedule.Time;
            DateTime now = DateTime.Now;

            if (now > dateTime)
            {
                Schedule.Status = "Завершено";
                DataWorker.UpdateSchedule(Schedule);
            }
        }

        private void OpenEditor(Schedule schedule)
        {
            EditScheduleWindow window = new EditScheduleWindow();
            EditScheduleViewModel viewModel = new EditScheduleViewModel(schedule);
            window.DataContext = viewModel;
            viewModel.RequestClose += () => window.Close();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is ScheduleInfoWindow)?
                .Close();
        }

        private void LeaveReview()
        {
            ReviewScheduleWindow window = new ReviewScheduleWindow();
            window.DataContext = new ReviewScheduleViewModel(Schedule);
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        private void ShowMessage(string message)
        {
            var messageWindow = new MessageWindow();
            var viewModel = new MessageViewModel(message);
            messageWindow.DataContext = viewModel;
            viewModel.RequestClose += () => messageWindow.Close();
            messageWindow.Owner = System.Windows.Application.Current.MainWindow;
            messageWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            messageWindow.ShowDialog();
        }
    }
}

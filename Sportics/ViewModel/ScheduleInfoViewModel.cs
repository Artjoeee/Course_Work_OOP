using Sportics.Model;
using Sportics.View;
using System;
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

        public ScheduleInfoViewModel(Schedule schedule)
        {
            Schedule = schedule;

            DeleteScheduleCommand = new RelayCommand(obj => DeleteSchedule());
            EditScheduleCommand = new RelayCommand(obj => OpenEditor(schedule));
        }

        public ScheduleInfoViewModel() { }

        public event Action RequestClose;

        private void DeleteSchedule()
        {
            DataWorker.DeleteSchedule(Schedule);
            RequestClose?.Invoke();
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
    }
}

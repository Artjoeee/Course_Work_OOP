using Sportics.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class ReviewScheduleViewModel : BaseViewModel
    {
        public bool IsAdmin => Session.CurrentUser?.Role == "Администратор";
        public ObservableCollection<SessionReview> Reviews { get; set; }

        public List<int> RatingOptions { get; } = new List<int> { 1, 2, 3, 4, 5 };

        public string NewComment { get; set; }
        public int NewRating { get; set; }

        public string CommentValidationError { get; set; }
        public string RatingValidationError { get; set; }

        public SessionReview SelectedReview { get; set; }
        public string AdminReplyText { get; set; }

        public ICommand SubmitAdminReplyCommand { get; }

        private readonly Schedule _schedule;
        private readonly User _user;

        public bool HasUserReviewed => DataWorker.HasUserReviewedSession(_user.Id, _schedule.Id);

        public bool CanSubmit => _user != null
            && !HasUserReviewed
            && string.IsNullOrEmpty(CommentValidationError)
            && string.IsNullOrEmpty(RatingValidationError)
            && IsScheduleReviewable;


        public bool IsScheduleReviewable =>
            _schedule.Date.Add(_schedule.Time) <= DateTime.Now &&
            _schedule.Date.Add(_schedule.Time).AddDays(1) >= DateTime.Now;

        public ReviewScheduleViewModel(Schedule schedule)
        {
            _schedule = schedule;
            _user = Session.CurrentUser;

            Reviews = new ObservableCollection<SessionReview>(
            DataWorker.LoadSessionReviews());

            SubmitAdminReplyCommand = new RelayCommand(
                _ => SubmitAdminReply(),
                _ => SelectedReview != null && !string.IsNullOrWhiteSpace(AdminReplyText)
            );
        }

        public ReviewScheduleViewModel() { }

        private void SubmitAdminReply()
        {
            if (SelectedReview == null || string.IsNullOrWhiteSpace(AdminReplyText))
                return;

            bool success = DataWorker.SaveAdminReply(SelectedReview.Id, AdminReplyText);

            if (success)
            {
                SelectedReview.AdminReply = AdminReplyText; // обновляем локальную копию
                OnPropertyChanged(nameof(SelectedReview));  // уведомляем UI
                AdminReplyText = string.Empty;
                OnPropertyChanged(nameof(AdminReplyText));
            }
        }
    }
}

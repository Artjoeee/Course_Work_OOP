using Sportics.Model;
using Sportics.Model.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class ReviewViewModel : BaseViewModel
    {
        public ObservableCollection<CoachReview> Reviews { get; set; }

        public List<int> RatingOptions { get; } = new List<int> { 1, 2, 3, 4, 5 };

        public string NewComment { get; set; }
        public int NewRating { get; set; }

        public string CommentValidationError { get; set; }
        public string RatingValidationError { get; set; }

        public ICommand SubmitReviewCommand { get; }

        private readonly Coach _coach;
        private readonly User _user;

        public bool CanSubmit => _user != null && !HasUserReviewed && string.IsNullOrEmpty(CommentValidationError) && string.IsNullOrEmpty(RatingValidationError);

        public bool HasUserReviewed => Reviews.Any(r => r.UserId == _user.Id);

        public bool IsAdmin => Session.CurrentUser?.Role == "Администратор";

        public string AdminReplyText { get; set; }
        public CoachReview SelectedReview { get; set; }
        public ICommand SubmitAdminReplyCommand { get; }

        public ReviewViewModel(Coach coach)
        {
            _coach = coach;
            _user = Session.CurrentUser;

            Reviews = new ObservableCollection<CoachReview>(
                DataWorker.LoadCoachReviews().Where(r => r.CoachId == coach.Id)
            );

            SubmitReviewCommand = new RelayCommand(obj => SubmitReview(), obj => CanSubmit);

            SubmitAdminReplyCommand = new RelayCommand(obj => SubmitAdminReply(), obj => CanSubmitReply());
        }

        public ReviewViewModel() { }

        private void SubmitReview()
        {

            if (!string.IsNullOrEmpty(CommentValidationError) || !string.IsNullOrEmpty(RatingValidationError))
                return;

            var review = new CoachReview
            {
                UserId = _user.Id,
                CoachId = _coach.Id,
                Rating = NewRating,
                Comment = NewComment.Trim(),
                Date = DateTime.Now
            };

            DataWorker.SaveCoachReview(review);

            Reviews.Add(review);

            // Очистка
            NewComment = string.Empty;
            NewRating = 0;
            OnPropertyChanged(nameof(NewComment));
            OnPropertyChanged(nameof(NewRating));
            OnPropertyChanged(nameof(CanSubmit));
        }


        private bool CanSubmitReply()
        {
            return IsAdmin && SelectedReview != null && !string.IsNullOrWhiteSpace(AdminReplyText);
        }

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

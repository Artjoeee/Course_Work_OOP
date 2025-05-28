using Sportics.Model;
using Sportics.Model.Data;
using Sportics.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class ReviewScheduleViewModel : BaseViewModel
    {
        private readonly Schedule _schedule;
        private readonly User _user;

        public ObservableCollection<SessionReview> Reviews { get; set; } = new ObservableCollection<SessionReview>();

        public List<int> RatingOptions { get; } = new List<int>() { 1, 2, 3, 4, 5 };

        private string _newComment;
        public string NewComment
        {
            get => _newComment;
            set
            {
                _newComment = value;
                ValidateComment();
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        private int _newRating;
        public int NewRating
        {
            get => _newRating;
            set
            {
                _newRating = value;
                ValidateRating();
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        public string CommentValidationError { get; set; }
        public string RatingValidationError { get; set; }

        public bool IsAdmin => Session.CurrentUser?.Role == "Администратор";

        private SessionReview _selectedReview;
        public SessionReview SelectedReview
        {
            get => _selectedReview;
            set
            {
                _selectedReview = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanReply));
            }
        }

        private string _adminReplyText;
        public string AdminReplyText
        {
            get => _adminReplyText;
            set
            {
                _adminReplyText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanReply));
            }
        }

        public ICommand SubmitReviewCommand { get; }
        public ICommand SubmitAdminReplyCommand { get; }
        public ICommand DeleteReviewCommand { get; }

        public bool CanDelete => IsAdmin && SelectedReview != null;

        public bool CanSubmit =>
            _user != null &&
            !HasUserReviewed &&
            string.IsNullOrWhiteSpace(CommentValidationError) &&
            string.IsNullOrWhiteSpace(RatingValidationError) &&
            !string.IsNullOrWhiteSpace(NewComment) &&
            NewRating > 0;

        public bool CanReply =>
            IsAdmin &&
            SelectedReview != null &&
            !string.IsNullOrWhiteSpace(AdminReplyText);

        public bool HasUserReviewed =>
            Reviews.Any(r => r.UserId == _user?.Id);

        public ReviewScheduleViewModel(Schedule schedule)
        {
            _schedule = schedule;
            _user = Session.CurrentUser;

            SubmitReviewCommand = new RelayCommand(_ => SubmitReview(), _ => CanSubmit);
            SubmitAdminReplyCommand = new RelayCommand(_ => SubmitAdminReply(), _ => CanReply);
            DeleteReviewCommand = new RelayCommand(_ => DeleteSelectedReview(), _ => CanDelete);

            LoadScheduleReviews();
        }

        public ReviewScheduleViewModel() { }

        private void ValidateComment()
        {
            CommentValidationError = string.IsNullOrWhiteSpace(NewComment)
                ? "Комментарий не может быть пустым"
                : null;
            OnPropertyChanged(nameof(CommentValidationError));
        }

        private void ValidateRating()
        {
            RatingValidationError = (NewRating < 1 || NewRating > 5)
                ? "Оценка должна быть от 1 до 5"
                : null;
            OnPropertyChanged(nameof(RatingValidationError));
        }

        private void SubmitReview()
        {
            if (!CanSubmit)
                return;

            var review = new SessionReview
            {
                UserId = _user.Id,
                ScheduleId = _schedule.Id,
                Comment = NewComment.Trim(),
                Rating = NewRating,
                Date = DateTime.Now
            };

            DataWorker.SaveSessionReview(review);

            NewComment = string.Empty;
            NewRating = 0;
            OnPropertyChanged(nameof(NewComment));
            OnPropertyChanged(nameof(NewRating));

            LoadScheduleReviews();
        }

        private void SubmitAdminReply()
        {
            if (!CanReply)
                return;

            bool success = DataWorker.SaveAdminReply(SelectedReview.Id, AdminReplyText);

            if (success)
            {
                AdminReplyText = string.Empty;
                OnPropertyChanged(nameof(AdminReplyText));
                LoadScheduleReviews();
            }

            LoadScheduleReviews();
        }

        private void DeleteSelectedReview()
        {
            if (SelectedReview == null) return;

            bool success = DataWorker.DeleteSessionReview(SelectedReview.Id);
            if (success)
            {
                Reviews.Remove(SelectedReview);
                SelectedReview = null;
                OnPropertyChanged(nameof(SelectedReview));
                OnPropertyChanged(nameof(CanReply));
                OnPropertyChanged(nameof(CanDelete));
                LoadScheduleReviews();

                ShowMessage("Отзыв успешно удалён.");
            }
            else
            {
                ShowMessage("Ошибка при удалении отзыва.");
            }
        }

        private void LoadScheduleReviews()
        {
            var reviews = DataWorker.LoadSessionReviews()
                                    .Where(r => r.ScheduleId == _schedule.Id)
                                    .ToList();

            Reviews.Clear();
            foreach (var review in reviews)
                Reviews.Add(review);

            OnPropertyChanged(nameof(Reviews));
            OnPropertyChanged(nameof(HasUserReviewed));
            OnPropertyChanged(nameof(CanSubmit));
        }

        private void ShowMessage(string message)
        {
            var messageWindow = new MessageWindow();
            var viewModel = new MessageViewModel(message);
            messageWindow.DataContext = viewModel;
            viewModel.RequestClose += () => messageWindow.Close();
            messageWindow.Owner = Application.Current.MainWindow;
            messageWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            messageWindow.ShowDialog();
        }
    }
}

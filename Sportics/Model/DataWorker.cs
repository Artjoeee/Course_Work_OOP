using Microsoft.EntityFrameworkCore;
using Sportics.Helper;
using Sportics.Model.Data;
using Sportics.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace Sportics.Model
{
    public static class DataWorker
    {
        #region User

        public static List<User> GetAllUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<User> users = db.Users.ToList();

                List<User> clients = new List<User>();

                foreach (var item in users)
                {
                    if (item.Role == "Клиент")
                    {
                        clients.Add(item);
                    }
                }

                return clients;
            }
        }


        public static void AddUser(string name, string email, string phoneNumber, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Users.Any(user => user.Email == email);

                if (!checkIsExist)
                {
                    string salt;
                    string hashedPassword = HashHelper.HashPassword(password, out salt);

                    User newUser = new User
                    {
                        Name = name,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        PasswordHash = hashedPassword,
                        PasswordSalt = salt,
                        Role = "Клиент",
                        Balance = 0,
                        Status = "Активен"
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();
                }
            }
        }

        public static void UpdateUser(User user)
        {
            if (user == null) return;

            using (var context = new ApplicationContext())
            {
                var dbUser = context.Users.FirstOrDefault(u => u.Id == user.Id);
                if (dbUser != null)
                {
                    dbUser.Name = user.Name;
                    dbUser.Email = user.Email;
                    dbUser.PhoneNumber = user.PhoneNumber;
                    dbUser.Status = user.Status;
                    context.SaveChanges();
                }
            }
        }

        public static void DeleteUser(User user)
        {
            if (user == null) return;

            using (var context = new ApplicationContext())
            {
                var dbUser = context.Users.FirstOrDefault(u => u.Id == user.Id);
                if (dbUser != null)
                {
                    context.Users.Remove(dbUser);
                    context.SaveChanges();
                }
            }
        }

        public static bool CheckUser(string email, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    return false;
                }

                return HashHelper.VerifyPassword(password, user.PasswordSalt, user.PasswordHash);
            }
        }


        public static User SelectUser(string email, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Email == email);

                if (user != null && HashHelper.VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
                {
                    return user;
                }

                return null;
            }
        }



        public static bool CheckEmailAndPhoneNumber(string email, string phoneNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User client = db.Users.FirstOrDefault(user => user.Email == email || user.PhoneNumber == phoneNumber);

                if (client == null)
                {
                    return true;
                }

                return false;
            }
        }

        public static void AddBalance(int userId, decimal amount)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    user.Balance += amount;
                    db.SaveChanges();
                }
            }
        }

        public static bool DeductBalance(int userId, decimal amount)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null && user.Balance >= amount)
                {
                    user.Balance -= amount;
                    db.SaveChanges();
                    return true;
                }

                return false;
            }
        }


        public static bool CancelUserSchedule(int userId, int scheduleId)
        {
            using (var context = new ApplicationContext())
            {
                var record = context.ClientSessionRecords
                    .FirstOrDefault(r => r.ClientId == userId && r.ScheduleId == scheduleId);
                if (record != null)
                {
                    context.ClientSessionRecords.Remove(record);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static List<UserMembershipInfo> GetUserMemberships(User user)
        {
            if (user == null)
                return new List<UserMembershipInfo>();

            using (var context = new ApplicationContext())
            {
                return context.MembershipOrders
                .Where(order => order.ClientId == user.Id)
                .Select(order => new UserMembershipInfo
                {
                    Membership = order.Membership,
                    EndDate = order.EndDate
                })
                .ToList();
                }
        }


        public static List<Schedule> LoadUserSchedules(int userId)
        {
            using (var context = new ApplicationContext())
            {
                var now = DateTime.Now;

                var records = context.ClientSessionRecords
                    .Include(r => r.Schedule)
                        .ThenInclude(s => s.Coach)
                    .Where(r => r.ClientId == userId &&
                           (r.Schedule.Date > now.Date ||
                           (r.Schedule.Date == now.Date && r.Schedule.Time >= now.TimeOfDay)))
                    .ToList();


                // Извлечь расписания из записей клиента
                return records.Select(r => r.Schedule).ToList();
            }
        }


        public static bool HasActiveMembership(int userId, int membershipId)
        {
            using (var context = new ApplicationContext())
            {
                var activeOrder = context.MembershipOrders
                .FirstOrDefault(o => o.ClientId == userId
                                     && o.MembershipId == membershipId
                                     && o.EndDate >= DateTime.Today);

                return activeOrder != null;
            }
                
        }


        #endregion


        #region Membership

        public static void AddMembership(string fullName, string shortName, string category, string description, decimal price, byte[] photo, int durationInDays)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Memberships.Any(user => user.FullName == fullName);

                if (!checkIsExist)
                {
                    Membership newMembership = new Membership
                    {
                        FullName = fullName,
                        ShortName = shortName,
                        Category = category,
                        Description = description,
                        Price = price,
                        Photo = photo,
                        DurationInDays = durationInDays,
                        IsWeeklyOffer = false
                    };

                    db.Memberships.Add(newMembership);
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteMembership(Membership membership)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Memberships.Remove(membership);
                db.SaveChanges();
            }
        }

        public static void EditMembership(Membership oldMembership, string fullName, string shortName,
            string description, string category, decimal price, byte[] photo, int durationInDays)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Membership membership = db.Memberships.FirstOrDefault(m => m.Id == oldMembership.Id);
                membership.FullName = fullName;
                membership.ShortName = shortName;
                membership.Description = description;
                membership.Category = category;
                membership.Price = price;
                membership.Photo = photo;
                membership.DurationInDays = durationInDays;
                db.SaveChanges();
            }
        }

        public static void UpdateMembership(Membership membership)
        {
            using (var context = new ApplicationContext())
            {
                context.Memberships.Update(membership);
                context.SaveChanges();
            }
        }


        public static List<Membership> GetAllMemberships()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Membership> result = db.Memberships.ToList();
                return result;
            }
        }


        public static Membership SelectMembership(byte[] photo, string shortName, int price)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Membership membership = db.Memberships.FirstOrDefault(m => m.Photo == photo && m.ShortName == shortName && m.Price == price);

                return membership;
            }
        }

        #endregion


        #region Coaches

        public static void AddCoach(string name, string specialization, string phoneNumber, string email, string information, byte[] photo)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool exists = db.Coaches.Any(c => c.Email == email);

                if (!exists)
                {
                    Coach coach = new Coach
                    {
                        Name = name,
                        Specialization = specialization,
                        PhoneNumber = phoneNumber,
                        Email = email,
                        Information = information,
                        Photo = photo
                    };

                    db.Coaches.Add(coach);
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteCoach(Coach coach)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Coaches.Remove(coach);
                db.SaveChanges();
            }
        }

        public static void EditCoach(Coach oldCoach, string name, string email, string phoneNumber, string specialization, string information, byte[] photo)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Coach coach = db.Coaches.FirstOrDefault(c => c.Id == oldCoach.Id);

                if (coach != null)
                {
                    coach.Name = name;
                    coach.Email = email;
                    coach.PhoneNumber = phoneNumber;
                    coach.Specialization = specialization;
                    coach.Information = information;
                    coach.Photo = photo;

                    db.SaveChanges();
                }
            }
        }

        public static List<Coach> GetAllCoaches()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Coaches
                    .Include(c => c.Schedules)
                    .ToList();
            }
        }


        #endregion


        #region MembershipOrder

        public static List<MembershipOrder> GetAllMembershipOrders()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.MembershipOrders
                    .Include(o => o.ClientSession)
                    .ToList();
            }
        }

        public static List<MembershipOrder> GetUserMembershipOrders(int userId)
        {
            using (var db = new ApplicationContext())
            {
                return db.MembershipOrders
                    .Include(o => o.Membership)
                    .Where(o => o.ClientId == userId)
                    .ToList();
            }
        }


        public static void SaveOrder(int userId, string clientName, int membershipId, string membershipName, DateTime endDate)
        {
            using (var db = new ApplicationContext())
            {
                var membership = db.Memberships.FirstOrDefault(c => c.Id == membershipId);
                var client = db.Users.FirstOrDefault(u => u.Id == userId);

                var order = new MembershipOrder
                {
                    ClientName = clientName,
                    ClientId = userId,
                    Category = membership.Category,
                    MembershipName = membershipName,
                    MembershipId = membershipId,
                    PurchaseDate = DateTime.Now,
                    EndDate = endDate,
                    Membership = membership,
                    Client = client
                };

                db.MembershipOrders.Add(order);
                db.SaveChanges();
            }
        }

        public static void DeleteOrder(MembershipOrder order)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.MembershipOrders.Remove(order);
                db.SaveChanges();
            }
        }

        public static void DeleteExpiredOrders()
        {
            using (var db = new ApplicationContext())
            {
                var expiredOrders = db.MembershipOrders
                    .Where(o => o.EndDate < DateTime.Now)
                    .ToList();

                db.MembershipOrders.RemoveRange(expiredOrders);
                db.SaveChanges();
            }
        }


        #endregion


        #region Schedule

        public static List<Schedule> GetAllSchedules()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Schedules
                         .Include(s => s.Coach)
                         .Include(o => o.ClientSessionRecords)
                         .ToList();
            }
        }

        public static void AddSchedule(string category, DateTime date, TimeSpan time, string coachFullName)
        {
            using (var context = new ApplicationContext())
            {
                var coach = context.Coaches.FirstOrDefault(c => c.Name == coachFullName);

                var schedule = new Schedule
                {
                    Category = category,
                    Date = date.Date,
                    Time = time,
                    CoachId = coach.Id,
                    Coach = coach,
                    Status = "Ожидается"
                };

                context.Schedules.Add(schedule);
                context.SaveChanges();
            }
        }

        public static List<string> GetCoachNames()
        {
            using (var context = new ApplicationContext())
            {
                return context.Coaches
                    .Select(c => c.Name)
                    .ToList();
            }
        }

        public static Coach GetCoachByName(string name)
        {
            using (var context = new ApplicationContext())
            {
                return context.Coaches.FirstOrDefault(c => c.Name == name);
            }
        }


        public static void DeleteSchedule(Schedule schedule)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Schedules.Remove(schedule);
                db.SaveChanges();
            }
        }

        public static void EditSchedule(Schedule oldSchedule, string category, int coachId, DateTime date, TimeSpan time)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var schedule = db.Schedules.FirstOrDefault(s => s.Id == oldSchedule.Id);
                var coach = db.Coaches.FirstOrDefault(c => c.Id == coachId);

                if (schedule != null && coach != null)
                {
                    schedule.Category = category;
                    schedule.CoachId = coachId;
                    schedule.Date = date;
                    schedule.Time = time;

                    db.SaveChanges();
                }
            }
        }

        public static void UpdateSchedule(Schedule schedule)
        {
            if (schedule == null) return;

            using (var context = new ApplicationContext())
            {
                var dbUser = context.Schedules.FirstOrDefault(u => u.Id == schedule.Id);
                if (dbUser != null)
                {
                    dbUser.Category = schedule.Category;
                    dbUser.CoachId = schedule.CoachId;
                    dbUser.Date = schedule.Date;
                    dbUser.Time = schedule.Time;
                    dbUser.Status = schedule.Status;
                    context.SaveChanges();
                }
            }
        }


        public static void CleanupOldSchedules()
        {
            using (var context = new ApplicationContext())
            {
                DateTime now = DateTime.Now;

                var allSchedules = context.Schedules
                    .Where(s => s.Date < now.Date.AddDays(-1))
                    .ToList()
                    .Where(s => s.Date.Add(s.Time) < now.AddDays(-1))
                    .ToList();

                foreach (var schedule in allSchedules)
                {
                    var relatedRecords = context.ClientSessionRecords
                        .Where(r => r.ScheduleId == schedule.Id)
                        .ToList();

                    context.ClientSessionRecords.RemoveRange(relatedRecords);
                    context.Schedules.Remove(schedule);
                }

                context.SaveChanges();
            }
        }

        #endregion


        #region ClientSessionRecord

        public static List<ClientSessionRecord> GetAllClientSessionRecords()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.ClientSessionRecords
                         .Include(r => r.Client)
                         .Include(r => r.Schedule)
                            .ThenInclude(s => s.Coach)
                         .ToList();
            }
        }

        public static void SaveClientSessionRecord(ClientSessionRecord record)
        {
            using (var db = new ApplicationContext())
            {
                db.ClientSessionRecords.Add(record);
                db.SaveChanges();
            }
        }


        public static void DeleteClientSessionRecord(ClientSessionRecord record)
        {
            if (record == null) return;

            using (var context = new ApplicationContext())
            {
                var dbRecord = context.ClientSessionRecords.FirstOrDefault(r => r.Id == record.Id);
                if (dbRecord != null)
                {
                    context.ClientSessionRecords.Remove(dbRecord);
                    context.SaveChanges();
                }
            }
        }


        #endregion


        #region CoachReview

        public static List<CoachReview> LoadCoachReviews()
        {
            using (var db = new ApplicationContext())
            {
                return db.CoachReviews
                         .Include(r => r.User)
                         .Include(r => r.Coach)
                         .ToList();
            }
        }

        public static void SaveCoachReview(CoachReview review)
        {
            using (var db = new ApplicationContext())
            {
                db.CoachReviews.Add(review);
                db.SaveChanges();
            }
        }

        public static bool HasUserReviewedCoach(int userId, int coachId)
        {
            using (var db = new ApplicationContext())
            {
                return db.CoachReviews.Any(r => r.UserId == userId && r.CoachId == coachId);
            }
        }

        internal static void UpdateCoachReview(CoachReview selectedReview)
        {
            using (var db = new ApplicationContext())
            {
                var reviewInDb = db.CoachReviews.FirstOrDefault(r => r.Id == selectedReview.Id);
                if (reviewInDb != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public static bool DeleteCoachReview(int reviewId)
        {
            using (var context = new ApplicationContext())
            {
                var review = context.CoachReviews.FirstOrDefault(r => r.Id == reviewId);
                if (review == null)
                    return false;

                context.CoachReviews.Remove(review);
                context.SaveChanges();
                return true;
            }
        }


        #endregion


        #region SessionReview

        public static List<SessionReview> LoadSessionReviews()
        {
            using (var db = new ApplicationContext())
            {
                return db.SessionReviews
                          .Include(r => r.User)
                          .Include(r => r.Schedule)
                          .ToList();
            }
        }

        public static void SaveSessionReview(SessionReview review)
        {
            using (var db = new ApplicationContext())
            {
                db.SessionReviews.Add(review);
                db.SaveChanges();
            }
        }

        public static bool HasUserReviewedSession(int userId, int scheduleId)
        {
            using (var db = new ApplicationContext())
            {
                return db.SessionReviews.Any(r => r.UserId == userId && r.ScheduleId == scheduleId);
            }
        }


        public static bool SaveAdminReply(int reviewId, string reply)
        {
            using (var context = new ApplicationContext())
            {
                var review = context.SessionReviews.FirstOrDefault(r => r.Id == reviewId);
                if (review == null)
                    return false;

                review.AdminReply = reply;
                context.SaveChanges();
                
            }
            return false;
        }

        public static bool SaveAdminCoachReply(int reviewId, string reply)
        {
            using (var context = new ApplicationContext())
            {
                var review = context.CoachReviews.FirstOrDefault(r => r.Id == reviewId);
                if (review == null)
                    return false;

                review.AdminReply = reply;
                context.SaveChanges();

            }
            return false;
        }


        public static bool DeleteSessionReview(int reviewId)
        {
            using (var context = new ApplicationContext())
            {
                var review = context.SessionReviews.FirstOrDefault(r => r.Id == reviewId);
                if (review == null)
                    return false;

                context.SessionReviews.Remove(review);
                context.SaveChanges();
                return true;
            }
        }

        #endregion
    }
}

using Microsoft.EntityFrameworkCore;
using Sportics.Helper;
using Sportics.Model.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

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


        public static async Task AddUser(string name, string email, string phoneNumber, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = await db.Users.AnyAsync(user => user.Email == email);

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
                        Status = "Активен"
                    };

                    await db.Users.AddAsync(newUser);
                    await db.SaveChangesAsync();
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

        #endregion


        #region Membership

        public static void AddMembership(string fullName, string shortName, string category, string description, int price, byte[] photo)
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
                        Photo = photo
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


        public static void EditMembership(Membership oldMembership, string fullName, string shortName, string description, string category, int price, byte[] photo)
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
                db.SaveChanges();
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
                return db.Coaches.ToList();
            }
        }


        #endregion


        #region MembershipOrder

        public static List<MembershipOrder> GetAllMembershipOrders()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.MembershipOrders.ToList();
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


        public static void SaveOrder(int userId, string clientName, int membershipId, string membershipName)
        {
            using (var db = new ApplicationContext())
            {
                var order = new MembershipOrder
                {
                    ClientName = clientName,
                    ClientId = userId,
                    MembershipName = membershipName,
                    MembershipId = membershipId,
                    PurchaseDate = DateTime.Now
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

        #endregion


        #region Schedule

        public static List<Schedule> GetAllSchedules()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Schedules
                         .Include(s => s.Coach)
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
                    Coach = coach
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

        #endregion


        #region SessionReview

        public static List<SessionReview> LoadSessionReviews()
        {
            using (var db = new ApplicationContext())
            {
                return db.SessionReviews
                                .Include(r => r.Schedule)
                                    .ThenInclude(s => s.Coach)
                                .Include(r => r.User)
                                .ToList();
            }
        }

        #endregion
    }
}

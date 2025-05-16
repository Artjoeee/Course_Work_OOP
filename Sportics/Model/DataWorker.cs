using Microsoft.EntityFrameworkCore;
using Sportics.Helper;
using Sportics.Model.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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

                return false; // Недостаточно средств
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


        #region MembershipOrders

        public static List<MembershipOrder> GetAllMembershipOrders()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.MembershipOrders.ToList();
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

        #endregion


        #region CoachReview

        public static List<CoachReview> LoadCoachReviews()
        {
            using (var db = new ApplicationContext())
            {
                return db.CoachReviews
                                .Include(r => r.Coach)
                                .Include(r => r.User)
                                .ToList();

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

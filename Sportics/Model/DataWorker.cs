using Sportics.Model.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Documents;

namespace Sportics.Model
{
    public static class DataWorker
    {
        public static void AdminAddMembership(string fullName, string shortName, string description, float price, string photoPath)
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
                        Description = description,
                        Price = price,
                        PhotoPath = photoPath
                    };

                    db.Memberships.Add(newMembership);
                    db.SaveChanges();
                }
            }
        }


        public static void AdminDeleteMembership(Membership membership)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Memberships.Remove(membership);
                db.SaveChanges();
            }
        }


        public static void AdminDEditMembership(Membership oldMembership, string fullName, string shortName, string description, float price, string photoPath)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Membership membership = db.Memberships.FirstOrDefault(m => m.Id == oldMembership.Id);
                membership.FullName = fullName;
                membership.ShortName = shortName;
                membership.Description = description;
                membership.Price = price;
                membership.PhotoPath = photoPath;
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

        public static List<User> GetAllUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<User> result = db.Users.ToList();
                return result;
            }
        }


        public static void AddUser(string name, string email, string phoneNumber, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Users.Any(user => user.Email == email);

                if (!checkIsExist)
                {
                    User newUser = new User
                    {
                        Name = name,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        Password = password,
                        Role = "Клиент"
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();
                }
            }
        }


        public static bool CheckUser(string email, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (SelectUser(email, password) != null)
                {
                    return true;
                }

                return false;
            }
        }


        public static User SelectUser(string email, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User client = db.Users.FirstOrDefault(user => user.Email == email && user.Password == password);

                return client;
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
    }
}

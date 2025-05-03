using Sportics.Model.Data;
using System.Linq;

namespace Sportics.Model
{
    public static class DataWorker
    {
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

                if (client != null)
                {
                    return true;
                }

                return false;
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GuestBook.Models
{
    public class GuestBookContext : DbContext
    {
        public GuestBookContext(DbContextOptions<GuestBookContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var users = new List<User>
            {
                CreateUser(1,"User 1","12345678"),
                CreateUser(2, "User 2", "12345678"),
                CreateUser(3, "User 3", "12345678")
            };

            modelBuilder.Entity<User>().HasData(users);


            modelBuilder.Entity<Message>().HasData(
                new Message { Id = 1, MessageContent = "Hello World 1", MessageDate = DateTime.Now, UserId = 1 },
                new Message { Id = 2, MessageContent = "Hello World 2", MessageDate = DateTime.Now, UserId = 2 },
                new Message { Id = 3, MessageContent = "Hello World 3", MessageDate = DateTime.Now, UserId = 3 },
                new Message { Id = 4, MessageContent = "Hello World 4", MessageDate = DateTime.Now, UserId = 3 },
                new Message { Id = 5, MessageContent = "Hello World 5", MessageDate = DateTime.Now, UserId = 2 },
                new Message { Id = 6, MessageContent = "Hello World 6", MessageDate = DateTime.Now, UserId = 3 },
                new Message { Id = 7, MessageContent = "Hello World 7", MessageDate = DateTime.Now, UserId = 2 },
                new Message { Id = 8, MessageContent = "Hello World 8", MessageDate = DateTime.Now, UserId = 2 },
                new Message { Id = 9, MessageContent = "Hello World 9", MessageDate = DateTime.Now, UserId = 1 },
                new Message { Id = 10, MessageContent = "Hello World 10", MessageDate = DateTime.Now, UserId = 1 },
                new Message { Id = 11, MessageContent = "Hello World 11", MessageDate = DateTime.Now, UserId = 2 },
                new Message { Id = 12, MessageContent = "Hello World 12", MessageDate = DateTime.Now, UserId = 2 },
                new Message { Id = 13, MessageContent = "Hello World 13", MessageDate = DateTime.Now, UserId = 3 }
                );
        }

        private User CreateUser(int id, string name, string password)
        {
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(password, salt);
            return new User { Id = id, Name = name, Password = hashedPassword, Salt = salt };
        }

        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator randomNumberGenerator=RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes( saltBytes );
            //var rng = new RNGCryptoServiceProvider();
            //rng.GetBytes(saltBytes);



            ////using (var rng = new RNGCryptoServiceProvider())
            ////{
            ////    rng.GetBytes(saltBytes);
            ////}
            
            
            //return Convert.ToBase64String(saltBytes);

            StringBuilder stringBuilder = new StringBuilder(16);
            
            for ( int i = 0;i<16;i++ )
            {
                stringBuilder.Append(string.Format("{0:X2}", saltBytes[i]));
            }
            
            return stringBuilder.ToString();
        }

        private string HashPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(salt + password);

            var md5= MD5.Create();

            byte[] hashBytes= md5.ComputeHash(passwordBytes);

            StringBuilder hash= new StringBuilder(hashBytes.Length);

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hash.Append(string.Format("{0:X2}", hashBytes[i]));
            }

            return hash.ToString();

            //using (var md5 = MD5.Create())
            //{
            //    byte[] hashBytes = md5.ComputeHash(passwordBytes);
            //    return Convert.ToBase64String(hashBytes);
            //}
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //	base.OnModelCreating(modelBuilder);

        //	modelBuilder.Entity<User>().HasData(
        //		new User { Id = 1, Name = "User 1", Password = "12345678" },
        //		new User { Id = 2, Name = "User 2", Password = "12345678" },				
        //		new User { Id = 3, Name = "User 3", Password = "12345678" }
        //		);

        //	modelBuilder.Entity<Message>().HasData(
        //		new Message { Id = 1, MessageContent = "Hello World 1", MessageDate = DateTime.Now, UserId = 1 },
        //		new Message { Id = 2, MessageContent = "Hello World 2", MessageDate = DateTime.Now, UserId = 2 },
        //		new Message { Id = 3, MessageContent = "Hello World 3", MessageDate = DateTime.Now, UserId = 3 },
        //		new Message { Id = 4, MessageContent = "Hello World 4", MessageDate = DateTime.Now, UserId = 3 },
        //		new Message { Id = 5, MessageContent = "Hello World 5", MessageDate = DateTime.Now, UserId = 2 },
        //		new Message { Id = 6, MessageContent = "Hello World 6", MessageDate = DateTime.Now, UserId = 3 },
        //		new Message { Id = 7, MessageContent = "Hello World 7", MessageDate = DateTime.Now, UserId = 2 },
        //		new Message { Id = 8, MessageContent = "Hello World 8", MessageDate = DateTime.Now, UserId = 2 },
        //		new Message { Id = 9, MessageContent = "Hello World 9", MessageDate = DateTime.Now, UserId = 1 },
        //		new Message { Id = 10, MessageContent = "Hello World 10", MessageDate = DateTime.Now, UserId = 1 },
        //		new Message { Id = 11, MessageContent = "Hello World 11", MessageDate = DateTime.Now, UserId = 2 },
        //		new Message { Id = 12, MessageContent = "Hello World 12", MessageDate = DateTime.Now, UserId = 2 },
        //		new Message { Id = 13, MessageContent = "Hello World 13", MessageDate = DateTime.Now, UserId = 3 }
        //		);
        //}
    }
}

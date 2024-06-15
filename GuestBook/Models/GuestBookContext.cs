using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using GuestBook.Helpers;
using GuestBook.Services;

namespace GuestBook.Models
{
    // Контекст базы данных для гостевой книги
    public class GuestBookContext : DbContext
    {
        //private readonly ISaltGenerator _saltGenerator;
        //private readonly IPasswordHasher _passwordHasher;

        // Конструктор контекста
        public GuestBookContext(DbContextOptions<GuestBookContext> options) : base(options)
        {
            
            Database.EnsureCreated(); // Создаем базу данных, если она не существует            
        }

        public DbSet<User> Users { get; set; } // DbSet для сущностей пользователей
        public DbSet<Message> Messages { get; set; } // DbSet для сущностей сообщений

        // Переопределяем метод для настройки модели при создании
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Создаем список пользователей для инициализации данных
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
            var salt = Utilities.GenerateSalt();
            var hashedPassword = Utilities.HashPassword(password, salt);
            return new User { Id = id, Name = name, Password = hashedPassword, Salt = salt };
        }        
    }
}

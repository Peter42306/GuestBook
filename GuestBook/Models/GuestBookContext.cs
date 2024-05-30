using Microsoft.EntityFrameworkCore;

namespace GuestBook.Models
{
	public class GuestBookContext:DbContext
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

			modelBuilder.Entity<User>().HasData(
				new User { Id = 1, Name = "User 1", Password = "12345678" },
				new User { Id = 2, Name = "User 2", Password = "12345678" },				
				new User { Id = 3, Name = "User 3", Password = "12345678" }
				);

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
	}
}

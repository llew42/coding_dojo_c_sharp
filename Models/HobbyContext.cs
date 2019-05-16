using Microsoft.EntityFrameworkCore;

namespace FinalExam.Models{
  public class HobbyContext : DbContext{
   public HobbyContext (DbContextOptions options) : base(options) { }
   public DbSet<User> users   { get; set; }
   public DbSet<Hobby> hobbies { get; set; }
   public DbSet<UserHobby> userhobbies { get; set; }
  }
}
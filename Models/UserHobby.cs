using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinalExam.Models;

namespace FinalExam{
  public class UserHobby{
    [Key]
    public int UserHobbyId { get; set; }

    public int UserId { get; set; }

    public int HobbyId { get; set; }

    public User User { get; set; }
    public Hobby Hobby  { get; set; }
  }
}
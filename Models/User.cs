using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExam.Models{
    public class User{

    [Key]
    public int UserId { get; set; }

    [Required]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z""'\s-]*$")]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z""'\s-]*$")]
    public string LastName { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(15)]
    public string UserName { get; set; }

    [DataType(DataType.Password)]
    [Required]
    [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage="Password must contain at least 1 lowercase, 1 uppercase, 1 number, and 1 special character")]
    public string Password { get; set; }
    
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string PwConfirm { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<UserHobby> UserHobby { get; set; }
    // public List<Hobby> Hobby { get; set; }
    }
}
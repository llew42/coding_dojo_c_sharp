using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using FinalExam.Models;

namespace FinalExam.Controllers{
    public class HomeController : Controller{
        private HobbyContext dbContext;
        private User CurrUser{
            get { return dbContext.users.Where(u => u.UserId == HttpContext.Session.GetInt32("Id")).FirstOrDefault();}
        }
        public HomeController(HobbyContext context){
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index(){
            return View("Index");
        }

        [HttpPost("Register")]
        public IActionResult NewUser(User user){
            if(ModelState.IsValid){
                if(dbContext.users.Any(u=>u.UserName == user.UserName)){
                    ModelState.AddModelError("UserName", "UserName already in use");
                    return View("Index");
                }
                // Initializing a PasswordHasher object, providing our User class as its
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                //Save your user object to the database
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("Id", user.UserId);
                HttpContext.Session.SetString("FirstName", user.FirstName);
                return RedirectToAction("AllHobbies");
            }
            else{
                return View("Index");
            }
        }
        
        [HttpGet("Login")]
        public IActionResult Login(){
            return View("Login");
        }

        [HttpPost("Login")]
        public IActionResult LoginUser(Login loginUser){
            if(ModelState.IsValid){
                // If inital ModelState is valid, query for a user with provided email
                var dbUser = dbContext.users.FirstOrDefault(u => u.UserName == loginUser.UserName);
                // If no user exists with provided UserName
                if(dbUser == null){
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("UserName", "Invalid UserName/Password");
                    return View("Login");
                }
                // Initialize hasher object
                var hasher = new PasswordHasher<User>();
                // varify provided password against hash stored in db
                // result can be compared to 0 for failure
                var result = hasher.VerifyHashedPassword(dbUser, dbUser.Password, loginUser.Password);   

                if(result == 0){
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("UserName", "Invalid UserName/Password");
                    return View("Login");
                }
                HttpContext.Session.SetInt32("Id", dbUser.UserId);
                HttpContext.Session.SetString("FirstName", dbUser.FirstName);
                return RedirectToAction("AllHobbies");
            }
            else{
                ModelState.AddModelError("UserName", "Invalid UserName/Password");
                return View("Login");
            }
        
        }
        

        [HttpGet("Hobbies")]
        public IActionResult AllHobbies(){
            var KeyId = HttpContext.Session.GetInt32("Id");
            var KeyName = HttpContext.Session.GetString("FirstName");
            if(KeyId == null){
                return RedirectToAction("Index");
            }
            List<Hobby> allHobbies = dbContext.hobbies.Include(c=>c.Creator).Include(uh=>uh.userhobbies).ThenInclude(u=>u.User).ToList();
            ViewBag.AllHobbies = allHobbies;
            return View("Hobbies");
        }

        [HttpPost("Add/{HobbyId}")]
        public IActionResult Hobby(int HobbyId){
            if(ModelState.IsValid){
                UserHobby Join = new UserHobby{
                    UserId = CurrUser.UserId,
                    HobbyId = HobbyId,
                };
                dbContext.Add(Join);
                dbContext.SaveChanges();
            }
            return RedirectToAction("AllHobbies");
        }

        [HttpGet("Hobby/{hobbyId}")]
        public IActionResult ViewHobbies(int hobbyId){
            var KeyId = HttpContext.Session.GetInt32("Id");
            if(KeyId == null){
                return RedirectToAction("Index");
            }
            Hobby OneHobby = dbContext.hobbies.Include(c=>c.Creator).Include(u=>u.userhobbies).ThenInclude(u=>u.User).FirstOrDefault(i=>i.HobbyId == hobbyId);
            ViewBag.OneHobby = OneHobby;
            return View("HobbyView", OneHobby);
        }

        [HttpGet("CreateHobby")]
        public IActionResult CreateHobby(){
            return View("CreateHobby");
        }

        [HttpPost("NewHobby")]
        public IActionResult Like(Hobby newHobby){
            if(ModelState.IsValid){
                newHobby.UserId = CurrUser.UserId;
                dbContext.Add(newHobby);
                dbContext.SaveChanges();
                return RedirectToAction("AllHobbies");
            }
            return View("CreateHobby");
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public partial class LoginCheckVM
    {
        public int StudentId { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [RegularExpression(@"^(?=.*\d)(?=.*[a - z])(?=.*[A - Z]).{6,20}$",
        ErrorMessage = "Please Enter Valid Password.")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}

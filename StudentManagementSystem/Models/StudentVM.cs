using System;
using System.Collections.Generic;

namespace StudentManagementSystem.Models
{
    public partial class StudentVM
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime? DOB { get; set; } 
        public string EmailId { get; set; } = "";
        public string PhoneNo { get; set; } = "";
        public string Address { get; set; } = "";
        public string BloodGroup { get; set; } = "";
        public string ProfilePic { get; set; } = "";
        public string FatherName { get; set; } = "";
        public string category { get; set; } = "";
        public string MainSubject { get; set; } = "";
        public string Document1 { get; set; } = "";
        public string Document2 { get; set; } = "";
        public string Password { get; set; } = "";
        public string Obtional1 { get; set; } = "";
        public string Obtional2 { get; set; } = "";
        public string Obtional3 { get; set; } = "";
        public string Obtional4 { get; set; } = "";
        public string Role { get; set; } = "";
        public int SubjectId { get; set; }
        
        public int BloodGroupId { get; set; }
        public bool? isDeleted { get; set; } = Convert.ToBoolean(0);
    }
    public partial class Allstd
    {
        public int StudentId { get; set; } = 0;
        public string Name { get; set; } = "ss";
        public string LastName { get; set; } = "aa";
        public string EmailId { get; set; } = "ss";

        public string PhoneNo { get; set; }
        public string ImgProfile { get; set; }
    }
}

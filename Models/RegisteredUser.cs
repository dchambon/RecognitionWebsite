using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CentricProjectTeam4.Models
{
    public class RegisteredUser
    {
        public Guid registeredUserID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The employee's first name is required.")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The employee's last name is required.")]
        public string lastName { get; set; }

        [Display(Name = "Employee")]
        [Required(ErrorMessage = "The employee's full name is required.")]
        public string fullName
        {
            get
            {
                return firstName + " " + lastName;
            }
        }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "The employee's phone number is required.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:###-###-####}", ApplyFormatInEditMode = true)]
        public string phone { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Display(Name = "Business Unit")]
        [Required(ErrorMessage = "The employee's business unit is required.")]
        public locations location { get; set; }

        public enum locations
        {
            [Display(Name = "Boston")]
            Boston = 1,

            [Display(Name = "Charlotte")]
            Charlotte = 2,

            [Display(Name = "Chicago")]
            Chicago = 3,

            [Display(Name = "Cincinnati")]
            Cincinnati = 4,

            [Display(Name = "Cleveland")]
            Cleveland = 5,

            [Display(Name = "Columbus")]
            Columbus = 6,

            [Display(Name = "Detroit")]
            Detroit = 7,

            [Display(Name = "India")]
            India = 8,

            [Display(Name = "Indianapolis")]
            Indianapolis = 9,

            [Display(Name = "Louisville")]
            Louisville = 10,

            [Display(Name = "Miami")]
            Miami = 11,

            [Display(Name = "Seattle")]
            Seattle = 12,

            [Display(Name = "St. Louis")]
            St_Louis = 13,

            [Display(Name = "Tampa")]
            Tampa = 14
        }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The employee's title is required.")]
        public roles role { get; set; }

        public enum roles
        {
            [Display(Name = "Architect")]
            Architect = 1,

            [Display(Name = "Consultant")]
            Consultant = 2,

            [Display(Name = "Director")]
            Director = 3,

            [Display(Name = "Manager")]
            Manager = 4,

            [Display(Name = "Senior Architect")]
            Senior_Architect = 5,

            [Display(Name = "Senior Consultant")]
            Senior_Consultant = 6,

            [Display(Name = "Senior Manager")]
            Senior_Manager = 7
        }

        [Display(Name = "Hire Date")]
        [Required(ErrorMessage = "The hire date is required.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime hireDate { get; set; }

        [Display(Name = "Profile Picture")]
        public string profilePic { get; set; }


        public ICollection<UserRecognition> userRecognition { get; set; }
    }
}

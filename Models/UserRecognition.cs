using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CentricProjectTeam4.Models
{
    public class UserRecognition
    {
        [Key]
        public int recognitionID { get; set; }

        [Display(Name = "Employee")]
        [Required(ErrorMessage = "Selecting an employee to receive the recognition is required.")]
        public Guid registeredUserID { get; set; }

        [Display(Name = "Sponser")]
        public string recGiver { get; set; }

        [Display(Name = "Core Value")]
        [Required(ErrorMessage = "Selecting a core value is required.")]
        public values coreValue { get; set; }

        public enum values
        {
            [Display(Name = "Culture")]
            Culture = 1,

            [Display(Name = "Delivery Excellence")]
            Delivery_Excellence = 2,

            [Display(Name = "Greater Good")]
            Greater_Good = 3,

            [Display(Name = "Innovation")]
            Innovation = 4,

            [Display(Name = "Integrity and Openness")]
            Integrity_And_Openness = 5,

            [Display(Name = "Live a Balanced Life")]
            Live_A_Balanced_Life = 6,

            [Display(Name = "Stewardship")]
            Stewardship = 7
        }

        [Display(Name = "Recognition")]
        [Required(ErrorMessage = "The recognition is required.")]
        public string recognition { get; set; }

        [Display(Name = "Date Created")]
        [Required(ErrorMessage = "The date created is required.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime createDate { get; set; }

        public string fullName { get; set; }

        public virtual RegisteredUser registeredUser { get; set; }
    }
}
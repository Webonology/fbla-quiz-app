using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace fbla_app_ui.ViewModels
{
    public class UserQuizViewModel
    {
        [Key]
        public Guid TrackingID { get; set; }
        [DisplayName("Your Full Name")]
        [MaxLength(500)]
        [Required(ErrorMessage = "Your full name is required!")]
        public string Name { get; set; }
        [DisplayName("Your Email")]
        [MaxLength(1000)]
        [Required(ErrorMessage = "Your email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; }
        public QuizViewModel Quiz { get; set; }
        public IList<UserQuestionAnswerViewModel> QuizQuestionAndAnswer { get; set; }
    }
}
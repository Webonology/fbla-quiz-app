using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace fbla_app_ui.ViewModels
{
    public class UserQuestionAnswerViewModel
    {
        [Key]
        public int QuestionID { get; set; }
        [DisplayName("Question")]
        public string Question { get; set; }
        [Required(ErrorMessage = "Your answer is required!")]
        public string UserAnswer { get; set; }
        public string AnswerType { get; set; }
        public IList<AnswerViewModel> Answers { get; set; }
    }
}
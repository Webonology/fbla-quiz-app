using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fbla_app_ui.ViewModels
{
    public class UserSummaryViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int NumberOfQuestions { get; set; }
        public int NumberOfCorrect { get; set; }
        public IList<QuestionsAndAnswersViewModel> QuestionsAndAnswers { get; set; }
    }
}
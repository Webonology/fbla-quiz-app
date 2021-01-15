using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fbla_app_ui.ViewModels
{
    public class QuestionsAndAnswersViewModel
    {
        public string Question { get; set; }
        public string UserAnswer { get; set; }
        public bool Correct { get; set; }
        public string CorrectAnswer { get; set; }
        public string WhyCorrect { get; set; }
    }
}
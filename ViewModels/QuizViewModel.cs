using System.ComponentModel.DataAnnotations;

namespace fbla_app_ui.ViewModels
{
    public class QuizViewModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int AmountOfQuestions { get; set; }
    }
}
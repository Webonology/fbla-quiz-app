using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace fbla_app_ui.ViewModels
{
    public class AnswerViewModel
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Answer")]
        public string Answer { get; set; }
    }
}
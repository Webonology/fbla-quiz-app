//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace fbla_app_ui.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class QuizQuestion
    {
        public int ID { get; set; }
        public int QuizID { get; set; }
        public string Question { get; set; }
        public System.DateTime DateCreated { get; set; }
    }
}

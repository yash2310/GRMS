using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.ApplicationCore.Entities
{
    public class NotificationLog
    {
        [Required]
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Receiver Email Required")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "Subject Required")]
        public virtual string Subject { get; set; }

        [Required(ErrorMessage = "Message Required")]
        public virtual string Message { get; set; }

        [Required(ErrorMessage = "Date Required")]
        public virtual DateTime Date { get; set; }
    }
}

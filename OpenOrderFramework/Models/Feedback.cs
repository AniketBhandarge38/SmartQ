using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpenOrderFramework.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        [MaxLength(750)]
        public string FullName { get; set; }

        [MaxLength(750)]
        public string Email { get; set; }

        [MaxLength(750)]
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
    }
}
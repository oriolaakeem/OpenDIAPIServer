using System;
using System.ComponentModel.DataAnnotations;

namespace ChapelHill.models
{
    public class EmailProcessed
    {
        [Key]
        public int EmpId { get; set; }
        public string PayrollBatch { get; set; }
        public string EmailSent { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

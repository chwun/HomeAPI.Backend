using System;
using System.ComponentModel.DataAnnotations;

namespace HomeAPI.Backend.Models.Accounting
{
    public class AccountingEntryUpdateDto
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? Timestamp { get; set; }

        [Required]
        public decimal? Value { get; set; }
    }
}
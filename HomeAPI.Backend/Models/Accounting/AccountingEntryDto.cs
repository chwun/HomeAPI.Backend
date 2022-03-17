using System;

namespace HomeAPI.Backend.Models.Accounting
{
    public class AccountingEntryDto
    {
        public int Id { get; set; }

        public DateTime? Timestamp { get; set; }

        public decimal? Value { get; set; }
    }
}
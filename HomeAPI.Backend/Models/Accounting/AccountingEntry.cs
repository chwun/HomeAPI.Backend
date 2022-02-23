using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAPI.Backend.Models.Accounting
{
    [Table("accountingEntries")]
    public class AccountingEntry
    {
        public int Id { get; set; }

        public DateOnly TimePeriod { get; set; }

        public int CategoryId { get; set; }
        public AccountingCategory Category { get; set; }
    }
}
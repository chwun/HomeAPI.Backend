using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAPI.Backend.Models.Accounting
{
    [Table("accountingEntries")]
    public class AccountingEntry
    {
        public int Id { get; set; }

        public DateTime? Timestamp { get; set; }

        public decimal? Value { get; set; }

        public int CategoryId { get; set; }
        public AccountingCategory Category { get; set; }

        public void Update(AccountingEntryUpdateDto updateDto)
        {
            Timestamp = updateDto.Timestamp;
            Value = updateDto.Value;
        }
    }
}
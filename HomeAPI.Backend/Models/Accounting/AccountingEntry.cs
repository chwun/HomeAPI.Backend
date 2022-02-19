using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAPI.Backend.Models.Accounting
{
	[Table("accountingEntries")]
	public class AccountingEntry
	{
		public int Id { get; set; }

		public DateOnly TimePeriod { get; set; }

		public ICollection<AccountingSubEntry> SubEntries { get; set; }

		public int ItemId { get; set; }
		public AccountingItem Item { get; set; }
	}
}
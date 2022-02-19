using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAPI.Backend.Models.Accounting
{
	[Table("accountingSubEntries")]
	public class AccountingSubEntry
	{
		public int Id { get; set; }

		public decimal Value { get; set; }

		public int EntryId { get; set; }
		public AccountingEntry Entry { get; set; }
	}
}
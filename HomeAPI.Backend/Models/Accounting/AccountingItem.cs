using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAPI.Backend.Models.Accounting
{
	[Table("accountingItems")]
	public class AccountingItem
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 1)]
		public string Name { get; set; }

		public ICollection<AccountingEntry> Entries { get; set; }

		public int SubCategoryId { get; set; }
		public AccountingSubCategory SubCategory { get; set; }
	}
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAPI.Backend.Models.Accounting
{
	[Table("accountingSubCategories")]
	public class AccountingSubCategory
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 1)]
		public string Name { get; set; }

		public ICollection<AccountingItem> Items { get; set; }

		public int CategoryId { get; set; }
		public AccountingCategory Category { get; set; }
	}
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAPI.Backend.Models.Accounting
{
	[Table("accountingCategories")]
	public class AccountingCategory
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 1)]
		public string Name { get; set; }

		public ICollection<AccountingSubCategory> SubCategories { get; set; }
	}
}
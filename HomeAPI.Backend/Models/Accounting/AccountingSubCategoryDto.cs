using System.ComponentModel.DataAnnotations;

namespace HomeAPI.Backend.Models.Accounting
{
	public class AccountingSubCategoryDto
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 1)]
		public string Name { get; set; }
	}
}
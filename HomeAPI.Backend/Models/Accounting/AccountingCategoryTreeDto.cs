using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeAPI.Backend.Models.Accounting
{
    public class AccountingCategoryTreeDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        public ICollection<AccountingCategoryTreeDto> SubCategories { get; set; }
    }
}
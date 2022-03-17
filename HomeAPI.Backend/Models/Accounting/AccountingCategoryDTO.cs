namespace HomeAPI.Backend.Models.Accounting
{
    public class AccountingCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
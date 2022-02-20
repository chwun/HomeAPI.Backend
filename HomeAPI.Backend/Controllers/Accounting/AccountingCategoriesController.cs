using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HomeAPI.Backend.Data.Accounting;
using HomeAPI.Backend.Models.Accounting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAPI.Backend.Controllers.Accounting
{
	[Route("api/accounting")]
	[ApiController]
	public class AccountingCategoriesController : ControllerBase
	{
		private readonly IAccountingCategoriesRepository repository;
		private readonly IMapper mapper;

		public AccountingCategoriesController(IAccountingCategoriesRepository repository, IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}

		[HttpGet("categories")]
		public async Task<ActionResult<List<AccountingCategoryDto>>> GetCategories()
		{
			var categories = await repository.GetAllCategories();

			if (categories is null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			var categoriesDtos = mapper.Map<List<AccountingCategory>, List<AccountingCategoryDto>>(categories);

			return Ok(categoriesDtos);
		}

		[HttpGet("categories/{id:int}")]
		public async Task<ActionResult<AccountingCategoryDto>> GetCategory(int id)
		{
			var category = await repository.GetCategory(id);

			if (category is null)
			{
				return NotFound();
			}

			var categoryDto = mapper.Map<AccountingCategory, AccountingCategoryDto>(category);

			return Ok(categoryDto);
		}

		[HttpPost("categories")]
		public async Task<ActionResult<AccountingCategoryDto>> CreateCategory([FromBody] AccountingCategoryDto categoryDto)
		{
			var category = mapper.Map<AccountingCategoryDto, AccountingCategory>(categoryDto);

			if (category is null)
			{
				return BadRequest();
			}

			await repository.AddCategory(category);

			if (category.Id <= 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			categoryDto = mapper.Map<AccountingCategory, AccountingCategoryDto>(category);

			return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryDto);
		}

		[HttpPut("categories/{id:int}")]
		public async Task<IActionResult> UpdateCategory(int id, [FromBody] AccountingCategoryDto categoryDto)
		{
			if (categoryDto is null || categoryDto.Id != id)
			{
				return BadRequest();
			}

			var category = await repository.GetCategory(id);
			if (category is null)
			{
				return NotFound();
			}

			category = mapper.Map<AccountingCategoryDto, AccountingCategory>(categoryDto);

			await repository.UpdateCategory(category);

			return Ok();
		}

		[HttpDelete("categories/{id:int}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var category = await repository.GetCategory(id);

			if (category is null)
			{
				return NotFound();
			}

			await repository.DeleteCategory(category);

			return Ok();
		}

		[HttpGet("categories/{categoryId:int}/subcategories")]
		public async Task<ActionResult<ICollection<AccountingSubCategoryDto>>> GetSubCategoriesOfCategory(int categoryId)
		{
			var category = await repository.GetCategoryWithRelatedData(categoryId);

			if (category is null)
			{
				return NotFound();
			}

			var subCategories = mapper.Map<ICollection<AccountingSubCategory>, ICollection<AccountingSubCategoryDto>>(category.SubCategories);

			return Ok(subCategories);
		}

		[HttpPost("categories/{categoryId:int}/subcategories")]
		public async Task<ActionResult<AccountingSubCategoryDto>> AddSubCategory(int categoryId, [FromBody] AccountingSubCategoryDto subCategoryDto)
		{
			if (subCategoryDto is null)
			{
				return BadRequest();
			}

			var category = await repository.GetCategoryWithRelatedData(categoryId);

			if (category is null)
			{
				return NotFound();
			}

			var subCategory = mapper.Map<AccountingSubCategoryDto, AccountingSubCategory>(subCategoryDto);

			category.SubCategories.Add(subCategory);
			await repository.UpdateCategory(category);

			if (subCategory.Id <= 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			subCategoryDto = mapper.Map<AccountingSubCategory, AccountingSubCategoryDto>(subCategory);

			return CreatedAtAction(
				nameof(AccountingSubCategoriesController.GetSubCategory),
				"AccountingSubCategories",
				new
				{
					subCategoryId = subCategory.Id
				},
				subCategoryDto);
		}
	}
}
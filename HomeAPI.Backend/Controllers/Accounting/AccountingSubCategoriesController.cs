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
	public class AccountingSubCategoriesController : ControllerBase
	{
		private readonly IAccountingSubCategoriesRepository repository;
		private readonly IMapper mapper;

		public AccountingSubCategoriesController(IAccountingSubCategoriesRepository repository,
			IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}

		[HttpGet("subcategories")]
		public async Task<ActionResult<List<AccountingSubCategoryDto>>> GetAllSubCategories()
		{
			var subCategories = await repository.GetAllSubCategories();

			if (subCategories is null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			var subCategoriesDtos = mapper.Map<List<AccountingSubCategory>, List<AccountingSubCategoryDto>>(subCategories);

			return Ok(subCategoriesDtos);
		}

		[HttpGet("subcategories/{subCategoryId:int}")]
		public async Task<ActionResult<AccountingSubCategoryDto>> GetSubCategory(int subCategoryId)
		{
			var subCategory = await repository.GetSubCategory(subCategoryId);

			if (subCategory is null)
			{
				return NotFound();
			}

			var subCategoryDto = mapper.Map<AccountingSubCategory, AccountingSubCategoryDto>(subCategory);

			return Ok(subCategoryDto);
		}

		[HttpPut("subcategories/{subCategoryId:int}")]
		public async Task<IActionResult> UpdateSubCategory(int subCategoryId, [FromBody] AccountingSubCategoryDto subCategoryDto)
		{
			if (subCategoryDto is null || subCategoryDto.Id != subCategoryId)
			{
				return BadRequest();
			}

			var existingSubCategory = await repository.GetSubCategory(subCategoryId);
			if (existingSubCategory is null)
			{
				return NotFound();
			}

			// TODO: better solution needed?
			existingSubCategory.Name = subCategoryDto.Name;

			await repository.UpdateSubCategory(existingSubCategory);

			return Ok();
		}

		[HttpDelete("subcategories/{subCategoryId:int}")]
		public async Task<IActionResult> DeleteSubCategory(int subCategoryId)
		{
			var subCategory = await repository.GetSubCategory(subCategoryId);

			if (subCategory is null)
			{
				return NotFound();
			}

			await repository.DeleteSubCategory(subCategory);

			return Ok();
		}


	}
}
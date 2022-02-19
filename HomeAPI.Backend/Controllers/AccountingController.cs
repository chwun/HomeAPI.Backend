using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HomeAPI.Backend.Data.Accounting;
using HomeAPI.Backend.Models.Accounting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAPI.Backend.Controllers.Accounting
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountingController : ControllerBase
	{
		private readonly IAccountingRepository repository;
		private readonly IMapper mapper;

		public AccountingController(IAccountingRepository repository, IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}

		[HttpGet("categories")]
		public async Task<ActionResult<List<AccountingCategoryDTO>>> GetCategories()
		{
			var categories = await repository.GetAllCategories();

			if (categories is null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			var categoriesDtos = mapper.Map<List<AccountingCategory>, List<AccountingCategoryDTO>>(categories);

			return Ok(categoriesDtos);
		}

		[HttpGet("categories/{id:int}")]
		public async Task<ActionResult<AccountingCategoryDTO>> GetCategory(int id)
		{
			var category = await repository.GetCategory(id);

			if (category is null)
			{
				return NotFound();
			}

			var categoryDto = mapper.Map<AccountingCategory, AccountingCategoryDTO>(category);

			return Ok(categoryDto);
		}

		[HttpPost("categories")]
		public async Task<ActionResult<AccountingCategory>> Create([FromBody] AccountingCategoryDTO categoryDto)
		{
			var category = mapper.Map<AccountingCategoryDTO, AccountingCategory>(categoryDto);

			if (category is null)
			{
				return BadRequest();
			}

			await repository.AddCategory(category);

			if (category.Id <= 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			categoryDto = mapper.Map<AccountingCategory, AccountingCategoryDTO>(category);

			return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryDto);
		}

		[HttpPut("categories/{id:int}")]
		public async Task<IActionResult> GetCategory(int id, [FromBody] AccountingCategoryDTO categoryDto)
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

			category = mapper.Map<AccountingCategoryDTO, AccountingCategory>(categoryDto);

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
	}
}
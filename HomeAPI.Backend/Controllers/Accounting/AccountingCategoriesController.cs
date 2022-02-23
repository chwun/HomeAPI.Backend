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
        public async Task<ActionResult<List<AccountingCategoryDto>>> GetCategories([FromQuery] bool asTree)
        {
            var categories = asTree
                ? await repository.GetCategoriesAsTree()
                : await repository.GetCategories();

            if (categories is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (asTree)
            {
                return Ok(mapper.Map<List<AccountingCategory>, List<AccountingCategoryTreeDto>>(categories));
            }
            else
            {
                return Ok(mapper.Map<List<AccountingCategory>, List<AccountingCategoryDto>>(categories));
            }
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
        public async Task<ActionResult<AccountingCategoryDto>> AddCategory(
            [FromBody] AccountingCategoryUpdateDto categoryDto,
            [FromQuery] int parentId)
        {
            var category = mapper.Map<AccountingCategoryUpdateDto, AccountingCategory>(categoryDto);

            if (category is null)
            {
                return BadRequest();
            }

            if (parentId > 0)
            {
                var parentCategory = await repository.GetCategory(parentId);

                if (parentCategory is null)
                {
                    return BadRequest();
                }

                category.ParentCategory = parentCategory;
            }

            await repository.AddCategory(category);

            if (category.Id <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var resultCategoryDto = mapper.Map<AccountingCategory, AccountingCategoryDto>(category);

            return CreatedAtAction(nameof(GetCategory), new { id = resultCategoryDto.Id }, resultCategoryDto);
        }

        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] AccountingCategoryUpdateDto categoryDto)
        {
            if (categoryDto is null)
            {
                return BadRequest();
            }

            var category = await repository.GetCategory(id);
            if (category is null)
            {
                return NotFound();
            }

            category.Update(categoryDto);
            await repository.UpdateCategory(category);

            return Ok();
        }

        [HttpDelete("categories/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await repository.GetCategoryWithSubCategories(id);

            if (category is null)
            {
                return NotFound();
            }

            // TODO: delete sub categories?!

            await repository.DeleteCategory(category);

            return Ok();
        }
    }
}
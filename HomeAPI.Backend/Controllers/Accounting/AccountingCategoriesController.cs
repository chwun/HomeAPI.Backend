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
    [Produces("application/json")]
    public class AccountingCategoriesController : ControllerBase
    {
        private readonly IAccountingCategoriesRepository repository;
        private readonly IMapper mapper;

        public AccountingCategoriesController(IAccountingCategoriesRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all accounting categories
        /// </summary>
        /// <param name="asTree">if true, categories are returned as hierarchy; if false, categories are returned as flat list</param>
        /// <returns>all accounting categories</returns>
        /// <response code="200">list/tree of categories</response>
        /// <response code="500">internal error reading categories</response>
        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AccountingCategoryDto>>> GetCategories([FromQuery] bool asTree)
        {
            var categories = asTree
                ? await repository.GetCategoriesAsTree()
                : await repository.GetCategories();

            if (categories is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error reading categories");
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

        /// <summary>
        /// Get accounting category by id
        /// </summary>
        /// <param name="id">category id</param>
        /// <returns>category with given id</returns>
        /// <response code="200">category object</response>
        /// <response code="404">category doesn't exist</response>
        [HttpGet("categories/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountingCategoryDto>> GetCategory(int id)
        {
            var category = await repository.GetCategory(id);

            if (category is null)
            {
                return NotFound($"Category {id} not found");
            }

            return Ok(mapper.Map<AccountingCategory, AccountingCategoryDto>(category));
        }

        /// <summary>
        /// Add a new accounting category
        /// </summary>
        /// <param name="categoryDto">New category object</param>
        /// <param name="parentId">id of parent category (optional)</param>
        /// <returns>newly created category object</returns>
        /// <response code="201">newly created category object</response>
        /// <response code="400">invalid category object or non-existing parent id</response>
        /// <response code="500">internal error creating category</response>
        [HttpPost("categories")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountingCategoryDto>> AddCategory(
            [FromBody] AccountingCategoryUpdateDto categoryDto,
            [FromQuery] int? parentId)
        {
            var category = mapper.Map<AccountingCategoryUpdateDto, AccountingCategory>(categoryDto);

            if (category is null)
            {
                return BadRequest("Invalid category data");
            }

            if (parentId.Value > 0)
            {
                var parentCategory = await repository.GetCategory(parentId.Value);

                if (parentCategory is null)
                {
                    return BadRequest($"Parent category {parentId} not found");
                }

                category.ParentCategory = parentCategory;
            }

            await repository.AddCategory(category);

            if (category.Id <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding category");
            }

            var resultCategoryDto = mapper.Map<AccountingCategory, AccountingCategoryDto>(category);

            return CreatedAtAction(nameof(GetCategory), new { id = resultCategoryDto.Id }, resultCategoryDto);
        }

        /// <summary>
        /// Update accounting category by id
        /// </summary>
        /// <param name="id">category id</param>
        /// <param name="categoryDto">category object</param>
        /// <returns></returns>
        /// <response code="200">successfully updated category</response>
        /// <response code="400">invalid category object</response>
        /// <response code="404">category with given id doesn't exist</response>
        [HttpPut("categories/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] AccountingCategoryUpdateDto categoryDto)
        {
            if (categoryDto is null)
            {
                return BadRequest("Invalid category data");
            }

            var category = await repository.GetCategory(id);
            if (category is null)
            {
                return NotFound($"Category {id} not found");
            }

            category.Update(categoryDto);
            await repository.UpdateCategory(category);

            return Ok();
        }

        /// <summary>
        /// Delete category by id
        /// </summary>
        /// <param name="id">category id</param>
        /// <returns></returns>
        /// <response code="200">successfully deleted category</response>
        /// <response code="400">sub-categories have to be deleted first</response>
        /// <response code="404">category with given id doesn't exist</response>
        [HttpDelete("categories/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await repository.GetCategoryWithSubCategories(id);

            if (category is null)
            {
                return NotFound($"Category {id} not found");
            }

            if (category.SubCategories?.Count > 0)
            {
                // only allow deleting category if it has no sub-categories
                return BadRequest("Deletion of category with sub-categories not allowed");
            }

            await repository.DeleteCategory(category);

            return Ok();
        }
    }
}
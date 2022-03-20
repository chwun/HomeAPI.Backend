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
    public class AccountingEntriesController : ControllerBase
    {
        private readonly IAccountingCategoriesRepository categoriesRepository;
        private readonly IAccountingEntriesRepository entriesRepository;
        private readonly IMapper mapper;

        public AccountingEntriesController(
            IAccountingCategoriesRepository categoriesRepository,
            IAccountingEntriesRepository entriesRepository,
            IMapper mapper)
        {
            this.categoriesRepository = categoriesRepository;
            this.entriesRepository = entriesRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all accounting entries of given category
        /// </summary>
        /// <param name="categoryId">category id</param>
        /// <returns>all entries of given category</returns>
        /// <response code="200">list of entry objects</response>
        /// <response code="404">category with given id doesn't exist</response>
        /// <response code="500">internal error reading entries</response>
        [HttpGet("categories/{categoryId:int}/entries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AccountingEntryDto>>> GetEntries(int categoryId)
        {
            var category = await categoriesRepository.GetCategory(categoryId);

            if (category is null)
            {
                return NotFound($"Category {categoryId} not found");
            }

            var entries = await entriesRepository.GetEntries(categoryId);

            if (entries is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error reading entries");
            }

            return Ok(mapper.Map<List<AccountingEntry>, List<AccountingEntryDto>>(entries));
        }

        /// <summary>
        /// Get accounting entry by id
        /// </summary>
        /// <param name="id">entry id</param>
        /// <returns>entry with given id</returns>
        /// <response code="200">entry object</response>
        /// <response code="404">entry with given id doesn't exist</response>
        [HttpGet("entries/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountingEntryDto>> GetEntry(int id)
        {
            AccountingEntry entry = await entriesRepository.GetEntry(id);

            if (entry is null)
            {
                return NotFound($"Entry {id} not found");
            }

            return Ok(mapper.Map<AccountingEntry, AccountingEntryDto>(entry));
        }

        /// <summary>
        /// Add a new accounting entry
        /// </summary>
        /// <param name="categoryId">id of category, to which entry is added</param>
        /// <param name="entryDto">entry object</param>
        /// <returns>newly created entry object</returns>
        /// <response code="201">newly created new entry</response>
        /// <response code="400">invalid entry object or non-existing category id</response>
        /// <response code="500">internal error creating entry</response>
        [HttpPost("categories/{categoryId:int}/entries")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountingEntryDto>> AddEntry(int categoryId, [FromBody] AccountingEntryUpdateDto entryDto)
        {
            var entry = mapper.Map<AccountingEntryUpdateDto, AccountingEntry>(entryDto);

            if (entry is null)
            {
                return BadRequest("Invalid entry data");
            }

            var category = await categoriesRepository.GetCategory(categoryId);

            if (category is null)
            {
                return BadRequest($"Category {categoryId} not found");
            }

            entry.Category = category;

            await entriesRepository.AddEntry(entry);

            if (entry.Id <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding entry");
            }

            var resultEntryDto = mapper.Map<AccountingEntry, AccountingEntryDto>(entry);

            return CreatedAtAction(nameof(GetEntry), new { id = resultEntryDto.Id }, resultEntryDto);
        }

        /// <summary>
        /// Update accounting entry by id
        /// </summary>
        /// <param name="id">entry id</param>
        /// <param name="entryDto">entry object</param>
        /// <returns></returns>
        /// <response code="200">successfully updated entry</response>
        /// <response code="400">invalid entry object</response>
        /// <response code="404">entry with given id doesn't exist</response>
        [HttpPut("entries/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEntry(int id, [FromBody] AccountingEntryUpdateDto entryDto)
        {
            if (entryDto is null)
            {
                return BadRequest("Invalid entry data");
            }

            var entry = await entriesRepository.GetEntry(id);
            if (entry is null)
            {
                return NotFound($"Entry {id} not found");
            }

            entry.Update(entryDto);
            await entriesRepository.UpdateEntry(entry);

            return Ok();
        }


        /// <summary>
        /// Delete accounting entry by id
        /// </summary>
        /// <param name="id">entry id</param>
        /// <returns></returns>
        /// <response code="200">successfully deleted entry</response>
        /// <response code="404">entry with given id doesn't exist</response>
        [HttpDelete("entries/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            var entry = await entriesRepository.GetEntry(id);

            if (entry is null)
            {
                return NotFound($"Entry {id} not found");
            }

            await entriesRepository.DeleteEntry(entry);

            return Ok();
        }
    }
}
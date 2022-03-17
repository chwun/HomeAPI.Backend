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

        [HttpGet("categories/{categoryId:int}/entries")]
        public async Task<ActionResult<List<AccountingEntryDto>>> GetEntries(int categoryId)
        {
            var entries = await entriesRepository.GetEntries(categoryId);

            if (entries is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error reading entries");
            }

            return Ok(mapper.Map<List<AccountingEntry>, List<AccountingEntryDto>>(entries));
        }

        [HttpGet("entries/{id:int}")]
        public async Task<ActionResult<AccountingEntryDto>> GetEntry(int id)
        {
            AccountingEntry entry = await entriesRepository.GetEntry(id);

            if (entry is null)
            {
                return NotFound($"Entry {id} not found");
            }

            return Ok(mapper.Map<AccountingEntry, AccountingEntryDto>(entry));
        }

        [HttpPost("categories/{categoryId:int}/entries")]
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

        [HttpPut("entries/{id:int}")]
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


        [HttpDelete("entries/{id:int}")]
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
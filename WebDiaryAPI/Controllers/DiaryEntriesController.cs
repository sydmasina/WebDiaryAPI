using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDiaryAPI.Controllers.Data;
using WebDiaryAPI.Models;

namespace WebDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryEntriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiaryEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryEntry>>> GetDiaryEntries()
        {
            try
            {
                return await _context.DiaryEntries.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to get diary endtries. Error: {ex.Message}");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiaryEntry>> GetDiaryEntryById(int id)
        {
            try
            {
                var diaryEntry = await _context.DiaryEntries.FindAsync(id);

                if (diaryEntry == null)
                {
                    return NotFound();
                }

                return diaryEntry;
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to get diary endtry. Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DiaryEntry>> NewDiaryEntry(DiaryEntry diaryEntry)
        {
            try
            {
                diaryEntry.EntryId = 0;

                _context.DiaryEntries.Add(diaryEntry);

                await _context.SaveChangesAsync();

                var resourceUrl = Url.Action(nameof(GetDiaryEntryById), new { id = diaryEntry.EntryId });
                return Created(resourceUrl, diaryEntry);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to add diary entry. \nError: {ex.Message} \n {ex.StackTrace}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutDiaryEntry(int id, [FromBody] DiaryEntry diaryEntry)
        {
            try
            {
                if (id != diaryEntry.EntryId)
                {
                    return BadRequest("The provided entry route Id does not match the body entryId.");
                }

                _context.Entry(diaryEntry).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                if (!DiaryEntryExists(id))
                {
                    return NotFound();
                }

                return BadRequest($"Unable to update diary entry. \nError: {ex.Message} \n {ex.StackTrace}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDiaryEntry(int id)
        {
            try
            {
                DiaryEntry diaryEntry = await _context.DiaryEntries.FindAsync(id);

                if (diaryEntry == null)
                {
                    return NotFound();
                }

                _context.DiaryEntries.Remove(diaryEntry);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message} \n {ex.StackTrace}");
            }
        }

        private bool DiaryEntryExists(int id) 
        { 
            return _context.DiaryEntries.Any(entry => entry.EntryId == id);
        }
    }
}

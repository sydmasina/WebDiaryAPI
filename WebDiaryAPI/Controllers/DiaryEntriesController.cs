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
                bool doesIdExist = await _context.DiaryEntries.AnyAsync(entry => entry.EntryId == diaryEntry.EntryId);

                if (doesIdExist)
                {
                    return Conflict(new
                    {
                        status = 409,
                        error = "Conflict",
                        message = "The ID you provided is already in use. Please try a different one.",
                        details = new { id = diaryEntry.EntryId }
                    });
                }

                _context.DiaryEntries.Add(diaryEntry);
                await _context.SaveChangesAsync();
                return diaryEntry;
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to add diary entry. \nError: {ex.Message} \n {ex.StackTrace}");
            }
        }
    }
}

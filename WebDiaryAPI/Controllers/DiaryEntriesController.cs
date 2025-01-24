using Microsoft.AspNetCore.Http;
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
            catch(Exception ex)
            {
                return BadRequest($"Unable get DiaryEndtries: {ex.Message}");
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
                return BadRequest($"Unable get DiaryEndtries: {ex.Message}");
            }
        }
    }
}

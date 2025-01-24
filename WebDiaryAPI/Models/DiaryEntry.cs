using System.ComponentModel.DataAnnotations;

namespace WebDiaryAPI.Models
{
    public class DiaryEntry
    {
        [Key]
        public int EntryId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength (1000, MinimumLength = 10)]
        public string Content { get; set; }

        [Required]
        public DateTime Created { get; set; }
    }
}

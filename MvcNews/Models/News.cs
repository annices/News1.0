using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcNews.Models
{
    /// <summary>
    /// This class defines the entity properties and input constraints for news entries.
    /// </summary>
    public class News
    {

        [DisplayName("Entry ID:")]
        public int EntryID { get; set; }

        [RegularExpression(@"[^<>]*$", ErrorMessage = "HTML or CSS code is not allowed in the title. Please exclude characters like '<' and '>'.")]
        [Required(ErrorMessage = "Please write a title.")]
        [StringLength(100, ErrorMessage = "The max length of the title is 50 characters.")]
        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Entry date:")]
        public string EntryDate { get; set; }

        [DataType(DataType.Html)]
        [Required(ErrorMessage = "Please write an entry.")]
        [StringLength(4000, ErrorMessage = "The max length of the entry is 4000 characters.")]
        public string Entry { get; set; }

        [Required(ErrorMessage = "Please select a status for the entry.")]
        public string Status { get; set; }

        public string Email { get; set; }


    } // End class.

} // End namespace.
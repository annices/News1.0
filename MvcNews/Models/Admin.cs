using System.ComponentModel.DataAnnotations;

namespace MvcNews.Models
{
    /// <summary>
    /// This class defines the entity properties and input constraints for admin users.
    /// </summary>
    public class Admin
    {

        // Admin properties:
        [StringLength(30, ErrorMessage = "The max length of the firstname is 30 characters.")]
        public string Firstname { get; set; }

        [StringLength(30, ErrorMessage = "The max length of the lastname is 30 characters.")]
        public string Lastname { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please write a valid email address.")]
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }


    } // End class.

} // End namespace.
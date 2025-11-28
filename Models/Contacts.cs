using System.ComponentModel.DataAnnotations;

namespace CryptocoinsFilter.Models
{
    public class Contacts
    {
        [Required(ErrorMessage ="Incorrect name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Incorrect Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Incorrect Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Incorrect Message")]
        [StringLength(150,ErrorMessage ="Max 200 symbols!")]
        public string Message { get; set; }

    }
}

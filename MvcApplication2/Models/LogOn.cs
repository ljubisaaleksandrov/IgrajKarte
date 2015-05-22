using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace IgrajKarte.Models
{
    public class LogOn
    {
        [Required]
        [Display(Name = "Username")]
        //[RegularExpression(@"^\s*(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*\s*$", ErrorMessage = "Email address is not valid.")]
        [StringLength(250)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
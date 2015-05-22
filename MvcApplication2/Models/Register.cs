using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace IgrajKarte.Models
{
    public class Register
    {
        [DataMember]
        [Required]
        [Display(Name = "First Name")]
        [StringLength(150)]
        public string FirstName { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(150)]
        public string LastName { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "User Name")]
        [StringLength(150)]
        public string UserName { get; set; }

        [DataMember]
        [Required]
        [RegularExpression(@"^\s*(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*\s*$", ErrorMessage = "Email address is not valid.")]
        [StringLength(250)]
        public string Email { get; set; }

        [DataMember]
        [StringLength(15)]
        public string Phone { get; set; }

        [DataMember]
        [Required]
        [StringLength(150)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [IgnoreDataMember]
        [Required]
        //[Compare("Password")]
        [StringLength(150)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "Address")]
        [StringLength(150)]
        public string Address { get; set; }

        [DataMember]
        [Required]
        [StringLength(150)]
        public string City { get; set; }

        [DataMember]
        [Required]
        public bool Terms { get; set; }
    }
}
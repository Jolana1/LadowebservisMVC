using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LadowebservisMVC.Models
{
    public class LoginModel : IEquatable<LoginModel>
    {
        [Required(ErrorMessage = "Email must be provided")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "You have not entered a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must be provided")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as LoginModel);
        }

        public bool Equals(LoginModel other)
        {
            return !(other is null) &&
                   Email == other.Email &&
                   Password == other.Password;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Email != null ? Email.GetHashCode() : 0);
                hash = hash * 23 + (Password != null ? Password.GetHashCode() : 0);
                return hash;
            }
        }
    }
}

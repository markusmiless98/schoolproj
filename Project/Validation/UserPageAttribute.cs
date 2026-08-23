using System.ComponentModel.DataAnnotations;
using PublicDatabaseAPI.Models;

namespace PublicSchoolProj.Validation
{
    public class UserPageAttribute : ValidationAttribute
    {
        public UserPageAttribute(int id) => Id = id;
        public int Id { get; }

        public string GetErrorMessage() =>
            $"Id must be unique for UserPage";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var userpage = (UserPage)validationContext.ObjectInstance;


            return base.IsValid(value, validationContext);
        }
    }
}

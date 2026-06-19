using System.ComponentModel.DataAnnotations.Schema;

namespace PublicSchoolProj.Models
{
    public class UserPost
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string description { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }

        public bool IsValidPost()
        {
            if (userId == null) return false;
            if (description == null && file == null) return false;

            return true;
        }
    }
}

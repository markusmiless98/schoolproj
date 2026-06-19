using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PublicSchoolProj.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Path { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }
    }
}

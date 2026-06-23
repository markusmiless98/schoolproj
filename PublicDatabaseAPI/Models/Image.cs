using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PublicDatabaseAPI.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Path { get; set; }

    }
}

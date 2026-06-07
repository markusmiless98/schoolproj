using System.ComponentModel.DataAnnotations.Schema;

namespace PublicSchoolProj.Models
{
    public class UserPageBlock
    {
        public int Id { get; set; }
        public int Column { get; set; } = 0;
        public int Row { get; set; } = 0;

        public int Width { get; set; } = 1;

        public string Title { get; set; }
        public string Text { get; set; }

        [NotMapped]
        public IFormFile _picture { get; set; }

        public bool IsValidBlock()
        {
            if (Column < 0 || Row < 0) return false;
            if (Width < 1) return false;

            if (Text == null && _picture == null)
            {
                return false;
            }


            return true;
        }

        public int GetPositionValue()
        {
            return Row * 5 + Column;
        }
    }
}

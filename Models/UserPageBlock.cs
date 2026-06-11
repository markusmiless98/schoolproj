using System.ComponentModel.DataAnnotations.Schema;

namespace PublicSchoolProj.Models
{
    public class UserPageBlock
    {
        public int Id { get; set; }
        public int? UserPageId { get; set; }
        public string? ImagePath { get; set; } = "";
        public int Column { get; set; } = 0;
        public int Row { get; set; } = 0;

        public int Width { get; set; } = 1;

        public string? Title { get; set; }
        public string? Text { get; set; }


        [NotMapped]
        public IFormFile? _picture { get; set; } // Might get removed later lol

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

        public virtual void Overwrite(UserPageBlock _over)
        {
            if (_over == null) return;

            if (_over.Title != null)
            {
                if (_over.Title.Length > 0)
                {
                    Title = _over.Title;
                }
            }
            if (_over.Text != null)
            {
                if (_over.Text.Length > 0)
                {
                    Text = _over.Text;
                }
            }
            if (_over.ImagePath != null)
            {
                if (_over.ImagePath.Length > 0)
                {
                    ImagePath = _over.ImagePath;
                }
            }
            if (_over._picture != null)
            {
                _picture = _over._picture;
                ImagePath = _picture.FileName;
            }
            if (_over.Row != Row)
            {
                Row = _over.Row;
            }
            if (_over.Column != Column)
            {
                Column = _over.Column;
            }
        }
    }
}

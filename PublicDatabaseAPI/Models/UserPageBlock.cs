using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicDatabaseAPI.Models
{
    public class UserPageBlock
    {
        public int Id { get; set; }
        public int? UserPageId { get; set; }
        public string? ImagePath { get; set; } = "";
        public int Column { get; set; } = 0;
        public int Row { get; set; } = 0;
        public int Width { get; set; } = 1;

        public string? Title { get; set; } = "";
        public string? Text { get; set; } = "";

        public bool IsValidBlock()
        {
            if (Column < 0 || Row < 0) return false;
            if (Width < 1) return false;

            if (Text == null && ImagePath == null)
            {
                return false;
            }
            if (Text == null || Title == null)
            {
                FixIfBroken();
            }


            return true;
        }

        public int GetPositionValue()
        {
            return Row * 5 + Column;
        }

        public virtual void FixIfBroken()
        {
            if (Title == null) Title = "";
            if (Text == null) Text = "";
        }

        public virtual void OverWrite(UserPageBlock _over)
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
            if (_over.Row != Row)
            {
                Row = _over.Row;
            }
            if (_over.Column != Column)
            {
                Column = _over.Column;
            }
        }
        public bool IsImagePathValid()
        {
            string _path = ImagePath;

            if (_path == null) return false;
            if (_path.Length < 3) return false;
            if (!_path.EndsWith(".png") && !_path.EndsWith(".jpg"))
            {
                return false;
            }
            var filePath = "~/img/" + _path;
            // TBA; Check if gotten permission to check for local files later
            if (Path.GetFileName(_path) != null)
            {
                return true;
            }
            return false;
        }
    }
}

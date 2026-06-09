using System.ComponentModel.DataAnnotations.Schema;
using Azure;

namespace PublicSchoolProj.Models
{
    public class UserPage
    {
        public int Id { get; set; }
        public int? userId { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }

        public int? views { get; set; }

        public List<string> links { get; set; } = new List<string>();

        [NotMapped]
        public List<IFormFile> _pictures { get; set; } = new List<IFormFile>();

        public virtual List<UserPageBlock> _blocks { get; set; } = new List<UserPageBlock>();

        public bool IsValidPage()
        {
            if (userId == null) return false;
            if (description == null && _pictures == null && _blocks == null) return false;
            int _length = 0;
            if (description != null)
            {
                _length += description.Length;
            }
            if (_pictures != null)
            {
                _length += _pictures.Count;
            }
            if (_blocks != null)
            {
                _length += _blocks.Count;
            }

            if (_length < 1) return false;

            return true;
        }

        public List<UserPageBlock> GetBlocks()
        {
            if (_blocks == null) return null;
            _blocks = BlockSort(_blocks);

            return _blocks;
        }

        public UserPageBlock GetBlock(int id)
        {
            if (_blocks == null)
            {
                _blocks = new List<UserPageBlock>();
                _blocks.Add(new UserPageBlock());
                return _blocks[0];
            }

            if (_blocks.Count <= id)
            {
                return _blocks[^1];
            }
            if (id < 0)
            {
                return _blocks[0];
            }

            return _blocks[id];
        }

        private List<UserPageBlock> BlockSort(List<UserPageBlock> _list)
        {
            List<UserPageBlock> week = _list;
            week.Sort(delegate (UserPageBlock c1, UserPageBlock c2) { return c1.GetPositionValue().CompareTo(c2.GetPositionValue()); });
            return week;
        }
    }
}

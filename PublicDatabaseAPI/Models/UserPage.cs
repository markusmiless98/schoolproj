using System.ComponentModel.DataAnnotations.Schema;

namespace PublicDatabaseAPI.Models
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
        public List<Links> _listOfLinks { get; set; }

        public virtual List<UserPageBlock> _blocks { get; set; } = new List<UserPageBlock>();

        public bool IsValidPage()
        {
            if (userId == null) return false;
            if (description == null && _blocks == null) return false;

            int _length = 0;
            if (description != null)
            {
                _length += description.Length;
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

            _blocks.Sort(BlockSort);

            return _blocks;
        }

        public int BlockSort(UserPageBlock A, UserPageBlock B)
        {
            if (A.Row > B.Row)
            {
                return 1;
            }
            if (B.Row < A.Row)
            {
                return -1;
            }

            if (A.Column > B.Column)
            {
                return 1;
            }
            if (B.Column > A.Column)
            {
                return -1;
            }

            return 0;
        }

        public List<int> GetLinks()
        {
            List<int> _ints = new List<int>();
            if (links == null) return _ints;

            foreach (var item in links)
            {
                int x;
                if (int.TryParse(item, out x))
                {
                    _ints.Add(x);
                }
            }

            return _ints;
        }

        public void AddLinks(string _list = null)
        {
            if (links == null)
            {
                links = new List<string>();
            }
            if (links.Count < 3)
            {
                if (_list != null)
                {
                    links.Add(_list);
                }
                else
                {
                    links.Add("0");
                }
            }
            if (links.Count > 3)
            {
                links.RemoveRange(3, links.Count - 1);
            }
        }
        public bool RemoveLatestLinkById(int _id)
        {
            if (links == null || _id == null)
            {
                return false;
            }
            if (links.Count < 1 || _id < 0)
            {
                return false;
            }
            List<int> _list = GetLinks();

            if (_id >= _list.Count)
            {
                return false;
            }
            else
            {
                _list.RemoveAt(_id);
                links = new List<string>();
                foreach (var item in _list)
                {
                    links.Add(item.ToString());
                }
            }
            return true;
        }

        public UserPageBlock GetBlock(int id)
        {
            if (_blocks == null || _blocks.Count < 1)
            {
                _blocks = new List<UserPageBlock>();
                _blocks.Add(new UserPageBlock());
                return _blocks[0];
            }

            if (_blocks.Count <= id)
            {
                return _blocks[_blocks.Count - 1];
            }
            if (id < 0)
            {
                return _blocks[0];
            }

            return _blocks[id];
        }

        public virtual void OverWrite(UserPage _page)
        {
            if (_page.userId != null)
            {
                this.userId = _page.userId;
            }
            if (_page.title != null)
            {
                this.title = _page.title;
            }
            if (_page.description != null)
            {
                this.description = _page.description;
            }
            if (_page.GetBlocks() != null)
            {
                this._blocks = _page._blocks;
            }
            if (_page.links != null)
            {
                this.links = _page.links;
            }
            if (_page._listOfLinks != null)
            {
                this._listOfLinks = _page._listOfLinks;
            }
        }

    }
}

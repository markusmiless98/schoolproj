using Microsoft.Extensions.FileProviders;

namespace PublicSchoolProj.Models
{
    public class UserPageManager
    {
        public UserPage _UserPage = new UserPage();

        private UserPageBlock _SelectedPageBlock { get; set; }

        public UserPageBlock GetBlock()
        {
            return _SelectedPageBlock;
        }

        public bool ValidateOrDeleteBlock()
        {
            if (!CanProceed("always"))
            {
                return false;
            }
            if (!_SelectedPageBlock.IsValidBlock())
            {
                _UserPage._blocks.Remove(_SelectedPageBlock);
                return false;
            }
            return true;
        }

        public void CreateNewBlock(bool _selectNew = true)
        {
            _UserPage._blocks.Add(new UserPageBlock());
            if (_selectNew)
            {
                _SelectedPageBlock = _UserPage.GetBlocks()[^1];
            }
        }

        public void CreateNewBlock(UserPageBlock _block)
        {
            _UserPage._blocks.Add(_block);
        }

        public void GetBlockById(int id)
        {
            if (_UserPage._blocks == null)
            {
                _UserPage.GetBlocks().Add(new UserPageBlock());
                _SelectedPageBlock = _UserPage.GetBlocks()[0];
            }
            int _count = _UserPage.GetBlocks().Count;
            if (id >= _count)
            {
                _SelectedPageBlock = _UserPage.GetBlocks()[_count - 1];
            }
            if (id < 0)
            {
                _SelectedPageBlock = _UserPage.GetBlocks()[0];
            }

            _SelectedPageBlock = _UserPage.GetBlocks()[id];
        }

        public void SetPictureOfBlock(IFormFile _file)
        {
            if (!CanProceed(_file)) return;

            _SelectedPageBlock._picture = _file;
        }
        public void SetTitleOfBlock(string _title)
        {
            if (!CanProceed(_title)) return;

            _SelectedPageBlock.Title = _title;
        }
        public void SetDescriptionOfBlock(string _desc)
        {
            if (!CanProceed(_desc)) return;

            _SelectedPageBlock.Text = _desc;
        }

        private bool CanProceed(object _sel)
        {
            if (_sel == null) return false;
            return _SelectedPageBlock != null;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;
using System.Diagnostics;
using System.Text.Json;

namespace PublicDatabaseAPI.Controllers
{
    public interface IUserPageController
    {
        async Task<List<UserPage>> ReadAll()
        {
            throw new NotImplementedException();
        }
        async Task Create(UserPage userpage)
        {
            throw new NotImplementedException();
        }
        async Task<UserPage> Read(int id)
        {
            throw new NotImplementedException();
        }
        async Task<UserPage> Update(UserPage modifier_page)
        {
            throw new NotImplementedException();
        }
        async Task Delete(int id)
        {
            throw new NotImplementedException();
        }
        async Task HandleFromList(EntityState _state, List<UserPage> blocks)
        {
            throw new NotImplementedException();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserPageController : ControllerBase, IUserPageController
    {
        private readonly ApplicationDbContext _context;
        private List<UserPage> _userpages = new List<UserPage>();
        private bool _active = false;

        public UserPageController(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new Exception("No DB Context given");
            }
            _context = context;
            // DB <-> repo <-> service <-> api(controller) <-> service <-> frontend
            // HTTPClient in service
            // Onion

            //Debug.WriteLine("--Created: User Page Controller");
        }

        public async Task SetUpCache()
        {
            if (_context == null) throw new Exception("No DB Context to help set up Cache for User Page Handler");

            _userpages = await _context.UserPage.ToListAsync();
            
            _active = true;
        }

        // POST: api/UserPage
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] object? userpage)
        {
            UserPage _page = new UserPage();

            _page.OverWrite(JsonSerializer.Deserialize<UserPage>(userpage.ToString()));

            await Create(_page);

            return Ok();
        }


        // public async Task<IActionResult> Create([FromBody] UserPage userpage)
        public async Task<IActionResult> Create(UserPage userpage)
        {
            if (IsNotActive()) await SetUpCache();
            //UserPage userpage = JsonSerializer.Deserialize<UserPage>(_json);

            UserPage _page = new UserPage();
            _page.OverWrite(userpage);

            try
            {
                //_context.Add(userpage);

                //_page.Id = await GetUserPageId();

                _context.UserPage.Add(_page);
                //_context.UserPage.Attach(_page).State = EntityState.Added;
                _userpages.Add(_page);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (UserPageExists(userpage.Id))
                {
                    throw new Exception("Failed due to Page with ID already exists");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // GET: /UserPage
        [HttpGet]
        public async Task<List<UserPage>> ReadAll()
        {
            if (IsNotActive()) await SetUpCache();

            if (_userpages != null)
            {
                return _userpages;
            }
            else
            {
                _userpages = await _context.UserPage.ToListAsync();
                return _userpages;
            }

            throw new Exception("Pages to be viewed not Found");
        }

        // GET: /UserPage/X
        [HttpGet("{id}", Name = "GetPage")]
        public async Task<UserPage> Read(int id)
        {
            if (IsNotActive()) await SetUpCache();

            UserPage? _page = _userpages.FirstOrDefault(m => m.Id == id);

            if (_page != null)
            {
                _page._listOfLinks = await GetLinksFromID(_page);
                return _page;
            }
            else
            {
                _page = await _context.UserPage.FirstOrDefaultAsync(m => m.Id == id);
                if (_page != null)
                {
                    _page._listOfLinks = await GetLinksFromID(_page);
                    return _page;
                }
            }

            //throw new Exception("Page to be viewed not Found");
            return null;
        }

        // DELETE: /UserPage/X
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            if (IsNotActive()) await SetUpCache();

            UserPage _page = await Read(id);

            if (_page != null)
            {
                _context.Attach(_page).State = EntityState.Deleted;

                try
                {
                    _userpages.Remove(_page);

                    var _referpages = _userpages.Where(m => m.GetLinks().Contains(m.Id));

                    if (_referpages != null)
                    {
                        foreach (var item in _referpages)
                        {
                            item.RemoveLinkById(id);
                            _context.Attach(item).State = EntityState.Modified;
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPageExists(_page.Id))
                    {
                        throw new Exception("Failed to find page with matching Id");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        [HttpPut("{id}")]
        public async Task Update([FromBody] UserPage modifier_page)
        {
            if (IsNotActive()) await SetUpCache();

            int _id = modifier_page.Id;

            UserPage _page = await Read(_id);
            if (_page != null)
            {
                _page.OverWrite(modifier_page);

                _context.Attach(_page).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    _userpages.FirstOrDefault(m => m.Id == _id).OverWrite(modifier_page);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPageExists(_page.Id))
                    {
                        throw new Exception("Failed to find page with matching Id");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        // PUT: api/UserPage
        [HttpPut]
        public async Task Update(UserPageBlock _block)
        {
            if (IsNotActive()) await SetUpCache();
            if (_block.UserPageId == null) return;

            int i = (int)_block.UserPageId;

            UserPage _page = await Read(i);
            if (_page != null)
            {
                _context.Attach(_page).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    _userpages.FirstOrDefault(m => m.Id == i).GetBlocks().FirstOrDefault(n => n.Id == _block.Id).OverWrite(_block);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPageExists(_page.Id))
                    {
                        throw new Exception("Failed to find page with matching Id");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
        public async Task HandleFromList(EntityState _state = EntityState.Modified, List<UserPage> blocks = null)
        {
            if (blocks == null) return;

            if (IsNotActive()) await SetUpCache();


            foreach (var block in blocks)
            {
                if (block != null)
                {
                    UserPage _temp = await Read(block.Id);
                    _temp.OverWrite(block);
                    _context.Attach(_temp).State = _state;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                int i = blocks.Count - 1;
                while (i > 0)
                {
                    UserPage _block = await Read(blocks[i].Id);
                    if (_block != null)
                    {
                        if (_state == EntityState.Modified)
                        {
                            _block.OverWrite(blocks[i]);
                        }
                        else if (_state == EntityState.Deleted)
                        {
                            _userpages.Remove(_block);
                        }
                        else if (_state == EntityState.Added)
                        {
                            _userpages.Add(blocks[i]);
                        }
                    }

                    i--;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Failed to update blocks in list");
            }
        }

        private bool IsNotActive()
        {
            return !_active && _context != null;
        }
        private bool UserPageExists(int id)
        {
            return _context.UserPage.Any(e => e.Id == id);
        }

        private bool _accessing = false;
        private async Task<List<Links>> GetLinksFromID(UserPage _page)
        {
            if (_accessing) return null;

            if (_page.GetLinks() == null)
            {
                return null;
            }
            if (_page.GetLinks().Count < 1)
            {
                return null;
            }

            List<Links> _links = new List<Links>();

            _accessing = true;
            foreach (var item in _page.GetLinks())
            {
                UserPage targ = await Read(item);
                if (targ != null)
                {
                    Links _link = new Links(targ.title, item.ToString());
                    _links.Add(_link);
                }
            }
            _accessing = false;

            if (_links.Count > 0)
            {
                return _links;
            }

            return null;
        }
        private async Task<int> GetUserPageId()
        {
            int _id = 0;

            List<UserPage> _pages = await ReadAll();

            foreach (var item in _pages)
            {
                if (_id <= item.Id)
                {
                    _id = item.Id + 1;
                }
            }

            return _id;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;



//using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;

namespace PublicDatabaseAPI.Controllers
{
    public interface IUserPageBlockController
    {
        async Task<List<UserPageBlock>> ReadAll()
        {
            throw new NotImplementedException();
        }
        async Task Create(UserPageBlock userblock)
        {
            throw new NotImplementedException();
        }
        async Task<UserPageBlock> Read(int id)
        {
            throw new NotImplementedException();
        }
        async Task<UserPageBlock> Update(UserPageBlock modifier_block)
        {
            throw new NotImplementedException();
        }
        async Task Delete(int id)
        {
            throw new NotImplementedException();
        }
        async Task AddToList(int id)
        {
            throw new NotImplementedException();
        }
        async Task UpdateFromList()
        {
            throw new NotImplementedException();
        }
    }
    [Route("/UserPageBlock")]
    [ApiController]
    public class UserPageBlockController : IUserPageBlockController
    {
        private readonly ApplicationDbContext _context;
        private List<UserPageBlock> _userblocks = new List<UserPageBlock>();
        private bool _active = false;

        public UserPageBlockController(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new Exception("No DB Context given");
            }
            _context = context;
            //Debug.WriteLine("--Created: User Page Block Controller");
        }

        public async Task SetUpCache()
        {
            if (_context == null) throw new Exception("No DB Context to help set up Cache for User Page Handler");

            _userblocks = await _context.UserBlockPage.ToListAsync();

            _active = true;
        }

        [HttpPost]
        public async Task Create(UserPageBlock userblock)
        {
            if (IsNotActive()) await SetUpCache();

            _context.Attach(userblock).State = EntityState.Added;

            try
            {
                await _context.SaveChangesAsync();
                _userblocks.Add(userblock);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (UserPageBlockExists(userblock.Id))
                {
                    throw new Exception("Failed due to Page with ID already exists");
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: /UserPageBlock/X
        [HttpGet("{id}", Name = "GetBlock")]
        public async Task<UserPageBlock> Read(int id)
        {
            if (IsNotActive()) await SetUpCache();

            UserPageBlock? _page = _userblocks.FirstOrDefault(m => m.Id == id);

            if (_page != null)
            {
                return _page;
            }
            else
            {
                _page = await _context.UserBlockPage.FirstOrDefaultAsync(m => m.Id == id);
                if (_page != null)
                {
                    return _page;
                }
            }

            throw new Exception("Page to be viewed not Found");
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            if (IsNotActive()) await SetUpCache();

            UserPageBlock _block = await Read(id);

            if (_block != null)
            {

                _context.Attach(_block).State = EntityState.Deleted;

                try
                {
                    await _context.SaveChangesAsync();
                    _userblocks.Remove(_block);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPageBlockExists(_block.Id))
                    {
                        throw new Exception("Failed to find page with matching Id");
                    }
                    else
                    {
                        Debug.WriteLine("Failed to update block through this controller, attempting to do so through next method");
                        _context.Dispose();
                        await _context.GetUserPageController().Update(_block); // Backup
                    }
                }
            }
        }

        // GET: /UserPageBlock
        [HttpGet]
        public async Task<List<UserPageBlock>> ReadAll()
        {
            if (IsNotActive()) await SetUpCache();

            if (_userblocks != null)
            {
                return _userblocks;
            }
            else
            {
                _userblocks = await _context.UserBlockPage.ToListAsync();
                return _userblocks;
            }

            throw new Exception("Pages to be viewed not Found");
        }

        public async Task<List<UserPageBlock>> ReadAll(int _id)
        {
            if (IsNotActive()) await SetUpCache();

            if (_userblocks != null)
            {
                List<UserPageBlock> _list = _userblocks.FindAll(m => m.UserPageId == _id);
                if (_list != null)
                {
                    return _list;
                }
            }
            else
            {
                _userblocks = await _context.UserBlockPage.ToListAsync();

                if (_userblocks != null)
                {
                    return await ReadAll(_id);
                }
            }

            return null;
        }

        public async Task Update(UserPageBlock modifier_page)
        {
            if (IsNotActive()) await SetUpCache();

            int _id = modifier_page.Id;

            UserPageBlock _block = await Read(_id);
            if (_block != null)
            {
                _block.OverWrite(modifier_page);

                _context.Attach(_block).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    _userblocks.FirstOrDefault(m => m.Id == _id).OverWrite(modifier_page);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPageBlockExists(_block.Id))
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

        private List<UserPageBlock> _listToChange { get; set; }
        public async Task AddToList(int id)
        {
            if (IsNotActive()) await SetUpCache();

            if (_listToChange == null)
            {
                _listToChange = new List<UserPageBlock>();
            }

            UserPageBlock _block = await Read(id);

            _listToChange.Add(_block);
        }
        public async Task AddToList(UserPageBlock _block)
        {
            if (_block == null) return;
            if (IsNotActive()) await SetUpCache();

            if (_listToChange == null)
            {
                _listToChange = new List<UserPageBlock>();
            }

            _listToChange.Add(_block);
        }

        public async Task HandleFromList(EntityState _state = EntityState.Modified, List<UserPageBlock> blocks = null)
        {
            if (blocks == null) return;

            if (IsNotActive()) await SetUpCache();


            foreach (var block in blocks)
            {
                if (block != null)
                {
                    UserPageBlock _temp = await Read(block.Id);
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
                    UserPageBlock _block = await Read(blocks[i].Id);
                    if (_block != null)
                    {
                        if (_state == EntityState.Modified)
                        {
                            _block.OverWrite(blocks[i]);
                        }
                        else if (_state == EntityState.Deleted)
                        {
                            _userblocks.Remove(_block);
                        }
                        else if (_state == EntityState.Added)
                        {
                            _userblocks.Add(blocks[i]);
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
        private bool UserPageBlockExists(int id)
        {
            return _context.UserBlockPage.Any(e => e.Id == id);
        }
    }
}

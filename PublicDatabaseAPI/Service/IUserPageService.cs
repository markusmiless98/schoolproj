using PublicDatabaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicDatabaseAPI.Service
{
    public interface IUserPageService
    {
        Task DeletePageAsync(int categoryId);
        Task EditPageAsync(UserPage category);
        Task<List<UserPage>> GetAllPagesAsync();
        Task<UserPage> GetPageAsync(int categoryId);
        Task SavePageAsync(UserPage category);
        Task AddPageAsync(UserPage page);
    }
}

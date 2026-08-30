using Azure;
using Microsoft.AspNetCore.Http;
using PublicDatabaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PublicDatabaseAPI.Service
{
    public class UserPageService : IUserPageService
    {
        private readonly HttpClient _client;

        public UserPageService(IHttpClientFactory httpclient)
        {
            _client = httpclient.CreateClient("UserPage");
        }

        public async Task AddPageAsync(UserPage page)
        {

            var final = JsonSerializer.Serialize<UserPage>(page);
            
            var httpContent = new StringContent(final, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Userpage", page);
        }

        public async Task DeletePageAsync(int id)
        {
            await _client.DeleteAsync($"api/UserPage/" + id);
        }

        public Task EditPageAsync(UserPage category)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserPage>> GetAllPagesAsync()
        {
            List<UserPage> _pages = new List<UserPage>();

            HttpResponseMessage response = await _client.GetAsync("api/UserPage");

            if (response.IsSuccessStatusCode)
            {
                string json = await GetJson(response);

                if (json != null)
                {
                    var final = JsonSerializer.Deserialize<List<UserPage>>(json);
                    
                    _pages = final.Select(_page => new UserPage
                    {
                        Id = _page.Id,
                        userId = _page.userId,
                        description = _page.description,
                        title = _page.title,
                        links = _page.links,
                        views = _page.views
                    }).ToList();

                }
                else
                {
                    throw new Exception(json);
                }
            }
            else
            {
                throw new Exception("Failed to load pages through Service");
            }
            return _pages;
        }

        public async Task<UserPage> GetPageAsync(int categoryId)
        {
            UserPage _userPage = new UserPage();

            List<UserPage> _pages = await GetAllPagesAsync();
            // Clumsy temp solution
            if (_pages != null)
            {
                List<UserPage> _temp = _pages.Where(t => t.Id == categoryId).ToList();
                if (_temp != null)
                {
                    _userPage = _temp[0];
                }
            }

            return _userPage;
        }

        public async Task SavePageAsync(UserPage category)
        {
            if (category.views == null)
            {
                category.views = 0;
            }
            if (category._listOfLinks == null)
            {
                category._listOfLinks = new List<Links>();
            }
            
            var final = JsonSerializer.Serialize<UserPage>(category);

            var httpContent = new StringContent(final, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsJsonAsync("api/UserPage", httpContent);
        }

        private async Task<string> GetJson(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();
            string json_fix = json.Replace("\"id\":", "\"Id\":");
            return json_fix;
        }
    }
}

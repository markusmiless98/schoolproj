using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using PublicCssAPI.DataType;

namespace PublicCssAPI.JSON
{
    public class JsonHandler
    {
        private string _filename = "editable.json";

        public async Task WriteFile(string _path, string _json_txt)
        {
            if (_path == null || _json_txt == null) return;

            string _filename = "editable.json";

        }
        public async Task WriteFile(string _path, CssData _data)
        {
            if (_data == null) return;

            var css_data = new CssData
            {
                LinkColor = _data.LinkColor,
                TitleName = _data.TitleName,
                TitleSize = _data.TitleSize,
                TitleColor = _data.TitleColor,
                TextName = _data.TextName,
                TextColor = _data.TextColor,
                TextSize = _data.TextSize
            };

            string jsonString = JsonSerializer.Serialize(css_data);

            string path = Directory.GetCurrentDirectory() + _path;

            await using FileStream createStream = File.Create(_path + _filename);
            await JsonSerializer.SerializeAsync(createStream, css_data);
            await File.WriteAllTextAsync(path + _filename, jsonString);
        }

        public async Task<List<string>> GetFile(string _path, bool willRetry = true)
        {
            List<string> _sol = new List<string>();
            if (File.Exists(_path + _filename))
            {
                await using FileStream readStream = File.OpenRead(_path + _filename);
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                };
                CssData _data = JsonSerializer.Deserialize<CssData>(readStream, options);
                if (_data != null)
                {
                    return await GetFile(_data);
                }
            }
            else
            {
                _sol.Add("Failed to load css, writing file");
                await WriteFile(_path, new CssData());
                if (willRetry)
                {
                    return await GetFile(_path, false);
                }
            }
            return _sol;
        }
        public async Task<List<string>> GetFile(CssData _data)
        {
            List<string> _sol = new List<string>();
            if (_data != null)
            {
                CssData _dataTrue = _data;
                _sol = _dataTrue.ConvertIntoString();
            }
            else
            {
                _sol.Add("Failed to load css");
            }
            return _sol;
        }
    }
}

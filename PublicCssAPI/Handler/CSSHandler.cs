using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicCssAPI.DataType;

namespace PublicCssAPI.Handler
{
    public class CSSHandler
    {
        private string editableCssName = "editable.css";

        private CssData data { get; set; }

        public async Task WriteTo(CssData _data = null)
        {
            if (_data == null)
            {
                _data = new CssData(true); // Backup for now, later use database to check
            }
            data = _data;

            if (data.ConvertIntoString().Count() < 1)
            {
                Debug.WriteLine("-------- WARNING: NO CSS DATA FOUND ---------");
                return;
            }

            var file = "./wwwroot/css/" + editableCssName;

            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                using (var sr = new StreamWriter(fileStream))
                {
                    foreach (var item in data.ConvertIntoString())
                    {
                        await sr.WriteLineAsync(item);
                    }
                    await sr.DisposeAsync();
                }
                await fileStream.DisposeAsync();
            }
            Debug.WriteLine("Finished updating");
        }
        public async Task WriteTo(List<string> _list)
        {
            CssData _newData = new CssData(false);
            _newData.ConvertFromString(_list);

            await WriteTo(_newData);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PublicCssAPI.DataType
{

    public class CssData
    {
        [JsonInclude]
        public string LinkColor { get; set; }
        
        public string TitleName { get; set; }
        public double TitleSize { get; set; }
        public string TitleColor { get; set; }

        public string TextName { get; set; }
        public double TextSize { get; set; }
        public string TextColor { get; set; }

        public CssData(bool _default = true)
        {
            if (_default)
            {
                LinkColor = "blue";
                TitleName = ".title";
                TitleSize = 20;
                TitleColor = "Black";
                TextName = ".text";
                TextSize = 10;
                TextColor = "Black";
            }
        }
        [JsonConstructor]
        public CssData(string LinkColor, string TitleName, double TitleSize, string TitleColor, string TextName, double TextSize, string TextColor)
        {
            this.LinkColor = LinkColor;
            this.TitleName = TextName;
            this.TitleSize = TitleSize;
            this.TitleColor = TitleColor;
            this.TextName = TextName;
            this.TextSize = TextSize;
            this.TextColor = TextColor;
        }

        public List<string> ConvertIntoString()
        {
            List<string> _strings = new List<string>();
            _strings.Add(LinkColor);
            _strings.Add(TitleName);
            _strings.Add(TitleColor);
            _strings.Add(TitleSize.ToString());
            _strings.Add(TextName);
            _strings.Add(TextColor);
            _strings.Add(TextSize.ToString());

            return _strings;
        }
    }
}

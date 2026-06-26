using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicDatabaseAPI.Models
{
    public class LayoutCSS
    {
        public int Id { get; set; }
        public string LinkColor { get; set; }

        public double TitleSize { get; set; }
        public string TitleColor { get; set; }

        public double TextSize { get; set; }
        public string TextColor { get; set; }

        public int PicWidth { get; set; }
        public int PicHeight { get; set; }

        public virtual void SetDefault()
        {
            LinkColor = "blue";
            TitleSize = 20;
            TitleColor = "Black";
            TextSize = 10;
            TextColor = "Black";
            PicWidth = 600;
            PicHeight = 600;
        }
        public List<string> ConvertIntoString()
        {
            List<string> _strings = new List<string>();
            _strings.Add(".links {");
            _strings.Add("color:" + LinkColor + ";");
            _strings.Add("}");
            _strings.Add(".title {");
            _strings.Add("color:" + TitleColor + ";");
            _strings.Add("font-size:" + TitleSize.ToString() + "px;");
            _strings.Add("}");
            _strings.Add(".text {");
            _strings.Add("color:" + TextColor + ";");
            _strings.Add("font-size:" + TextSize.ToString() + "px;");
            _strings.Add("}");
            _strings.Add("img {");
            _strings.Add("width:" + PicWidth.ToString() + "px;");
            _strings.Add("height:" + PicWidth.ToString() + "px;");
            _strings.Add("}");

            return _strings;
        }

        public void ConvertFromString(List<string> _strings)
        {
            // This will always assume you get it right for now, poor programming but short on time

            _strings.RemoveAt(_strings.Count - 1);
            _strings.RemoveAt(_strings.Count - 3);
            _strings.RemoveAt(_strings.Count - 3);
            _strings.RemoveAt(_strings.Count - 5);
            _strings.RemoveAt(_strings.Count - 5);
            _strings.RemoveAt(_strings.Count - 7);
            _strings.RemoveAt(_strings.Count - 7);
            _strings.RemoveAt(0);

            if (_strings.Count < 6)
            {
                throw new Exception("There are too few items");
            }

            LinkColor = GetTrimmedString(_strings[0]);
            TitleColor = GetTrimmedString(_strings[1]);
            TitleSize = Convert.ToDouble(GetTrimmedString(_strings[2]));
            TextColor = GetTrimmedString(_strings[3]);
            TextSize = Convert.ToDouble(GetTrimmedString(_strings[4]));
            PicWidth = Convert.ToInt32(GetTrimmedString(_strings[5]));
            PicHeight = Convert.ToInt32(GetTrimmedString(_strings[6]));
        }

        public void OverWrite(LayoutCSS _overwriter)
        {
            if (_overwriter.LinkColor != null)
            {
                this.LinkColor = _overwriter.LinkColor;
                Debug.WriteLine("Link");
            }
            if (_overwriter.TitleColor != null)
            {
                this.TitleColor = _overwriter.TitleColor;
                Debug.WriteLine(this.TitleColor + " IS ? " + _overwriter.TitleColor);
            }
            if (_overwriter.TitleSize != null)
            {
                if (_overwriter.TitleSize > 0)
                {
                    this.TitleSize = _overwriter.TitleSize;
                    Debug.WriteLine("TITLE SIZE");
                }
            }
            if (_overwriter.TextColor != null)
            {
                this.TextColor = _overwriter.TextColor;
                Debug.WriteLine("TEXT COLOR");
            }
            if (_overwriter.TextSize != null)
            {
                if (_overwriter.TextSize > 0)
                {
                    this.TextSize = _overwriter.TextSize;
                    Debug.WriteLine("TEXT SIZE");
                }
            }
            if (_overwriter.PicHeight != null)
            {
                if (_overwriter.PicHeight > 0)
                {
                    this.PicHeight = _overwriter.PicHeight;
                    Debug.WriteLine("Pic Height");
                }
            }
            if (_overwriter.PicWidth != null)
            {
                if (_overwriter.PicWidth > 0)
                {
                    this.PicWidth = _overwriter.PicWidth;
                    Debug.WriteLine("Pic Width");
                }
            }

            Debug.WriteLine("Attempts Done");
        }
        private string GetTrimmedString(string _str)
        {
            string _string = _str;
            string[] txt = _string.Split(":");

            string _split = "";

            if (txt[1].EndsWith("px;"))
            {
                _split = "px;";
            }
            else
            {
                _split = ";";
            }
            txt = txt[1].Split(_split);

            return txt[0];
        }
    }
}

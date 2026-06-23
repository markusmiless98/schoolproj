namespace PublicDatabaseAPI.Models
{
    public class Links
    {
        public string _text { get; set; }
        public string _link { get; set; }

        public Links(string _txt, string _lnk)
        {
            _text = _txt;
            _link = _lnk;
        }
    }
}

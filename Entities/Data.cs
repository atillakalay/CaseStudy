namespace Entities
{
    public class Data
    {
        public string sectionType { get; set; }
        public string repeatType { get; set; }
        public int itemCountInRow { get; set; }
        public bool lazyLoadingEnabled { get; set; }
        public bool titleVisible { get; set; }
        public object title { get; set; }
        public object titleColor { get; set; }
        public string titleBgColor { get; set; }
        public string sectionBgColor { get; set; }
        public List<Item> itemList { get; set; }
    }
}

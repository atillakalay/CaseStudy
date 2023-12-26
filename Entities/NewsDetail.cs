namespace Entities
{
    public class NewsDetail
    {
        public string resource { get; set; }
        public string bodyText { get; set; }
        public bool hasPhotoGallery { get; set; }
        public bool hasVideo { get; set; }
        public string publishDate { get; set; }
        public string fullPath { get; set; }
        public string shortText { get; set; }
        public Category category { get; set; }
        public string itemId { get; set; }
        public string title { get; set; }
        public string video { get; set; }
        public string imageUrl { get; set; }
        public string itemType { get; set; }
    }
}

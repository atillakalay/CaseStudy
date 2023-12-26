using Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace TurkMedya_MVC.Controllers
{
    public class HaberController : Controller
    {
        private const int PageSize = 5; // Bir sayfada gösterilecek haber sayısı

        public async Task<IActionResult> Index(int? page, string categoryFilter, string keywordFilter)
        {
            ViewBag.CategoryFilter = categoryFilter; // View tarafında CategoryFilter değişkenini kullanmak için ViewBag'e atanıyor
            ViewBag.KeywordFilter = keywordFilter; // View tarafında KeywordFilter değişkenini kullanmak için ViewBag'e atanıyor

            // Belirli bir sayfadaki filtrelenmiş haberleri getiren metot çağrılıyor
            IPagedList<NewsItem> pagedFilteredNewsItems = await GetPagedFilteredNewsItems(page ?? 1, categoryFilter, keywordFilter);

            return View(pagedFilteredNewsItems); // Filtrelenmiş haberlerin olduğu sayfaya dönüş yapılıyor
        }


        private async Task<IPagedList<NewsItem>> GetPagedFilteredNewsItems(int pageNumber, string categoryFilter, string keywordFilter)
        {
            List<NewsItem> result = new List<NewsItem>();  // Haberlerin listesi için bir List oluşturuluyor

            using (HttpClient client = new HttpClient()) // HttpClient kullanarak bir HTTP istemcisi oluşturuluyor
            {
                HttpResponseMessage response = await client.GetAsync("https://www.turkmedya.com.tr/anasayfa.json");  // Belirtilen URL'den veri alınıyor

                if (response.IsSuccessStatusCode) // İstek başarılı olduysa devam ediliyor
                {
                    string jsonString = await response.Content.ReadAsStringAsync(); // Cevap JSON içeriğe dönüştürülüyor
                    var jsonObject = JsonConvert.DeserializeObject<RootObject>(jsonString); // JSON verisi RootObject tipine dönüştürülüyor


                    if (jsonObject != null && jsonObject.data != null)  // Veri varsa devam ediliyor
                    {
                        foreach (var dataItem in jsonObject.data) // JSON verisindeki veri öğeleri döngüyle alınıyor
                        {
                            foreach (var item in dataItem.itemList) // Her veri öğesindeki haberler alınıyor
                            {
                                // Alınan haberler NewsItem tipine dönüştürülerek listeye ekleniyor
                                NewsItem newsItem = new NewsItem
                                {
                                    hasPhotoGallery = item.hasPhotoGallery,
                                    hasVideo = item.hasVideo,
                                    titleVisible = item.titleVisible,
                                    fLike = item.fLike,
                                    publishDate = item.publishDate,
                                    shortText = item.shortText,
                                    fullPath = item.fullPath,
                                    category = new Category
                                    {
                                        categoryId = item.category.categoryId,
                                        title = item.category.title,
                                        slug = item.category.slug
                                    },
                                    videoUrl = item.videoUrl,
                                    externalUrl = item.externalUrl,
                                    columnistName = item.columnistName,
                                    itemId = item.itemId,
                                    title = item.title,
                                    imageUrl = item.imageUrl,
                                    itemType = item.itemType
                                };
                                // Listeye haber ekleme işlemi gerçekleştiriliyor.
                                result.Add(newsItem);
                            }
                        }
                    }
                }
            }

            // Elde edilen haber listesi filtreleniyor
            categoryFilter = categoryFilter?.ToLowerInvariant(); // categoryFilter'ı küçük harfe dönüştür

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                // Kategoriye göre filtreleme yapılıyor
                result = result.Where(news => news.category.title.ToLowerInvariant() == categoryFilter).ToList();
            }

            if (!string.IsNullOrEmpty(keywordFilter))
            {
                // Anahtar kelimeye göre filtreleme yapılıyor
                result = result.Where(news => news.title.Contains(keywordFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            // Sayfalama işlemi yapılıyor ve sonuç döndürülüyor
            return result.ToPagedList(pageNumber, PageSize);
        }
    }
}

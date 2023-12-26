using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    // DataAccess isim alanında AppDbContext adında bir sınıf tanımlanıyor ve DbContext sınıfından kalıtım alıyor
    public class AppDbContext : DbContext
    {
        // Haber öğeleri tablosunu temsil eden DbSet
        public DbSet<NewsItem> NewsItems { get; set; }

        // Kategoriler tablosunu temsil eden DbSet
        public DbSet<Category> Categories { get; set; }

        // AppDbContext sınıfı, DbContext sınıfının parametreli bir yapıcı metodunu kullanarak türetiliyor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}

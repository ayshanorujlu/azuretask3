using AzureStorageLibrary.Models;

namespace MVCWebApp.Models
{
    public class HomeViewModel
    {
        public Product Product { get; set; }
        public Store Store { get; set; }
        public List<Store>? Stores { get; set; }
        public List<Product>? Products { get; set; }
    }
}

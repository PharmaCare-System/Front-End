using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PharmaCare.Models.Product;

namespace PharmaCare.Controllers
{
    public class ProductsController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _env;

        public ProductsController(HttpClient httpClient, IWebHostEnvironment env)
        {
            _httpClient = httpClient;
            _env = env;
        }
            

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<ProductReadVM>>("https://localhost:44350/api/Products");

            // Optional: get category names if needed
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryVM>>("https://localhost:44350/api/Categories");
            ViewBag.Categories = categories.ToDictionary(c => c.Id, c => c.Name);

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create() { 
        
        return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductAddVM product)
        {
            if (!ModelState.IsValid)
                return View(product);
            var response = await _httpClient.PostAsJsonAsync("https://localhost:44350/api/Products", product);
            if (!response.IsSuccessStatusCode)
                ModelState.AddModelError("", "Error creating product.");
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<ProductReadVM>($"https://localhost:44350/api/Products/{id}");
            if (product == null)
               return NotFound();

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<ProductReadVM>($"https://localhost:44350/api/Products/{id}");
            var UpdateModel = new ProductUpdateVM
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                ImageUrl = product.ImageUrl,
                BulkAllowed = product.BulkAllowed,
                PrescriptionRequired = product.PrescriptionRequired,
                InventoryId = product.InventoryId,
                CategoryId = product.CategoryId
            };
            return View(UpdateModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateVM product)
        {
            if (!ModelState.IsValid)
                return View(product);
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:44350/api/Products/{product.Id}", product);
            if (!response.IsSuccessStatusCode)
                ModelState.AddModelError("", "Error updating product.");
            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:44350/api/Products/{id}");
            if (!response.IsSuccessStatusCode)
                ModelState.AddModelError("", "Error deleting product.");

            return RedirectToAction(nameof(Index));
        }
    }
}

using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PharmaCare.Models.Categories;
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

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create() { 
        
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryReadVM>>("https://localhost:44350/api/Categories");
            var inventories = await _httpClient.GetFromJsonAsync<List<PharmaCare.Models.Inventory.InventoryReadVM>>("https://localhost:44350/api/Inventory ");
            ViewBag.Inventories = new SelectList(inventories, "Id", "Name");
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
            try
            {

                var product = await _httpClient.GetFromJsonAsync<ProductReadVM>($"https://localhost:44350/api/Products/{id}");
                return View(product);

            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    stackTrace = ex.StackTrace,
                    innerex = ex.InnerException

                });
            }

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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<ProductReadVM>($"https://localhost:44350/api/Products/{id}");
            var Inventories = await _httpClient.GetFromJsonAsync<List<PharmaCare.Models.Inventory.InventoryReadVM>>("https://localhost:44350/api/Inventory ");
            ViewBag.Inventories = new SelectList(Inventories, "Id", "Name");
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



    }
}

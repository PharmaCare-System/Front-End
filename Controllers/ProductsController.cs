using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PharmaCare.Models.Categories;
using PharmaCare.Models.Inventory;
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


        public async Task<IActionResult> Index(int? categoryId)
        {

            var categories = await _httpClient.GetFromJsonAsync<List<CategoryReadVM>>("https://localhost:44350/api/Categories");
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName", categoryId);

            List<ProductReadVM> products;

            if (categoryId.HasValue)
            {
                var CategoryProducts = await _httpClient.GetFromJsonAsync<CategoryProducts>($"https://localhost:44350/api/Categories/{categoryId}/Products");
                products = CategoryProducts.Products.ToList();

            }
            else
            {
                products = await _httpClient.GetFromJsonAsync<List<ProductReadVM>>("https://localhost:44350/api/Products");
            }

            return View(products);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var categories = await _httpClient.GetFromJsonAsync<List<CategoryReadVM>>("https://localhost:44350/api/Categories");
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            var inventories = await _httpClient.GetFromJsonAsync<List<PharmaCare.Models.Inventory.InventoryReadVM>>("https://localhost:44350/api/Inventory ");
            ViewBag.Inventories = new SelectList(inventories, "Id", "Name");

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductWithImage product)
        {
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryReadVM>>("https://localhost:44350/api/Categories");
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            var inventories = await _httpClient.GetFromJsonAsync<List<PharmaCare.Models.Inventory.InventoryReadVM>>("https://localhost:44350/api/Inventory ");
            ViewBag.Inventories = new SelectList(inventories, "Id", "Name");

            if (!ModelState.IsValid)
                return View(product);
            if (product.Image != null)
            {
                string imageroot = _env.WebRootPath;
                string imagename = product.Image.FileName;
                string imagepath = Path.Combine(imageroot, "Images", "Products", imagename);
                using (var stream = System.IO.File.Create(imagepath))
                {
                    await product.Image.CopyToAsync(stream);
                }
                product.ImageURL = Path.Combine("/Images/Products", imagename).Replace("\\", "/");

            }
            var ProductModel = new ProductAddVM
            {
                Name = product.Name,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                ExpiryDate = product.ExpiryDate,
                BarCode = product.BarCode,
                BulkAllowed = product.BulkAllowed,
                PrescriptionRequired = product.PrescriptionRequired,
                CategoryId = product.CategoryId,
                ImageURL = product.ImageURL,
                InventoryId = product.InventoryId

            };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:44350/api/Products", ProductModel);
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
            catch (Exception ex)
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

            var categories = await _httpClient.GetFromJsonAsync<List<CategoryReadVM>>("https://localhost:44350/api/Categories");
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");

            var inventories = await _httpClient.GetFromJsonAsync<List<InventoryReadVM>>("https://localhost:44350/api/Inventory");
            ViewBag.Inventories = new SelectList(inventories, "Id", "Name");
            var productUpdate = new ProductUpdateVM
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                BulkAllowed = product.BulkAllowed,
                PrescriptionRequired = product.PrescriptionRequired,
                ImageURL = product.ImageUrl, 
                InventoryId = product.InventoryId,
                CategoryId = product.CategoryId
            };

            return View(productUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateVM model)
        {
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryReadVM>>("https://localhost:44350/api/Categories");
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");

            var inventories = await _httpClient.GetFromJsonAsync<List<InventoryReadVM>>("https://localhost:44350/api/Inventory");
            ViewBag.Inventories = new SelectList(inventories, "Id", "Name");
            if (!ModelState.IsValid)

                return View(model);


            var response = await _httpClient.PutAsJsonAsync($"https://localhost:44350/api/Products/{model.Id}", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error updating product.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }



    }
}


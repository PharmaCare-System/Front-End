using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PharmaCare.Models.Inventory;

namespace PharmaCare.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly HttpClient _httpClient;
        public InventoriesController(HttpClient httpClient)
        {
            _httpClient = httpClient;   
        }
        public async Task< IActionResult> Index()
        {
            var inventories = await _httpClient.GetFromJsonAsync<List<PharmaCare.Models.Inventory.InventoryReadVM>>("https://localhost:44350/api/Inventory ");
            return View(inventories);
        }
        public async Task< IActionResult> Create()
        {
            var pharmacies = await _httpClient.GetFromJsonAsync<List<PharmacyReadVm>>("https://localhost:44350/api/Pharmacy ");
            ViewBag.Pharmacies = new SelectList(pharmacies, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PharmaCare.Models.Inventory.InventoryAddVM inventory)
        {
            if (!ModelState.IsValid)
            {
                return View(inventory);
            }
            var response = await _httpClient.PostAsJsonAsync("https://localhost:44350/api/Inventory", inventory);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Something went wrong while creating the inventory.");
            return View(inventory);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var pharmacies = await _httpClient.GetFromJsonAsync<List<PharmacyReadVm>>("https://localhost:44350/api/Pharmacy ");
            ViewBag.Pharmacies = new SelectList(pharmacies, "Id", "Name");
            var response = await _httpClient.GetFromJsonAsync<PharmaCare.Models.Inventory.InventoryReadVM>($"https://localhost:44350/api/Inventory/{id}");
          
            var updateModel = new PharmaCare.Models.Inventory.InventoryUpdateVM
            {
                Id = response.Id,
                Name = response.Name,
                Location = response.Location,
                PharmacyId = response.PharmacyId
            };
            return View(updateModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PharmaCare.Models.Inventory.InventoryUpdateVM inventory)
        {
            var pharmacies = await _httpClient.GetFromJsonAsync<List<PharmacyReadVm>>("https://localhost:44350/api/Pharmacy ");
            ViewBag.Pharmacies = new SelectList(pharmacies, "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View(inventory);
            }
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:44350/api/Inventory/{inventory.Id}", inventory);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Something went wrong while updating the inventory.");
            return View(inventory);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<PharmaCare.Models.Inventory.InventoryReadVM>($"https://localhost:44350/api/Inventory/{id}");
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:44350/api/Inventory/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Failed to delete category.");
            return RedirectToAction(nameof(Index));
        }
    }
}

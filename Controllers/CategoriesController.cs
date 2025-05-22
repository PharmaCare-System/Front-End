    using Microsoft.AspNetCore.Mvc;
    using PharmaCare.Models.Categories;

    namespace PharmaCare.Controllers
    {
        public class CategoriesController : Controller
        {
            private readonly HttpClient _httpClient;
            public CategoriesController(HttpClient httpClient)
            {
                _httpClient = httpClient;

            }


            public async  Task <IActionResult> Index()
            {
                var Categories = await _httpClient.GetFromJsonAsync<List<PharmaCare.Models.Categories.CategoryReadVM>>("https://localhost:44350/api/Categories ");
                return View(Categories);
            }
            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> Create(CategoryAddVM category)
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }

                var response = await _httpClient.PostAsJsonAsync("https://localhost:44350/api/Categories", category);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                // Optional: Add model error if the response failed
                ModelState.AddModelError("", "Something went wrong while creating the category.");
                return View(category);
            }

            [HttpGet]
            public async Task<IActionResult> Edit(int id)
            {
                var response = await _httpClient.GetFromJsonAsync<CategoryReadVM>($"https://localhost:44350/api/Categories/{id}");
                if (response == null)
                {
                    return NotFound();
                }

                var updateModel = new CategoryUpdateVM
                {
                    Id = response.Id,
                    CategoryName = response.CategoryName,
                    IsActive = response.IsActive


                };

                return View(updateModel);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(CategoryUpdateVM model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var response = await _httpClient.PutAsJsonAsync($"https://localhost:44350/api/Categories/{model.Id}", model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to update category.");
                return View(model);
            }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _httpClient.GetFromJsonAsync<CategoryReadVM>($"https://localhost:44350/api/Categories/{id}");
            if (category == null)
            {
                return NotFound();
            }
            return View(category);  
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:44350/api/Categories/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Failed to delete category.");
            return RedirectToAction(nameof(Index));
        }


    }
}

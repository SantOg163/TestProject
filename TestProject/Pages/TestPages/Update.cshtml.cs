using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestProject.Models;

namespace TestProject.Pages.TestPages
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public ServiceObject serviceObject { get; set; }
        private readonly DataService _service;
        public UpdateModel(DataService service)
        {
            _service= service;
        }
        public async void OnGet(int id)
        {
            serviceObject = _service.GetJsonObject(id).Result;
        }
 
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                int id = await  _service.UpdateObject(serviceObject);
                return RedirectToPage($"View", new { id = id });
            }
            return RedirectToPage();
        }
    }
}

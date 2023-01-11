using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestProject.Models;

namespace TestProject.Pages.TestPages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ServiceObject serviceObject { get; set; }
        private readonly DataService _service;
        public CreateModel(DataService service)
        {
            _service = service;
        }
        public async Task<IActionResult> OnPost()
        {
            serviceObject.ID = 1;
            if (ModelState.IsValid)
            {
                int id= await _service.Create(serviceObject);
                return RedirectToPage($"View", new {id=id});

            }
            return RedirectToPage();
            
        }
    }
}

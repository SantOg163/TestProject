using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestProject.Models;

namespace TestProject.Pages.TestPages
{
    public class BookingModel : PageModel
    {

        [BindProperty]
        public ServiceBooking serviceBooking { get; set; } = new ServiceBooking();
        private readonly DataService _service;
        public BookingModel(DataService service)
        {
            _service = service;
        }
        public async void OnGet(int id)
        {
            serviceBooking.ObjectID = id;
        }

        public async Task<IActionResult> OnPost()
        {
            serviceBooking.ID = 1;
            if (ModelState.IsValid)
            {
                //не уверен, на какую страницу это можно деть
                BookingResult booking = await _service.Booking(serviceBooking);
                return RedirectToPage($"View", new { id = serviceBooking.ObjectID });

            }
            return RedirectToPage();
        }
    }
}

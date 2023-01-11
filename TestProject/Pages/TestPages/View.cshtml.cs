using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestProject.Models;

namespace TestProject.Pages.TestPages
{
    public class ViewModel : PageModel
    {
        [BindProperty]
        public ServiceObject serviceObject { get; set; }
        public List<ServiceBooking> serviceBookings { get; set; }
        List<BookingModel> bookings { get; set; } = new List<BookingModel>();
        private readonly DataService _service;
        public ViewModel(DataService service)
        {
            _service = service;
            serviceBookings = new List<ServiceBooking>();
        }
        public async Task OnGet(int id)
        {
            serviceObject =  _service.GetJsonObject(id).Result;
            serviceBookings = _service.GetBookingsByObjectId(id).Result;
        }
        public async Task<IActionResult> OnPost()
        {
           await _service.DeleteObject(serviceObject.ID);
            return RedirectToPage("/Index");
        }
    }
}

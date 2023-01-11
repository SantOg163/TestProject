using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TestProject.Models;

namespace TestProject.Pages
{
    public class IndexModel : PageModel
    {
        public List<ServiceObject> serviceObjects { get; set; }
        public List<ServiceBooking> serviceBookings { get; set; }
        private readonly DataService _service;
        public IndexModel(DataService service)
        {
            _service = service;
            serviceObjects = new List<ServiceObject>();
            serviceBookings = new List<ServiceBooking>();
        }

        public async void OnGet()
        {
            serviceObjects = _service.GetJsonAllObject().Result;
            serviceBookings = _service.GetAllBookings().Result;
        }
    }
}
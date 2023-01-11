using System.IO;
using System.Text.Json;
using TestProject.Models;

namespace TestProject
{
    public class DataService
    {
        HttpClient _client;
        public DataService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7009");
        }
        public async Task<List<ServiceObject>> GetJsonAllObject()
        {
            Stream json = await _client.GetStreamAsync("/services/GetAllObjects");
            return JsonSerializer.Deserialize<List<ServiceObject>>(new StreamReader(json).ReadToEnd());
        }
        public async Task<ServiceObject> GetJsonObject(int id)
        {
            Stream json = await _client.GetStreamAsync($"/services/GetObject/{id}");
            return JsonSerializer.Deserialize<ServiceObject>(new StreamReader(json).ReadToEnd());
        }
        public async Task<int> UpdateObject(ServiceObject serviceObject)
        {
            
            Stream json = await _client.GetStreamAsync($"/services/update/{serviceObject.ID}?name={serviceObject.Name}&amount={serviceObject.Amount}");
            return serviceObject.ID ;
        }
        public async Task DeleteObject(int id)
        {
            await _client.DeleteAsync($"/services/DeleteObject?id={id}");
        }
        public async Task<int> Create(ServiceObject serviceObject)
        {
            Stream id = await _client.GetStreamAsync($"/services/create?name={serviceObject.Name}&amount={serviceObject.Amount}");
            return Convert.ToInt32(new StreamReader(id).ReadToEnd());
        }
        public async Task<BookingResult> Booking(ServiceBooking serviceBooking)
        {
            Stream json = await _client.GetStreamAsync($"/services/booking?ObjectID={serviceBooking.ObjectID}&amount={serviceBooking.Amount}");
            return JsonSerializer.Deserialize<BookingResult>(new StreamReader(json).ReadToEnd());
        }
        public async Task<List<ServiceBooking>> GetBookingsByObjectId(int ObjectId)
        {
            Stream json = await _client.GetStreamAsync($"/services/GetBookingsByObjectId?ObjectId={ObjectId}");
            return JsonSerializer.Deserialize<List<ServiceBooking>>(new StreamReader(json).ReadToEnd());
        }
        public async Task<List<ServiceBooking>> GetAllBookings()
        {
            Stream json = await _client.GetStreamAsync($"/services/GetAllBookings");
            return JsonSerializer.Deserialize<List<ServiceBooking>>(new StreamReader(json).ReadToEnd());
        }
    }
}

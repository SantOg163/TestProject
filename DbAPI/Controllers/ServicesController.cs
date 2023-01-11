using DbAPI.Data;
using DbAPI.Models;
using DbAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Xml.Linq;

namespace DbAPI.Controllers
{
    [ApiController]
    [Route("services/")]
    public class ServicesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorsService _errorsService;
        public ServicesController(AppDbContext context)
        {
            _context = context;
            //объект не может передоваться в конструкторе (AddSingleton не работает)
            _errorsService = new ErrorsService();
            
        }
        [HttpGet]
        [Route("create")]
        public async Task<int> Create(string name, int amount)
        {
            int id;
            try
            {
                id = _context.Objects.ToList().Last().ID + 1;
            }
            catch { id = 1; }
            ServiceObject serviceObject = new ServiceObject() { ID = id, Name = name, Amount = amount };
            try
            {
                _context.Objects.Add(serviceObject);
                await _context.SaveChangesAsync();
                return id;
            }
            catch
            {
                _errorsService.AddError($"{DateTime.Now}: не удалось добавить запись в таблицу Objects \n{JsonSerializer.Serialize(serviceObject)}");
                return id;
            }
        }
        [HttpGet]
        [Route("update/{id:int}")]
        public async Task<int> Update(int id, string? name, int? amount)
        {
            ServiceObject serviceObject = _context.Objects.Where(o => o.ID == id).FirstOrDefault();
            if (serviceObject == null)
                return 0;
            if (name != null)
                serviceObject.Name = name;
            if (amount != null)
                serviceObject.Amount = amount ?? default(int);
            try
            {
                await _context.SaveChangesAsync();
                return id;
            }
            catch
            {
                _errorsService.AddError($"{DateTime.Now}: не удалось обновить запись в таблице Objects \n{JsonSerializer.Serialize(serviceObject)}");
                return id;
            }
        }
        [HttpGet]
        [Route("booking")]
        public async Task<BookingResult> Booking(int ObjectID, int amount)
        {
            ServiceObject serviceObject = _context.Objects.Where(o => o.ID == ObjectID).FirstOrDefault();
            BookingResult bookingResult = new BookingResult();
            ServiceBooking serviceBooking = new ServiceBooking() {ID=1 , ObjectID = ObjectID, Amount = amount };
            int id;
            try
            {
                id = _context.Bookings.ToList().Last().ID + 1;
            }
            catch { id = 1; }
            serviceBooking.ID = id;
            //валидация
            if (serviceObject == null)
            {
                bookingResult.Error = "Оборудование не найдено";
                return bookingResult;
            }
            if (amount > serviceObject.Amount)
            {
                bookingResult.Ok = false;
                bookingResult.Amount = serviceObject.Amount;
                bookingResult.Error += "превышено количество";
            }
            else
            {
                bookingResult.Ok = true;
                serviceObject.Amount -= amount;
                bookingResult.Amount = serviceObject.Amount;
            }
            try
            {
                _context.Bookings.Add(serviceBooking);
                await _context.SaveChangesAsync();
            }
            catch
            {
                _errorsService.AddError($"{DateTime.Now}: не удалось добавить запись в таблицу Bookings \n{JsonSerializer.Serialize(serviceBooking)}");
            }
            return bookingResult;
        }

        //добавлено для тестов
        [HttpGet]
        [Route("GetAllObjects")]
        public string GetAllObjects() => JsonSerializer.Serialize(_context.Objects.ToList());
        [HttpGet]
        [Route("GetObject/{id:int}")]
        public string GetObject(int id)
        {
            ServiceObject serviceObjet = _context.Objects.ToList().Where(o => o.ID == id).FirstOrDefault();
            try
            {
                ServiceObject serviceObject = _context.Objects.ToList().Where(o => o.ID == id).FirstOrDefault();
                return JsonSerializer.Serialize(serviceObject);
            }
            catch
            {
                return "object not found";
            }
        }
        [HttpDelete]
        [Route("DeleteObject")]
        public async Task DeleteObject(int id)
        {
            try
            {
                _context.Objects.Remove(_context.Objects.Where(o => o.ID == id).FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            catch
            {
                _errorsService.AddError($"{DateTime.Now}: не удалось удалить запись в таблице Objects \nid={id}");
            }
        }

        [HttpGet]
        [Route("GetAllBookings")]
        public string GetAllBookings() => JsonSerializer.Serialize(_context.Bookings.ToList());
        [HttpGet]
        [Route("GetBookings")]
        public string GetBookings(int id)
        {
            try
            {
                return JsonSerializer.Serialize(_context.Bookings.ToList().Where(o => o.ID == id));
            }
            catch
            {
                return "bookings not found";
            }
        }
        [HttpGet]
        [Route("GetBookingsByObjectId")]
        public string GetBookingsByObjectId(int ObjectId)
        {
            try
            {
                return JsonSerializer.Serialize(_context.Bookings.ToList().Where(o => o.ObjectID == ObjectId));
            }
            catch
            {
                return "bookings not found";
            }
        }
        [HttpDelete]
        [Route("DeleteBooking")]
        public async Task<int> DeleteBooking(int id)
        {
            try
            {
                _context.Objects.Remove(_context.Objects.Where(o => o.ID == id).FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            catch
            {
                _errorsService.AddError($"{DateTime.Now}: не удалось удалить запись в таблице Objects \nid={id}");
            }
            return id;
        }

    }
}

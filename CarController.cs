using Microsoft.AspNetCore.Mvc;
using RentCar.Web.Data;
using RentCar.Web.Models.ViewModels;

namespace RentCar.Web.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PAGE_SIZE = 5;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult List(DateTime rentalDate, DateTime returnDate, int page = 1)
        {
            if (rentalDate >= returnDate)
            {
                ViewBag.Error = "Tanggal kembali harus lebih besar dari tanggal sewa";
                return View(new List<CarListViewModel>());
            }

            var query = _context.MsCars
                .Where(m => !_context.TrRentals.Any(t =>
                    t.CarId == m.CarId &&
                    rentalDate < t.ReturnDate &&
                    returnDate > t.RentalDate
                ));

            int totalData = query.Count();
            int totalPage = (int)Math.Ceiling(totalData / (double)PAGE_SIZE);
        
            var cars = query
                .Skip((page - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .Select(m => new CarListViewModel
                {
                    CarId = m.CarId,
                    Name = m.Name,
                    Model = m.Model,
                    PricePerDay = m.PricePerDay
                })
                .ToList();

            ViewBag.Page = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.RentalDate = rentalDate;
            ViewBag.ReturnDate = returnDate;

            return View(cars);
        }
    }
}

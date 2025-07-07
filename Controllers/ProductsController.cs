using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TestApps.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using TestApps.Models;


namespace TestApps.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        [AllowAnonymous]
        public IActionResult Create() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(product);

            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

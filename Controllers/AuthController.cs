using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestApps.Data;      
using TestApps.Services;
public class AuthController : Controller
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwt;
    private readonly ILogger<AuthController> _logger;

    public AuthController(AppDbContext context, JwtService jwt, ILogger<AuthController> logger)
    {
        _context = context;
        _jwt = jwt;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string username, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            ModelState.AddModelError("", "Username already exists.");
            return View();
        }
        var user = new User
        {
            Username = username,
            PasswordHash = ComputeSha256Hash(password)
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        _logger.LogInformation("New user registered: {username}", username);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null || user.PasswordHash != ComputeSha256Hash(password))
        {
            ModelState.AddModelError("", "Invalid credentials.");
            return View();
        }
        var token = _jwt.GenerateToken(user.Username);
        Response.Cookies.Append("jwt", token);
        return RedirectToAction("Index", "Products");
    }

    private string ComputeSha256Hash(string rawData)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Convert.ToBase64String(bytes);
    }
}

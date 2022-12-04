using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCStoreData;
using MVCStoreWeb.Models;
using NETCore.MailKit.Core;
using System.Diagnostics;
using System.Security.Claims;

namespace MVCStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IShoppingCartService shoppingCartService;

        public HomeController(
            ILogger<HomeController> logger,
            IEmailService emailService,
            IConfiguration configuration,
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IShoppingCartService shoppingCartService
            )
        {
            _logger = logger;
            this.emailService = emailService;
            this.configuration = configuration;
            this.context = context;
            this.userManager = userManager;
            this.shoppingCartService = shoppingCartService;
        }

        [ResponseCache(Duration = 86400)]
        public async Task<IActionResult> Index()
        {
            ViewBag.Featured = await context.Products.OrderBy(p => p.DiscountedPrice).Take(4).ToListAsync();
            ViewBag.BestSeller = await context.Products.OrderByDescending(p => p.OrderItems.Count).Take(4).ToListAsync();
            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                var categories = user.Orders.SelectMany(p => p.OrderItems).SelectMany(p => p.Product.Categories).Select(p => p.Id).Distinct();
                ViewBag.ShowCase = await context.Products.OrderBy(p => p.Id).Take(12).ToListAsync();
            }
            else
            {
                ViewBag.ShowCase = await context.Products.OrderBy(p => p.Id).Take(12).ToListAsync();
            }
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactUsViewModel model)
        {
            await emailService.SendAsync(
                configuration.GetValue<string>("EMailSettings:SenderEmail"),
                $"Ziyaretçi Mesajı ({model.Name})",
                $"Gönderen: \t{model.Name}\nTel: \t\t{model.PhoneNumber ?? "Tel. Belirtilmemiş"}\nE-Posta: \t{model.EMail}\nMesaj:\n------\n{model.Message}"
                );
            TempData["messageSent"] = true;
            return View(new ContactUsViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Category(Guid id, int? page)
        {
            var model = await context.Categories.FindAsync(id);
            ViewBag.Page = page;
            return View(model);
        }

        [HttpGet]

        public async Task<IActionResult> Product(Guid id)
        {
            var model = await context.Products.FindAsync(id);
            ViewBag.CreditCards = new[]
            {
                new CreditCardViewModel
                {
                    Code = "bonus",
                    Installments =  new[] {
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1.06m, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1.15m, Exists = true },
                    }.ToList()
                },
                new CreditCardViewModel
                {
                    Code = "world",
                    Installments =  new[] {
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1.052m, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1.15m, Exists = true },
                    }.ToList()
                },
                new CreditCardViewModel
                {
                    Code = "maximum",
                    Installments =  new[] {
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1.06m, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1.15m, Exists = true },
                    }.ToList()
                },
                new CreditCardViewModel
                {
                    Code = "axess",
                    Installments =  new[] {
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1.06m, Exists = true },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1, Exists = false },
                        new InstallmentViewModel { Rate = 1.15m, Exists = true },
                    }.ToList()
                },
            };
            return View(model);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> AddToCart(Guid id)
        {
            await shoppingCartService.AddToCart(id);
            TempData["addedToCart"] = true;
            return Redirect(Request.Headers["Referer"].ToString());
        }


        [HttpGet, Authorize]
        public async Task<IActionResult> RemoveFromCart(Guid id)
        {
            await shoppingCartService.RemoveFromCart(id);
            return RedirectToAction("ShoppingCart", "Account");
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> RemoveAllFromCart(Guid id)
        {
            await shoppingCartService.RemoveAllFromCart(id);
            return RedirectToAction("ShoppingCart", "Account");
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> ClearCart()
        {
            await shoppingCartService.ClearCart();
            return RedirectToAction("ShoppingCart", "Account");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
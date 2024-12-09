using Chat.Data.Entities;
using Chat.Services.Interfaces;
using Chat.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Chat.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConnectionService _connectionService;
        private readonly IMessagesService _messagesService;

        public HomeController(UserManager<ApplicationUser> userManager,ILogger<HomeController> logger , SignInManager<ApplicationUser> signInManager , IConnectionService connectionService , IMessagesService messagesService)
        {
            _connectionService = connectionService;
            _messagesService = messagesService;
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? ActiveContactId)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var ActiveUser =await _userManager.FindByIdAsync(userId);
            var AllConnections = _connectionService.getAllContacts(userId).Result;
            TempData["Contacts"] = AllConnections;
            TempData["User"] = ActiveUser;
            if (ActiveContactId != null)
            { 
                TempData["ActiveConnection"] = AllConnections.FirstOrDefault(c => c.ConnectionId == Guid.Parse(ActiveContactId));
                var data = await _messagesService.GetChatMessgages(Guid.Parse(ActiveContactId));
                return View(data);
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddnewContact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddnewContact(CheckPhoneNumberViewModel input)
        {
            if (ModelState.IsValid)
            {

                var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _connectionService.AddNewContact(input.PhoneNumber, userId);
                if (result == "done")
                {
                    var ConnectionId = _connectionService.getAllContacts(userId).Result.FirstOrDefault()?.ConnectionId;
                    return RedirectToAction("index", new { ActiveContactId = result.ToString() });
                }
                else if(result == "No User Found")
                {
                    TempData["Message"] = "This User is not Found";
                    return View(input);
                }
                else
                {
                    return RedirectToAction("index", new { ActiveContactId = result.ToString() });

                }
            }
            return View(input);
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LogIn", "Auth");
        }
    }
}

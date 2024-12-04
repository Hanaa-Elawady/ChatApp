using Chat.Data.Entities;
using Chat.Services.Helper;
using Chat.Services.Interfaces;
using Chat.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Web.Controllers
{
    public class AuthController : Controller
    {
        #region DI
        private readonly ISMSServices _smsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(ISMSServices smsService , UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
            _smsService = smsService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion

        #region Register 

        #region Phone

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CheckPhoneNumberViewModel input)
        {
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByNameAsync(input.PhoneNumber);
                //if (user is null)
                //{

                Random random = new Random();
                int randomNumber = random.Next(100000, 1000000);
                int Code = randomNumber;
                var sms = new SMS
                {
                    PhoneNumber = input.PhoneNumber,
                    Body = Code.ToString()
                };

                _smsService.SendSms(sms);
                TempData["VerificationCode"] = Code;
                TempData["PhoneNumber"] = input.PhoneNumber;
                return RedirectToAction("CheckVarificationCode");
                //}
                //else
                //{
                //    return BadRequest("userRepeated");
                //}
            }
            return View(input);
        }
        #endregion

        #region Varification Code
        [HttpGet]
        public IActionResult CheckVarificationCode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckVarificationCode(VarificationViewModel input)
        {
            if (ModelState.IsValid)
            {
                var storedCode = TempData["VerificationCode"];
                if (storedCode != null && (storedCode.ToString() == input.Code))
                {
                    return RedirectToAction("ContinueRegister");
                }
            }
            return View(input);
        }
        #endregion

        #region finish Register


        [HttpGet]
        public IActionResult ContinueRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContinueRegister(RegisterViewModel input)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    PhoneNumber = input.PhoneNumber,
                    DisplayName = input.DisplayName,
                    UserName = input.PhoneNumber,
                    ProfilePicture = DocumentSettings.UploadFile(input.ProfilePicture, "Images")
                };
                var result = await _userManager.CreateAsync(user, input.Password);

                if (result.Succeeded)
                    return RedirectToAction("LogIn");
            }
            return View(input);
        }
        #endregion
        #endregion

        #region Login
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel input)
        {
            if (ModelState.IsValid)
            {
                var user =await  _userManager.FindByNameAsync(input.PhoneNumber);
                if(user is not null)
                {
                    var checkPass = await _userManager.CheckPasswordAsync(user, input.Password);
                    if (checkPass)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, input.Password, true, true);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(input);
        }

        #endregion

    }
}

using System.Collections.Generic;
//using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
 using Microsoft.AspNet.Identity;
 using Project33.Models; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Project33.Data;
using Project33.Services.Models;

 namespace Project33.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return Content(User.Identity.Name);
        }
        
        private UserContext db;
        
        public AccountController(UserContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Edit()
        {
            var userName = User.Identity.GetUserName();
            User user = await db.Users.FirstOrDefaultAsync(x => x.Login == userName);
            
            EditViewModel model = new EditViewModel {Login = user.Login, Age = user.Age};
            return View(model);
        }
 
        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.GetUserName();
                User user = await db.Users.FirstOrDefaultAsync(x => x.Login == userName);
                
                if (model.OldPassword != user.Password && model.OldPassword != null)
                {
                    ModelState.AddModelError("", "Пароль введен неверно");
                    return View(model);
                }

                if (model.OldPassword == null && model.NewPassword != null)
                {
                    ModelState.AddModelError("", "Для смены пароля введите старый пароль");
                    return View(model);
                }
                
                /*if (model.OldPassword == user.Password && model.NewPassword == null)
                {
                    ModelState.AddModelError("", "Пароль не может быть равен ничего!");
                    return View(model);
                }*/

                User userAlreadyExist = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (userAlreadyExist == null || model.Login == user.Login)
                {
                    user.Login = model.Login;
                    user.Age = model.Age;

                    if (model.NewPassword != null)
                    {
                        user.Password = model.NewPassword;
                    }
                    
                    await db.SaveChangesAsync();

                    await Authenticate(model.Login);

                    return RedirectToAction("Index", "Books");
                }
                ModelState.AddModelError("", "Этот логин уже занят");
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Register()
        { 
            return View();
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            { 
                User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (user == null)
                    {
                        // добавляем пользователя в бд
                        db.Users.Add(new User {Login = model.Login, Email = model.Email, Age = model.Age, Password = model.Password,});
                        
                        await db.SaveChangesAsync();

                        await Authenticate(model.Email); // аутентификация

                        return RedirectToAction("Index", "Books");
                    }
                    ModelState.AddModelError("", "Эта почта уже занята");
                }
                else
                    ModelState.AddModelError("", "Этот логин уже занят");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Login(LoginViewModel model)
        { 
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                { 
                    await Authenticate(model.Login); // аутентификация
                    
                    return RedirectToAction("Index", "Books");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

            public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Books");
        }
        
        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
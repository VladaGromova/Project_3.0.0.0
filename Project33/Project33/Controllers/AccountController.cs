using System.Collections.Generic;
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

                    return RedirectToAction("IndexForUsers", "Books");
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

                        return RedirectToAction("IndexForUsers", "Books");
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
            [HttpGet]
            public IActionResult ProfilePageFromLayout()
            {
                //return RedirectToAction("ProfilePage", "Account");
                return RedirectToAction("Login", "Account");
                
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
     
                        return RedirectToAction("IndexForUsers", "Books");
                    }
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
                return View(model);
            }
    
        /*[HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = 
                    await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Books");
        }*/
        
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
/*

        public async Task<IActionResult> ProfilePage()
        {
            UserContext user_db = new UserContext();
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName); // UserId found
            
            LikesContext db_likes = new LikesContext();
            FavoritesContext db_favors = new FavoritesContext();
            
            //LikesService likesService =new LikesService();
            
            var likes = from l in db_likes.Likes select l;
            likes = likes.Where(l => l.user_id.Contains(user.Id));
            var likes_list = likes.ToList();

            var favors = from f in db_favors.Favorites select f;
            favors = favors.Where(f => f.user_id.Contains(user.Id));
            var favors_list = favors.ToList();
            
            UserActionsInfo userActionsInfo = new UserActionsInfo()
            {
                username = user.Login,
                likes = likes_list,
                favors = favors_list
            };
            
            return View(userActionsInfo);
        }
        */
    }
}
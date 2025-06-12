using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPortfolioWebApp.Models;

namespace MyPortfolioWebApp.Controllers
{
    public class AccountController : Controller
    {
        // ASP.NET Core Identity 필요한 변수
        private readonly UserManager<CustomUser> userManager;
        private readonly SignInManager<CustomUser> signInManager;

        // 생성자
        public AccountController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            // userManager나 signInManager가 null인 경우 예외를 발생시킴
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        // NewsController GET Create(), POST Create()와 동일하게 생각
        [HttpGet] // [HttpGet]가 default이므로 생략 가능
        public IActionResult Register()
        {
            return View(); // Register.cshtml 뷰를 반환
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CustomUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    City = model.City,
                    Mobile = model.Mobile,
                    Hobby = model.Hobby,
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 위의 저장한 유저로 로그인, isPersistent 로그인상태 유지
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home"); // 회원가입 후 홈으로 이동
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description); // 에러 메시지를 모델 상태에 추가
                }

            }
            return View(model); // 회원가입 오류가나면 다시 회원가입화면으로 돌아감
        }


        [HttpGet]

        public IActionResult Login()
        {
            return View(); // Login.cshtml 뷰를 반환
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); // 로그인 성공시 홈으로 이동
                }
                ModelState.AddModelError(string.Empty, "로그인 실패"); // 로그인 실패시 에러 메시지 추가
            }
            return View(model); // 로그인 오류가나면 다시 로그인화면으로 돌아감
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync(); // 로그아웃
            return RedirectToAction("Index", "Home"); // 로그아웃 후 홈으로 이동
        }


    }
}

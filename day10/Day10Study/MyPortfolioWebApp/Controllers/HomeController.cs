using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebApp.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace MyPortfolioWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; // DB연동

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> About()
        {
            // 정적 HTML을 DB 데이터로 동적처리
            // DB에서 데이터를 불러온 뒤 About, Skill 객체에 데이터 담아서 뷰로 넘겨줌
            var skillCount = _context.Skill.Count();
            var skill = await _context.Skill.ToListAsync();
            // FirstAsync는 데이터가 없으면 예외발생. FirstOrDefaultAsync 데이터가 없으면 널값
            var about = await _context.About.FirstOrDefaultAsync(); 

            ViewBag.SkillCount = skillCount; // ex. 7이 넘어감
            ViewBag.ColNum = (skillCount / 2) + (skillCount % 2); // 3(7/2) + 1(7%2)

            var model = new AboutModel();
            model.About = about;
            model.Skill = skill;

            return View(model);
        }
        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Contact(ContactModel model)
        {
            if(ModelState.IsValid) // 모델에 들어간 네개의 값이 제대로 들어갔다면
            {
                try
                {
                    var smtpClient = new SmtpClient("smtp.gamil.com") // Gmail을 사용하면
                    {
                        Port = 465, // 메일 SMTP 서비스포트 변경필요
                        Credentials = new NetworkCredential("fake4131@naver.com", "비밀번호노출"),
                        EnableSsl = true, // SSL 사용
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(model.Email), // 보내는 사람 문의하기에 작성한 메일주소
                        Subject = model.Subject ?? "[제목없음]", // 제목
                        Body = $"보낸사람 : {model.Name} 이메일 : {model.Email} 내용 : {model.Message}",
                        IsBodyHtml = false, // 메일 본문에 HTML 형식으로 메일 내용 작성
                    };

                    mailMessage.To.Add("fake5378@naver.com"); // 받을 메일 주소

                    await smtpClient.SendMailAsync(mailMessage); // 위 생성된 메일을 비동기 메일 전송

                }
                catch (Exception ex)
                {
                    ViewBag.Success = false;
                    ViewBag.Error = $"메일전송 실패! {ex.Message}";
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

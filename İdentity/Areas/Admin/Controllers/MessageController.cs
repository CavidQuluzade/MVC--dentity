using İdentity.Areas.Admin.Models.Message;
using İdentity.Data;
using İdentity.Data.Entities;
using İdentity.Utilities.EmailHandler.Abstract;
using İdentity.Utilities.EmailHandler.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;

namespace İdentity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MessageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        public MessageController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = new MessageIndexVM
            {
                Messages = _context.Messages.ToList()
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult Send()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Send(MessageSendVM model)
        {
            if (!ModelState.IsValid) return View(model);
            string link = Url.Action("Index", "Home", new { area = "" }, Request.Scheme);

            var users = _context.Users.Where(x => x.isSubscribed == true).ToList();
            foreach (var user in users)
            {
                _emailService.SendEmail(new Utilities.EmailHandler.Models.Message(new List<string> { user.Email }, model.Tittle, model.Description + " " + link));
            }
            var message = new Data.Entities.Message
            {
                CreatedAt = DateTime.Now,
                Description = model.Description,
                Tittle = model.Tittle
            };
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Emails;

namespace TARge25Shop.Controllers

{
    public class EmailsController : Controller
    {
        private readonly IEmailsServices _services;

        public EmailsController(IEmailsServices services)
        {
            _services = services;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(EmailsViewModel model)
        {
            var files = Request.Form.Files;
            EmailDto dto = new()
            {
                To = model.To,
                Subject = model.Subject,
                Body = model.Body,
                Attachment = files
            };

            _services.SendEmail(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
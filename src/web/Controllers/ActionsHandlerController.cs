using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Routing;
using Web.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Route("[controller]")]
    //[AutoValidateAntiforgeryToken] // TODO: security check.
    public class ActionsHandlerController : Controller
    {
        private readonly IContactService _contact;
        public ActionsHandlerController(
            IContactService contactService)
        {
            _contact = contactService;
        }

        #region Public actions

        [HttpGet("[action]")]
        public IActionResult GetGroups()
        {
            var result = "[]";

            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> PostContact([FromBody]ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _contact.Add(model);

                // REFACTOR: pub/sub this
                // await _mailchimp.SendDirectNotification(model);

                return Ok(new { done = result });
            }

            return BadRequest(ModelState);
        }

        #endregion

    }
}
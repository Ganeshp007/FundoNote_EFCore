namespace FundoNote_EFCore.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.NoteModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NLogger.Interface;
    using RepositoryLayer.Services;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NoteController : Controller
    {
        private readonly ILoggerManager logger;
        private readonly FundoContext fundoContext;
        private readonly INoteBL noteBL;

        public NoteController(FundoContext fundoContext, INoteBL noteBL, ILoggerManager logger)
        {
            this.fundoContext = fundoContext;
            this.noteBL = noteBL;
            this.logger = logger;
        }

        [HttpPost("AddNote")]
        public async Task<IActionResult> AddNote(NotePostModel notePostModel)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                await this.noteBL.AddNote(UserId,notePostModel);
                this.logger.LogInfo($"Note Created Successfully UserId = {userId}");
                return Ok(new { sucess = true, Message = "Note Created Successfully..." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetALlNote")]
        public async Task<IActionResult> GetAllNote()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                var NoteData = await this.noteBL.GetAllNote(UserId);
                if (NoteData == null)
                {
                    this.logger.LogInfo($"No Notes Exists At Moment!! UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "You Dont Have Any Notes!!" });
                }
                this.logger.LogInfo($"All Notes Retrieved Successfully UserId = {userId}");
                return this.Ok(new { sucess = true, Message = "Notes Data Retrieved successfully...", data = NoteData });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

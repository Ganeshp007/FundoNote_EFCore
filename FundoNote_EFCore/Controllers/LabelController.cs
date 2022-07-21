namespace FundoNote_EFCore.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NLogger.Interface;
    using RepositoryLayer.Services;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LabelController : ControllerBase
    {
        private readonly ILoggerManager logger;
        private readonly FundoContext fundoContext;
        private readonly ILabelBL LabelBL;

        public LabelController(FundoContext fundoContext, ILabelBL labelBL, ILoggerManager logger)
        {
            this.fundoContext = fundoContext;
            this.LabelBL = labelBL;
            this.logger = logger;
        }

        [HttpPost("AddLabel")]
        public async Task<IActionResult> AddLabel(int noteId,string Labelname)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                var NoteIdRecord = fundoContext.Notes.FirstOrDefault(x => x.UserId == UserId && x.NoteId == noteId);
                var NoteLabelName = fundoContext.Labels.FirstOrDefault(l => l.LabelName == Labelname && l.NoteId == noteId);

                if (NoteIdRecord == null || NoteIdRecord.IsTrash == true)
                {
                    this.logger.LogError($"Note Does not Exixts!! NoteId={noteId}|UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                if (Labelname == "" || Labelname == "string" )
                {
                    this.logger.LogError($"Entered InValid Inputs!! {noteId}|UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message = "Please Provide Valid and Unique Fields for LabelName!!" });
                }

                if (NoteLabelName != null)
                {
                    this.logger.LogError($"Label Already Exists!! {noteId}|UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message =$"Label Name Already Exists for Note {noteId}!!" });
                }

                await this.LabelBL.AddLabel(UserId, noteId, Labelname);
                this.logger.LogError($"Label Set Successfully for {noteId}|UserId = {UserId}");
                return this.Ok(new { sucess = true, Message = $"Label Added to NoteId {noteId} Successfully..." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

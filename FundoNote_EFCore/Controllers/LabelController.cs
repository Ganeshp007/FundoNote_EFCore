namespace FundoNote_EFCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.LabelModels;
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

        [HttpGet("GetALlLabels")]
        public async Task<IActionResult> GetAllLabels()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                List<GetAllLabelsModel> NoteData = await this.LabelBL.GetAllLabel(UserId);
                if (NoteData == null)
                {
                    this.logger.LogError($"No Labels Exists At Moment!! UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "You Dont Have Any Notes!!" });
                }

                this.logger.LogInfo($"All Labels Retrieved Successfully UserId = {userId}");
                return this.Ok(new { sucess = true, Message = "Labels Data Retrieved successfully...", data = NoteData });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetAllLabelByNoteId/{NoteId}")]
        public async Task<IActionResult> GetAllLabelByNoteId(int NoteId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                List<GetAllLabelsModel> NoteData = await this.LabelBL.GetLabelByNoteId(UserId,NoteId);

                if (NoteData == null)
                {
                    this.logger.LogError($"No Labels Exists At Moment!! NoteId = {NoteId} | UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "You Dont Have Any Notes!!" });
                }

                if (NoteData.Count==0)
                {
                    this.logger.LogError($"Note Moved To Trash!! NoteId = {NoteId} | UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = $"Can not Fetch Label Records as NoteId {NoteId} is in Trash!!" });
                }

                this.logger.LogInfo($"All Labels Retrieved Successfully NoteId = {NoteId} | UserId = {userId}");
                return this.Ok(new { sucess = true, Message = "Labels Data Retrieved successfully...", data = NoteData });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateLabel/{LabelId}/{NewLabelName}")]
        public async Task<IActionResult> UpdatedLabel(int LabelId, string NewLabelName)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                var label = this.fundoContext.Labels.FirstOrDefault(x => x.LabelId == LabelId);

                if (label == null)
                {
                    return this.BadRequest(new { sucess = false, Message = $"LabelId {LabelId} Does Not Exists!!" });
                }

                bool result = await this.LabelBL.UpdateLable(UserId,LabelId, NewLabelName);
                if (result)
                {
                    return this.Ok(new { sucess = true, Message = $"Updated Label {LabelId} Successfully..." });
                }

                return this.BadRequest(new { sucess = false, Message = $"Entered Label Name already exsists for LabelId{LabelId}!!" });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpDelete("DeleteLabel/{LabelId}")]
        public async Task<IActionResult> DeleteLabel(int LabelId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                bool result = await this.LabelBL.DeleteLabel(UserId, LabelId);
                if (result)
                {
                    return this.Ok(new { sucess = true, Message = $"LabelId {LabelId}  Deleted SuccessFully..." });
                }

                return this.BadRequest(new { sucess = false, Message = $"LabelId {LabelId} Does not Exists!!" });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}

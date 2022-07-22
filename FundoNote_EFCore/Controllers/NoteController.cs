namespace FundoNote_EFCore.Controllers
{
    using System;
    using System.Collections.Generic;
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
    public class NoteController : ControllerBase
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

                if (notePostModel.Title == "" || notePostModel.Title == "string" && notePostModel.Description == "string" && notePostModel.Bgcolor == "string")
                {
                    this.logger.LogError($"Please Provide Valid Inputs!!");
                    return this.BadRequest(new { sucess = false, Message = "Please Provide Valid Fields for Note!!" });
                }

                await this.noteBL.AddNote(UserId, notePostModel);
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
                List<GetNoteResponse> NoteData = await this.noteBL.GetAllNote(UserId);
                if (NoteData.Count == 0)
                {
                    this.logger.LogError($"No Notes Exists At Moment!! UserId = {userId}");
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

        [HttpPut("UpdateNote/{NoteId}")]
        public async Task<IActionResult> UpdateNote(int NoteId, UpdateNoteModel updateNoteModel)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                var updateNote = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId);

                if (updateNote == null || updateNote.IsTrash == true)
                {
                    this.logger.LogError($"Note Does not Exixts!! NoteId={NoteId}|UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                if (updateNoteModel.Title == "" || updateNoteModel.Title == "string" && updateNoteModel.Description == "string" && updateNoteModel.Bgcolor == "string")
                {
                    this.logger.LogError($"Please Provide Valid Inputs!! {NoteId}|UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "Please Provide Valid Fields for Note!!" });
                }

                await this.noteBL.UpdateNote(UserId, NoteId, updateNoteModel);
                this.logger.LogInfo($"Note Updated Successfully NoteId={NoteId}|UserId = {userId}");
                return Ok(new { sucess = true, Message = $"NoteId {NoteId} Updated Successfully..." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("DeleteNote/{NoteId}")]
        public async Task<IActionResult> DeleteNote(int NoteId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                var updateNote = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId);
                if (updateNote == null || updateNote.IsTrash == true)
                {
                    this.logger.LogError($"Note Does Not Exists!! {NoteId}|UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                await this.noteBL.DeleteNote(UserId, NoteId);
                this.logger.LogInfo($"Note Moved to Trash  NoteId:{NoteId} | UserId:{UserId}");
                return Ok(new { success = true, Message = $"NoteId : {NoteId} Moved to Trash SuccessFully..." });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPut("PinNote/{NoteId}")]
        public async Task<IActionResult> PinNote(int NoteId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                var updateNote = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId);
                if (updateNote == null || updateNote.IsTrash == true)
                {
                    this.logger.LogError($"Note Does Not Exists!! {NoteId}|UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                await this.noteBL.PinNote(UserId, NoteId);
                this.logger.LogInfo($"Note Pinned Successfully NoteId={NoteId}|UserId = {userId}");
                return this.Ok(new { sucess = true, Message = $"NoteId {NoteId} Pinned Successfully..." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("Archive/{NoteId}")]
        public async Task<IActionResult> ArchiveNote(int NoteId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                var updateNote = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId);
                if (updateNote == null || updateNote.IsTrash == true)
                {
                    this.logger.LogError($"Note Does Not Exists!! {NoteId}|UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                await this.noteBL.ArchiveNote(UserId, NoteId);
                this.logger.LogInfo($"Note Archived Successfully NoteId={NoteId}|UserId = {userId}");
                return this.Ok(new { sucess = true, Message = $"NoteId {NoteId} Archived Successfully..." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("ReminderNote/{NoteId}")]

        public async Task<IActionResult> ReminderNote(int NoteId, RemainderNoteModel reminderNoteModel)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var reminder = Convert.ToDateTime(reminderNoteModel.Reminder);
                var result = await this.noteBL.ReminderNote(userId, NoteId, reminder);

                if (result != null)
                {
                    this.logger.LogInfo($"Note Reminder Set Successfully  NoteId={NoteId}|UserId = {userId}");
                    return this.Ok(new { status = 200, success = true, message = result });
                }
                else
                {
                    this.logger.LogError($"Note Reminder Set UnSuccessfull!! {NoteId}|UserId = {userId}");
                    return this.BadRequest(new { success = false, Message = $"NoteId {NoteId} Reminder set Failed!!" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("Trash/{NoteId}")]
        public async Task<IActionResult> TrashNote(int NoteId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                var Note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId);
                if (Note == null)
                {
                    this.logger.LogError($"Note Does Not Exists!! {NoteId}|UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                await this.noteBL.TrashNote(UserId, NoteId);

                if(Note.IsTrash==true)
                {
                    this.logger.LogInfo($"Note Moved to Trash Successfully NoteId={NoteId}|UserId = {userId}");
                    return this.Ok(new { sucess = true, Message = $"NoteId {NoteId} Moved to Trash Successfully..." });
                }
                else
                {
                    this.logger.LogInfo($"Note Removed from Trash Successfully NoteId={NoteId}|UserId = {userId}");
                    return this.Ok(new { sucess = true, Message = $"NoteId {NoteId} Removed from Trash Successfully..." });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

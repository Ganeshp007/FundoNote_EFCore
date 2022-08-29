namespace FundoNote_EFCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.NoteModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json;
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
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;

        public NoteController(FundoContext fundoContext, INoteBL noteBL, ILoggerManager logger,IDistributedCache distributedCache,IMemoryCache memoryCache)
        {
            this.fundoContext = fundoContext;
            this.noteBL = noteBL;
            this.logger = logger;
            this.distributedCache = distributedCache;
            this.memoryCache = memoryCache;
        }

        [HttpPost("AddNote")]
        public async Task<IActionResult> AddNote(NotePostModel notePostModel)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);

                if (notePostModel.Title == "string" || (notePostModel.Title == "string" && notePostModel.Description == "string" && notePostModel.Bgcolor == "string"))
                {
                    this.logger.LogError($"Please Provide Valid Inputs!!");
                    return this.BadRequest(new { sucess = false, Message = "Please Provide Valid Fields for Note!!" });
                }

                await this.noteBL.AddNote(UserId, notePostModel);
                this.logger.LogInfo($"Note Created Successfully UserId = {UserId}");
                return Ok(new { sucess = true, Message = "Note Created Successfully..." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetAllNote")]
        public async Task<IActionResult> GetAllNote()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                List<GetNoteResponse> NoteData = await this.noteBL.GetAllNote(UserId);
                if (NoteData.Count == 0)
                {
                    this.logger.LogError($"No Notes Exists At Moment!! UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message = "You Dont Have Any Notes!!" });
                }

                this.logger.LogInfo($"All Notes Retrieved Successfully UserId = {UserId}");
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
                    this.logger.LogError($"Note Does not Exixts!! NoteId={NoteId}|UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                if (updateNoteModel.Title == "string" || updateNoteModel.Title == "string" && updateNoteModel.Description == "string" && updateNoteModel.Bgcolor == "string")
                {
                    this.logger.LogError($"Please Provide Valid Inputs!! {NoteId}|UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message = "Please Provide Valid Fields for Note!!" });
                }

                await this.noteBL.UpdateNote(UserId, NoteId, updateNoteModel);
                this.logger.LogInfo($"Note Updated Successfully NoteId={NoteId}|UserId = {UserId}");
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
                var Note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId && x.UserId == UserId);
                if (Note == null)
                {
                    this.logger.LogError($"Note Does Not Exists!! {NoteId}|UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                bool result = await this.noteBL.DeleteNote(UserId, NoteId);
                if (result)
                {
                    this.logger.LogInfo($"Note Deleted Sucessfully  NoteId:{NoteId} | UserId:{UserId}");
                    return this.Ok(new { success = true, Message = $"NoteId : {NoteId} deleted successFully..." });
                }
                else
                {
                    this.logger.LogInfo($"Note Delete Unsucessfull  NoteId:{NoteId} | UserId:{UserId}");
                    return Ok(new { success = true, Message = $"NoteId : {NoteId} Delete UnsccessFully..." });
                }
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
                    this.logger.LogError($"Note Does Not Exists!! {NoteId}|UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                bool result = await this.noteBL.PinNote(UserId, NoteId);
                if (result)
                {
                    this.logger.LogInfo($"Note Pinned Successfully NoteId={NoteId}|UserId = {UserId}");
                    return this.Ok(new { sucess = true, Message = $"NoteId {NoteId} Pinned Successfully..." });
                }
                else
                {
                    this.logger.LogInfo($"Note Unpinned Successfully NoteId={NoteId}|UserId = {UserId}");
                    return this.Ok(new { sucess = true, Message = $"NoteId {NoteId} Unpinned Successfully..." });
                }
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
                var note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId);
                if (note == null || note.IsTrash == true)
                {
                    this.logger.LogError($"Note Does Not Exists!! {NoteId}|UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                bool result = await this.noteBL.ArchiveNote(UserId, NoteId);
                if (result)
                {
                    this.logger.LogInfo($"Note Archived Successfully NoteId={NoteId}|UserId = {UserId}");
                    return this.Ok(new { sucess = true, Message = $"NoteId {NoteId} Archived Successfully..." });
                }
                else
                {
                    this.logger.LogInfo($"Note Removed from Archived Successfully... NoteId={NoteId}|UserId = {UserId}");
                    return this.Ok(new { sucess = true, Message = $"NoteId {NoteId} removed Archived Successfully..." });
                }
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
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value); //another way to claim id and give its value no need to parse
                var reminder = Convert.ToDateTime(reminderNoteModel.Reminder);
                var note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId && x.IsTrash == false);
                if (note != null)
                {
                    var result = await this.noteBL.ReminderNote(userId, NoteId, reminder);
                    if (result != null)
                    {
                        this.logger.LogInfo($"Note Reminder Set Successfully  NoteId={NoteId}|UserId = {userId}");
                        return this.Ok(new { status = 200, success = true, message = result });
                    }
                    else
                    {
                        this.logger.LogError($"Note Reminder removed Sucessfully... {NoteId}|UserId = {userId}");
                        return this.Ok(new { success = false, Message = $"Reminder removed sucessfully NoteId {NoteId}..." });
                    }
                }
                else
                {
                    this.logger.LogError($"Note Does Not Exists!! {NoteId}|UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
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
                int UserId = Convert.ToInt32(userId.Value);
                var Note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId);
                if (Note == null)
                {
                    this.logger.LogError($"Note Does Not Exists!! {NoteId}|UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                await this.noteBL.TrashNote(UserId, NoteId);
                var note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId);
                if (note.IsTrash == true)
                {
                    this.logger.LogInfo($"Note Moved to Trash Successfully NoteId={NoteId}|UserId = {UserId}");
                    return this.Ok(new { sucess = true, Message = $"NoteId {NoteId} Moved to Trash Successfully..." });
                }
                else
                {
                    this.logger.LogInfo($"Note Removed from Trash Successfully NoteId={NoteId}|UserId = {UserId}");
                    return this.Ok(new { sucess = true, Message = $"NoteId {NoteId} Removed from Trash Successfully..." });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateBgcolor/{NoteId}/{bgcolor}")]
        public async Task<IActionResult> UpdateBgcolor(int NoteId,string bgcolor)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Convert.ToInt32(userId.Value);
                var Note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId && x.IsTrash == false);
                if (Note == null)
                {
                    this.logger.LogError($"Note Does Not Exists!! {NoteId}|UserId = {UserId}");
                    return this.BadRequest(new { sucess = false, Message = "Note Does not Exists!!" });
                }

                bool result = await this.noteBL.UpdateBgcolor(NoteId, bgcolor);
                if (result == true)
                {
                    this.logger.LogInfo($"Note Bgcolor changed Successfully NoteId={NoteId}|UserId = {UserId}");
                    return this.Ok(new { sucess = true, Message = $"Note Bgcolor changed Successfully NoteId={NoteId}|UserId = {UserId}" });
                }
                else
                {
                    this.logger.LogInfo($"Note Bgcolor change Unsuccessfull NoteId={NoteId}|UserId = {UserId}");
                    return this.Ok(new { sucess = true, Message = $"Note Bgcolor Change Unsuccessfull NoteId={NoteId}|UserId = {UserId}" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetAllNoteUsingRedis")]
        public async Task<IActionResult> GetAllNoteUsingRedis()
        {
            try
            {
                string CacheKey = "NoteList";
                string SerializeNoteList;
                List<GetNoteResponse> notelist = new List<GetNoteResponse>();
                var redisnotelist = await distributedCache.GetAsync(CacheKey);
                if (redisnotelist != null)
                {
                    SerializeNoteList = Encoding.UTF8.GetString(redisnotelist);
                    notelist = JsonConvert.DeserializeObject<List<GetNoteResponse>>(SerializeNoteList);
                }
                else
                {
                    var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                    int userId = int.Parse(userid.Value);
                    notelist = await this.noteBL.GetAllNote(userId);
                    SerializeNoteList = JsonConvert.SerializeObject(notelist);
                    redisnotelist = Encoding.UTF8.GetBytes(SerializeNoteList);
                    var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20)).SetAbsoluteExpiration(TimeSpan.FromHours(6));
                    await distributedCache.SetAsync(CacheKey, redisnotelist, option);
                }
                return this.Ok(new { success = true, message = $"Get Note Successful", data = notelist });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

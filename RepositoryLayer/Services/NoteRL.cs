namespace RepositoryLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DatabaseLayer.NoteModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using RepositoryLayer.Interface;
    using RepositoryLayer.Services.Entity;

    public class NoteRL : INoteRL
    {
        private readonly FundoContext fundoContext;
        private readonly IConfiguration iconfiguration;

        public NoteRL(FundoContext fundoContext, IConfiguration iconfiguration)
        {
            this.fundoContext = fundoContext;
            this.iconfiguration = iconfiguration;
        }

        public async Task AddNote(int UserId, NotePostModel notePostModel)
        {
            try
            {
                Note note = new Note();
                note.UserId = UserId;
                note.Title= notePostModel.Title;
                note.Description = notePostModel.Description;
                note.Bgcolor = notePostModel.Bgcolor;
                note.RegisteredDate = DateTime.Now;
                note.ModifiedDate = DateTime.Now;
                this.fundoContext.Notes.Add(note);
                await this.fundoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GetNoteResponse>> GetAllNote(int UserId)
        {
            try
            {
                return await fundoContext.Users
               .Where(u => u.UserId == UserId)
               .Join(fundoContext.Notes.Where(n=>n.UserId==UserId),
               u => u.UserId,
               n => n.UserId,
               (u, n) => new GetNoteResponse
               {
                   NoteId = n.NoteId,
                   UserId = u.UserId,
                   Title = n.Title,
                   Description = n.Description,
                   Bgcolor = n.Bgcolor,
                   Firstname = u.Firstname,
                   Lasttname = u.Lastname,
                   Email = u.Email,
                   CreatedDate = u.CreateDate,
                   IsPin=n.IsPin,
                   IsArchive = n.IsArchive,
                   IsReminder = n.IsRemainder,
                   IsTrash = n.IsTrash,
               }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task UpdateNote(int userId, int noteId, UpdateNoteModel updateNoteModel)
        {
            try
            {
                var updateNote = fundoContext.Notes.FirstOrDefault(x => x.NoteId == noteId);
                updateNote.Title = updateNoteModel.Title;
                updateNote.Description = updateNoteModel.Description;
                updateNote.Bgcolor = updateNoteModel.Bgcolor;
                updateNote.IsPin = updateNoteModel.IsPin;
                updateNote.IsArchive = updateNoteModel.IsArchive;
                updateNote.IsRemainder = updateNoteModel.IsRemainder;
                updateNote.IsTrash = updateNoteModel.IsTrash;
                updateNote.ModifiedDate = DateTime.Now;
                this.fundoContext.Notes.Update(updateNote);// Optional
                await this.fundoContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteNote(int userId, int noteId)
        {
            try
            {
                var Note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == noteId);
                fundoContext.Remove(Note);
                int result = await this.fundoContext.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ArchiveNote(int userId, int noteId)
        {
            try
            {
                var note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == noteId);
                if (note != null)
                {
                    if (note.IsArchive == false)
                    {
                        note.IsArchive = true;
                        await this.fundoContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        note.IsArchive = false;
                        await this.fundoContext.SaveChangesAsync();
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PinNote(int userId, int noteId)
        {
            try
            {
                var note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == noteId);
                if (note != null)
                {
                    if (note.IsPin == false)
                    {
                        note.IsPin = true;
                        await this.fundoContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        note.IsPin = false;
                        await this.fundoContext.SaveChangesAsync();
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> ReminderNote(int userId, int noteId, DateTime Reminder)
        {
            try
            {
                var ReminderNoteResult = fundoContext.Notes.Where(x => x.NoteId == noteId && x.UserId == userId).FirstOrDefault();
                if (ReminderNoteResult != null && ReminderNoteResult.IsRemainder == false)
                {
                    ReminderNoteResult.IsRemainder = true;
                    ReminderNoteResult.Remainder = Reminder;
                    await this.fundoContext.SaveChangesAsync();
                    return "Reminder Set Successfull for date:" + Reminder.Date + " And Time : " + Reminder.TimeOfDay;
                }
                else
                {
                    ReminderNoteResult.IsRemainder = false;
                    await this.fundoContext.SaveChangesAsync();
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> TrashNote(int userId, int noteId)
        {
            try
            {
                var note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == noteId);
                if (note != null)
                {
                    if (note.IsTrash == false)
                    {
                        note.IsTrash = true;
                        await this.fundoContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        note.IsTrash = false;
                        await this.fundoContext.SaveChangesAsync();
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateBgcolor(int NoteId, string Bgcolor)
        {
            try
            {
                var note = fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId);
                note.Bgcolor = Bgcolor;
                var result = fundoContext.SaveChangesAsync();
                if (result != null)
                {
                   return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

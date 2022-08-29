namespace BusinessLayer.Servies
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.NoteModels;
    using RepositoryLayer.Interface;

    public class NoteBL : INoteBL
    {
        INoteRL noteRL;

        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }

        public async Task AddNote(int UserId, NotePostModel notePostModel)
        {
            try
            {
                await this.noteRL.AddNote(UserId, notePostModel);
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
                return await this.noteRL.GetAllNote(UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task UpdateNote(int UserId, int NoteId, UpdateNoteModel updateNoteModel)
        {
            try
            {
               await this.noteRL.UpdateNote(UserId, NoteId, updateNoteModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteNote(int UserId, int NoteId)
        {
            try
            {
                return await this.noteRL.DeleteNote(UserId, NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ArchiveNote(int UserId, int NoteId)
        {
            try
            {
               return await this.noteRL.ArchiveNote(UserId, NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PinNote(int UserId, int NoteId)
        {
            try
            {
                return await this.noteRL.PinNote(UserId, NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> TrashNote(int UserId, int NoteId)
        {
            try
            {
                return await this.noteRL.TrashNote(UserId, NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> ReminderNote(int UserId, int NoteId, DateTime Reminder)
        {
            try
            {
                return await this.noteRL.ReminderNote(UserId, NoteId, Reminder);
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
                return await this.noteRL.UpdateBgcolor(NoteId,Bgcolor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

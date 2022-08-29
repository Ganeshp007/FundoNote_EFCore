namespace BusinessLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DatabaseLayer.NoteModels;

    public interface INoteBL
    {
        Task AddNote(int UserId, NotePostModel notePostModel);

        Task<List<GetNoteResponse>> GetAllNote(int UserId);

        Task UpdateNote(int UserId, int NoteId, UpdateNoteModel updateNoteModel);

        Task<bool> DeleteNote(int UserId, int NoteId);

        Task<bool> ArchiveNote(int UserId, int NoteId);

        Task<bool> PinNote(int UserId, int NoteId);

        Task<string> ReminderNote(int UserId, int NoteId, DateTime Reminder);

        Task<bool> TrashNote(int UserId, int NoteId);

        Task<bool> UpdateBgcolor(int NoteId, string Bgcolor);
    }
}

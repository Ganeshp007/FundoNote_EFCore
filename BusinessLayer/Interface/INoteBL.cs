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

        Task DeleteNote(int UserId, int NoteId);

        Task ArchiveNote(int UserId, int NoteId);

        Task PinNote(int UserId, int NoteId);

        Task<string> ReminderNote(int UserId,int NoteId,DateTime Reminder);

        Task TrashNote(int UserId, int NoteId);
    }
}

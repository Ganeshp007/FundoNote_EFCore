using DatabaseLayer.NoteModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        Task AddNote(int UserId,NotePostModel notePostModel);
        Task<List<GetNoteResponse>> GetAllNote(int UserId);
        Task UpdateNote(int UserId, int NoteId, UpdateNoteModel updateNoteModel);
        Task DeleteNote(int UserId, int NoteId);
    }
}

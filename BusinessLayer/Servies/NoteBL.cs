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

        public Task DeleteNote(int UserId, int NoteId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetNoteResponse>> GetAllNote(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNote(int UserId, int NoteId, UpdateNoteModel updateNoteModel)
        {
            throw new NotImplementedException();
        }
    }
}

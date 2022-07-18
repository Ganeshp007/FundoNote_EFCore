﻿namespace RepositoryLayer.Services
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
               .Join(fundoContext.Notes,
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
                   CreatedDate = u.CreateDate
               }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Task UpdateNote(int UserId, int NoteId, UpdateNoteModel updateNoteModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNote(int UserId, int NoteId)
        {
            throw new NotImplementedException();
        }
    }
}

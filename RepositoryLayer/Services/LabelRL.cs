namespace RepositoryLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DatabaseLayer.LabelModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using RepositoryLayer.Interface;
    using RepositoryLayer.Services.Entity;

    public class LabelRL: ILabelRL
    {
        private readonly FundoContext fundoContext;
        private readonly IConfiguration iconfiguration;

        public LabelRL(FundoContext fundoContext, IConfiguration iconfiguration)
        {
            this.fundoContext = fundoContext;
            this.iconfiguration = iconfiguration;
        }

        public async Task AddLabel(int UserId, int NoteId, string Labelname)
        {
            try
            {
                Label label = new Label();
                label.LabelName = Labelname;
                label.UserId = UserId;
                label.NoteId = NoteId;
                this.fundoContext.Labels.Add(label);
                await this.fundoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GetAllLabelsModel>> GetAllLabel(int UserId)
        {
            try
            {
                var label = fundoContext.Labels.FirstOrDefault(u => u.UserId == UserId);
                if (label == null)
                {
                    return null;
                }

                var res = await (from user in fundoContext.Users
                                 join notes in fundoContext.Notes on user.UserId equals UserId
                                 join labels in fundoContext.Labels on notes.NoteId equals labels.NoteId
                                 where labels.UserId == UserId
                                 select new GetAllLabelsModel
                                 {
                                     UserId = user.UserId,
                                     NoteId = notes.NoteId,
                                     LabelId = labels.LabelId,
                                     Title = notes.Title,
                                     Description = notes.Description,
                                     LabelName = labels.LabelName,
                                     FirstName = user.Firstname,
                                     LastName = user.Lastname,
                                     Email = user.Email,
                                 }).ToListAsync();
                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<GetAllLabelsModel>> GetLabelByNoteId(int UserId, int NoteId)
        {
            try
            {
                var label = fundoContext.Labels.FirstOrDefault(u => u.UserId == UserId && u.NoteId == NoteId);
                if (label == null)
                {
                    return null;
                }

                var res = await (from user in fundoContext.Users
                                 join notes in fundoContext.Notes on user.UserId equals UserId
                                 where notes.NoteId == NoteId
                                 join labels in fundoContext.Labels on notes.NoteId equals labels.NoteId
                                 select new GetAllLabelsModel
                                 {
                                     UserId = user.UserId,
                                     NoteId = notes.NoteId,
                                     LabelId = labels.LabelId,
                                     Title = notes.Title,
                                     Description = notes.Description,
                                     LabelName = labels.LabelName,
                                     FirstName = user.Firstname,
                                     LastName = user.Lastname,
                                     Email = user.Email,
                                 }).ToListAsync();
                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

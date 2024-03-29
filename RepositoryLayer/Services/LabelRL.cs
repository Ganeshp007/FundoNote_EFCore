﻿namespace RepositoryLayer.Services
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

    public class LabelRL : ILabelRL
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
                var label = fundoContext.Labels.FirstOrDefault(l => l.UserId == UserId);
                if (label == null)
                {
                    return null;
                }

                var res = await (from user in fundoContext.Users
                                 join notes in fundoContext.Notes on user.UserId equals UserId where notes.IsTrash == false
                                 join labels in fundoContext.Labels on notes.NoteId equals labels.NoteId
                                 where labels.UserId == UserId
                                 select new GetAllLabelsModel
                                 {
                                     UserId = user.UserId,
                                     NoteId = notes.NoteId,
                                     LabelId = labels.LabelId,
                                     LabelName = labels.LabelName,
                                     Title = notes.Title,
                                     Description = notes.Description,
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
                                 where notes.NoteId == NoteId && notes.IsTrash == false
                                 join labels in fundoContext.Labels on notes.NoteId equals labels.NoteId
                                 where label.UserId==UserId
                                 select new GetAllLabelsModel
                                 {
                                     UserId = user.UserId,
                                     NoteId = notes.NoteId,
                                     LabelId = labels.LabelId,
                                     LabelName = labels.LabelName,
                                     Title = notes.Title,
                                     Description = notes.Description,
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

        public async Task<bool> UpdateLable(int UserId,int LabelId, string NewLabelName)
        {
            try
            {
                var label = this.fundoContext.Labels.FirstOrDefault(x => x.LabelId == LabelId && x.UserId == UserId);
                var check = this.fundoContext.Labels.FirstOrDefault(x => x.NoteId == label.NoteId && x.LabelName == NewLabelName);
                if (label != null && check == null)
                {
                    label.LabelName = NewLabelName;
                    await this.fundoContext.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteLabel(int UserId, int LabelId)
        {
            try
            {
                var label = this.fundoContext.Labels.FirstOrDefault(x => x.LabelId == LabelId && x.UserId == UserId);
                if (label != null)
                {
                    this.fundoContext.Remove(label);
                    this.fundoContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GetAllLabelsModel>> GetLabelByLabelId(int UserId, int LabelId)
        {
            try
            {
                var label = fundoContext.Labels.FirstOrDefault(u => u.UserId == UserId && u.LabelId == LabelId);
                var NoteId = label.NoteId;
                if (label == null)
                {
                    return null;
                }

                var res = await (from user in fundoContext.Users
                                 join notes in fundoContext.Notes on user.UserId equals UserId
                                 where notes.NoteId == NoteId && notes.IsTrash == false
                                 join labels in fundoContext.Labels on notes.NoteId equals labels.NoteId
                                 where label.UserId == UserId && labels.LabelId == LabelId
                                 select new GetAllLabelsModel
                                 {
                                     UserId = user.UserId,
                                     NoteId = notes.NoteId,
                                     LabelId = labels.LabelId,
                                     LabelName = labels.LabelName,
                                     Title = notes.Title,
                                     Description = notes.Description,
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

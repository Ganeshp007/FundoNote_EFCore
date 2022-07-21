﻿namespace BusinessLayer.Servies
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.LabelModels;
    using RepositoryLayer.Interface;

    public class LabelBL: ILabelBL
    {
        ILabelRL labelRL;

        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }

        public async Task AddLabel(int UserId, int NoteId, string Labelname)
        {
            try
            {
                await this.labelRL.AddLabel(UserId,NoteId, Labelname);
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
                 return await this.labelRL.GetAllLabel(UserId);
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
                return await this.labelRL.GetLabelByNoteId(UserId, NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
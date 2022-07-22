namespace BusinessLayer.Servies
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.LabelModels;
    using RepositoryLayer.Interface;

    public class LabelBL : ILabelBL
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

        public async Task<bool> UpdateLable(int UserId,int LabelId, string NewLabelName)
        {
            try
            {
                return await this.labelRL.UpdateLable(UserId,LabelId, NewLabelName);
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
                return await this.labelRL.DeleteLabel(UserId,LabelId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

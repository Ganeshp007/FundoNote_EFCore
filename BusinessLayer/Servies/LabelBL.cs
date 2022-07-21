namespace BusinessLayer.Servies
{
    using System;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
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
    }
}

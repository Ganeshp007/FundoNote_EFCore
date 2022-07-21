namespace RepositoryLayer.Services
{
    using System;
    using System.Threading.Tasks;
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
    }
}

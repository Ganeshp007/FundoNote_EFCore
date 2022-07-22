namespace RepositoryLayer.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DatabaseLayer.LabelModels;

    public interface ILabelRL
    {
        Task AddLabel(int UserId, int NoteId,string Labelname);

        Task<List<GetAllLabelsModel>> GetAllLabel(int UserId);

        Task<List<GetAllLabelsModel>> GetLabelByNoteId(int UserId, int NoteId);

        Task<bool> UpdateLable(int UserId,int LabelId, string NewLabelName);

        Task<bool> DeleteLabel(int UserId, int LabelId);


    }
}

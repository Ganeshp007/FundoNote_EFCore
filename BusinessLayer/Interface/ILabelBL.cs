namespace BusinessLayer.Interface
{
    using DatabaseLayer.LabelModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILabelBL
    {
        Task AddLabel(int UserId, int NoteId, string Labelname);

        Task<List<GetAllLabelsModel>> GetAllLabel(int UserId);

        Task<List<GetAllLabelsModel>> GetLabelByNoteId(int UserId, int NoteId);

        Task<bool> UpdateLable(int UserId,int LabelId, string NewLabelName);

        Task<bool> DeleteLabel(int UserId, int LabelId);

        Task<List<GetAllLabelsModel>> GetLabelByLabelId(int UserId, int LabelId);

    }
}

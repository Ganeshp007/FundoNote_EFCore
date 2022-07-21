namespace BusinessLayer.Interface
{
    using System.Threading.Tasks;

    public interface ILabelBL
    {
        Task AddLabel(int UserId, int NoteId, string Labelname);
    }
}

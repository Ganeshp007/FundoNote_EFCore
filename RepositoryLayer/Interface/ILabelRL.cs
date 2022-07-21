namespace RepositoryLayer.Interface
{
    using System.Threading.Tasks;

    public interface ILabelRL
    {
        Task AddLabel(int UserId, int NoteId,string Labelname);

    }
}

namespace RepositoryLayer.Services.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Label
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }

        public string LabelName { get; set; }

        [ForeignKey("User")]
        public virtual int? UserId { get; set; }

        public virtual User user { get; set; }

        [ForeignKey("Note")]
        public virtual int? NoteId { get; set; }

        public virtual Note note { get; set; }

    }
}

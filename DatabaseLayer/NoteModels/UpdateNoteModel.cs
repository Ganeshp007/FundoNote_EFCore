namespace DatabaseLayer.NoteModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UpdateNoteModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Bgcolor { get; set; }

        public bool IsPin { get; set; }

        public bool IsArchive { get; set; }

        public bool IsRemainder { get; set; }

        public bool IsTrash { get; set; }
    }
}

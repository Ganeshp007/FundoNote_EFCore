namespace DatabaseLayer.NoteModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;

    public class UpdateNoteModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Bgcolor { get; set; }

        [DefaultValue("false")]
        public bool IsPin { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        [DefaultValue("false")]
        public bool IsRemainder { get; set; }

        [DefaultValue("false")]
        public bool IsTrash { get; set; }
    }
}

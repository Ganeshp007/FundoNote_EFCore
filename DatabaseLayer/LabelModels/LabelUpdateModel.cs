namespace DatabaseLayer.LabelModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class LabelUpdateModel
    {
        public int UserId { get; set; }

        public int NoteId { get; set; }

        public string NewLabelName { get; set; }
    }
}

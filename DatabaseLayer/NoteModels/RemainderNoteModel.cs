using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer.NoteModels
{
    public class RemainderNoteModel
    {
        [Required]
        [RegularExpression(@"\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d(?:\.\d+)?Z$")]
        public DateTime Reminder { get; set; }
    }
}

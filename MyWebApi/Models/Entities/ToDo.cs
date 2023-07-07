using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace MyWebApi.Models.Entities
{
    public class ToDo
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime InsertTime { get; set; }
        public bool IsRemoved { get; set; }
        public long categoryId { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}

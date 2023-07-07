using System;
using System.Collections.Generic;

namespace MyWebApi.Models.Entities
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime InsertTime { get; set; }
        public bool IsRemoved { get; set; }
        public ICollection<ToDo> ToDos { get; }
    }
}

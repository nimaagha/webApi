using System;
using System.Collections.Generic;

namespace MyWebApi.Models.Dto
{
    public class ToDoItemDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime InsertTime { get; set; }
        public List<Links> Links { get; set; }

    }
}

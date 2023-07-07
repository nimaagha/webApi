using System.Collections.Generic;
using System;
using MyWebApi.Models.Entities;

public class ToDoDto
{
    public long Id { get; set; }
    public string Text { get; set; }
    public DateTime InsertTime { get; set; }
    public bool IsRemoved { get; set; }
    public long categoryId { get; set; }
    public ICollection<Category> Categories { get; set; }
}

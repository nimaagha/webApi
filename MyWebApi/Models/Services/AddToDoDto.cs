using MyWebApi.Models.Entities;
using System.Collections.Generic;

namespace MyWebApi.Models.Services
{
    public class AddToDoDto
    {
        public ToDoDto todo { get; set; }
        public List<Category> categories { get; set; }
    }
}

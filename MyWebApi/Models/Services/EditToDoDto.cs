using MyWebApi.Models.Entities;
using System.Collections.Generic;

namespace MyWebApi.Models.Services
{
    public class EditToDoDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public List<Category> categories { get; set; }
    }
}

using MyWebApi.Models.Context;
using MyWebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApi.Models.Services
{
    public class ToDoRepository
    {
        private readonly DatabaseContext _context;
        public ToDoRepository(DatabaseContext context)
        {
            _context = context;
        }
        public List<ToDoDto> GetAll()
        {
            return _context.ToDos.Select(p => new ToDoDto
            {
                Id = p.Id,
                Text = p.Text,
                InsertTime = p.InsertTime,
                IsRemoved = p.IsRemoved

            }).ToList();
        }
        public ToDoDto Get(int Id)
        {
            var toDo = _context.ToDos.SingleOrDefault(p => p.Id == Id);
            return new ToDoDto()
            {
                Id = toDo.Id,
                Text = toDo.Text,
                InsertTime = toDo.InsertTime,
                IsRemoved = toDo.IsRemoved
            };
        }
        public AddToDoDto Add(AddToDoDto toDo)
        {

            ToDo newToDo = new ToDo()
            {
                Id = toDo.todo.Id,
                InsertTime = toDo.todo.InsertTime,
                Text = toDo.todo.Text,
                IsRemoved = false
            };
            foreach (var item in toDo.todo.Categories)
            {
                var category = _context.Categories.SingleOrDefault(c => c.Id == toDo.todo.categoryId);
                newToDo.Categories.Add(category);
            }
            _context.ToDos.Add(newToDo);
            _context.SaveChanges();
            return new AddToDoDto
            {
                todo = new ToDoDto
                {
                    Id = newToDo.Id,
                    Text = newToDo.Text,
                    IsRemoved = false,
                    InsertTime = newToDo.InsertTime
                },
                categories = toDo.categories
            };
        }
        public void Delete(int Id)
        {
            _context.ToDos.Remove(new ToDo { Id = Id });
            _context.SaveChanges();
        }

        public bool Edit(EditToDoDto edit)
        {
            var toDo = _context.ToDos.SingleOrDefault(p => p.Id == edit.Id);
            toDo.Text = edit.Text;
            _context.SaveChanges();
            return true;
        }
    }
}

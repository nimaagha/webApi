using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models.Dto;
using MyWebApi.Models.Services;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoRepository toDoRepository;
        public ToDoController(ToDoRepository toDoRepository)
        {
            this.toDoRepository = toDoRepository;
        }

        // GET: api/<ToDoController>
        [HttpGet]
        public IActionResult Get()
        {
            var todoList = toDoRepository.GetAll().Select(p => new ToDoItemDto
            {
                Id = p.Id,
                Text = p.Text,
                InsertTime = p.InsertTime
            }).ToList();
            return Ok(todoList);
        }

        // GET api/<ToDoController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var todo = toDoRepository.Get(id);

            return Ok(new ToDoItemDto
            {
                Id = todo.Id,
                Text = todo.Text,
                InsertTime = todo.InsertTime
            });
        }

        // POST api/<ToDoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ToDoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ToDoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

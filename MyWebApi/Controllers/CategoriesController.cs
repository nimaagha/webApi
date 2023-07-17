using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models.Services;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;
        public CategoriesController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        /// <summary>
        /// Get List of Categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_categoryRepository.GetAll());
        }

        /// <summary>
        /// Get Category with id
        /// </summary>
        /// <param name="id">identification</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            return Ok(_categoryRepository.Find(id));
        }

        [HttpPut]
        public IActionResult Put(CategoryDto categoryDto)
        {
            return Ok(_categoryRepository.Edit(categoryDto));
        }

        [HttpPost]
        public IActionResult Post(string name)
        {
            var result = _categoryRepository.AddCategory(name);
            return Created(Url.Action(nameof(Get),"Categories",new {Id = result},Request.Scheme),true);
        }
        [HttpDelete]
        public IActionResult Delete(long id)
        {
            return Ok(_categoryRepository.Delete(id));
        }
    }
}

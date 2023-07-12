using MyWebApi.Models.Context;
using MyWebApi.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApi.Models.Services
{
    public class CategoryRepository
    {
        private readonly DatabaseContext _context;
        public CategoryRepository(DatabaseContext context)
        {
            _context = context;
        }
        public List<CategoryDto> GetAll()
        {
            var data = _context.Categories.ToList()
                .Select(p=> new CategoryDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();
            return data;
        }

        public long AddCategory(string name)
        {
            Category category = new Category()
            {
                Name = name
            };
            _context.Add(category);
            _context.SaveChanges();
            return category.Id;
        }

        public int Delete(long id)
        {
            _context.Categories.Remove(new Category { Id = id });
            return _context.SaveChanges();
        }

        public int Edit(CategoryDto categoryDto)
        {
            var category = _context.Categories.SingleOrDefault(p => p.Id == categoryDto.Id);
            category.Name = categoryDto.Name;
            return _context.SaveChanges();
        }

        public CategoryDto Find(long id)
        {
            var category = _context.Categories.Find(id);
            return new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
    public class CategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}

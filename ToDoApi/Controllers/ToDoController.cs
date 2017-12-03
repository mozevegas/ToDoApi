using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    public class ToDoController : Controller
    {
        private readonly ToDoContext _context;
        public ToDoController(ToDoContext context)
        {
            _context = context;
            if (_context.ToDoItems.Count() == 0)
            {
                _context.ToDoItems.Add(new ToDoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<ToDoItem> GetAll()
        {
            return _context.ToDoItems.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetToDo")]
        public IActionResult GetById(long id)
        {
            var item = _context.ToDoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Create([FromBody] ToDoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            _context.ToDoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetToDo", new { id = item.Id }, item);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ToDoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = _context.ToDoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.ToDoItems.Update(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.ToDoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}

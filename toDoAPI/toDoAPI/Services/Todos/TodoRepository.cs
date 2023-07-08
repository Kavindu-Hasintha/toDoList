using Microsoft.EntityFrameworkCore.Diagnostics;
using toDoAPI.Data;
using toDoAPI.Models;

namespace toDoAPI.Services.Todos
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DataContext _context;
        public TodoRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Todo> GetTodos()
        {
            return _context.Todos.OrderBy(t => t.Id).ToList();
        }

        public ICollection<Todo> GetTodos(int userId)
        {
            return _context.Todos.Where(t => t.User.Id == userId).ToList();
        }

        public Todo GetTodo(int todoId)
        {
            return _context.Todos.Where(t => t.Id == todoId).FirstOrDefault();
        }

        public bool TodoExists(int todoId)
        {
            return _context.Todos.Any(t => t.Id == todoId);
        }

        public bool CreateTodo(Todo todo)
        {
            _context.Add(todo);
            return Save();
        }

        public bool UpdateTodo(Todo todo)
        {
            _context.Update(todo);
            return Save();
        }

        public bool DeleteTodo(Todo todo)
        {
            _context.Remove(todo);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}

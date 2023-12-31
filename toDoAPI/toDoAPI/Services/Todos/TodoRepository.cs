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

        public async Task<List<Todo>> GetTodos(int userId)
        {
            return await _context.Todos.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<Todo> GetTodoById(int todoId)
        {
            return await _context.Todos.FirstOrDefaultAsync(t => t.Id == todoId);
        }

        public async Task<bool> TodoExists(int todoId)
        {
            return await _context.Todos.AnyAsync(t => t.Id == todoId);
        }

        public async Task<bool> TodoExists(int userId, string name)
        {
            return await _context.Todos.AnyAsync(t => t.UserId == userId && (t.TaskName.Trim().ToUpper() == name.Trim().ToUpper()));
        }

        public async Task<bool> AddTodo(Todo todo)
        {
            _context.Add(todo);
            return await Save();
        }

        public async Task<bool> UpdateTodo(Todo todo)
        {
            try
            {
                _context.Update(todo);
                _context.Entry(todo).Property(t => t.UserId).IsModified = false;
                return await Save();
            } catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteTodo(Todo todo)
        {
            _context.Remove(todo);
            return await Save();
        }

        public async Task<bool> Save()
        {
            try
            {
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }

    }
}

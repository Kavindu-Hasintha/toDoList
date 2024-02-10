namespace toDoAPI.Repositories.TodoRepository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DataContext _context;
        public TodoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Todo>> GetAllTasksAsync()
        {
            return await _context.Todos.OrderBy(t => t.Id).ToListAsync();
        }

        public async Task<Todo> GetTodoByIdAsync(int todoId)
        {
            return await _context.Todos.FirstOrDefaultAsync(t => t.Id == todoId);
        }

        public async Task<IEnumerable<Todo>> GetTodosByUserIdAsync(int userId)
        {
            return await _context.Todos.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<bool> TodoExistsByIdAsync(int todoId)
        {
            return await _context.Todos.AnyAsync(t => t.Id == todoId);
        }

        public async Task<bool> TodoExistsByUserIdTodoNameAsync(int userId, string name)
        {
            return await _context.Todos.AnyAsync(t => t.UserId == userId && (t.TaskName.Trim().ToUpper() == name.Trim().ToUpper()));
        }

        public async Task<bool> AddTodo(Todo todo)
        {
            try
            {
                _context.Todos.Add(todo);
                return await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTodo(Todo todo)
        {
            try
            {
                _context.Update(todo);
                _context.Entry(todo).Property(t => t.UserId).IsModified = false;
                return await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteTodo(Todo todo)
        {
            try
            {
                _context.Remove(todo);
                return await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SaveChangesAsync()
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

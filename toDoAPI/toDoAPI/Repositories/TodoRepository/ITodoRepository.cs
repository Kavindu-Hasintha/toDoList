namespace toDoAPI.Repositories.TodoRepository
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllTasksAsync();
        Task<IEnumerable<Todo>> GetTodosByUserIdAsync(int userId);
        Task<Todo> GetTodoByIdAsync(int todoId);
        Task<bool> TodoExistsByIdAsync(int todoId);
        Task<bool> TodoExistsByUserIdTodoNameAsync(int userId, string name);
        Task<bool> AddTodo(Todo todo);
        Task<bool> UpdateTodo(Todo todo);
        Task<bool> DeleteTodo(Todo todo);
        Task<bool> SaveChangesAsync();
    }
}

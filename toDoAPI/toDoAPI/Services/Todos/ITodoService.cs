using toDoAPI.Models;

namespace toDoAPI.Services.Todos
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDetailsDto>> GetAllTasks();
        Task<List<Todo>> GetAllTasksAsync();

        Task<List<Todo>> GetTodos(int userId);

        Task<Todo> GetTodoById(int todoId);

        Task<bool> TodoExists(int todoId);

        Task<bool> TodoExists(int userId, string name);

        Task<bool> AddTodo(Todo todo);

        Task<bool> UpdateTodo(Todo todo);

        Task<bool> DeleteTodo(Todo todo);

        Task<bool> Save();
    }
}

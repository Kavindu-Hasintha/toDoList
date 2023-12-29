using toDoAPI.Models;

namespace toDoAPI.Services.Todos
{
    public interface ITodoRepository
    {
        ICollection<Todo> GetTodos();

        Task<List<Todo>> GetTodos(int userId);

        Todo GetTodo(int todoId);
        bool TodoExists(int todoId);

        Task<bool> TodoExists(int userId, string name);

        Task<bool> AddTodo(Todo todo);

        Task<bool> UpdateTodo(Todo todo);
        Task<bool> DeleteTodo(Todo todo);
        Task<bool> Save();
    }
}

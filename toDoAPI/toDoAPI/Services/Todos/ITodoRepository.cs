using toDoAPI.Models;

namespace toDoAPI.Services.Todos
{
    public interface ITodoRepository
    {
        ICollection<Todo> GetTodos();
        ICollection<Todo> GetTodos(int userId);
        Todo GetTodo(int todoId);
        bool TodoExists(int todoId);
        bool CreateTodo(Todo todo);
        bool UpdateTodo(Todo todo);
        bool DeleteTodo(Todo todo);
        bool Save();
    }
}

namespace toDoAPI.Services.Todos
{
    public interface ITodoService
    {
        Task<OperationResult> AddTodoAsync(TodoCreateDto request);
        Task<OperationResult> UpdateTodoAsync(TodoDto request);
        Task<OperationResult> DeleteTodoAsync(int todoId);
        Task<IEnumerable<TodoDetailsDto>> GetAllTasksAsync();
        Task<IEnumerable<TodoDto>> GetTasksByUserIdAsync();
    }
}

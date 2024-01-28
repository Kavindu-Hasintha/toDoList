namespace toDoAPI.Repositories.TodoRepository
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllTasksAsync();
        Task<IEnumerable<Todo>> GetTodosByUserIdAsync(int userId);
        Task<bool> SaveChangesAsync();
    }
}

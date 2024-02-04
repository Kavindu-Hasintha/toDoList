using AutoMapper;
using Microsoft.EntityFrameworkCore.Diagnostics;
using toDoAPI.Data;
using toDoAPI.Dto;
using toDoAPI.Enums;
using toDoAPI.Models;
using toDoAPI.Repositories.TodoRepository;

namespace toDoAPI.Services.Todos
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public TodoService(ITodoRepository todoRepository, IUserService userService, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<OperationResult> AddTodoAsync(TodoCreateDto request)
        { 
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException("Request body is null.");
                }

                int userId = await _userService.GetUserId();
                if (userId == 0)
                {
                    return OperationResult.NotFound;
                }

                if (request.TaskName.Length == 0 || request.StartDate.ToString().Length == 0 || request.DueDate.ToString().Length == 0)
                {
                    return OperationResult.InvalidInput;
                }

                var isTodoExists = await _todoRepository.TodoExistsByUserIdTodoNameAsync(userId, request.TaskName);
                if (isTodoExists)
                {
                    return OperationResult.AlreadyExists;
                }

                var todoMap = _mapper.Map<Todo>(request);
                todoMap.UserId = userId;

                var isAdded = await _todoRepository.AddTodo(todoMap);
                if (!isAdded)
                {
                    return OperationResult.Error;
                }

                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Error;
            }
        }

        public async Task<OperationResult> UpdateTodoAsync(TodoDto request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException("Request body is null.");
                }

                if (request.TaskName.Length == 0 || request.StartDate.ToString().Length == 0 || request.DueDate.ToString().Length == 0)
                {
                    return OperationResult.InvalidInput;
                }

                var isTaskExists = await _todoRepository.TodoExistsByIdAsync(request.Id);
                if (!isTaskExists)
                {
                    return OperationResult.NotFound;
                }

                var todoMap = _mapper.Map<Todo>(request);

                var isUpdated = await _todoRepository.UpdateTodo(todoMap);
                if (!isUpdated)
                {
                    return OperationResult.Error;
                }

                return OperationResult.Success;

            }
            catch (Exception ex)
            {
                return OperationResult.Error;
            }
        }

            public async Task<IEnumerable<TodoDetailsDto>> GetAllTasks()
        {
            var tasks = await _todoRepository.GetAllTasksAsync();
            return _mapper.Map<List<TodoDetailsDto>>(tasks);
        }

        public async Task<IEnumerable<TodoDto>> GetTasksByUserId()
        {
            int userId = await _userService.GetUserId();
            if (userId == 0)
            {
                return null;
            }

            var tasks = await _todoRepository.GetTodosByUserIdAsync(userId);
            return _mapper.Map<List<TodoDto>>(tasks);
        }

        public async Task<List<Todo>> GetAllTasksAsync()
        {
            try
            {
                var tasks = await _context.Todos.OrderBy(t => t.Id).ToListAsync();
                return tasks;
            }
            catch (Exception ex)
            {
                throw;
            }
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
            } 
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteTodo(Todo todo)
        {
            try
            {
                _context.Remove(todo);
                return await Save();
            } 
            catch(Exception ex)
            {
                return false;
            }
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

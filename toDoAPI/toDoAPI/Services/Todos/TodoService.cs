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

        public async Task<OperationResult> DeleteTodoAsync(int todoId)
        {
            try
            {
                var todoDelete = await _todoRepository.GetTodoByIdAsync(todoId);
                if (todoDelete == null)
                {
                    return OperationResult.NotFound;
                }

                var isDeleted = await _todoRepository.DeleteTodo(todoDelete);
                if (!isDeleted)
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

        public async Task<IEnumerable<TodoDetailsDto>> GetAllTasksAsync()
        {
            var tasks = await _todoRepository.GetAllTasksAsync();
            return _mapper.Map<List<TodoDetailsDto>>(tasks);
        }

        public async Task<IEnumerable<TodoDto>> GetTasksByUserIdAsync()
        {
            int userId = await _userService.GetUserId();
            if (userId == 0)
            {
                return null;
            }

            var tasks = await _todoRepository.GetTodosByUserIdAsync(userId);
            return _mapper.Map<List<TodoDto>>(tasks);
        }
    }
}

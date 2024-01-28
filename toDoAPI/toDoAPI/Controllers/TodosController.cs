using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using toDoAPI.Dto;
using toDoAPI.Models;
using toDoAPI.Services.Todos;
using toDoAPI.Services.Users;

namespace toDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : Controller
    {
        private readonly ITodoService _todoRepository;
        private readonly IUserService _userRepository;
        private readonly IMapper _mapper;
        public TodosController (ITodoService todoRepository, IUserService userRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getalltasks")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _todoRepository.GetAllTasksAsync();

                var tasksMap = _mapper.Map<List<TodoDetailsDto>>(tasks);

                return Ok(tasksMap);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        
        [HttpGet]
        [Route("getasksforuser")]
        [Authorize]
        public async Task<IActionResult> GetTasksForUser()
        {
            int userId = await _userRepository.GetUserId();
            if (userId == 0)
            {
                return NotFound();
            }

            var todos = await _todoRepository.GetTodos(userId);

            var mappedTodos = _mapper.Map<List<TodoDto>>(todos);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mappedTodos);
        }

        
        [HttpPost]
        [Route("addtask")]
        [Authorize]
        public async Task<IActionResult> AddTask([FromBody] TodoCreateDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            int userId = await _userRepository.GetUserId();
            if (userId == 0)
            {
                return BadRequest();
            }

            var isTaskExist = await _todoRepository.TodoExists(userId, request.TaskName);

            if (isTaskExist)
            {
                ModelState.AddModelError("TodoError", "Todo already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState); 
            }

            var todoMap = _mapper.Map<Todo>(request);
            todoMap.UserId = userId;

            var isAdded = await _todoRepository.AddTodo(todoMap);

            if (!isAdded)
            {
                ModelState.AddModelError("TodoError", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Added.");
        }

        
        [HttpPut]
        [Route("updatetask")]
        [Authorize]
        public async Task<IActionResult> UpdateTask([FromBody] TodoDto todoUpdate)
        {
            try
            {
                if (todoUpdate == null)
                {
                    return BadRequest("Invalid request data");
                }

                var isTaskExist = await _todoRepository.TodoExists(todoUpdate.Id);

                if (!isTaskExist)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingTodo = await _todoRepository.GetTodoById(todoUpdate.Id);

                if (existingTodo == null)
                {
                    return NotFound();
                }

                _mapper.Map(todoUpdate, existingTodo);

                var isUpdated = await _todoRepository.UpdateTodo(existingTodo);

                if (!isUpdated)
                {
                    ModelState.AddModelError("TodoError", "Something went wrong while updating.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully Updated.");
            } 
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        
        [HttpDelete]
        [Route("deletetask")]
        [Authorize]
        public async Task<IActionResult> DeleteTask([FromQuery] int todoId)
        {
            try
            {
                var isTaskExist = await _todoRepository.TodoExists(todoId);
                
                if (!isTaskExist)
                {
                    return NotFound();
                }

                var todoDelete = await _todoRepository.GetTodoById(todoId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isDeleted = await _todoRepository.DeleteTodo(todoDelete);

                if (!isDeleted)
                {
                    ModelState.AddModelError("TodoError", "Something went wrong while deleting.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully Deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        
    }
}

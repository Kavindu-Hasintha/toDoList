using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using toDoAPI.Dto;
using toDoAPI.Enums;
using toDoAPI.Models;
using toDoAPI.Services.Todos;
using toDoAPI.Services.Users;

namespace toDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : Controller
    {
        private readonly ITodoService _todoService;
        private readonly IUserService _userRepository;
        private readonly IMapper _mapper;
        public TodosController (ITodoService todoService, IUserService userRepository, IMapper mapper)
        {
            _todoService = todoService;
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
                var tasks = await _todoService.GetAllTasks();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        
        [HttpGet]
        [Route("getasksforuser")]
        [Authorize]
        public async Task<IActionResult> GetTasksByUserId()
        {
            try
            {
                var tasks = await _todoService.GetTasksByUserId();

                if (tasks == null)
                {
                    return NotFound();
                }
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        
        [HttpPost]
        [Route("addtask")]
        [Authorize]
        public async Task<IActionResult> AddTask([FromBody] TodoCreateDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _todoService.AddTodoAsync(request);

                return result switch
                {
                    OperationResult.Success => NoContent(),
                    OperationResult.NotFound => NotFound(),
                    OperationResult.InvalidInput => StatusCode(422, "Invalid Input"),
                    OperationResult.AlreadyExists => StatusCode(422, "Task already exists"),
                    OperationResult.Error => StatusCode(500, "Something went wrong while saving task"),
                    _ => BadRequest(),
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
            /*
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
            */
        }

        
        [HttpPut]
        [Route("updatetask")]
        [Authorize]
        public async Task<IActionResult> UpdateTask([FromBody] TodoDto todoUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _todoService.UpdateTodoAsync(todoUpdate);
                /*
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
                */
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

namespace toDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodosController (ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        [Route("getalltasks")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _todoService.GetAllTasksAsync();
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
                var tasks = await _todoService.GetTasksByUserIdAsync();

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

                return result switch
                {
                    OperationResult.Success => NoContent(),
                    OperationResult.InvalidInput => StatusCode(422, "Invalid Input"),
                    OperationResult.NotFound => NotFound(),
                    OperationResult.Error => StatusCode(500, "Something went wrong while updating task"),
                    _ => BadRequest(),
                };
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _todoService.DeleteTodoAsync(todoId);

                return result switch
                {
                    OperationResult.Success => NoContent(),
                    OperationResult.NotFound => NotFound(),
                    OperationResult.Error => StatusCode(500, "Something went wrong while deleting task"),
                    _ => BadRequest(),
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        
    }
}

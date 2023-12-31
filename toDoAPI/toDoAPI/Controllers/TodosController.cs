﻿using AutoMapper;
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
        private readonly ITodoRepository _todoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public TodosController (ITodoRepository todoRepository, IUserRepository userRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _userRepository = userRepository;
            _mapper = mapper;
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

        /*
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTodo([FromBody] TodoDto todoUpdate)
        {
            if (todoUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_todoRepository.TodoExists(todoUpdate.Id))
            {
                return NotFound();
            }

            if (todoUpdate.TaskName.Length == 0 || todoUpdate.StartDate.ToString().Length == 0 || todoUpdate.DueDate.ToString().Length == 0)
            {
                ModelState.AddModelError("TodoError", "Please fill all the fields.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var todoMap = _mapper.Map<Todo>(todoUpdate);

            if (!_todoRepository.UpdateTodo(todoMap))
            {
                ModelState.AddModelError("TodoError", "Something went wrong while updating.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Updated.");
        }

        [HttpDelete("{todoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTodo(int todoId)
        {
            if (!_todoRepository.TodoExists(todoId))
            {
                return NotFound();
            }

            var todoDelete = _todoRepository.GetTodo(todoId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_todoRepository.DeleteTodo(todoDelete))
            {
                ModelState.AddModelError("TodoError", "Something went wrong while deleting.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Deleted.");
        }
        */
    }
}

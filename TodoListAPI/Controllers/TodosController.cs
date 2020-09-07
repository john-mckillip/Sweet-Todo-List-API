using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListAPI.Entities;
using TodoListAPI.Models;
using TodoListAPI.Services.Abstractions;

namespace TodoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ILogger<TodosController> _logger;
        private readonly ITodoListInfoRepository _todoListInfoRepository;
        private readonly IMapper _mapper;

        public TodosController(
            ILogger<TodosController> logger,
            ITodoListInfoRepository todoListInfoRepository,
             IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));

            _todoListInfoRepository = todoListInfoRepository ??
                throw new ArgumentNullException(nameof(todoListInfoRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper)); ;
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetTodos()
        {
            var todoEntities = await _todoListInfoRepository.GetTodos();

            return Ok(_mapper.Map<IEnumerable<TodoDto>>(todoEntities));
        }

        [HttpGet("{id}", Name ="GetTodo")]
        public async Task<IActionResult> GetTodo(Guid id)
        {
            var todo = await  _todoListInfoRepository.GetTodo(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TodoDto>(todo));
        }

        [HttpPost]
        [Route("", Name = "CreateTodo")]
        public async Task<IActionResult> CreateTodo([FromBody] TodoForCreationDto todo)
        {
            if (todo.Description == todo.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoEntity = _mapper.Map<Todo>(todo);
            todoEntity.Id = Guid.NewGuid();
            todoEntity.LastModified = DateTime.Now;

            var success = await _todoListInfoRepository.CreateTodo(todoEntity);

            if (!success)
            {
                System.Web.Http.HttpError err = new System.Web.Http.HttpError("An unexpected fault happened. Try again later");
                _logger.LogError("An exception happened when trying to save a todo entity");

                return StatusCode(500, err.ToString());
            }

            var todoToReturn = _mapper.Map<TodoDto>(todoEntity);

            return CreatedAtRoute(
                "GetTodo",
                new { id = todoToReturn.Id },
                todoToReturn);
        }

        [HttpPut("{id}", Name = "UpdateTodo")]
        public async Task<IActionResult> UpdateTodo(Guid id,
            [FromBody] TodoForUpdateDto todo)
        {
            if (todo.Description == todo.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (! await _todoListInfoRepository.TodoExists(id))
            {
                return NotFound();
            }

            var todoEntityForUpdate = await _todoListInfoRepository.GetTodo(id);
            if (todoEntityForUpdate == null)
            {
                return NotFound();
            }

            _mapper.Map(todo, todoEntityForUpdate);

            // Update Last Modified
            todoEntityForUpdate.LastModified = DateTime.Now;

            _todoListInfoRepository.UpdateTodo(id, todoEntityForUpdate);

            var success = await _todoListInfoRepository.Save();
            if (!success)
            {
                System.Web.Http.HttpError err = new System.Web.Http.HttpError("An unexpected fault happened. Try again later");
                _logger.LogError("An exception happened when trying to update a todo entity");

                return StatusCode(500, err.ToString());
            }

            return NoContent();
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateTodo")]
        public async Task<IActionResult> PartiallyUpdateTodo(Guid id,
            [FromBody] Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TodoForUpdateDto> patchDoc)
        {
            if (!await _todoListInfoRepository.TodoExists(id))
            {
                return NotFound();
            }

            var todoEntityForUpdate = await _todoListInfoRepository.GetTodo(id);
            if (todoEntityForUpdate == null)
            {
                return NotFound();
            }

            var todoToPatch = _mapper.Map<TodoForUpdateDto>(todoEntityForUpdate);

            patchDoc.ApplyTo(todoToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (todoToPatch.Description == todoToPatch.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }
            
            if (!TryValidateModel(todoToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(todoToPatch, todoEntityForUpdate);

            // Update Last Modified
            todoEntityForUpdate.LastModified = DateTime.Now;

            _todoListInfoRepository.UpdateTodo(id, todoEntityForUpdate);

            var success = await _todoListInfoRepository.Save();
            if (!success)
            {
                System.Web.Http.HttpError err = new System.Web.Http.HttpError("An unexpected fault happened. Try again later");
                _logger.LogError("An exception happened when trying to partially update a todo entity");

                return StatusCode(500, err.ToString());
            }

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteTodo")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            if (!await _todoListInfoRepository.TodoExists(id))
            {
                return NotFound();
            }

            var todoEntityToDelete = await _todoListInfoRepository.GetTodo(id);
            if (todoEntityToDelete == null)
            {
                return NotFound();
            }

            _todoListInfoRepository.DeleteTodo(todoEntityToDelete);

            var success = await _todoListInfoRepository.Save();
            if (!success)
            {
                System.Web.Http.HttpError err = new System.Web.Http.HttpError("An unexpected fault happened. Try again later");
                _logger.LogError("An exception happened when trying to delete a todo entity");

                return StatusCode(500, err.ToString());
            }

            return NoContent();
        }
    }
}

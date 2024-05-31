using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/tasks")]
public class TodoController : ControllerBase
{
  private readonly TodoContext _context;
  public TodoController(TodoContext context)
  {
    _context = context;
  }
  [HttpGet]
  public async Task<ActionResult> GetTodos()
  {
    var tarea = await _context.tareas.ToListAsync();
    return Ok(tarea);
  }
  [HttpGet("{id}")]
  public async Task<ActionResult> GetTodoById(string id)
  {
    try
    {
      var tarea = await _context.tareas.FindAsync(id);

      if (tarea == null)
      {
        return NotFound();
      }

      return Ok(tarea);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, $"Se produjo un error al buscar la tarea: {ex.Message}");
    }
  }

  [HttpPost]
  public async Task<ActionResult> CreateTarea([FromBody] TareaDTO tarea)
  {
    try
    {
      var todo = new TareaModel
      {
        id = Guid.NewGuid().ToString(),
        descripcion = tarea.descripcion,
        usuarioid = tarea.usuarioid,
        fecha = DateTime.UtcNow,
      };

      _context.tareas.Add(todo);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetTodoById), new { todo.id }, todo);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, $"Se produjo un error al crear la tarea: {ex.Message}");
    }
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteTarea(string id)
  {
    try
    {
      var tarea = await _context.tareas.FindAsync(id);
      if (tarea == null)
      {
        return NotFound();
      }

      _context.tareas.Remove(tarea);
      await _context.SaveChangesAsync();

      return Ok();
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, $"Se produjo un error al eliminar la tarea: {ex.Message}");
    }
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateTarea(string id, TareaDTO tareaDTO)
  {
    try
    {
      var tarea = await _context.tareas.FindAsync(id);
      if (tarea is null)
      {
        return NotFound();
      }

      tarea.descripcion = tareaDTO.descripcion;
      await _context.SaveChangesAsync();

      return Ok(tarea);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error al actualizar la tarea: {ex.Message}");
      return StatusCode(StatusCodes.Status500InternalServerError, "Se produjo un error al actualizar la tarea");
    }
  }
}

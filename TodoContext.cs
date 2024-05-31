using Microsoft.EntityFrameworkCore;

public class TodoContext : DbContext
{
  public TodoContext(DbContextOptions<TodoContext> opt) : base(opt)
  {

  }
  public DbSet<UsuarioModel> usuarios { get; set; }
  public DbSet<TareaModel> tareas { get; set; }
}
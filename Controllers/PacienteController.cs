using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PacienteController : ControllerBase
{
    private readonly PacienteRepository _repo;

    public PacienteController(PacienteRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public IActionResult ObtenerTodos()
    {
        return Ok(_repo.ObtenerTodos());
    }
}  
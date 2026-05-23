using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HospitalContext>(options =>
    options.UseSqlite("Data Source=hospital.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PacienteRepository>();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// muestra todo los pacientes 
app.MapGet("/pacientes", (PacienteRepository repo) =>
{
    return repo.ObtenerTodos();
});
//muestra personas 
//app.MapGet("/persona",()=>
//{
  //  return personal;

//});
//pacientes con fiebre 
app.MapGet("/PACIENTES/CON-FIEBRE", (PacienteRepository repo) =>
{
    return repo.ObtenerConFiebre();
});
//pacientes mayores de 40 
app.MapGet("/PACIENTES/MAYORES-DE-40", (PacienteRepository repo) =>
{
    return repo.ObtenerMayoresDe40();
});
//pacientes sin obra social 
app.MapGet("/PACIENTES/SIN-OBRA-SOCIAL",(PacienteRepository repo)=>
{
   return repo.ObtenerConObraSocial();
   

});
//pacientes ordenados por edad (mayor a menor)
app.MapGet("/PACIENTES-ORDENADOS-POR-EDAD",(PacienteRepository repo)=>
{
  return repo.ObtenerOrdenadoPorEdad();
});
//Buscar paciente por nombre 
app.MapGet("/paciente/buscar/{nombre}",(string nombre, PacienteRepository repo)=>
{
    var encontrado = repo.ObtenerPorNombre(nombre);

    if (encontrado != null)
    {
       return Results.Ok(encontrado);
    }
    else
    {
        return Results.NotFound (new{mensaje = "Paciente no encontrado"});
    }
});
//borrar paciente 
app.MapDelete("/paciente/buscar/{nombre}",(string nombre, PacienteRepository repo)=>
{
     var encontrado = repo.ObtenerPorNombre(nombre);
     if (encontrado != null)
    {
       repo.Eliminar(nombre);  
        return Results.Ok(new { mensaje = "Paciente eliminado" });
    }
    else
    {
        return Results.NotFound (new{MENSAJE = "Paciente no encontrado"});

    }
});
    // modificar paciente
app.MapPut("/paciente/buscar/{nombre}",(string nombre , Paciente datosNuevos, PacienteRepository repo)=>
{
    var encontrado = repo.ObtenerPorNombre(nombre);
    if (encontrado != null)
    {
        repo.Modificar(nombre, datosNuevos);
        return Results.Ok(new{ mensaje = "paciente modificado"});
    }
    else
    {
        return Results.NotFound (new{mensaje = "paciente no encontrado"});

    }
 });

//agrega paciente
app.MapPost("/pacientes", (Paciente nuevoPaciente, PacienteRepository repo) =>
{
    repo.Agregar(nuevoPaciente);
   
    return Results.Ok(nuevoPaciente);
});
app.Run ();

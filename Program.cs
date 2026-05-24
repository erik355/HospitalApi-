using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HospitalContext>(options =>
    options.UseSqlite("Data Source=hospital.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PacienteRepository>();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


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
}).RequireAuthorization();
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
 }).RequireAuthorization();

//agrega paciente
app.MapPost("/pacientes", (Paciente nuevoPaciente, PacienteRepository repo) =>
{
    repo.Agregar(nuevoPaciente);
   
    return Results.Ok(nuevoPaciente);
}).RequireAuthorization();
//Usuario y contraseña
app.MapPost("/login", (Usuario usuario) =>
{
    if (usuario.NombreUsuario == "admin" && usuario.Contrasena == "1234")
    {
        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("esta-es-mi-clave-secreta-super-larga-123456"));
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "HospitalApi",
            audience: "HospitalApi",
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credenciales
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(new { token = tokenString });
    }
    else
    {
        return Results.Unauthorized();
    }
});
app.Run ();

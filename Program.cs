using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HospitalContext>(options =>
    options.UseSqlite("Data Source=hospital.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// muestra todo los pacientes 
app.MapGet("/pacientes", (HospitalContext db) =>
{
    return db.Pacientes.ToList();
});
//muestra personas 
//app.MapGet("/persona",()=>
//{
  //  return personal;

//});
//pacientes con fiebre 
app.MapGet("/PACIENTES/CON-FIEBRE",(HospitalContext db)=>
{
   var CONFIEBRE = db.Pacientes.Where(p=>p.Temperatura > 37.5);
   return CONFIEBRE;

});
//pacientes mayores de 40 
app.MapGet("/PACIENTES/MAYORES-DE-40",(HospitalContext db)=>
{
    var MAYORESDE40 = db.Pacientes.Where(p=> p.Edad > 40);
    return MAYORESDE40;
});
//pacientes sin obra social 
app.MapGet("/PACIENTES/SIN-OBRA-SOCIAL",(HospitalContext db)=>
{
   var SINOBRASOCIAL = db.Pacientes.Where(p=>p.ObraSocial ==false);
   return SINOBRASOCIAL;

});
//pacientes ordenados por edad (mayor a menor)
app.MapGet("/PACIENTES-ORDENADOS-POR-EDAD",(HospitalContext db)=>
{
    var ORDENADOSPOREDAD = db.Pacientes.OrderByDescending(p => p.Edad);
    return ORDENADOSPOREDAD;

});
//Buscar paciente por nombre 
app.MapGet("/paciente/buscar/{nombre}",(string nombre, HospitalContext db)=>
{
    var encontrado = db.Pacientes.FirstOrDefault(p=> p.Nombre.ToLower().Contains(nombre.ToLower()));

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
app.MapDelete("/paciente/buscar/{nombre}",(string nombre, HospitalContext db)=>
{
     var encontrado = db.Pacientes.FirstOrDefault(p=> p.Nombre.ToLower().Contains(nombre.ToLower()));
     if (encontrado != null)
    {
        db.Pacientes.Remove(encontrado);
        db.SaveChanges();
        return Results.Ok(new { mensaje = "Paciente eliminado" });
    }
    else
    {
        return Results.NotFound (new{MENSAJE = "Paciente no encontrado"});

    }
});
    // modificar paciente
app.MapPut("/paciente/buscar/{nombre}",(string nombre , Paciente datosNuevos, HospitalContext db)=>
{
    var encontrado = db.Pacientes.FirstOrDefault(P=> P.Nombre.ToLower().Contains(nombre.ToLower()));
    if (encontrado != null)
    {
        encontrado.Diagnostico = datosNuevos.Diagnostico;
        encontrado.Temperatura = datosNuevos.Temperatura;
        encontrado.Internado = datosNuevos.Internado;
        encontrado.ObraSocial = datosNuevos.ObraSocial;
        db.SaveChanges();
        return Results.Ok(new{ mensaje = "paciente modificado"});
    }
    else
    {
        return Results.NotFound (new{mensaje = "paciente no encontrado"});

    }
 });

//agrega pasiente
app.MapPost("/pacientes", (Paciente nuevoPaciente, HospitalContext db) =>
{
    db.Pacientes.Add(nuevoPaciente);
    db.SaveChanges();
    return Results.Ok(nuevoPaciente);
});
app.Run ();

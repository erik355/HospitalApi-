using Microsoft.EntityFrameworkCore;
using SQLitePCL;

public class PacienteRepository
{
    private HospitalContext _db;

    public PacienteRepository(HospitalContext db)
    {
        _db = db;
    }
//lista de pacientes
    public List<Paciente> ObtenerTodos()
{
    return _db.Pacientes.ToList();
}
//pacientes mayoresd de 40
public List<Paciente> ObtenerMayoresDe40()
{
    return _db.Pacientes.Where(p => p.Edad > 40).ToList();
}
//pacientes con fiabre 
public List<Paciente> ObtenerConFiebre()
    {
        return _db.Pacientes.Where(p => p.Temperatura > 37.5).ToList();
    }
// pacientes con obra social
public List<Paciente> ObtenerConObraSocial()
    {
        return _db.Pacientes.Where(p => p.ObraSocial == false).ToList();
    }
//Obtenet ordenados por edad
public List<Paciente> ObtenerOrdenadoPorEdad()
    {
        return _db.Pacientes.OrderByDescending(p => p.Edad).ToList();
    }
//buscar por nombre de paciente
public Paciente? ObtenerPorNombre (string nombre)
    {
        return _db.Pacientes.FirstOrDefault(P=> P.Nombre.ToLower().Contains(nombre.ToLower()));   
    }
// agregar paciente
public void Agregar(Paciente nuevoPaciente)
{
    _db.Pacientes.Add(nuevoPaciente);
    _db.SaveChanges();
}
// modificar paciente
public void Modificar(string nombre, Paciente datosNuevos)
{
    var encontrado = _db.Pacientes.FirstOrDefault(p => p.Nombre.ToLower().Contains(nombre.ToLower()));
    if (encontrado != null)
    {
        encontrado.Diagnostico = datosNuevos.Diagnostico;
        encontrado.Temperatura = datosNuevos.Temperatura;
        encontrado.Internado = datosNuevos.Internado;
        encontrado.ObraSocial = datosNuevos.ObraSocial;
        _db.SaveChanges();
    }
}
// eliminar paciente}
public void Eliminar(string nombre)
{
     var encontrado = _db.Pacientes.FirstOrDefault(p=> p.Nombre.ToLower().Contains(nombre.ToLower()));
     if (encontrado != null)
    {
        _db.Pacientes.Remove(encontrado);
        _db.SaveChanges();
    }
}
}

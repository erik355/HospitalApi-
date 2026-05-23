
public class Paciente : Persona
{
    public int Id { get; set; }
    public string Nacionalidad { get; set; } = "";
    public string EstadoCivil { get; set; } = "";
    public string Diagnostico { get; set; } = "";
    public double Temperatura { get; set; } = 0;
    public bool Internado { get; set; } = false;
    public bool ObraSocial { get; set; } = false;
   
}

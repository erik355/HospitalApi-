using Microsoft.EntityFrameworkCore;

public class HospitalContext : DbContext
{
    public HospitalContext(DbContextOptions<HospitalContext> options) : base(options) { }

    public DbSet<Paciente> Pacientes { get; set; }
}
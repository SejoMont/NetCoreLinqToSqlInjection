namespace NetCoreLinqToSqlInjection.Models
{
    public interface ICoche
    {
        string Marca { get; set; }
        string Modelo { get; set; }
        string Imagen { get; set; }
        int Velocidad { get; set; }
        int VelocidadMaxima { get; set; }
        int Acelerar();
        int Frenar();

    }
}

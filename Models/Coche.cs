namespace NetCoreLinqToSqlInjection.Models
{
    public class Coche : ICoche
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Imagen { get; set; }
        public int Velocidad { get; set; }
        public int VelocidadMaxima { get; set; }

        public Coche()
        {
            this.Marca = "Mazda";
            this.Modelo = "Miata 👉👈";
            this.Imagen = "miata.jpg";
            this.Velocidad = 0;
            this.VelocidadMaxima = 270;
        }

        public int Acelerar()
        {
            this.Velocidad += 20;
            if (this.Velocidad >= this.VelocidadMaxima)
            {
                this.Velocidad = this.VelocidadMaxima;
            }
            return this.Velocidad;
        }

        public int Frenar()
        {
            this.Velocidad -= 20;
            if (this.Velocidad < 0)
            {
                this.Velocidad = 0;
            }
            return this.Velocidad;

        }
    }
}

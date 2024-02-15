using NetCoreLinqToSqlInjection.Models;

namespace NetCoreLinqToSqlInjection.Repositories
{
    public interface IRepositoryPersonajes
    {
        List<Personaje> GetPersonajes();
        void InsertPersonaje(int id, string nombre, string imagen);

    }
}

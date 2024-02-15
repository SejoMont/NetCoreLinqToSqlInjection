using NetCoreLinqToSqlInjection.Models;
using System.Data.SqlClient;
using System.Data;

namespace NetCoreLinqToSqlInjection.Repositories
{
    public class RepositoryPersonajesSQL : IRepositoryPersonajes
    {
        private DataTable tablaPersonajes;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryPersonajesSQL()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS01;Initial Catalog=NETCORE;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;

            this.tablaPersonajes = new DataTable();
            string sql = "select * from PERSONAJES";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tablaPersonajes);
        }
        public List<Personaje> GetPersonajes()
        {
            var consulta = from datos in this.tablaPersonajes.AsEnumerable() select datos;

            List<Personaje> personajes = new List<Personaje>();
            foreach (var row in consulta)
            {
                Personaje per = new Personaje
                {
                    IdPersonaje = row.Field<int>("IDPERSONAJE"),
                    Nombre = row.Field<string>("PERSONAJE"),
                    Imagen = row.Field<string>("IMAGEN"),
                };
                personajes.Add(per);
            }
            return personajes;
        }

        public void InsertPersonaje(int id, string nombre, string imagen)
        {
            string sql = "insert into PERSONAJES values (@idpersonaje, @nombre, @imagen)";

            this.com.Parameters.AddWithValue("@idpersonaje", id);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);


            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();

            this.com.Parameters.Clear();
        }
    }
}

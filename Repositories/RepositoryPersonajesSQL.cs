using NetCoreLinqToSqlInjection.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region MyRegion
//create procedure SP_DELETE_PERSONAJE
//(@idpersonaje int)
//as
//	delete from PERSONAJES where IDPERSONAJE=@idpersonaje
//go

//create procedure SP_INSERT_PERSONAJE
//(
//@IDPERSONAJE INT,
//@PERSONAJE NVARCHAR(30),
//@IMAGEN NVARCHAR(30)
//)
//AS
//INSERT INTO PERSONAJES
//     VALUES
//           (@IDPERSONAJE, @PERSONAJE, @IMAGEN)
//GO

//create procedure SP_UPDATE_PERSONAJE
//(
//@IDPERSONAJE INT,
//@PERSONAJE NVARCHAR(30),
//@IMAGEN NVARCHAR(30)
//)
//AS
//     UPDATE PERSONAJES
//    SET 
//        PERSONAJE = @PERSONAJE,
//        IMAGEN = @IMAGEN
//    WHERE
//        IDPERSONAJE = @IDPERSONAJE;
//GO

#endregion

namespace NetCoreLinqToSqlInjection.Repositories
{
    public class RepositoryPersonajesSQL : IRepositoryPersonajes
    {
        private DataTable tablaPersonajes;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryPersonajesSQL()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=NETCORE;Persist Security Info=True;User ID=SA;Password=MCSD2023";
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
            this.com.Parameters.AddWithValue("@IDPERSONAJE", id);
            this.com.Parameters.AddWithValue("@PERSONAJE", nombre);
            this.com.Parameters.AddWithValue("@IMAGEN", imagen);


            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_PERSONAJE";

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void DeletePersonajes(int idpersonaje)
        {
            this.com.Parameters.AddWithValue("@idpersonaje", idpersonaje);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_PERSONAJE";

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Personaje GetPersonaje(int id)
        {
            var consulta = from datos in this.tablaPersonajes.AsEnumerable()
                           where datos.Field<int>("IDPERSONAJE") == id
                           select datos;
            var row = consulta.First();
            Personaje personaje = new Personaje
            {

                IdPersonaje = row.Field<int>("IDPERSONAJE"),
                Nombre = row.Field<string>("PERSONAJE"),
                Imagen = row.Field<string>("IMAGEN")
            };
            return personaje;
        }

        public void UpdatePersonaje(Personaje personaje)
        {
            this.com.Parameters.AddWithValue("@IDPERSONAJE", personaje.IdPersonaje);
            this.com.Parameters.AddWithValue("@PERSONAJE", personaje.Nombre);
            this.com.Parameters.AddWithValue("@IMAGEN", personaje.Imagen);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_UPDATE_PERSONAJE";

            this.cn.OpenAsync();
            int af = this.com.ExecuteNonQuery();

            this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}

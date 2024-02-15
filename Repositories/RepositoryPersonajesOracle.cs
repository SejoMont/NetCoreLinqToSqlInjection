using Microsoft.AspNetCore.Mvc;
using NetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace NetCoreLinqToSqlInjection.Repositories
{
    public class RepositoryPersonajesOracle : IRepositoryPersonajes
    {
        private DataTable tablaPersonajes;
        private OracleConnection cn;
        private OracleCommand com;
        public RepositoryPersonajesOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from PERSONAJES";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            this.tablaPersonajes = new DataTable();
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
            string sql = "insert into PERSONAJES values (:idpersonaje, :nombre, :imagen)";

            OracleParameter pamIdpersonaje = new OracleParameter(":idpersonaje", id);
            this.com.Parameters.Add(pamIdpersonaje);

            OracleParameter pamNombre = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(pamNombre);

            OracleParameter pamImagen = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(pamImagen);


            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}

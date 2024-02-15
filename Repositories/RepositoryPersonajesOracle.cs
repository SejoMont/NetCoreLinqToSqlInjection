using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region MyRegion
//create or replace procedure sp_delete_personaje
//(p_idpersonaje PERSONAJES.IDPERSONAJE%TYPE)
//as
//begin
//  delete from PERSONAJES where IDPERSONAJE=p_idpersonaje;
//commit;
//end;

//create or replace procedure sp_insert_personaje
//(
//p_idpersonaje PERSONAJES.IDPERSONAJE%TYPE,
//p_personaje PERSONAJES.PERSONAJE%TYPE,
//p_imagen PERSONAJES.IMAGEN%TYPE
//)
//as
//begin
//  INSERT INTO PERSONAJES VALUES(p_idpersonaje, p_personaje, p_imagen)
//commit;
//end;

//create or replace procedure sp_update_personaje
//(
//p_idpersonaje PERSONAJES.IDPERSONAJE%TYPE,
//p_personaje PERSONAJES.PERSONAJE%TYPE,
//p_imagen PERSONAJES.IMAGEN%TYPE
//)
//as
//begin
//  update PERSONAJES set PERSONAJE=p_personaje,
//                        IMAGEN = p_imagen
//                    where IDPERSONAJE=p_idpersonaje;
//commit;
//end;
#endregion

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
            OracleParameter pamIdpersonaje = new OracleParameter(":idpersonaje", id);
            this.com.Parameters.Add(pamIdpersonaje);

            OracleParameter pamNombre = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(pamNombre);

            OracleParameter pamImagen = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(pamImagen);


            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_insert_personaje";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
        public void DeletePersonajes(int idpersonaje)
        {
            OracleParameter pamIdPersonaje = new OracleParameter(":p_idpersonaje", idpersonaje);
            this.com.Parameters.Add(pamIdPersonaje);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_delete_personaje";

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
            OracleParameter pamIdpersonaje = new OracleParameter(":p_idpersonaje", personaje.IdPersonaje);
            this.com.Parameters.Add(pamIdpersonaje);

            OracleParameter pamNombre = new OracleParameter(":p_nombre", personaje.Nombre);
            this.com.Parameters.Add(pamNombre);

            OracleParameter pamImagen = new OracleParameter(":p_imagen", personaje.Imagen);
            this.com.Parameters.Add(pamImagen);


            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_update_personaje";

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}

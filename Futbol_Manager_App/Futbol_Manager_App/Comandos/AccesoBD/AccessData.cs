using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Data.SQLite;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Comandos;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.AccesoBD
{
    /**
     * Interfaz con la Base de Datos
     * Permite recuperar toda la información de los encuentros, equipos, jugadores, árbitros, comentaristas, etc.
     */
    public class AccessData : IAccesoBD
    {
        private SQLiteConnection _conexion;


        /**
         * Constructor
         * Recibe la ruta en la que se encuentra la base de datos.
         * Crea la conexion a la base de datos a partir de la cadena de conexión
         */
        public AccessData(string rutaFichero)
        {
            string cad_conexion = @"Data Source=" + rutaFichero + ";Version=3;New=False;Compress=True;";

            try
            {
                _conexion = new SQLiteConnection(cad_conexion);
                _conexion.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al crear la conexión: " + e.Message);
            }
        }


        /**
         * Lista de encuentros
         * Recopilación del id y descripción de todos los encuentros
         */
        public List<Encuentro> ListaEncuentros()
        {
            SQLiteDataReader dataReader = ejecutarSelect("SELECT encuentros.id, EquipoL.nombre AS equipoL, EquipoV.nombre AS equipoV, encuentros.resultado_ida FROM equipos AS EquipoV INNER JOIN (equipos AS EquipoL INNER JOIN encuentros ON EquipoL.id = encuentros.id_equipo_local) ON EquipoV.id = encuentros.id_equipo_visitante;");
            
            List<Encuentro> lista = new List<Encuentro>();
            
            while (dataReader.Read()){
                Encuentro encuentro = new Encuentro();
                encuentro.Id = Convert.ToInt32(dataReader[0]);
                encuentro.Descripcion = dataReader["equipoL"] + " - " + dataReader["equipoV"];
                encuentro.Ida = dataReader["resultado_ida"].Equals("-");
                lista.Add(encuentro);
            }

            dataReader.Close();
            return lista;
        }

        /**
         * Datos de un encuentro
         * Recupera todos los datos relacionados a un encuentro
         */
        public EncuentroData DatosEncuentro(int idEncuentro)
        {
            EncuentroData datos = new EncuentroData();

            // Equipo Local
            datos.EquipoL = getEquipo(idEncuentro, true);
            datos.EquipoL.Jugadores = getJugadores(idEncuentro, true, true);
            foreach (Jugador j in datos.EquipoL.Jugadores)
                j.Equipo = datos.EquipoL;
            datos.EquipoL.Banquillo = getJugadores(idEncuentro, true, false);
            foreach (Jugador j in datos.EquipoL.Banquillo)
                j.Equipo = datos.EquipoL;

            // Equipo Visitante
            datos.EquipoV = getEquipo(idEncuentro, false);
            datos.EquipoV.Jugadores = getJugadores(idEncuentro, false, true);
            foreach (Jugador j in datos.EquipoV.Jugadores)
                j.Equipo = datos.EquipoV;
            datos.EquipoV.Banquillo = getJugadores(idEncuentro, false, false);
            foreach (Jugador j in datos.EquipoV.Banquillo)
                j.Equipo = datos.EquipoV;

            // Arbitros
            datos.Arbitros = getArbitros(idEncuentro);

            // Comentaristas
            datos.Comentaristas = getComentaristas(idEncuentro);

            // Resultado ida (Agregate)
            datos.ResultadoIda = getAgregate(idEncuentro);

            datos.DatosEncuentro = getDatosEncuentro(idEncuentro);

            return datos;
        }

        private Encuentro getDatosEncuentro(int idEncuentro)
        {
            SQLiteDataReader dataReader = ejecutarSelect("SELECT * FROM encuentros WHERE id = " + idEncuentro + ";");
            dataReader.Read();

            Encuentro encuentro = new Encuentro();

            encuentro.NombreCompeticion = Convert.ToString(dataReader["texto_nombre_competicion"]);
            encuentro.NombrePabellon = Convert.ToString(dataReader["nombre_pabellon"]);
            encuentro.Ciudad = Convert.ToString(dataReader["ciudad"]);
            encuentro.Fecha = Convert.ToString(dataReader["fecha"]);

            dataReader.Close();
            return encuentro;
        }


        private Equipo getEquipo(int idEncuentro, bool local)
        {
            SQLiteDataReader dataReader = ejecutarSelect("SELECT equipos.* FROM equipos INNER JOIN encuentros ON equipos.id = encuentros." + (local ? "id_equipo_local" : "id_equipo_visitante") + " WHERE encuentros.id=" + idEncuentro + ";");
            dataReader.Read();

            Equipo equipo = new Equipo();

            equipo.FullName = Convert.ToString(dataReader["nombre"]);
            equipo.ShortName = Convert.ToString(dataReader["nombre_corto"]);
            equipo.TeamCode = Convert.ToString(dataReader["siglas"]);
            equipo.Color1 = Color.FromArgb(Convert.ToInt32(dataReader["RGB1"]));
            equipo.Color2 = Color.FromArgb(Convert.ToInt32(dataReader["RGB2"]));
            equipo.Badge = Convert.ToString(dataReader["escudo"]);
            equipo.Local = local;

            equipo.Entrenador = new Jugador();
            equipo.Entrenador.FullName = Convert.ToString(dataReader["nombre_entrenador"]);
            equipo.Entrenador.ShortName = Convert.ToString(dataReader["nombre_corto_entrenador"]);
            equipo.Entrenador.RutaFoto = Convert.ToString(dataReader["ruta_foto_entrenador"]);
            equipo.Entrenador.SancionSiAmarilla = Convert.ToBoolean(dataReader["apercibido_entrenador"]);
            equipo.Entrenador.Equipo = equipo;
            equipo.Entrenador.Posicion = 0;

            equipo.Asistente = new Jugador();
            equipo.Asistente.FullName = Convert.ToString(dataReader["nombre_entrenador_asistente"]);
            equipo.Asistente.ShortName = Convert.ToString(dataReader["nombre_corto_entrenador_asistente"]);
            equipo.Asistente.RutaFoto = Convert.ToString(dataReader["ruta_foto_entrenador_asistente"]);
            equipo.Asistente.SancionSiAmarilla = Convert.ToBoolean(dataReader["apercibido_entrenador_asistente"]);
            equipo.Asistente.Equipo = equipo;
            equipo.Asistente.Posicion = -1;

            dataReader.Close();
            return equipo;
        }

        private List<Jugador> getJugadores(int idEncuentro, bool local, bool titular)
        {
            SQLiteDataReader dataReader;
            if (titular)
                dataReader = ejecutarSelect("SELECT plantillas.*, jugadores_usados.pos_x, jugadores_usados.pos_y FROM jugadores_usados INNER JOIN (plantillas INNER JOIN (encuentros INNER JOIN equipos ON encuentros." + (local ? "id_equipo_local" : "id_equipo_visitante") + " = equipos.id) ON plantillas.id_equipo = equipos.id) ON (jugadores_usados.id_encuentro = encuentros.id) AND (jugadores_usados.id_jugador = plantillas.id) WHERE (((encuentros.id) = " + idEncuentro + ") AND ((jugadores_usados.es_titular) = 1));");
            else
                dataReader = ejecutarSelect("SELECT plantillas.*, jugadores_usados.pos_x, jugadores_usados.pos_y FROM jugadores_usados INNER JOIN (plantillas INNER JOIN (encuentros INNER JOIN equipos ON encuentros." + (local ? "id_equipo_local" : "id_equipo_visitante") + " = equipos.id) ON plantillas.id_equipo = equipos.id) ON (jugadores_usados.id_encuentro = encuentros.id) AND (jugadores_usados.id_jugador = plantillas.id) WHERE (((encuentros.id) = " + idEncuentro + ") AND ((jugadores_usados.es_suplente) = 1));");

            List<Jugador> lista = new List<Jugador>();
            while (dataReader.Read())
            {
                Jugador jugador = new Jugador();
                jugador.FullName = Convert.ToString(dataReader["nombre"]);
                jugador.ShortName = Convert.ToString(dataReader["alias"]);
                jugador.RutaFoto = Convert.ToString(dataReader["ruta_foto"]);
                jugador.Number = Convert.ToInt32(dataReader["dorsal"]);                
                jugador.Capitan = Convert.ToInt32(dataReader["capitan"]) == 1;
                jugador.Matches = Convert.ToInt32(dataReader["partidos_jugados"]);
                jugador.Goals = Convert.ToInt32(dataReader["goles"]);                
                jugador.SancionSiAmarilla = Convert.ToBoolean(dataReader["apercibido"]);
                jugador.PosX = Convert.ToString(dataReader["pos_x"]);
                jugador.PosY = Convert.ToString(dataReader["pos_y"]);
                
                string posicionStr = Convert.ToString(dataReader["posicion"]);
                if (posicionStr == "Portero")
                    jugador.Posicion = Jugador.Portero;
                else if (posicionStr == "Defensa")
                    jugador.Posicion = Jugador.Defensa;
                else if (posicionStr == "Centrocampista")
                    jugador.Posicion = Jugador.Centrocampista;
                else // Delantero
                    jugador.Posicion = Jugador.Delantero;

                lista.Add(jugador);
            }

            dataReader.Close();
            return lista;
        }

        private List<ICommand> getArbitros(int idEncuentro)
        {
            try
            {
                SQLiteDataReader dataReader =
                     ejecutarSelect("SELECT arbitros.*, 1 AS cargo FROM encuentros, arbitros WHERE (encuentros.id_arbitro1 = arbitros.id) and (encuentros.id=" + idEncuentro + ") " +
                                "UNION SELECT arbitros.*, 2 AS cargo FROM encuentros, arbitros WHERE (encuentros.id_arbitro2 = arbitros.id) and (encuentros.id=" + idEncuentro + ") " +
                                "UNION SELECT arbitros.*, 3 AS cargo FROM encuentros, arbitros WHERE (encuentros.id_arbitro3 = arbitros.id) and (encuentros.id=" + idEncuentro + ") " +
                                "UNION SELECT arbitros.*, 4 AS cargo FROM encuentros, arbitros WHERE (encuentros.id_arbitro4 = arbitros.id) and (encuentros.id=" + idEncuentro + ");");
                

                List<ICommand> arbitros = new List<ICommand>();

                Arbitro arbitro1 = null, arbitro2 = null, arbitro3 = null, arbitro4 = null;

                int indice = 0;
                while (dataReader.Read())
                {
                    Arbitro arbitro = new Arbitro();
                    arbitro.FullName = Convert.ToString(dataReader["nombre"]);
                    arbitro.ShortName = Convert.ToString(dataReader["nombre_corto"]);
                    arbitro.Colegio = Convert.ToString(dataReader["colegio"]);
                    if (indice == 0)
                        arbitro.Cargo = Arbitro.arbitro1;
                    else if(indice == 1)
                        arbitro.Cargo = Arbitro.arbitro2;
                    else if (indice == 2)
                        arbitro.Cargo = Arbitro.arbitro3;
                    else if (indice == 3)
                        arbitro.Cargo = Arbitro.arbitro4;
                    if (!(dataReader["nacionalidad"] is DBNull))
                        arbitro.Nacionalidad = Convert.ToString(dataReader["nacionalidad"]);

                    arbitros.Add(new IdentificationCommand(arbitro));

                    switch (arbitro.Cargo)
                    {
                        case Arbitro.arbitro1:
                            arbitro1 = arbitro;
                            break;
                        case Arbitro.arbitro2:
                            arbitro2 = arbitro;
                            break;
                        case Arbitro.arbitro3:
                            arbitro3 = arbitro;
                            break;
                        case Arbitro.arbitro4:
                            arbitro4 = arbitro;
                            break;
                    }
                    indice++;
                }

                Arbitro arbitroDummy = new Arbitro();
                arbitroDummy.FullName = "";
                arbitroDummy.ShortName = "";
                arbitroDummy.Colegio = "";
                arbitroDummy.Cargo = 1;
                arbitroDummy.Nacionalidad = "";

                if (arbitro1 == null)
                    arbitro1 = arbitroDummy;

                if (arbitro2 == null)
                    arbitro2 = arbitroDummy;

                if (arbitro3 == null)
                    arbitro3 = arbitroDummy;

                if (arbitro4 == null)
                    arbitro4 = arbitroDummy;                                          

                if (arbitro1 != null && arbitro2 != null && arbitro3 != null && arbitro4 != null)
                    arbitros.Insert(0, new RefereesCommand(arbitro1, arbitro2, arbitro3, arbitro4));

                dataReader.Close();
                return arbitros;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error obteniendo arbitros: " + e.Message);
                return new List<ICommand>();
            }
        }

        private List<ICommand> getComentaristas(int idEncuentro)
        {
            try
            {
                SQLiteDataReader dataReader = ejecutarSelect("SELECT comentaristas.nombre, comentaristas.nombre_corto, comentaristas.cargo FROM encuentros, comentaristas WHERE (encuentros.id_comentarista1 = comentaristas.id or encuentros.id_comentarista2 = comentaristas.id or encuentros.id_comentarista3 = comentaristas.id ) and (encuentros.id=" + idEncuentro + ");");

                List<ICommand> comentaristas = new List<ICommand>();

                while (dataReader.Read())
                {
                    comentaristas.Add(new FreeTextCommand((string)dataReader["nombre"], (string)dataReader["nombre_corto"], (string)dataReader["cargo"]));
                }

                dataReader.Close();
                return comentaristas;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error obteniendo comentaristas: " + e.Message);
                return new List<ICommand>();
            } 
        }

        private string getAgregate(int idEncuentro)
        {
            SQLiteDataReader dataReader = ejecutarSelect("SELECT encuentros.resultado_ida FROM encuentros WHERE encuentros.id=" + idEncuentro + ";");
            dataReader.Read();

            string agregate = (string)dataReader["resultado_ida"];

            dataReader.Close();
            return agregate;
        }


        /**
         * Testea la conexión a la base de datos
         * Devuelve true si es posible conectar con la base de datos
         */
        public bool TestConexion()
        {
            if (AbreConexion())
            {
                CierraConexion();
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * Abre la conexión
         * Devuelve true si la conexión se ha abirto con éxito
         */
        public bool AbreConexion()
        {
            bool conexionOk = false;

            try
            {
                _conexion.Close();
                //if (_conexion.State == ConnectionState.Closed)
                //{
                    _conexion.Open();
                    conexionOk = true;
                //}
            }

            catch(Exception e)
            {
                Console.WriteLine("Error al abrir conexión: "+ e.Message);

                conexionOk = false;
            }

            return conexionOk;
        }

        /**
         * Cierra la conexión
         */
        public void CierraConexion()
        {
            if (_conexion.State == ConnectionState.Open)
                _conexion.Close();
        }


        // Metodo para lanzar sentencias tipo "Select" a la base de datos
        public SQLiteDataReader ejecutarSelect(string sql)
        {
            SQLiteDataReader sqlDataReader = null;
            SQLiteCommand comando = new SQLiteCommand();

            try
            {
                //Console.WriteLine(">>" + sql);

                // Compongo el comando que me hace falta para lanzarlo a la base de datos mas tarde
                comando.CommandText = sql;
                comando.Connection = this._conexion;
                sqlDataReader = comando.ExecuteReader();

                comando.Dispose();
            }

            catch (SQLiteException exc)
            {
                Console.ReadLine();
                Console.WriteLine("Excepción: NO se ha podido realizar la operacion");
                Console.WriteLine(exc);
            }

            return sqlDataReader;
        }


    }
}
using System.Collections.Generic;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Comandos;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.AccesoBD
{
    /**
     * Interfaz Dummy con la Base de Datos
     * En lugar de una conexión real con la base de datos, se trata de un "dummy" 
     * con datos de sintéticos para realizar pruebas.
     */
    public class DummyData : IAccesoBD
    {

        public List<Encuentro> ListaEncuentros()
        {
            List<Encuentro> lista = new List<Encuentro>();

            Encuentro encuentro = new Encuentro();
            encuentro.Id = 1;
            encuentro.Descripcion = "ATLÉTICO - LIVERPOOL";
            encuentro.Ida = false;
            lista.Add(encuentro);

            return lista;
        }


        public EncuentroData DatosEncuentro(int idEncuentro)
        {
            EncuentroData d = new EncuentroData();

            d.EquipoL = DummyData.getEquipoLocal();
            d.EquipoV = DummyData.getEquipoVisitante();
            d.Arbitros = DummyData.getArbitros();
            d.Comentaristas = DummyData.getComentaristas();

            d.ResultadoIda = "1-1";

            return d;
        }


        private static Equipo getEquipoLocal()
        {
            Equipo e = new Equipo();

            e.Local = true;
            e.TeamCode = "ATL";
            e.ShortName = "ATLÉTICO";
            e.FullName = "ATLÉTICO";
            e.Badge = "Atletico de Madrid_MP_HD.tga";
            e.Color1 = System.Drawing.Color.Red;
            //e.Campo = "Vicente Calderón";
            //e.Ciudad = "Madrid";

            e.Entrenador = new Jugador();
            e.Entrenador.FullName = "Quique Sánchez Flores";
            e.Entrenador.ShortName = "Quique";
            e.Entrenador.Equipo = e;

            Jugador[] jugadores = new Jugador[11];
            #region Creacion de Jugadores

            jugadores[0] = new Jugador();
            jugadores[0].FullName = "David De Gea";
            jugadores[0].ShortName = "De Gea";
            jugadores[0].Number = 43;
            jugadores[0].Posicion = Jugador.Portero;
            jugadores[0].PosX = "0.55";
            jugadores[0].PosY = "1";
            jugadores[0].Equipo = e;

            jugadores[1] = new Jugador();
            jugadores[1].FullName = "Antonio López";
            jugadores[1].ShortName = "López";
            jugadores[1].Number = 3;
            jugadores[1].Capitan = true;
            jugadores[1].Posicion = Jugador.Cierre;
            jugadores[1].PosX = "0.1";
            jugadores[1].PosY = "0.8";
            jugadores[1].Equipo = e;

            jugadores[2] = new Jugador();
            jugadores[2].FullName = "Tomas Ujfalusi";
            jugadores[2].ShortName = "Ujfalusi";
            jugadores[2].Number = 17;
            jugadores[2].SancionSiAmarilla = true;
            jugadores[2].Posicion = Jugador.Cierre;
            jugadores[2].PosX = "0.35";
            jugadores[2].PosY = "0.8";
            jugadores[2].Equipo = e;

            jugadores[3] = new Jugador();
            jugadores[3].FullName = "Álvaro Domínguez";
            jugadores[3].ShortName = "Domínguez";
            jugadores[3].Number = 18;
            jugadores[3].Posicion = Jugador.Cierre;
            jugadores[3].PosX = "0.6";
            jugadores[3].PosY = "0.8";
            jugadores[3].Equipo = e;

            jugadores[4] = new Jugador();
            jugadores[4].FullName = "Luis Perea";
            jugadores[4].ShortName = "Perea";
            jugadores[4].Number = 21;
            jugadores[4].Posicion = Jugador.Cierre;
            jugadores[4].PosX = "0.9";
            jugadores[4].PosY = "0.8";
            jugadores[4].Equipo = e;

            jugadores[5] = new Jugador();
            jugadores[5].FullName = "Raúl García";
            jugadores[5].ShortName = "Raúl García";
            jugadores[5].Number = 8;
            jugadores[5].SancionSiAmarilla = true;
            jugadores[5].Posicion = Jugador.Ala;
            jugadores[5].PosX = "0.75";
            jugadores[5].PosY = "0.6";
            jugadores[5].Equipo = e;

            jugadores[6] = new Jugador();
            jugadores[6].FullName = "Paulo Assunçao";
            jugadores[6].ShortName = "Assunçao";
            jugadores[6].Number = 12;
            jugadores[6].Posicion = Jugador.Ala;
            jugadores[6].PosX = "0.4";
            jugadores[6].PosY = "0.5";
            jugadores[6].Equipo = e;

            jugadores[7] = new Jugador();
            jugadores[7].FullName = "José Antonio Reyes";
            jugadores[7].ShortName = "Reyes";
            jugadores[7].Number = 19;
            jugadores[7].Posicion = Jugador.Ala;
            jugadores[7].PosX = "0.85";
            jugadores[7].PosY = "0.3";
            jugadores[7].Equipo = e;

            jugadores[8] = new Jugador();
            jugadores[8].FullName = "Simao Sabrosa";
            jugadores[8].ShortName = "Simao";
            jugadores[8].Number = 20;
            jugadores[8].Posicion = Jugador.Ala;
            jugadores[8].PosX = "0.5";
            jugadores[8].PosY = "0.3";
            jugadores[8].Equipo = e;

            jugadores[9] = new Jugador();
            jugadores[9].FullName = "Diego Forlán";
            jugadores[9].ShortName = "Forlán";
            jugadores[9].Number = 7;
            jugadores[9].Posicion = Jugador.Pivot;
            jugadores[9].PosX = "0.1";
            jugadores[9].PosY = "0.3";
            jugadores[9].Equipo = e;

            jugadores[10] = new Jugador();
            jugadores[10].FullName = "Sergio Agüero";
            jugadores[10].ShortName = "Agüero";
            jugadores[10].Number = 10;
            jugadores[10].Posicion = Jugador.Pivot;
            jugadores[10].PosX = "0.5";
            jugadores[10].PosY = "0.1";
            jugadores[10].Equipo = e;
            #endregion
            e.Jugadores = new List<Jugador>(jugadores);

            //Jugador[] banquillo = new Jugador[7];
            //#region Creacion de Sumplentes

            //banquillo[0] = new Jugador();
            //banquillo[0].FullName = "Joel Robles";
            //banquillo[0].ShortName = "Joel";
            //banquillo[0].Number = 42;
            //banquillo[0].Posicion = Jugador.Portero;
            //banquillo[0].Equipo = e;

            //banquillo[1] = new Jugador();
            //banquillo[1].FullName = "Juan Valera";
            //banquillo[1].ShortName = "Valera";
            //banquillo[1].Number = 2;
            //banquillo[1].Posicion = Jugador.Cierre;
            //banquillo[1].Equipo = e;

            //banquillo[2] = new Jugador();
            //banquillo[2].FullName = "Juanito";
            //banquillo[2].ShortName = "Juanito";
            //banquillo[2].Number = 16;
            //banquillo[2].Posicion = Jugador.Cierre;
            //banquillo[2].Equipo = e;

            //banquillo[3] = new Jugador();
            //banquillo[3].FullName = "Leandro Cabrera";
            //banquillo[3].ShortName = "Cabrera";
            //banquillo[3].Number = 24;
            //banquillo[3].Posicion = Jugador.Cierre;
            //banquillo[3].Equipo = e;

            //banquillo[4] = new Jugador();
            //banquillo[4].FullName = "Camacho";
            //banquillo[4].ShortName = "Camacho";
            //banquillo[4].Number = 6;
            //banquillo[4].Posicion = Jugador.Ala;
            //banquillo[4].Equipo = e;

            //banquillo[5] = new Jugador();
            //banquillo[5].FullName = "José Manuel Jurado";
            //banquillo[5].ShortName = "Jurado";
            //banquillo[5].Number = 9;
            //banquillo[5].Posicion = Jugador.Ala;
            //banquillo[5].Equipo = e;

            //banquillo[6] = new Jugador();
            //banquillo[6].FullName = "Eduardo Salvio";
            //banquillo[6].ShortName = "Salvio";
            //banquillo[6].Number = 14;
            //banquillo[6].Posicion = Jugador.Ala;
            //banquillo[6].Equipo = e;

            //#endregion
            //e.Banquillo = new List<Jugador>(banquillo);

            return e;
        }

        private static Equipo getEquipoVisitante()
        {
            Equipo e = new Equipo();

            e.Local = false;
            e.TeamCode = "LIV";
            e.ShortName = "LIVERPOOL";
            e.FullName = "LIVERPOOL";
            e.Badge = "Liverpool FC_MP_HD.tga";
            e.Color1 = System.Drawing.Color.White;

            e.Entrenador = new Jugador();
            e.Entrenador.FullName = "Rafael Benítez";
            e.Entrenador.ShortName = "Benítez";
            e.Entrenador.Equipo = e;

            Jugador[] jugadores = new Jugador[11];
            #region Creacion de Jugadores

            jugadores[0] = new Jugador();
            jugadores[0].FullName = "Reina";
            jugadores[0].ShortName = "Reina";
            jugadores[0].Number = 25;
            jugadores[0].Posicion = Jugador.Portero;
            jugadores[0].PosX = "0.55";
            jugadores[0].PosY = "1";
            jugadores[0].Equipo = e;

            jugadores[1] = new Jugador();
            jugadores[1].FullName = "Johnson";
            jugadores[1].ShortName = "Johnson";
            jugadores[1].Number = 2;
            jugadores[1].Posicion = Jugador.Cierre;
            jugadores[1].PosX = "0.1";
            jugadores[1].PosY = "0.8";
            jugadores[1].Equipo = e;

            jugadores[2] = new Jugador();
            jugadores[2].FullName = "Agger";
            jugadores[2].ShortName = "Agger";
            jugadores[2].Number = 5;
            jugadores[2].Posicion = Jugador.Cierre;
            jugadores[2].PosX = "0.35";
            jugadores[2].PosY = "0.8";
            jugadores[2].Equipo = e;

            jugadores[3] = new Jugador();
            jugadores[3].FullName = "Carragher";
            jugadores[3].ShortName = "Carragher";
            jugadores[3].Number = 23;
            jugadores[3].Posicion = Jugador.Cierre;
            jugadores[3].PosX = "0.6";
            jugadores[3].PosY = "0.8";
            jugadores[3].Equipo = e;

            jugadores[4] = new Jugador();
            jugadores[4].FullName = "Aquilani";
            jugadores[4].ShortName = "Aquilani";
            jugadores[4].Number = 4;
            jugadores[4].Posicion = Jugador.Ala;
            jugadores[4].PosX = "0.9";
            jugadores[4].PosY = "0.8";
            jugadores[4].Equipo = e;

            jugadores[5] = new Jugador();
            jugadores[5].FullName = "Gerrard";
            jugadores[5].ShortName = "Gerrard";
            jugadores[5].Number = 8;
            jugadores[5].Posicion = Jugador.Ala;
            jugadores[5].PosX = "0.75";
            jugadores[5].PosY = "0.6";
            jugadores[5].Equipo = e;

            jugadores[6] = new Jugador();
            jugadores[6].FullName = "Benayoun";
            jugadores[6].ShortName = "Benayoun";
            jugadores[6].Number = 15;
            jugadores[6].Posicion = Jugador.Ala;
            jugadores[6].PosX = "0.4";
            jugadores[6].PosY = "0.5";
            jugadores[6].Equipo = e;

            jugadores[7] = new Jugador();
            jugadores[7].FullName = "Mascherano";
            jugadores[7].ShortName = "Mascherano";
            jugadores[7].Number = 20;
            jugadores[7].SancionSiAmarilla = true;
            jugadores[7].Posicion = Jugador.Ala;
            jugadores[7].PosX = "0.85";
            jugadores[7].PosY = "0.3";
            jugadores[7].Equipo = e;

            jugadores[8] = new Jugador();
            jugadores[8].FullName = "Lucas";
            jugadores[8].ShortName = "Lucas";
            jugadores[8].Number = 21;
            jugadores[8].Posicion = Jugador.Ala;
            jugadores[8].PosX = "0.5";
            jugadores[8].PosY = "0.3";
            jugadores[8].Equipo = e;

            jugadores[9] = new Jugador();
            jugadores[9].FullName = "Kuyt";
            jugadores[9].ShortName = "Kuyt";
            jugadores[9].Number = 18;
            jugadores[9].Posicion = Jugador.Pivot;
            jugadores[9].PosX = "0.1";
            jugadores[9].PosY = "0.3";
            jugadores[9].Equipo = e;

            jugadores[10] = new Jugador();
            jugadores[10].FullName = "Babel";
            jugadores[10].ShortName = "Babel";
            jugadores[10].Number = 19;
            jugadores[10].Posicion = Jugador.Pivot;
            jugadores[10].PosX = "0.5";
            jugadores[10].PosY = "0.1";
            jugadores[10].Equipo = e;
            #endregion
            e.Jugadores = new List<Jugador>(jugadores);

            Jugador[] banquillo = new Jugador[5];
            #region Creacion de Sumplentes

            banquillo[0] = new Jugador();
            banquillo[0].FullName = "Cavalieri";
            banquillo[0].ShortName = "Cavalieri";
            banquillo[0].Number = 1;
            banquillo[0].Posicion = Jugador.Portero;
            banquillo[0].Equipo = e;

            banquillo[1] = new Jugador();
            banquillo[1].FullName = "Kyrgiakos";
            banquillo[1].ShortName = "Kyrgiakos";
            banquillo[1].Number = 16;
            banquillo[1].Posicion = Jugador.Cierre;
            banquillo[1].Equipo = e;

            banquillo[2] = new Jugador();
            banquillo[2].FullName = "Pepito";
            banquillo[2].ShortName = "Pepito";
            banquillo[2].Number = 27;
            banquillo[2].Capitan = true;
            banquillo[2].Posicion = Jugador.Cierre;
            banquillo[2].Equipo = e;

            banquillo[3] = new Jugador();
            banquillo[3].FullName = "Ayala";
            banquillo[3].ShortName = "Ayala";
            banquillo[3].Number = 40;
            banquillo[3].Posicion = Jugador.Cierre;
            banquillo[3].Equipo = e;

            banquillo[4] = new Jugador();
            banquillo[4].FullName = "Pacheco";
            banquillo[4].ShortName = "Pacheco";
            banquillo[4].Number = 47;
            banquillo[4].Posicion = Jugador.Ala;
            banquillo[4].Equipo = e;

            #endregion
            e.Banquillo = new List<Jugador>(banquillo);

            return e;
        }

        private static List<ICommand> getArbitros()
        {
            List<ICommand> arbitros = new List<ICommand>();

            Arbitro arbitro1 = new Arbitro();
            arbitro1.FullName = "Germán Navarro";
            arbitro1.ShortName = "Germán";
            arbitro1.Nacionalidad = "ESP";
            arbitro1.Colegio = "Inventado";
            arbitro1.Cargo = Arbitro.arbitro1;

            Arbitro arbitro2 = new Arbitro();
            arbitro2.FullName = "Carolina Gomez";
            arbitro2.ShortName = "Carolina";
            arbitro1.Colegio = "Inventado";
            arbitro2.Cargo = Arbitro.arbitro2;

            arbitros.Add(new RefereesCommand(arbitro1, arbitro2));
            arbitros.Add(new IdentificationCommand(arbitro1));
            arbitros.Add(new IdentificationCommand(arbitro2));
          
            return arbitros;
        }

        private static List<ICommand> getComentaristas()
        {
            List<ICommand> comentaristas = new List<ICommand>();

            comentaristas.Add(new FreeTextCommand("Comentarista 1"));
            comentaristas.Add(new FreeTextCommand("Comentarista 2"));

            return comentaristas;
        }


        public bool TestConexion()
        {
            return true;
        }

        public bool AbreConexion()
        {
            return true;
        }

        public void CierraConexion() { }


    }
}

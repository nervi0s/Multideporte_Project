using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Futbol_Sala_Manager_App
{
    public abstract class Consola
    {
        // Atributos comunes        
        public Interprete interprete;
        public int id_deporte;
        public string puerto_com; // Puede ser un puerto com o un puerto tcp, dependiendo de la consola


        // Métodos comunes

        /* Constructor
         * 
         */
        public Consola()
        { 
        }
        

        public void asigna_tipo_interprete(string tipo_interprete)
        {
            switch (tipo_interprete) 
            {
                case "mondo":
                    this.interprete = new Interprete_mondo(this.id_deporte, this.puerto_com);
                    break;
                //case "stb":
                //    this.interprete = new Interprete_stb(this.id_deporte, this.puerto_com);
                //    break;
                //case "nautronic":
                //    this.interprete = new Interprete_nautronic(this.id_deporte, this.puerto_com);
                //    break;
                //case "virtualia":
                //    this.interprete = new Interprete_virtualia(this.id_deporte, this.puerto_com);
                //    break;
                //case "manual":
                //    this.interprete = new Interprete_manual(this.id_deporte, this.puerto_com);
                //    break;
            }
        }


        // Metodo que se sobreescribira despues en las consolas de todos los deporte
        public abstract bool conecta();

        // Metodo que se sobreescribira despues en las consolas de todos los deporte
        public abstract void desconecta();
    }



    # region Consola_baloncesto
    
    // Consola especifica de baloncesto
    public class Consola_baloncesto : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_crono(string crono); public event dispara_crono actualiza_crono;
        public delegate void dispara_periodo(string periodo); public event dispara_periodo actualiza_periodo;
        public delegate void dispara_flechas(string flechas); public event dispara_flechas actualiza_flechas;
        public delegate void dispara_tiempos_muertos(string equipo, string num_tiempos_muertos); public event dispara_tiempos_muertos actualiza_tiempos_muertos;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_faltas_equipo(string equipo, string num_faltas_equipo); public event dispara_faltas_equipo actualiza_faltas_equipo;
        public delegate void dispara_puntos_jugador(string equipo, string dorsal, string num_puntos_totales); public event dispara_puntos_jugador actualiza_puntos_jugador;
        public delegate void dispara_puntos_jugador_con_estadisticas(string equipo, string dorsal, string num_puntos_totales, string tiros_libres, string canastas_de_2, string triples); public event dispara_puntos_jugador_con_estadisticas actualiza_puntos_jugador_con_estadisticas;
        public delegate void dispara_faltas_jugador(string equipo, string dorsal, string num_faltas); public event dispara_faltas_jugador actualiza_faltas_jugador;
        public delegate void dispara_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual); public event dispara_dorsales_jugadores actualiza_dorsales_jugadores;
        public delegate void dispara_crono_posesion(string crono_posesion); public event dispara_crono_posesion actualiza_crono_posesion;
        //public delegate void dispara_reset(); public event dispara_reset actualiza_reset;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;
        public delegate void dispara_bocina(); public event dispara_bocina actualiza_bocina;


        // Metodos

        /*
         * Constructor
         */
        public Consola_baloncesto(string tipo_interprete, string puerto)
        {
            this.id_deporte = 1;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete); 
        }
        
        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola
        public void update_crono(string crono)
        {
            actualiza_crono(crono);
        }
        public void update_periodo(string periodo)
        {
            actualiza_periodo(periodo);
        }
        public void update_flechas(string flecha)
        {
            actualiza_flechas(flecha);
        }
        public void update_puntos_equipo(string equipo, string puntos)
        {
            actualiza_puntos_equipo(equipo, puntos);
        }
        public void update_tiempos_muertos(string equipo, string num_tiempos_muertos)
        {
            actualiza_tiempos_muertos(equipo, num_tiempos_muertos);
        }
        public void update_faltas_equipo(string equipo, string num_faltas_equipo)
        {
            actualiza_faltas_equipo(equipo, num_faltas_equipo);
        }
        public void update_puntos_jugador(string equipo, string dorsal, string num_puntos_totales)
        {
            actualiza_puntos_jugador(equipo, dorsal, num_puntos_totales);
        }
        public void update_puntos_jugador(string equipo, string dorsal, string num_puntos_totales, string tiros_libres, string canastas_de_2, string triples)
        {
            actualiza_puntos_jugador_con_estadisticas(equipo, dorsal, num_puntos_totales, tiros_libres, canastas_de_2, triples);
        }
        public void update_faltas_jugador(string equipo, string dorsal, string num_faltas)
        {
            actualiza_faltas_jugador(equipo, dorsal, num_faltas);
        }
        public void update_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual)
        {
            actualiza_dorsales_jugadores(equipo, dorsal_anterior, dorsal_actual);
        }
        public void update_crono_posesion(string crono_posesion)
        {
            actualiza_crono_posesion(crono_posesion);
        }
        //public void update_reset()
        //{
        //    actualiza_reset();
        //}
        public void update_inicia_videomarcador()
        {
            actualiza_inicia_videomarcador();
        }
        public void update_bocina()
        {
            actualiza_bocina();
        }
    }

    # endregion
       
   
    # region Consola_balonmano
        
    // Consola especifica de balonmano
    public class Consola_balonmano : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_crono(string crono); public event dispara_crono actualiza_crono;
        public delegate void dispara_estado_crono(string estado); public event dispara_estado_crono actualiza_estado_crono;
        public delegate void dispara_periodo(string periodo); public event dispara_periodo actualiza_periodo;
        public delegate void dispara_tiempos_muertos(string equipo, string num_tiempos_muertos); public event dispara_tiempos_muertos actualiza_tiempos_muertos;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_goles_jugador(string equipo, string dorsal, string num_goles_totales); public event dispara_goles_jugador actualiza_goles_jugador;
        public delegate void dispara_suma_expulsion_jugador(string equipo, string dorsal); public event dispara_suma_expulsion_jugador suma_expulsion_jugador;
        public delegate void dispara_resta_expulsion_jugador(string equipo, string dorsal); public event dispara_resta_expulsion_jugador resta_expulsion_jugador;
        public delegate void dispara_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual); public event dispara_dorsales_jugadores actualiza_dorsales_jugadores;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;
                

        // Metodos

        /*
         * Constructor
         */
        public Consola_balonmano(string tipo_interprete, string puerto)
        {
            this.id_deporte = 2;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete); 
        }

        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola
        public void update_crono(string crono)
        {
            actualiza_crono(crono);
        }
        public void update_estado_crono(string estado)
        {
            actualiza_estado_crono(estado);
        }
        public void update_periodo(string periodo)
        {
            actualiza_periodo(periodo);
        }
        public void update_puntos_equipo(string equipo, string puntos)
        {
            // actualiza_puntos_equipo(equipo, puntos);                                             //luis
        }
        public void update_tiempos_muertos(string equipo, string num_tiempos_muertos)
        {
            //actualiza_tiempos_muertos(equipo, num_tiempos_muertos);
        }        
        public void update_goles_jugador(string equipo, string dorsal, string num_goles_totales)
        {
            //actualiza_goles_jugador(equipo, dorsal, num_goles_totales);                              //luis
        }        
        public void update_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual)
        {
            /**///actualiza_dorsales_jugadores(equipo, dorsal_anterior, dorsal_actual);                 //luis
        }
        public void update_suma_expulsion_jugador(string equipo, string dorsal)
        {   
            /*                                                                                          //luis    */    
            Console.WriteLine("EXPULSION: " + equipo + ", " + dorsal);
            suma_expulsion_jugador(equipo, dorsal);
        }
        public void update_resta_expulsion_jugador(string equipo, string dorsal)
        {
            /*                                                                                          //luis      */
            Console.WriteLine("RESTA EXPULSION: " + equipo + ", " + dorsal);
            resta_expulsion_jugador(equipo, dorsal);
        }
        public void update_inicia_videomarcador()
        {
            /**///actualiza_inicia_videomarcador();                                                     //luis
        }
    }

    # endregion


    # region Consola_futbol_sala
    
    // Consola especifica de futbol sala
    public class Consola_futbol_sala : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_crono(string crono); public event dispara_crono actualiza_crono;
        public delegate void dispara_periodo(string periodo); public event dispara_periodo actualiza_periodo;
        public delegate void dispara_tiempos_muertos(string equipo, string num_tiempos_muertos); public event dispara_tiempos_muertos actualiza_tiempos_muertos;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_faltas_equipo(string equipo, string num_faltas_equipo); public event dispara_faltas_equipo actualiza_faltas_equipo;
        public delegate void dispara_goles_jugador(string equipo, string dorsal, string num_goles_totales); public event dispara_goles_jugador actualiza_goles_jugador;
        public delegate void dispara_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual); public event dispara_dorsales_jugadores actualiza_dorsales_jugadores;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;


        // Metodos

        /*
         * Constructor
         */
        public Consola_futbol_sala(string tipo_interprete, string puerto)
        {
            this.id_deporte = 3;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete); 
        }
        
        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola
        public void update_crono(string crono)
        {
            actualiza_crono(crono);
        }
        public void update_periodo(string periodo)
        {
            actualiza_periodo(periodo);
        }
        public void update_puntos_equipo(string equipo, string puntos)
        {
            actualiza_puntos_equipo(equipo, puntos);
        }
        public void update_tiempos_muertos(string equipo, string num_tiempos_muertos)
        {
            actualiza_tiempos_muertos(equipo, num_tiempos_muertos);
        }
        public void update_faltas_equipo(string equipo, string num_faltas_equipo)
        {
            actualiza_faltas_equipo(equipo, num_faltas_equipo);
        }
        public void update_goles_jugador(string equipo, string dorsal, string num_goles_totales)
        {
            actualiza_goles_jugador(equipo, dorsal, num_goles_totales);
        }
        public void update_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual)
        {
            actualiza_dorsales_jugadores(equipo, dorsal_anterior, dorsal_actual);
        }
        public void update_inicia_videomarcador()
        {
            actualiza_inicia_videomarcador();
        }
    }

    # endregion


    # region Consola_waterpolo

    // Consola especifica de waterpolo
    public class Consola_waterpolo : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_crono(string crono); public event dispara_crono actualiza_crono;
        public delegate void dispara_periodo(string periodo); public event dispara_periodo actualiza_periodo;
        public delegate void dispara_flechas(string flechas); public event dispara_flechas actualiza_flechas;
        public delegate void dispara_tiempos_muertos(string equipo, string num_tiempos_muertos); public event dispara_tiempos_muertos actualiza_tiempos_muertos;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_goles_jugador(string equipo, string dorsal, string num_goles_totales); public event dispara_goles_jugador actualiza_goles_jugador;
        public delegate void dispara_exclusiones_jugador(string equipo, string dorsal, string num_exclusiones); public event dispara_exclusiones_jugador actualiza_exclusiones_jugador;
        public delegate void dispara_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual); public event dispara_dorsales_jugadores actualiza_dorsales_jugadores;
        public delegate void dispara_crono_posesion(string crono_posesion); public event dispara_crono_posesion actualiza_crono_posesion;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;


        // Metodos

        /*
         * Constructor
         */
        public Consola_waterpolo(string tipo_interprete, string puerto)
        {
            this.id_deporte = 4;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete); 
        }

        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola
        public void update_crono(string crono)
        {
            actualiza_crono(crono);
        }
        public void update_periodo(string periodo)
        {
            actualiza_periodo(periodo);
        }
        public void update_flechas(string flecha)
        {
            actualiza_flechas(flecha);
        }
        public void update_puntos_equipo(string equipo, string puntos)
        {
            actualiza_puntos_equipo(equipo, puntos);
        }
        public void update_tiempos_muertos(string equipo, string num_tiempos_muertos)
        {
            actualiza_tiempos_muertos(equipo, num_tiempos_muertos);
        }        
        public void update_goles_jugador(string equipo, string dorsal, string num_goles_totales)
        {
            actualiza_goles_jugador(equipo, dorsal, num_goles_totales);
        }
        public void update_expulsiones_jugador(string equipo, string dorsal, string num_exclusiones)
        {
            actualiza_exclusiones_jugador(equipo, dorsal, num_exclusiones);
        }
        public void update_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual)
        {
            actualiza_dorsales_jugadores(equipo, dorsal_anterior, dorsal_actual);
        }
        public void update_crono_posesion(string crono_posesion)
        {
            //actualiza_crono_posesion(crono_posesion);
        }
        public void update_inicia_videomarcador()
        {
            actualiza_inicia_videomarcador();
        }
    }

    # endregion


    # region Consola_hockey_hielo

    // Consola especifica de hockey hielo
    public class Consola_hockey_hielo : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_crono(string crono); public event dispara_crono actualiza_crono;
        public delegate void dispara_estado_crono(string estado); public event dispara_estado_crono actualiza_estado_crono;
        public delegate void dispara_periodo(string periodo); public event dispara_periodo actualiza_periodo;
        public delegate void dispara_tiempos_muertos(string equipo, string num_tiempos_muertos); public event dispara_tiempos_muertos actualiza_tiempos_muertos;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_goles_jugador(string equipo, string dorsal, string num_goles_totales); public event dispara_goles_jugador actualiza_goles_jugador;
        public delegate void dispara_suma_expulsion_jugador(string equipo, string dorsal, string minutos); public event dispara_suma_expulsion_jugador suma_expulsion_jugador;
        public delegate void dispara_resta_expulsion_jugador(string equipo, string dorsal); public event dispara_resta_expulsion_jugador resta_expulsion_jugador;
        public delegate void dispara_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual); public event dispara_dorsales_jugadores actualiza_dorsales_jugadores;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;

        
        // Metodos

        /*
         * Constructor
         */
        public Consola_hockey_hielo(string tipo_interprete, string puerto)
        {
            this.id_deporte = 5;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete); 
        }


        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola
        public void update_crono(string crono)
        {
            actualiza_crono(crono);
        }
        public void update_estado_crono(string estado)
        {
            actualiza_estado_crono(estado);
        }
        public void update_periodo(string periodo)
        {
            actualiza_periodo(periodo);
        }        
        public void update_puntos_equipo(string equipo, string puntos)
        {
            actualiza_puntos_equipo(equipo, puntos);
        }
        public void update_tiempos_muertos(string equipo, string num_tiempos_muertos)
        {
            actualiza_tiempos_muertos(equipo, num_tiempos_muertos);
        }
        public void update_goles_jugador(string equipo, string dorsal, string num_goles_totales)
        {
            actualiza_goles_jugador(equipo, dorsal, num_goles_totales);
        }             
        public void update_suma_expulsion_jugador(string equipo, string dorsal, string minutos)
        {
            suma_expulsion_jugador(equipo, dorsal, minutos);
        }
        public void update_resta_expulsion_jugador(string equipo, string dorsal)
        {
            resta_expulsion_jugador(equipo, dorsal);
        }
        public void update_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual)
        {
            actualiza_dorsales_jugadores(equipo, dorsal_anterior, dorsal_actual);
        }
        public void update_inicia_videomarcador()
        {
            actualiza_inicia_videomarcador();
        }
    }

    # endregion


    # region Consola_hockey_sala

    // Consola especifica de hockey sala
    public class Consola_hockey_sala : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_crono(string crono); public event dispara_crono actualiza_crono;
        public delegate void dispara_estado_crono(string estado); public event dispara_estado_crono actualiza_estado_crono;
        public delegate void dispara_periodo(string periodo); public event dispara_periodo actualiza_periodo;
        public delegate void dispara_tiempos_muertos(string equipo, string num_tiempos_muertos); public event dispara_tiempos_muertos actualiza_tiempos_muertos;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_goles_jugador(string equipo, string dorsal, string num_goles_totales); public event dispara_goles_jugador actualiza_goles_jugador;
        public delegate void dispara_suma_expulsion_jugador(string equipo, string dorsal); public event dispara_suma_expulsion_jugador suma_expulsion_jugador;
        public delegate void dispara_resta_expulsion_jugador(string equipo, string dorsal); public event dispara_resta_expulsion_jugador resta_expulsion_jugador;
        public delegate void dispara_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual); public event dispara_dorsales_jugadores actualiza_dorsales_jugadores;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;


        // Metodos

        /*
         * Constructor
         */
        public Consola_hockey_sala(string tipo_interprete, string puerto)
        {
            this.id_deporte = 6;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete); 
        }

        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola
        public void update_crono(string crono)
        {
            actualiza_crono(crono);
        }
        public void update_estado_crono(string estado)
        {
            actualiza_estado_crono(estado);
        }
        public void update_periodo(string periodo)
        {
            actualiza_periodo(periodo);
        }
        public void update_puntos_equipo(string equipo, string puntos)
        {
            actualiza_puntos_equipo(equipo, puntos);
        }
        public void update_tiempos_muertos(string equipo, string num_tiempos_muertos)
        {
            actualiza_tiempos_muertos(equipo, num_tiempos_muertos);
        }
        public void update_goles_jugador(string equipo, string dorsal, string num_goles_totales)
        {
            actualiza_goles_jugador(equipo, dorsal, num_goles_totales);
        }
        public void update_suma_expulsion_jugador(string equipo, string dorsal)
        {
            suma_expulsion_jugador(equipo, dorsal);
        }
        public void update_resta_expulsion_jugador(string equipo, string dorsal)
        {
            resta_expulsion_jugador(equipo, dorsal);
        }
        public void update_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual)
        {
            actualiza_dorsales_jugadores(equipo, dorsal_anterior, dorsal_actual);
        }
        public void update_inicia_videomarcador()
        {
            actualiza_inicia_videomarcador();
        }
    }

    # endregion


    # region Consola_voleibol

    // Consola especifica de voleibol
    public class Consola_voleibol : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_sets_ganados(string equipo, string sets); public event dispara_sets_ganados actualiza_sets_ganados;
        public delegate void dispara_periodo(string periodo); public event dispara_periodo actualiza_periodo;
        public delegate void dispara_cambio_set(string equipo); public event dispara_cambio_set actualiza_cambio_set;
        public delegate void dispara_flechas(string flechas); public event dispara_flechas actualiza_flechas;
        public delegate void dispara_tiempos_muertos(string equipo, string num_tiempos_muertos); public event dispara_tiempos_muertos actualiza_tiempos_muertos;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_sets_equipo(string equipo, string set, string num_puntos); public event dispara_sets_equipo actualiza_sets_equipo;
        public delegate void dispara_sets_acumulados(string sets_acumulados_local, string sets_acumulados_visitante); public event dispara_sets_acumulados actualiza_sets_acumulados;
        public delegate void dispara_modo_edicion(string si_activado); public event dispara_modo_edicion actualiza_modo_edicion;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;


        // Metodos

        /*
         * Constructor
         */
        public Consola_voleibol(string tipo_interprete, string puerto)
        {
            this.id_deporte = 7;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete); 
        }


        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola
        public void update_sets_ganados(string equipo, string sets)
        {
            actualiza_sets_ganados(equipo, sets);
        }
        public void update_periodo(string periodo)
        {
            actualiza_periodo(periodo);
        }
        public void update_cambio_set(string equipo)
        {
            actualiza_cambio_set(equipo);
        }
        public void update_flechas(string flecha)
        {
            actualiza_flechas(flecha);
        }
        public void update_puntos_equipo(string equipo, string puntos)
        {
            actualiza_puntos_equipo(equipo, puntos);
        }
        public void update_tiempos_muertos(string equipo, string num_tiempos_muertos)
        {
            actualiza_tiempos_muertos(equipo, num_tiempos_muertos);
        }
        public void update_sets_equipo(string equipo, string set, string num_puntos)
        {
            actualiza_sets_equipo(equipo, set, num_puntos);
        }
        public void update_sets_acumulados(string sets_acumulados_local, string sets_acumulados_visitante)
        {
            actualiza_sets_acumulados(sets_acumulados_local, sets_acumulados_visitante);
        }
        public void update_modo_edicion(string si_activado)
        {
            actualiza_modo_edicion(si_activado);
        }
        public void update_inicia_videomarcador()
        {
            actualiza_inicia_videomarcador();
        }
    }

    # endregion


    # region Consola_tenis

    // Consola especifica de tenis
    public class Consola_tenis : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_juegos(string equipo, string set, string juegos); public event dispara_juegos actualiza_juegos;
        public delegate void dispara_periodo(string periodo); public event dispara_periodo actualiza_periodo;
        public delegate void dispara_cambio_juego(string equipo, string hay_cambio_de_set); public event dispara_cambio_juego actualiza_cambio_juego;
        public delegate void dispara_cambio_set(string equipo); public event dispara_cambio_set actualiza_cambio_set;
        public delegate void dispara_flechas(string flechas); public event dispara_flechas actualiza_flechas;
        public delegate void dispara_tie_break(string tie_break); public event dispara_tie_break actualiza_tie_break;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_sets_equipo(string equipo, string set, string num_puntos); public event dispara_sets_equipo actualiza_sets_equipo;
        public delegate void dispara_sets_acumulados(string sets_acumulados_local, string sets_acumulados_visitante); public event dispara_sets_acumulados actualiza_sets_acumulados;
        public delegate void dispara_modo_edicion(string si_activado); public event dispara_modo_edicion actualiza_modo_edicion;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;

        // Metodos

        /*
         * Constructor
         */
        public Consola_tenis(string tipo_interprete, string puerto)
        {
            this.id_deporte = 8;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete);
        }

        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola
        public void update_juegos(string equipo, string set, string juegos)
        {
            actualiza_juegos(equipo, set, juegos);
        }
        public void update_periodo(string periodo)
        {
            actualiza_periodo(periodo);
        }
        public void update_cambio_juego(string equipo, string hay_cambio_de_set)
        {
            actualiza_cambio_juego(equipo, hay_cambio_de_set);
        }
        public void update_cambio_set(string equipo)
        {
            actualiza_cambio_set(equipo);
        }
        public void update_flechas(string flecha)
        {
            actualiza_flechas(flecha);
        }
        public void update_tie_break(string tie_break)
        {
            actualiza_tie_break(tie_break);
        }
        public void update_puntos_equipo(string equipo, string puntos)
        {
            actualiza_puntos_equipo(equipo, puntos);
        }
        public void update_sets_equipo(string equipo, string set, string num_puntos)
        {
            actualiza_sets_equipo(equipo, set, num_puntos);
        }
        public void update_sets_acumulados(string sets_acumulados_local, string sets_acumulados_visitante)
        {
            actualiza_sets_acumulados(sets_acumulados_local, sets_acumulados_visitante);
        }
        public void update_modo_edicion(string si_activado)
        {
            actualiza_modo_edicion(si_activado);
        }
        public void update_inicia_videomarcador()
        {
            actualiza_inicia_videomarcador();
        }
    }

    # endregion


    # region Consola_futbol

    // Consola especifica de futbol
    public class Consola_futbol : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_crono(string crono); public event dispara_crono actualiza_crono;
        public delegate void dispara_periodo(string periodo); public event dispara_periodo actualiza_periodo;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_goles_jugador(string equipo, string dorsal, string goles); public event dispara_goles_jugador actualiza_goles_jugador;
        public delegate void dispara_tarjetas_jugador(string equipo, string dorsal, string tipo_tarjeta, string num_tarjetas); public event dispara_tarjetas_jugador actualiza_tarjetas_jugador;
        public delegate void dispara_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual); public event dispara_dorsales_jugadores actualiza_dorsales_jugadores;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;


        // Metodos

        /*
         * Constructor
         */
        public Consola_futbol(string tipo_interprete, string puerto)
        {
            this.id_deporte = 9;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete); 
        }

        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola
        public void update_crono(string crono)
        {
            actualiza_crono(crono);
        }
        public void update_periodo(string periodo)
        {
            actualiza_periodo(periodo);
        }
        public void update_puntos_equipo(string equipo, string puntos)
        {
            actualiza_puntos_equipo(equipo, puntos);
        }
        public void update_goles_jugador(string equipo, string dorsal, string goles)
        {
            actualiza_goles_jugador(equipo, dorsal, goles);
        }
        public void update_tarjetas_jugador(string equipo, string dorsal, string tipo_tarjeta, string num_tarjetas)
        {
            actualiza_tarjetas_jugador(equipo, dorsal, tipo_tarjeta, num_tarjetas);
        }
        public void update_dorsales_jugadores(string equipo, string dorsal_anterior, string dorsal_actual)
        {
            actualiza_dorsales_jugadores(equipo, dorsal_anterior, dorsal_actual);
        }
        public void update_inicia_videomarcador()
        {
            actualiza_inicia_videomarcador();
        }
    }

    # endregion

    
    # region Consola_fronton

    // Consola especifica de fronton
    public class Consola_fronton : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_crono(string crono); public event dispara_crono actualiza_crono;
        public delegate void dispara_flechas(string flechas); public event dispara_flechas actualiza_flechas;
        //public delegate void dispara_tiempos_muertos(string equipo, string num_tiempos_muertos); public event dispara_tiempos_muertos actualiza_tiempos_muertos;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_tanteo_necesario(string tanteo_necesario); public event dispara_tanteo_necesario actualiza_tanteo_necesario;
        public delegate void dispara_sets_equipo(string equipo, string num_sets_equipo); public event dispara_sets_equipo actualiza_sets_equipo;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;


        // Metodos

        /*
         * Constructor
         */
        public Consola_fronton(string tipo_interprete, string puerto)
        {
            this.id_deporte = 10;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete); 
        }

        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola.
        public void update_crono(string crono)
        {
            actualiza_crono(crono);
        }
        public void update_flechas(string flecha)
        {
            actualiza_flechas(flecha);
        }
        public void update_puntos_equipo(string equipo, string puntos)
        {
            actualiza_puntos_equipo(equipo, puntos);
        }
        public void update_tanteo_necesario(string tanteo_necesario)
        {
            actualiza_tanteo_necesario(tanteo_necesario);
        }
        //public void update_tiempos_muertos(string equipo, string num_tiempos_muertos)
        //{
        //    actualiza_tiempos_muertos(equipo, num_tiempos_muertos);
        //}
        public void update_sets_equipo(string equipo, string num_sets_equipo)
        {
            actualiza_sets_equipo(equipo, num_sets_equipo);
        }
        public void update_inicia_videomarcador()
        {
            actualiza_inicia_videomarcador();
        }     
    }

    # endregion


    # region Consola_padel

    // Consola especifica de padel
    public class Consola_padel : Consola
    {
        // Eventos que se disparan con la consola
        public delegate void dispara_juegos(string equipo, string set, string juegos); public event dispara_juegos actualiza_juegos;
        public delegate void dispara_periodo(string periodo); public event dispara_periodo actualiza_periodo;
        public delegate void dispara_cambio_juego(string equipo, string hay_cambio_de_set); public event dispara_cambio_juego actualiza_cambio_juego;
        public delegate void dispara_cambio_set(string equipo); public event dispara_cambio_set actualiza_cambio_set;
        public delegate void dispara_flechas(string flechas); public event dispara_flechas actualiza_flechas;
        public delegate void dispara_tie_break(string tie_break); public event dispara_tie_break actualiza_tie_break;
        public delegate void dispara_puntos_equipo(string equipo, string puntos); public event dispara_puntos_equipo actualiza_puntos_equipo;
        public delegate void dispara_sets_equipo(string equipo, string set, string num_puntos); public event dispara_sets_equipo actualiza_sets_equipo;
        public delegate void dispara_sets_acumulados(string sets_acumulados_local, string sets_acumulados_visitante); public event dispara_sets_acumulados actualiza_sets_acumulados;
        public delegate void dispara_modo_edicion(string si_activado); public event dispara_modo_edicion actualiza_modo_edicion;
        public delegate void dispara_inicia_videomarcador(); public event dispara_inicia_videomarcador actualiza_inicia_videomarcador;

        // Metodos

        /*
         * Constructor
         */
        public Consola_padel(string tipo_interprete, string puerto)
        {
            this.id_deporte = 12;
            this.puerto_com = puerto;
            this.asigna_tipo_interprete(tipo_interprete); 
        }

        // Metodos para conectarnos y desconectarnos dependiendo del interprete que estemos usando
        public override bool conecta()
        {
            return this.interprete.conecta(this);
        }
        public override void desconecta()
        {
            this.interprete.desconecta();
        }

        // Metodos que usamos a modo de interfaz para que sean llamados desde el interprete, y que llaman a los delegados propios de esta consola
        public void update_juegos(string equipo, string set, string juegos)
        {
            actualiza_juegos(equipo, set, juegos);
        }
        public void update_periodo(string periodo)
        {
            actualiza_periodo(periodo);
        }
        public void update_cambio_juego(string equipo, string hay_cambio_de_set)
        {
            actualiza_cambio_juego(equipo, hay_cambio_de_set);
        }
        public void update_cambio_set(string equipo)
        {
            actualiza_cambio_set(equipo);
        }
        public void update_flechas(string flecha)
        {
            actualiza_flechas(flecha);
        }
        public void update_tie_break(string tie_break)
        {
            actualiza_tie_break(tie_break);
        }
        public void update_puntos_equipo(string equipo, string puntos)
        {
            actualiza_puntos_equipo(equipo, puntos);
        }
        public void update_sets_equipo(string equipo, string set, string num_puntos)
        {
            actualiza_sets_equipo(equipo, set, num_puntos);
        }
        public void update_sets_acumulados(string sets_acumulados_local, string sets_acumulados_visitante)
        {
            actualiza_sets_acumulados(sets_acumulados_local, sets_acumulados_visitante);
        }
        public void update_modo_edicion(string si_activado)
        {
            actualiza_modo_edicion(si_activado);
        }
        public void update_inicia_videomarcador()
        {
            actualiza_inicia_videomarcador();
        }
    }

    # endregion
    
}
using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class GolCommand : ICommandExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;
        private bool _visible;
        private int tipo_gol;

        //CONSTANTES TIPO GOL
        private const int Gol_Normal = 0;
        private const int Gol_7_M = 1;
        private const int Gol_Contraataque = 2;


        public GolCommand(Momento tiempo, Jugador jugador, int tipo_goal)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
            tipo_gol = tipo_goal;

            Reset();
        }
        public GolCommand(Momento tiempo, Equipo equipo, int tipo_goal)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
            tipo_gol = tipo_goal;

            Reset();
        }

        public void Reset()
        {
            _visible = false;
        }

        public void setNuevoMomento(int nuevoMomento)
        {
            Momento.SegundoAbsoluto = nuevoMomento;
        }

        public int getTipoGol()
        {
            return tipo_gol;
        }

        public Momento getNuevoMomento()
        {
            return Momento;
        }

        public void cambiaTipoGol(int tipoNuevo)
        {
            Console.WriteLine("Cambia a "+ _jugador.ShortName+" a tipo: "+tipoNuevo);
            switch (tipo_gol)
            {
                case Gol_Normal:
                    switch (tipoNuevo)
                    {
                        case Gol_7_M:
                            _equipo.Goles7M.Add(Momento);
                            _equipo.Tiros7M.Add(Momento);
                            break;

                        case Gol_Contraataque:
                            _equipo.GolesContraataque.Add(Momento);
                            _equipo.TirosContraataque.Add(Momento);
                            break;

                        default:
                            break;
                    }
                    break;

                case Gol_7_M:
                    _equipo.Goles7M.Remove(Momento);
                    _equipo.Tiros7M.Remove(Momento);
                    switch (tipoNuevo)
                    {
                        case Gol_Contraataque:
                            _equipo.GolesContraataque.Add(Momento);
                            _equipo.TirosContraataque.Add(Momento);
                            break;

                        default:
                            break;
                    }
                    break;

                case Gol_Contraataque:
                    _equipo.GolesContraataque.Remove(Momento);
                    _equipo.TirosContraataque.Remove(Momento);
                    switch (tipoNuevo)
                    {
                        case Gol_Contraataque:
                            _equipo.Goles7M.Add(Momento);
                            _equipo.Tiros7M.Add(Momento);
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;

            }

            tipo_gol = tipoNuevo;
        }

        public void Execute()
        {
            //SE SUMA UN GOL Y UN TIRO AL EQUIPO
            _equipo.Goles.Add(Momento);
            _equipo.Tiros.Add(Momento);

            //DEPENDIENDO DEL TIPO DE GOL SE SUMA SU ESTADÍSTICA (JUNTO CON SU TIRO)
            switch (tipo_gol)
            {
                case Gol_7_M:
                    _equipo.Goles7M.Add(Momento);
                    _equipo.Tiros7M.Add(Momento);
                    break;

                case Gol_Contraataque:
                    _equipo.GolesContraataque.Add(Momento);
                    _equipo.TirosContraataque.Add(Momento);
                    break;

                default:
                    break;

            }
            
            //_equipo.Tirosapuerta.Add(Momento);

            if (_jugador != null)
            {
                _jugador.Goles.Add(Momento);
                //_jugador.Tirosapuerta.Add(Momento);
            }
        }
        public void Undo()
        {
            //SE RESTA EL GOL Y UN TIRO AL EQUIPO
            _equipo.Goles.Remove(Momento);
            _equipo.Tiros.Remove(Momento);

            //DEPENDIENDO DEL TIPO DE GOL SE RESTA SU ESTADÍSTICA (JUNTO CON SU TIRO)
            switch (tipo_gol)
            {
                case Gol_7_M:
                    _equipo.Goles7M.Remove(Momento);
                    _equipo.Tiros7M.Remove(Momento);
                    break;

                case Gol_Contraataque:
                    _equipo.GolesContraataque.Remove(Momento);
                    _equipo.TirosContraataque.Remove(Momento);
                    break;

                default:
                    break;
            }

            //_equipo.Tirosapuerta.Remove(Momento);

            if (_jugador != null)
            {
                _jugador.Goles.Remove(Momento);
                //_jugador.Tirosapuerta.Remove(Momento);
            }
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    string s = "PlayerGolIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "' ,";
                    if (_jugador != null)
                        s += "'" + _jugador.Number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "', '" + getGoles(_jugador, idioma[i]) + "'])";
                    else
                        s += "'', '', '', '', '', ''])";

                    //AÑADE EL TIPO DE GOL COMO ÚLTIMO PARÁMETRO    luis
                    //switch (tipo_gol)
                    //{
                    //    case Gol_Normal:
                    //        s += " 0])";
                    //        break;

                    //    case Gol_7_M:
                    //        s += " 1])";
                    //        break;

                    //    case Gol_Contraataque:
                    //        s += " 2])";
                    //        break;

                    //    default:
                    //        s += " 0])";
                    //        break;

                    //}

                    if (Program.EstaActivado(i))
                        ipf[i].Envia(s);
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("PlayerGolOUT()");

                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            string s;

            switch (tipo_gol)
            {
                case Gol_Normal:
                    s = Momento + " Gol\n";
                    break;

                case Gol_7_M:
                    s = Momento + " Gol 7 M.\n";
                    break;

                case Gol_Contraataque:
                    s = Momento + " Gol CntAtq.\n";
                    break;

                default:
                    s = Momento + " Gol\n";
                    break;

            }

            if (_jugador != null)
            {
                s += _jugador.Number + " " + _jugador.ShortName;
            }
            else
            {
                s += _equipo.ShortName;
            }

            return s;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

        public Jugador GetJugador()
        {
            return _jugador;
        }

        public string getNameCommand()
        {
            return "GolCommand";
        }


        private string getPosicion(Jugador jugador, IdiomaData idioma)
        {
            switch (jugador.Posicion)
            {
                case Jugador.Portero:
                    return idioma.Goalkeeper;
                case Jugador.Extremo:
                    return idioma.Extremo;
                case Jugador.ExtremoDerecho:
                    return idioma.ExtremoDerecho;
                case Jugador.ExtremoIzquierdo:
                    return idioma.ExtremoIzquierdo;
                case Jugador.Lateral:
                    return idioma.Lateral;
                case Jugador.LateralDerecho:
                    return idioma.LateralDerecho;
                case Jugador.LateralIzquierdo:
                    return idioma.LateralIzquierdo;
                case Jugador.Central:
                    return idioma.Central;
                default:
                    return idioma.Pivote;
            }
        }

        private string getGoles(Jugador jugador, IdiomaData idioma)
        {
            return idioma.GoalMatch + " " + Convert.ToString(jugador.Goles.Count);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    class ExclusionCommand : ICommandExecutable, ICommandImmediateExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;

        private bool _visible;

        public ExclusionCommand(Momento momento, Jugador jugador)   // Controlador _control
        {
            Momento = momento;
            _jugador = jugador;
            _equipo = _jugador.Equipo;

            Reset();
        }

        public Jugador GetJugador()
        {
            return _jugador;
        }

        public string getNameCommand()
        {
            return "ExclusionCommand";
        }

        public ExclusionCommand(Momento tiempo, Equipo equipo)      // Controlador _control
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;

            Reset();
        }

        public void Execute()
        {
            _equipo.exclusiones_Totales++;


            if (_jugador != null)
            {
                _jugador.exclusiones_Totales++;
            }
        }

        //public int exclusionsActivas()
        //{
        //    //LISTA DE TODOS LOS COMANDOS DE EXCLUSION
        //    List<ICommand> comandos = _controller.getListaCmd().FindAll(x => x.getNameCommand().Equals("ExclusionCommand"));

        //    List<ExclusionCommand> _comandos = new List<ExclusionCommand>();

        //    foreach (ICommand comando in comandos)
        //    {
        //        _comandos.Add((ExclusionCommand)comando);
        //    }

        //    List<ExclusionCommand> activas;

        //    //AÑADE A LA LISTA TODOS LOS COMANDOS DE EXCLUSIONES EN LOS ULTIMOS 2 MINUTOS
        //    activas = _comandos.FindAll(x => x.Momento.SegundoAbsoluto > (_controller.getCrono().GetMomento().SegundoAbsoluto - 120));

        //    return activas.Count;
        //}

        public void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            string s = "Suspension(['" + _equipo.TeamCode.Replace("'", "\\'") + "', 0, ";
            if (_jugador != null)
            {
                s += "'" + _jugador.Number + "'";
            }
            else
            {
                s += "''";
            }
            s += "])";

            for (int i = 0; i < n; i++)
            {
                if (Program.EstaActivado(i))
                    ipf[i].Envia(s);
            }
        }

        public void Undo()
        {
            _equipo.exclusiones_Totales--;

            if (_jugador != null)
            {
                _jugador.exclusiones_Totales--;
            }

        }

        public void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            //for (int i = 0; i < n; i++)
            //{
            //    if (Program.EstaActivado(i))
            //    {
            //        ipf[i].Envia("Suspension(['" + _equipo.TeamCode + "', " + _equipo.exclusiones_Totales + "])");
            //    }
            //}

            string s = "Suspension(['" + _equipo.TeamCode.Replace("'", "\\'") + "', 1, ";
            if (_jugador != null)
            {
                s += "'" + _jugador.Number + "'";
            }
            else
            {
                s += "''";
            }

            s += "])";
            for (int i = 0; i < n; i++)
            {
                if (Program.EstaActivado(i))
                    ipf[i].Envia(s);

            }
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] _ipfs, IdiomaData[] _idioma, int n)
        {
            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    string s = "PlayerSuspensionIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "', ";
                    if (_jugador != null)
                        s += "'" + _jugador.Number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, _idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "', '" + getExclusions(_jugador, _idioma) + "'])";
                    else
                        s += "'', '', '', '', '', ''])";

                    if (Program.EstaActivado(i))
                        _ipfs[i].Envia(s);

                    //Console.WriteLine("Método Show 1ª Rama de SevenMThrowsCommand: " + s);
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        _ipfs[i].Envia("PlayerSuspensionOUT()");

                    //Console.WriteLine("Método Show 2ª Rama de SevenMThrowsCommand: PlayerOUT()");

                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            string s = Momento + " Exclusion\n";

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

        private string getExclusions(Jugador jugador, IdiomaData[] idioma)
        {
            switch (jugador.exclusiones_Totales)
            {
                case 0:
                    return jugador.exclusiones_Totales + idioma[0].Exclusion_Ordinal1 + " " + idioma[0].Exclusion_Text;

                case 1:
                    return jugador.exclusiones_Totales + idioma[0].Exclusion_Ordinal1 + " " + idioma[0].Exclusion_Text;

                case 2:
                    return jugador.exclusiones_Totales + idioma[0].Exclusion_Ordinal2 + " " + idioma[0].Exclusion_Text;

                case 3:
                    return jugador.exclusiones_Totales + idioma[0].Exclusion_Ordinal3 + " " + idioma[0].Exclusion_Text;

                default:
                    return jugador.exclusiones_Totales + idioma[0].Exclusion_Ordinal4 + " " + idioma[0].Exclusion_Text;
            }
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
    }
}

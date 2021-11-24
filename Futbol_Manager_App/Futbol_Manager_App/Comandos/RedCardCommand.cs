using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class RedCardCommand : ICommandExecutable, ICommandShowable, ICommandImmediateExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private bool _visible;


        public RedCardCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;

            Reset();
        }

        public void Reset()
        {
            _visible = false;
        }

        public void Execute()
        {
            if (_jugador.Equipo.Entrenador != _jugador)
                _jugador.Equipo.TRojas.Add(Momento);
            _jugador.TRojas.Add(Momento);
        }

        public void Undo()
        {
            _jugador.Equipo.TRojas.Remove(Momento);
            _jugador.TRojas.Remove(Momento);
        }

        public void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (_jugador.Posicion >= 1) // No cuentan en el global del equipo, las tarjetas rojas del entrenador y del entrenador asistente
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("RedCard(['" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugador.Equipo.TRojas.Count + "'])");
                }
            }
        }
        public void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (_jugador.Posicion >= 1) // No cuentan en el global del equipo, las tarjetas rojas del entrenador y del entrenador asistente
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("RedCard(['" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugador.Equipo.TRojas.Count + "'])");
                }
            }
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {            
            string number = (_jugador.Number == 0 ? "" : _jugador.Number.ToString());

            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("RedCardIN(['" + idioma[i].RedCardMatch + "', '" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\")+ "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("RedCardOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            string s = Momento + " T. Roja\n";

            // El entrenador no tiene numero
            if (_jugador.Number != 0)
                s += _jugador.Number + " ";

            s += _jugador.ShortName;

            return s;
        }

        public Color GetColor()
        {
            return _jugador.Equipo.Color1;
        }

        private string getPosicion(Jugador jugador, IdiomaData idioma)
        {
            switch (jugador.Posicion)
            {
                case Jugador.Portero:
                    return idioma.Goalkeeper;

                case Jugador.Defensa:
                    return idioma.Defender;

                case Jugador.Centrocampista:
                    return idioma.Midfielder;

                default:
                    return idioma.Forward;
            }
        }

    }
}

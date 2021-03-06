using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    public class AssistantCoachCommand : ICommandShowable
    {
        private Jugador _jugador;
        private bool _visible;


        public AssistantCoachCommand(Jugador jugador)
        {
            _jugador = jugador;

            Reset();
        }

        public Jugador GetJugador()
        {
            return null;
        }

        public string getNameCommand()
        {
            return "AssistantCoachCommand";
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("CoachIdentificationIN(['" + idioma[i].AssistantCoach + "', '" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\")+ "', " + _jugador.SancionSiAmarilla + "])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("CoachIdentificationOUT()");
                }
                _visible = false;

            }
            return _visible;
        }

        override public string ToString()
        {
            return "Entrenador Asistente\n" + _jugador.ShortName;
        }

        public Color GetColor()
        {
            return _jugador.Equipo.Color1;
        }

    }
}


using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class RedCardCommand : ICommandExecutable, ICommandImmediateExecutable, ICommandShowable
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

        public void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (Program.EstaActivado(i))
                {
                    ipf[i].Envia("Crono_Expulsion(['" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '1'])");
                }
            }
        }

        public void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (Program.EstaActivado(i))
                {
                    ipf[i].Envia("Crono_Expulsion(['" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '0'])");
                }
            }
        }

        public void Execute()
        {
            _jugador.Equipo.TRojas.Add(Momento);
            _jugador.TRojas.Add(Momento);
        }
        public void Undo()
        {
            _jugador.Equipo.TRojas.Remove(Momento);
            _jugador.TRojas.Remove(Momento);
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {
            string number = (_jugador.Number == 0 ? "" : _jugador.Number.ToString());

            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {                    
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("RedCardIN(['" + idioma[0].RedCard + "', '" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\")+ "'])");
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

                case Jugador.Cierre:
                    return idioma.Cierre;

                case Jugador.AlaCierre:
                    return idioma.AlaCierre;

                case Jugador.Ala:
                    return idioma.Ala;

                case Jugador.AlaPivot:
                    return idioma.AlaPivot;

                case Jugador.Pivot:
                    return idioma.Pivot;

                default:
                    return idioma.Universal;
            }
        }
    }
}

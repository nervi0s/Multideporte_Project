using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class ChangeCommand : ICommandExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        private Jugador _jugadorIn;
        private Jugador _jugadorOut;

        private int _pasosPendientes;

        public ChangeCommand(Momento tiempo, Jugador jugadorIn, Jugador jugadorOut)
        {
            Momento = tiempo;
            _jugadorIn = jugadorIn;
            _jugadorOut = jugadorOut;

            Reset();
        }
        
        public void Reset()
        {
            _pasosPendientes = 3;
        }

        public void Execute()
        {
            Equipo equipo = _jugadorIn.Equipo;

            equipo.Cambios.Add(Momento);

            equipo.Banquillo.Remove(_jugadorIn);
            equipo.Jugadores.Remove(_jugadorOut);
            equipo.Banquillo.Add(_jugadorOut);
            equipo.Jugadores.Add(_jugadorIn);
        }

        public void Undo()
        {
            Equipo equipo = _jugadorIn.Equipo;

            equipo.Cambios.Remove(Momento);

            equipo.Banquillo.Remove(_jugadorOut);
            equipo.Jugadores.Remove(_jugadorIn);
            equipo.Banquillo.Add(_jugadorIn);
            equipo.Jugadores.Add(_jugadorOut);
        }

        // Comprueba si es posible deshacer el cambio
        // Si, por otros cambios posteriores, los jugadores involucrados no se encuentran no es posible
        // En mensaje se devuelve un texto descriptivo del motivo
        public bool CheckUndo(out string mensaje)
        {
            Equipo equipo = _jugadorIn.Equipo;

            bool posible = true;
            mensaje = "No es posible deshacer el cambio por los siguientes motivos:\n";

            if (!equipo.Banquillo.Contains(_jugadorOut))
            {
                posible = false;
                mensaje += "\n-El jugador " + _jugadorOut.Number + " " + _jugadorOut.FullName + " no se encuentra actualmente en el banquillo.";
            }
            if (!equipo.Jugadores.Contains(_jugadorIn))
            {
                posible = false;
                mensaje += "\n-El jugador " + _jugadorIn.Number + " " + _jugadorIn.FullName + " no se encuentra actualmente en el campo.";
            }

            return posible;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {
          
            if (_pasosPendientes == 3)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("ChangeIN(['" + idioma[i].Change + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorIn.Number + "', '" + _jugadorIn.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.ShortName.Replace("'", "\\'") + "', '" + _jugadorOut.Number + "', '" + _jugadorOut.FullName.Replace("'", "\\'") + "', '" + _jugadorOut.ShortName.Replace("'", "\\'") + "'])");
                }
                _pasosPendientes = 2;
            }
            else if (_pasosPendientes == 2)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("ChangeChange()");
                }
                _pasosPendientes = 1;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("ChangeOUT()");
                }
                _pasosPendientes = 3;
            }

            return _pasosPendientes != 3;
        }

        override public string ToString()
        {
            return Momento + " Cambio\n" + _jugadorIn.Number + " " + _jugadorIn.ShortName + "\n" + _jugadorOut.Number + " " + _jugadorOut.ShortName;
        }

        public Color GetColor()
        {
            return _jugadorIn.Equipo.Color1;
        }

    }
}

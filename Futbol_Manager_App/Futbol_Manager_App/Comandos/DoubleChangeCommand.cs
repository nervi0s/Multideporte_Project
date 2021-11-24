using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class DoubleChangeCommand : ICommandExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador1In;
        private Jugador _jugador2In;
        private Jugador _jugador1Out;
        private Jugador _jugador2Out;
        private int _pasosPendientes;


        public DoubleChangeCommand(Momento tiempo, Jugador jugador1In, Jugador jugador2In, Jugador jugador1Out, Jugador jugador2Out)
        {
            Momento = tiempo;
            _jugador1In = jugador1In;
            _jugador2In = jugador2In;
            _jugador1Out = jugador1Out;
            _jugador2Out = jugador2Out;

            Reset();
        }
        
        public void Reset()
        {
            _pasosPendientes = 3;
        }

        public void Execute()
        {
            Equipo equipo = _jugador1In.Equipo;

            equipo.Cambios.Add(Momento);
            equipo.Banquillo.Remove(_jugador1In);
            equipo.Jugadores.Remove(_jugador1Out);
            equipo.Banquillo.Add(_jugador1Out);
            equipo.Jugadores.Add(_jugador1In);

            equipo.Cambios.Add(Momento);
            equipo.Banquillo.Remove(_jugador2In);
            equipo.Jugadores.Remove(_jugador2Out);
            equipo.Banquillo.Add(_jugador2Out);
            equipo.Jugadores.Add(_jugador2In);
        }

        public void Undo()
        {
            Equipo equipo = _jugador1In.Equipo;

            equipo.Cambios.Remove(Momento);
            equipo.Banquillo.Remove(_jugador1Out);
            equipo.Jugadores.Remove(_jugador1In);
            equipo.Banquillo.Add(_jugador1In);
            equipo.Jugadores.Add(_jugador1Out);

            equipo.Cambios.Remove(Momento);
            equipo.Banquillo.Remove(_jugador2Out);
            equipo.Jugadores.Remove(_jugador2In);
            equipo.Banquillo.Add(_jugador2In);
            equipo.Jugadores.Add(_jugador2Out);
        }

        // Comprueba si es posible deshacer el cambio
        // Si, por otros cambios posteriores, los jugadores involucrados no se encuentran no es posible
        // En mensaje se devuelve un texto descriptivo del motivo
        public bool CheckUndo(out string mensaje)
        {
            Equipo equipo = _jugador1In.Equipo;

            bool posible = true;
            mensaje = "No es posible deshacer el cambio por los siguientes motivos:\n";

            if (!equipo.Banquillo.Contains(_jugador1Out))
            {
                posible = false;
                mensaje += "\n-El jugador " + _jugador1Out.Number + " " + _jugador1Out.FullName + " no se encuentra actualmente en el banquillo.";
            }
            if (!equipo.Jugadores.Contains(_jugador1In))
            {
                posible = false;
                mensaje += "\n-El jugador " + _jugador1In.Number + " " + _jugador1In.FullName + " no se encuentra actualmente en el campo.";
            }
            if (!equipo.Banquillo.Contains(_jugador2Out))
            {
                posible = false;
                mensaje += "\n-El jugador " + _jugador2Out.Number + " " + _jugador2Out.FullName + " no se encuentra actualmente en el banquillo.";
            }
            if (!equipo.Jugadores.Contains(_jugador2In))
            {
                posible = false;
                mensaje += "\n-El jugador " + _jugador2In.Number + " " + _jugador2In.FullName + " no se encuentra actualmente en el campo.";
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
                        ipf[i].Envia("DoubleChangeIN(['" + _jugador1In.Equipo.TeamCode.Replace("'", "\\'") + "', '" +
                            _jugador1In.Number + "', '" + _jugador1In.FullName.Replace("'", "\\'") + "', '" + _jugador1In.ShortName.Replace("'", "\\'") + "', '" +
                            _jugador2In.Number + "', '" + _jugador2In.FullName.Replace("'", "\\'") + "', '" + _jugador2In.ShortName.Replace("'", "\\'") + "', '" +
                            _jugador1Out.Number + "', '" + _jugador1Out.FullName.Replace("'", "\\'") + "', '" + _jugador1Out.ShortName.Replace("'", "\\'") + "', '" +
                            _jugador2Out.Number + "', '" + _jugador2Out.FullName.Replace("'", "\\'") + "', '" + _jugador2Out.ShortName.Replace("'", "\\'") + "'])");
                }
                _pasosPendientes = 2;
            }
            else if (_pasosPendientes == 2)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("DoubleChangeChange()");
                }
                _pasosPendientes = 1;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("DoubleChangeOUT()");
                }
                _pasosPendientes = 3;
            }

            return _pasosPendientes != 3;
        }

        override public string ToString()
        {
            return Momento + " Cambio doble\n" + _jugador1In.Number + " " + _jugador1In.ShortName + " ; " +
                _jugador2In.Number + " " + _jugador2In.ShortName + "\n" + 
                _jugador1Out.Number + " " + _jugador1Out.ShortName + " ; " +
                _jugador2Out.Number + " " + _jugador2Out.ShortName;
        }

        public Color GetColor()
        {
            return _jugador1In.Equipo.Color1;
        }

    }
}

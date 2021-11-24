using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class IdentificationCommand : ICommandShowable
    {
        private Arbitro _arbitro;
        private bool _visible;
        
        public IdentificationCommand(Arbitro arbitro)
        {
            _arbitro = arbitro;

            Reset();
        }

        public string getNameCommand()
        {
            return "IdentificationCommand";
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {           
            if (!_visible)
            {
                //string nombre = _arbitro.FullName.Replace("'", "\\'");
                //if (_arbitro.Nacionalidad != "")
                //    nombre += " (" + _arbitro.Nacionalidad + ")";

                for (int i = 0; i < n; i++)
                {
                    string cargo = cargoName(_arbitro.Cargo, idioma[i]);
                                    
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("IdentificationIN(['" + cargo + "', '" + _arbitro.FullName.Replace("'", "\\'") + "', '" + _arbitro.ShortName.Replace("'", "\\'") + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("IdentificationOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            string cargo = cargoName(_arbitro.Cargo);

            return cargo + "\n" + _arbitro.FullName;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

        public Jugador GetJugador()
        {
            return null;
        }

        private string cargoName(int cargo, IdiomaData idioma)
        {
            switch (cargo)
            {
                case Arbitro.Arbitro1:
                    return idioma.Referee;

                case Arbitro.Arbitro2:
                    return idioma.Referee;

                default:
                    return "";
            }
        }
        private string cargoName(int cargo)
        {
            switch (cargo)
            {
                case Arbitro.Arbitro1:
                    return "Árbitro 1";

                case Arbitro.Arbitro2:
                    return "Árbitro 2";

                default:
                    return "";
            }
        }

    }
}

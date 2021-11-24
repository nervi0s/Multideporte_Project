using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
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

        private string cargoName(int cargo, IdiomaData idioma)
        {
            switch (cargo)
            {
                case Arbitro.arbitro1:
                    return idioma.Referee;

                case Arbitro.arbitro2:
                    return idioma.Assistant1;

                case Arbitro.arbitro3:
                    return idioma.Assistant2;

                case Arbitro.arbitro4:
                    return idioma.Referee4;

                //case Arbitro.Auxiliar1:
                //    return idioma.AssistantAux;
                //case Arbitro.Auxiliar2:
                //    return idioma.AssistantAux;

                default:
                    return "";
            }
        }
        private string cargoName(int cargo)
        {
            switch (cargo)
            {
                case Arbitro.arbitro1:
                    return "Árbitro Principal";

                case Arbitro.arbitro2:
                    return "Árbitro Linier 1";

                case Arbitro.arbitro3:
                    return "Árbitro Linier 2";

                case Arbitro.arbitro4:
                    return "4º Árbitro";

                //case Arbitro.Auxiliar1:
                //    return "Árbitro Auxiliar 1";
                //case Arbitro.Auxiliar2:
                //    return "Árbitro Auxiliar 2";

                default:
                    return "";
            }
        }

    }
}

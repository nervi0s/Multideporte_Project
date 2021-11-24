using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
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
                    //string cargo = cargoName(_arbitro.Cargo, idioma[i]);
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
                    return idioma.Referee;

                default:
                    return "";
            }
        }
        private string cargoName(int cargo)
        {
            switch (cargo)
            {
                case Arbitro.arbitro1:
                    return "Árbitro";

                case Arbitro.arbitro2:
                    return "Árbitro";

                default:
                    return "";
            }
        }

    }
}

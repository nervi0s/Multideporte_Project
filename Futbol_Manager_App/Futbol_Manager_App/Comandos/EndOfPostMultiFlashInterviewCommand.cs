using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class EndOfPostInterviewCommand : ICommandShowable
    {
        public EndOfInterview EndOfpostInterview { get; set; }
        private bool _visible;


        public EndOfPostInterviewCommand(EndOfInterview en)
        {
            EndOfpostInterview = en;
            Reset();
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            // Si existe telefono se llama a las funciones PreMatch_2lineas_IN/OUT en lugar de PreMatchIN/OUT
            //string funName = (Prematch.Telefono == "" ? "PreMatch" : "PreMatch_2lineas_");

            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("EndPostMultiFlashInterviewsIN(['" + EndOfpostInterview.Linea1.Replace("'", "\\'") + "', '" + EndOfpostInterview.Linea2.Replace("'", "\\'") + "', '" + EndOfpostInterview.Linea3.Replace("'", "\\'") + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("EndPostMultiFlashInterviewsOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return EndOfpostInterview.Linea1 + "\n" + EndOfpostInterview.Linea2 + "\n" + EndOfpostInterview.Linea3;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

    }
}

using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class YellowCardCommand : ICommandExecutable, ICommandShowable, ICommandImmediateExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private int _pasosPendientes;


        public YellowCardCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;

            Reset();
        }


        public void Reset()
        {
            _pasosPendientes = 3;
        }

        public void Execute()
        {
            /*if (_jugador.Equipo.Entrenador != _jugador)
                _jugador.Equipo.TarjetasAmarillas.Add(Momento);*/

            if (_jugador != null)
                _jugador.TAmarillas.Add(Momento);
        }

        public void Undo()
        {
            _jugador.Equipo.TAmarillas.Remove(Momento);
            int aux = _jugador.TAmarillas.Count;

            _jugador.TAmarillas.Remove(Momento);

        }            

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            string number = (_jugador.Number == 0 ? "" : _jugador.Number.ToString());
                        
            if (_pasosPendientes == 3)
            {
                //string miss = (_jugador.SancionSiAmarilla ? idioma[0].MissesNextMatch : "");
                //Console.WriteLine(miss);
                //Console.WriteLine("-{0}-", idioma[0].YellowSingleCard);
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("YellowCardIN(['" + idioma[i].YellowSingleCard + "', '" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "'])");
                }
                _pasosPendientes = 1;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("YellowCardOUT()");
                }
                _pasosPendientes = 3;
            }
            
            return _pasosPendientes != 3;
        }

        override public string ToString()
        {
            string s = Momento + " T. Amarilla\n";

            // El entrenador no tiene numero
            if (_jugador.Number != 0)
                s += _jugador.Number + " ";

            s += _jugador.ShortName;

            //if (_jugador.SancionSiAmarilla)
            //    s += "\nNo juega el próximo";

            return s;
        }

        public Color GetColor()
        {
            return _jugador.Equipo.Color1;
        }

        public Jugador GetJugador()
        {
            return null;
        }

        public string getNameCommand()
        {
            return "YellowCardCommand";
        }

        public void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            //try
            //{
            //    // Doble Amarailla
            //    if (_jugador.TAmarillas.Count > 1)
            //    {
            //        for (int i = 0; i < n; i++)
            //        {
            //            if (Program.EstaActivado(i))
            //                ipf[i].Envia("RedCard(['" + _jugador.Equipo.TeamCode + "', '" + _jugador.Equipo.TRojas.Count + "'])");
            //        }
            //    }
            //}
            //catch (Exception immediate)
            //{
            //    Console.WriteLine("Error en execute immediate de tarjeta amarillo " + immediate);
            //}
        }


        public void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            //try
            //{
            //    // Doble Amarilla
            //    if (_jugador.TAmarillas.Count > 0)
            //    {
            //        for (int i = 0; i < n; i++)
            //        {
            //            if (Program.EstaActivado(i))
            //                ipf[i].Envia("RedCard(['" + _jugador.Equipo.TeamCode + "', '" + _jugador.Equipo.TRojas.Count + "'])");
            //        }
            //    }
            //}
            //catch (Exception immediate)
            //{
            //    Console.WriteLine("Error en undo immediate de tarjeta amarillo " + immediate);
            //}
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

using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class BlueCardCommand : ICommandExecutable, ICommandShowable, ICommandImmediateExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private int _pasosPendientes;

        public BlueCardCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;

            Reset();
        }

        public Jugador GetJugador()
        {
            return null;
        }

        public string getNameCommand()
        {
            return "BlueCardCommand";
        }

        public void Reset()
        {
            _pasosPendientes = 3;
        }

        public void Execute()
        {
            if (_jugador.Equipo.Entrenador != _jugador)
                _jugador.Equipo.TAzules.Add(Momento);

            _jugador.TAzules.Add(Momento);
     
        }

        public void Undo()
        {
            _jugador.Equipo.TAzules.Remove(Momento);
            int aux = _jugador.TAzules.Count;

            _jugador.TAzules.Remove(Momento);

        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {
            string number = (_jugador.Number == 0 ? "" : _jugador.Number.ToString());

            if (_pasosPendientes == 3)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("BlueCardIN(['" + idioma[i].BlueCard + "', '" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "'])");
                }
                _pasosPendientes = 1;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("BlueCardOUT()");
                }
                _pasosPendientes = 3;
            }

            return _pasosPendientes != 3;
        }

        override public string ToString()
        {
            string s = Momento + " T. Azul\n";

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

        public void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            //try
            //{
            //    for (int i = 0; i < n; i++)
            //    {
            //        if (Program.EstaActivado(i))
            //            ipf[i].Envia("RedCard(['" + _jugador.Equipo.TeamCode + "', '" + _jugador.Equipo.TarjetasAzules.Count + "'])");
            //    }
            //}
            //catch (Exception immediate)
            //{
            //    Console.WriteLine("Error en execute immediate de tarjeta azul " + immediate);
            //}
        }

        public void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            //try
            //{
            //    for (int i = 0; i < n; i++)
            //    {
            //        ////if (Program.EstaActivado(i))
            //        ////    ipf[i].Envia("RedCard(['" + _jugador.Equipo.TeamCode + "', '" + _jugador.Equipo.TarjetasAzules.Count + "'])");
            //    }
            //}
            //catch (Exception immediate)
            //{
            //    Console.WriteLine("Error en undo immediate de tarjeta azul " + immediate);
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

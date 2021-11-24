using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
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
            if (_jugador.Equipo.Entrenador != _jugador)
                _jugador.Equipo.TAmarillas.Add(Momento);
            _jugador.TAmarillas.Add(Momento);

            if (_jugador.TAmarillas.Count > 1)
            {
                if (_jugador.Equipo.Entrenador != _jugador)
                    _jugador.Equipo.TRojas.Add(Momento);
                _jugador.TRojas.Add(Momento);
            }
        }

        public void Undo()
        {
            _jugador.Equipo.TAmarillas.Remove(Momento);
            int aux = _jugador.TAmarillas.Count;
            _jugador.TAmarillas.Remove(Momento);

            if (aux > 1)
            {
                _jugador.Equipo.TRojas.Remove(Momento);
                _jugador.TRojas.Remove(Momento);
            }
        }

        public void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            // Doble Amarilla
            if (_jugador.TAmarillas.Count > 1)
            {
                if (_jugador.Posicion >= 1) // No cuentan en el global del equipo, las tarjetas rojas del entrenador y del entrenador asistente
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("RedCard(['" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugador.Equipo.TRojas.Count + "'])");
                    }
                }
            }
        }
        public void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            // Doble Amarilla
            if (_jugador.TAmarillas.Count > 0)
            {
                if (_jugador.Posicion >= 1) // No cuentan en el global del equipo, las tarjetas rojas del entrenador y del entrenador asistente
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("RedCard(['" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugador.Equipo.TRojas.Count + "'])");
                    }
                }
            }
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {
            string number = (_jugador.Number == 0 ? "" : _jugador.Number.ToString());
           
            // Doble Amarilla
            if (_jugador.TAmarillas.Count > 1)
            {
                if (_pasosPendientes == 3)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("DoubleYellowCardIN(['" + idioma[i].SecondYellowCard + "', '" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\")+ "'])");
                    }
                    _pasosPendientes = 2;
                }
                else if (_pasosPendientes == 2)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("ChangeDoubleYellowCard()");
                    }
                    _pasosPendientes = 1;
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("DoubleYellowCardOUT()");
                    }
                    _pasosPendientes = 3;
                }
            }
            else // Amarilla sencilla
            {
                if (_pasosPendientes == 3)
                {
                    //Console.WriteLine(_jugador.Posicion);
                    
                    string miss = (_jugador.SancionSiAmarilla ? idioma[0].MissesNextMatch : "");
                    //Console.WriteLine(miss);
                    //Console.WriteLine("-{0}-", idioma[0].YellowSingleCard);
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("YellowCardIN(['" + idioma[i].YellowSingleCard + "', '" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\")+ "', '" + miss + "'])");
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

            if (_jugador.SancionSiAmarilla)
                s += "\nNo juega el próximo";

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

                case Jugador.Defensa:
                    return idioma.Defender;

                case Jugador.Centrocampista:
                    return idioma.Midfielder;

                default:
                    return idioma.Forward;
            }
        }
    }
}

using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class YellowCardCommand : ICommandExecutable, ICommandImmediateExecutable, ICommandShowable
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
            _jugador.Equipo.TAmarillas.Add(Momento);
            _jugador.TAmarillas.Add(Momento);

            if (_jugador.TAmarillas.Count > 1)
                _jugador.Equipo.TRojas.Add(Momento);
        }

        public void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (_jugador.TAmarillas.Count > 1)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                    {
                        ipf[i].Envia("Crono_Expulsion(['" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '1'])");
                    }
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

        public void Undo()
        {
            _jugador.Equipo.TAmarillas.Remove(Momento);
            _jugador.TAmarillas.Remove(Momento);
            _jugador.Equipo.TRojas.Remove(Momento);
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
                            ipf[i].Envia("DoubleYellowCardIN(['" + idioma[i].SecondYellowCard + "', '" +  _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\")+ "'])");
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
                    string miss = (_jugador.SancionSiAmarilla ? idioma[0].MissesNextMatch : "");

                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("YellowCardIN(['" + idioma[0].YellowCard + "', '" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "', '" + miss + "'])");
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

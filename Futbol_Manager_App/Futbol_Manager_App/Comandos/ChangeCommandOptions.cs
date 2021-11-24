using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class ChangeCommandOptions : ICommandExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        public int _option = -1;
        public string EstadisticaCambio = "";

        public const int CambioIn = 200;
        public const int CambioOut = 201;
        public const int CambioInInfo = 202;
        public const int CambioOutInfo = 203;
        public const int CambioInfo = 204;

        public const int Corners = 3;
        public const int Faltas = 4;
        public const int TirosAPuerta = 6;
        public const int TirosAPuertaDentro = 61;
        public const int TirosAPuertaComp = 63;
        public const int Paradas = 7;
        public const int TAmarillas = 8;
        public const int TRojas = 9;
        public const int FuerasDeJuego = 10;
        public const int Cambios = 11;
        public const int Goles = 12;

        private Jugador _jugadorIn;
        private Jugador _jugadorOut;
        private bool _descanso;

        private int _pasosPendientes;
        private bool _visible;

        
        public ChangeCommandOptions(Momento tiempo, Jugador jugadorIn, Jugador jugadorOut, int option)
        {
            Momento = tiempo;
            _jugadorIn = jugadorIn;
            _jugadorOut = jugadorOut;
            this._option = option;
            _descanso = tiempo.EsDescanso();
            Reset();
        }

        public void Reset()
        {
            _pasosPendientes = 3;
            _visible = false;
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

        public string EstadisticaJugadorIn(Jugador jugador, IdiomaData idioma)
        {
            string s = "";
            int goles = jugador.Goals ;
            int partidos = jugador.Matches;

            s = goles + " " + (goles != 1 ? idioma.GoalsPerMatch : idioma.GoalPerMatch) + " " + partidos + " " + (partidos != 1 ? idioma.Matches : idioma.Match);
                
            return s;
        }

        public string EstadisticaJugadorOut(Jugador jugador, IdiomaData idioma)
        {
            string s = "";
            int golesTotal = jugador.Goals + jugador.Goles.Count;
            int golesPartido = jugador.Goles.Count; 
            int partidos = jugador.Matches + 1;
            int aPuerta = jugador.Tirosfuera.Count;
            int entrePalos = jugador.Tirosapuerta.Count;            
            int tiros = aPuerta + entrePalos;
            
            if (golesPartido > 0)
            {
                s = tiros + " " + (tiros != 1 ? idioma.Kicks : idioma.Kick) + "  " + entrePalos + " " + idioma.IntoGoal + "  " + golesPartido + " " + (golesPartido != 1 ? idioma.Goals : idioma.Goal);
            }
            else if (tiros > 1)
            {
                s = tiros + " " + idioma.Kicks + "  " + entrePalos + " " + idioma.IntoGoal;
            }
            else
            {
                // GOLES y PARTIDOS
                s = golesTotal + " " + (golesTotal != 1 ? idioma.GoalsPerMatch : idioma.GoalPerMatch) + " " + partidos + " " + (partidos != 1 ? idioma.Matches : idioma.Match);
            }

            return s;
        }
                              
        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            this._option = Program.CambioEscogido;
            
            if (_option == 204) // CAMBIO INFO
            {
                if (_pasosPendientes == 3)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                        {
                            if (_descanso)
                                ipf[i].Envia("ChangeInInfo(['" + idioma[i].HalfTimeChange + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorIn.Number + "', '" + _jugadorIn.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.ShortName.Replace("'", "\\'") + "', '" + _jugadorOut.Number + "', '" + _jugadorOut.FullName.Replace("'", "\\'") + "', '" + _jugadorOut.ShortName.Replace("'", "\\'") + "', '" + EstadisticaJugadorIn(_jugadorIn, idioma[i]) + "','" + EstadisticaJugadorOut(_jugadorOut, idioma[i]) + "'])");
                            else
                                ipf[i].Envia("ChangeInInfo(['" + idioma[i].Change + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorIn.Number + "', '" + _jugadorIn.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.ShortName.Replace("'", "\\'") + "', '" + _jugadorOut.Number + "', '" + _jugadorOut.FullName.Replace("'", "\\'") + "', '" + _jugadorOut.ShortName.Replace("'", "\\'") + "', '" + EstadisticaJugadorIn(_jugadorIn, idioma[i]) + "','" + EstadisticaJugadorOut(_jugadorOut, idioma[i]) + "'])");
                        }
                    }
                    _pasosPendientes = 2;
                }
                else if (_pasosPendientes == 2)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("ChangeChangeInfo()");
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

            else if (_option == 203) //CAMBIO OUT INFO
            {
                if (!_visible)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            if (_descanso)
                                ipf[i].Envia("ChangeOnlyOutInfo(['" + idioma[i].HalfTimeChange + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorOut.Number + "', '" + _jugadorOut.FullName.Replace("'", "\\'") + "', '" + _jugadorOut.ShortName.Replace("'", "\\'") + "', '" + EstadisticaJugadorOut(_jugadorOut, idioma[i]) + "'])");
                            else
                                ipf[i].Envia("ChangeOnlyOutInfo(['" + idioma[i].Change + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorOut.Number + "', '" + _jugadorOut.FullName.Replace("'", "\\'") + "', '" + _jugadorOut.ShortName.Replace("'", "\\'") + "', '" + EstadisticaJugadorOut(_jugadorOut, idioma[i]) + "'])");                            
                    }
                    _visible = true;
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("ChangeOUT()");
                    }
                    _visible = false;
                }
                return _visible;
            }

            else if (_option == 202) // CAMBIO IN INFO
            {
                if (!_visible)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                        {
                            if (_descanso)
                                ipf[i].Envia("ChangeOnlyInInfo(['" + idioma[i].HalfTimeChange + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorIn.Number + "', '" + _jugadorIn.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.ShortName.Replace("'", "\\'") + "', '" + EstadisticaJugadorIn(_jugadorIn, idioma[i]) + "'])");
                            else
                                ipf[i].Envia("ChangeOnlyInInfo(['" + idioma[i].Change + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorIn.Number + "', '" + _jugadorIn.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.ShortName.Replace("'", "\\'") + "', '" + EstadisticaJugadorIn(_jugadorIn, idioma[i]) + "'])");                            
                        }
                    }
                    _visible = true;
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("ChangeOUT()");
                    }
                    _visible = false;

                }
                return _visible;
            }

            else if (_option == 201) //CAMBIO OUT
            {
                if (!_visible)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            if (_descanso)
                                ipf[i].Envia("ChangeOnlyOUT(['" + idioma[i].HalfTimeChange + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorOut.Number + "', '" + _jugadorOut.FullName.Replace("'", "\\'") + "', '" + _jugadorOut.ShortName.Replace("'", "\\'") + "'])");
                            else
                                ipf[i].Envia("ChangeOnlyOUT(['" + idioma[i].Change + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorOut.Number + "', '" + _jugadorOut.FullName.Replace("'", "\\'") + "', '" + _jugadorOut.ShortName.Replace("'", "\\'") + "'])");
                    }
                    _visible = true;
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("ChangeOUT()");
                    }
                    _visible = false;
                }
                return _visible;

            }
            else if (_option == 200) //CAMBIO IN
            {
                if (!_visible)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            if (_descanso)
                                ipf[i].Envia("ChangeOnlyIN(['" + idioma[i].HalfTimeChange + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorIn.Number + "', '" + _jugadorIn.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.ShortName.Replace("'", "\\'") + "'])");
                            else
                                ipf[i].Envia("ChangeOnlyIN(['" + idioma[i].Change + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorIn.Number + "', '" + _jugadorIn.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.ShortName.Replace("'", "\\'") + "'])");
                    }
                    _visible = true;
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("ChangeOUT()");
                    }
                    _visible = false;

                }
                return _visible;
            }

            else
            {
                if (_pasosPendientes == 3)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            if (_descanso)
                                ipf[i].Envia("ChangeIN(['" + idioma[i].HalfTimeChange + "', '" + _jugadorIn.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugadorIn.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugadorIn.Number + "', '" + _jugadorIn.FullName.Replace("'", "\\'") + "', '" + _jugadorIn.ShortName.Replace("'", "\\'") + "', '" + _jugadorOut.Number + "', '" + _jugadorOut.FullName.Replace("'", "\\'") + "', '" + _jugadorOut.ShortName.Replace("'", "\\'") + "'])");
                            else
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
        }

        override public string ToString()
        {
            IdiomaData[] _idiomas = new IdiomaData[10];
            // Carga configuración
            ConfigData _config = PersistenciaUtil.CargaConfig();

            // Carga los ficheros de idiomas de los Ipfs configurados
            for (int i = 0; i < _config.NumIpf; i++)
            {
                switch (i)
                {
                    case 0: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero); break;
                    case 1: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero2); break;
                    case 2: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero3); break;
                    case 3: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero4); break;
                    case 4: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero5); break;
                    case 5: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero6); break;
                    case 6: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero7); break;
                    case 7: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero8); break;
                    case 8: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero9); break;
                    case 9: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero10); break;
                    default: break;
                }
            }

            return Momento + " Cambio\n" + _jugadorIn.Number + " " + _jugadorIn.ShortName + "\n" + _jugadorOut.Number + " " + _jugadorOut.ShortName + "\n" + _jugadorIn.ShortName+ " " + EstadisticaJugadorIn(_jugadorIn, _idiomas[0]) + "\n" + _jugadorOut.ShortName + " " +  EstadisticaJugadorOut(_jugadorOut,_idiomas[0]);
        }

        public Color GetColor()
        {
            return _jugadorIn.Equipo.Color1;
        }

    }
}


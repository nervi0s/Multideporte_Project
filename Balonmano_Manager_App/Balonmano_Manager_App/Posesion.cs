using System;

namespace Balonmano_Manager_App
{

    /**
     * Lógica de la Posesión
     */
    [Serializable]
    public class Posesion
    {

        private int _acumuladoLocal;
        private int _acumuladoVisitante;
        private DateTime _posesionLast;
        private bool _posesionLocal;
        private bool _enjuego;

        public bool _modificado = false; // Variable que indica si se ha forzado una modificación manual en las estadísticas
        private int _acumuladoLocalModificado;
        private int _acumuladoVisitanteModificado;

        /**
         * Constructor
         */
        public Posesion()
        {
            _acumuladoLocal = 0;
            _acumuladoVisitante = 0;
            _enjuego = false;
        }

        /**
         * Inicia un periodo de posesión
         * Se indica si el equipo es el local, sino será el visitante.
         */
        public void posesionStart(bool local)
        {
            if (!(_enjuego && _posesionLocal == local))
            {
                if (_enjuego) posesionStop();

                Console.WriteLine("Ini Posesión " + (local ? "local" : "visitante"));

                _posesionLast = DateTime.Now;

                _enjuego = true;
                _posesionLocal = local;
            }
        }

        /**
         * Detiene la posesión
         */
        public void posesionStop()
        {
            if (_enjuego)
            {
                DateTime newDate = DateTime.Now;
                TimeSpan ts = newDate - _posesionLast;
                int difference = ts.Seconds;

                if (_posesionLocal)
                {
                    _acumuladoLocal += difference;
                }
                else
                {
                    _acumuladoVisitante += difference;
                }

                _enjuego = false;

                Console.WriteLine("Fin Posesión " + (_posesionLocal ? "local" : "visitante") + 
                    " (Acumulado " + (_posesionLocal ? _acumuladoLocal : _acumuladoVisitante) + "s)");
            }
        }

        /**
         * Devuelve el porcentaje de posesión del equipo local
         */
        public string getPorcentajeLocal()
        {
            if (_modificado)
                return _acumuladoLocalModificado + "%";

            int acumulado = _acumuladoLocal + _acumuladoVisitante;
            if (acumulado == 0)
            {
                return "0%";
            }
            else
            {
                return Convert.ToString(Math.Round(100.0 * _acumuladoLocal / acumulado)) + "%";
            }
        }

        /**
         * Devuelve el porcentaje de posesión del equipo visitante
         */
        public string getPorcentajeVisitante()
        {
            if (_modificado)
                return _acumuladoVisitanteModificado + "%";

            int acumulado = _acumuladoLocal + _acumuladoVisitante;
            if (acumulado == 0)
            {
                return "0%";
            }
            else
            {
                return Convert.ToString(Math.Round(100.0 * _acumuladoVisitante / acumulado)) + "%";
            }
        }
        
        public void setPorcentajeLocal(int porcentaje)
        {
            if (!_modificado)
                _modificado = true;

            _acumuladoLocalModificado = porcentaje;
        }

        public void setPorcentajeVisitante(int porcentaje)
        {
            if (!_modificado)
                _modificado = true;

            _acumuladoVisitanteModificado = porcentaje;
        }

    }
}

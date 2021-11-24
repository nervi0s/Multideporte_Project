using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Futbol_Sala_Manager_App
{
    class PausableTimer : Timer
    {
        private int minutosIniciales;
        private int segundosIniciales;

        public double RemainingAfterPause { get; private set; }

        private readonly Stopwatch _stopwatch;
        private readonly double _initialInterval;
        private bool _resumed;
        public PausableTimer(double interval, int minutosIniciales, int segundosIniciales) : base(interval)
        {
            this.minutosIniciales = minutosIniciales;
            this.segundosIniciales = segundosIniciales;
            _initialInterval = interval;
            Elapsed += OnElapsed;
            _stopwatch = new Stopwatch();
        }

        public new void Start()
        {
            ResetStopwatch();
            base.Start();
        }

        private void OnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_resumed)
            {
                _resumed = false;
                Stop();
                Interval = _initialInterval;
                Start();


            }

            segundosIniciales--;
            if (segundosIniciales < 0)
            {
                segundosIniciales = 59;
                minutosIniciales--;
            }

            ResetStopwatch();
        }

        private void ResetStopwatch()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public void Pause()
        {
            Stop();
            _stopwatch.Stop();
            RemainingAfterPause = Interval - _stopwatch.Elapsed.TotalMilliseconds;
        }

        public void Resume()
        {
            _resumed = true;
            Interval = RemainingAfterPause;
            RemainingAfterPause = 0;
            Start();
        }

    }
    
}

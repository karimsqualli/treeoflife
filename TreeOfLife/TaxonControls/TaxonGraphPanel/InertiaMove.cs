using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

namespace TreeOfLife
{
    //====================================================================
    // Time tool
    //
    static class CurrentMillis
    {
        private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long Millis { get { return (long)((DateTime.UtcNow - Jan1St1970).TotalMilliseconds); } }
    }

    //====================================================================
    // Inertia move
    //
    public class InertiaMove : IDisposable
    {
        //-------------------------------------------------------------------
        // constants
        static readonly int NumberOfDynamicSample = 10;
        static readonly float SpeedMin = 100;
        static readonly float SpeedMax = 1000;
        static readonly int Interval = 50;
        static readonly float OoInterval = 0.02f;

        //-------------------------------------------------------------------
        TaxonGraphPanel _Owner = null;
        System.Timers.Timer _DynamicMoveTimer = null;
        //
        public InertiaMove( TaxonGraphPanel _owner )
        {
            _Owner = _owner;
            ResetAll();
        }

        //-------------------------------------------------------------------
        public void Dispose()
        {
            if (_DynamicMoveTimer != null) _DynamicMoveTimer.Dispose();
            _DynamicMoveTimer = null;

        }

        //-------------------------------------------------------------------
        struct Sample
        {
            public Point position;
            public long millis;
        }

        readonly Sample[] _DynamicSamples = new Sample[NumberOfDynamicSample];
        Sample _RefSample = new Sample();

        int currentNumber = 0;
        int currentIndex = 0;

        int NextIndex(int _index)
        {
            if (_index + 1 >= NumberOfDynamicSample) return 0;
            return _index + 1;
        }

        int PreviousIndex(int _index)
        {
            if (_index <= 0) return NumberOfDynamicSample - 1;
            return _index - 1;
        }

        //-------------------------------------------------------------------
        void ResetAll()
        {
            currentNumber = 0;
            currentIndex = 0;
            ResetSpeed();
        }

        //-------------------------------------------------------------------
        public void AddSample(Point _position)
        {
            if (!TaxonUtils.MyConfig.Options.InertiaActive) return;

            int x = _position.X - _RefSample.position.X;
            int y = _position.Y - _RefSample.position.Y;
            if (x * x + y * y < 8) return;

            long millis = CurrentMillis.Millis;
            if (millis == _RefSample.millis)
                return;

            _DynamicSamples[currentIndex].position.X = _position.X - _RefSample.position.X;
            _DynamicSamples[currentIndex].position.Y = _position.Y - _RefSample.position.Y;
            _DynamicSamples[currentIndex].millis = millis - _RefSample.millis;
            _RefSample.millis = millis;
            _RefSample.position = _position;

            currentIndex++;
            currentNumber = Math.Max(currentNumber, currentIndex);
            if (currentIndex == NumberOfDynamicSample)
                currentIndex = 0;
        }

        //-------------------------------------------------------------------
        public void OffsetRef(int dx, int dy)
        {
            _RefSample.position.X += dx;
            _RefSample.position.Y += dy;
        }

        //-------------------------------------------------------------------
        float brutSpeedX = 0;
        float brutSpeedY = 0;
        float finalSpeedX = 0;
        float finalSpeedY = 0;
        float currentSpeedX = 0;
        float currentSpeedY = 0;

        //-------------------------------------------------------------------
        void ResetSpeed()
        {
            brutSpeedX = 0;
            brutSpeedY = 0;
            finalSpeedX = 0;
            finalSpeedY = 0;
            currentSpeedX = 0;
            currentSpeedY = 0;
        }

        //-------------------------------------------------------------------
        bool HasSpeed()
        {
            return currentSpeedX != 0 || currentSpeedY != 0;
        }

        //-------------------------------------------------------------------
        void ComputeSpeed()
        {
            if (_DynamicMoveTimer != null) return;

            ResetSpeed();

            if (currentNumber != NumberOfDynamicSample) return;

            int index = currentIndex;
            int x = _DynamicSamples[index].position.X;
            int y = _DynamicSamples[index].position.Y;
            int previousX = x;
            int previousY = y;
            long totalMillis = CurrentMillis.Millis - _RefSample.millis;
            if (totalMillis > 0)
                return;

            totalMillis += _DynamicSamples[index].millis;
            index++;
            if (index == NumberOfDynamicSample) index = 0;

            while (index != currentIndex)
            {
                int nextX = _DynamicSamples[index].position.X;
                int nextY = _DynamicSamples[index].position.Y;
                if (nextX * previousX + nextY * previousY <= 0)
                {
                    currentNumber = 0;
                    currentIndex = 0;
                    return;
                }

                x += nextX;
                y += nextY;
                previousX = nextX;
                previousY = nextY;
                totalMillis += _DynamicSamples[index].millis;

                index++;
                if (index == NumberOfDynamicSample) index = 0;
            }

            float ratio = 1000.0f / (float)totalMillis;
            brutSpeedX = (float)x * ratio;
            brutSpeedY = (float)y * ratio;

            float norm = brutSpeedX * brutSpeedX + brutSpeedY * brutSpeedY;

            if (norm < SpeedMin * SpeedMin) return;
            if (norm > SpeedMax * SpeedMax)
            {
                finalSpeedX = (float)(brutSpeedX * SpeedMax / Math.Sqrt(norm));
                finalSpeedY = (float)(brutSpeedY * SpeedMax / Math.Sqrt(norm));
            }
            else
            {
                finalSpeedX = brutSpeedX;
                finalSpeedY = brutSpeedY;
            }

            currentSpeedX = finalSpeedX;
            currentSpeedY = finalSpeedY;
        }

        //-------------------------------------------------------------------
        public void Stop()
        {
            if (_DynamicMoveTimer != null)
            {
                _DynamicMoveTimer.Stop();
                _DynamicMoveTimer = null;
            }
            ResetAll();
        }

        //-------------------------------------------------------------------
        public void StartSamples(Point _position)
        {
            if (!TaxonUtils.MyConfig.Options.InertiaActive) return;

            _RefSample.position = _position;
            _RefSample.millis = CurrentMillis.Millis;
            currentNumber = 0;
            currentIndex = 0;
        }

        //-------------------------------------------------------------------                
        public void EndSamples()
        {
            ComputeSpeed();
            if (HasSpeed())
            {
                _DynamicMoveTimer = new System.Timers.Timer { Interval = Interval };
                _DynamicMoveTimer.Elapsed += new ElapsedEventHandler(TimerTick);
                _DynamicMoveTimer.Start();
            }
        }

        //-------------------------------------------------------------------                
        private void TimerTick(object source, ElapsedEventArgs e)
        {
            currentSpeedX *= 0.9f;
            currentSpeedY *= 0.9f;

            int dx = (int)(currentSpeedX * OoInterval);
            int dy = (int)(currentSpeedY * OoInterval);
            if (dx == 0 && dy == 0)
                Stop();

            if (_Owner != null)
                _Owner.Offset(dx, dy);
        }

        //---------------------------------------------------------------------------------------
        // debug display for inertia
        //
        public void DrawDebug(Graphics _graphics, Font _font, Rectangle _inside)
        {
            // test flags
            if (!TaxonUtils.MyConfig.Options.InertiaDebug) return;
            if (!TaxonUtils.MyConfig.Options.InertiaActive) return;

            // draw number of samples
            string text = string.Format("Inertia samples : {0}/{1}", currentNumber, NumberOfDynamicSample);
            _graphics.DrawString(text, _font, Brushes.Black, 5, 5);
            if (currentNumber != NumberOfDynamicSample)
                return;

            // draw displacement vector
            int centerX = (_inside.Left + _inside.Right) / 2;
            int centerY = (_inside.Top + _inside.Bottom) / 2;

            int index = currentIndex;
            index++;
            if (index == NumberOfDynamicSample) index = 0;

            Pen pen1 = new Pen(Color.White, 4);
            Pen pen2 = new Pen(Color.Black, 2);

            int dX = 0;
            int dY = 0;
            int dT = 0;
            int x = centerX;
            int y = centerY;
            while (index != currentIndex)
            {
                int nextX = x + _DynamicSamples[index].position.X;
                int nextY = y + _DynamicSamples[index].position.Y;

                _graphics.DrawLine(pen1, x, y, nextX, nextY);
                _graphics.DrawLine(pen2, x, y, nextX, nextY);

                x = nextX;
                y = nextY;

                dX += _DynamicSamples[index].position.X;
                dY += _DynamicSamples[index].position.Y;
                dT += (int)_DynamicSamples[index].millis;

                index++;
                if (index == NumberOfDynamicSample) index = 0;
            }

            // draw displacement vector information
            text = string.Format("Displacement ( {0},{1} ) in {2} ms", dX, dY, dT);
            _graphics.DrawString(text, _font, Brushes.Black, 5, 25);

            ComputeSpeed();

            text = string.Format("Brut    Speed ( {0},{1} ) pixels/ms", brutSpeedX, brutSpeedY);
            _graphics.DrawString(text, _font, Brushes.Black, 5, 45);
            x = (int) (brutSpeedX / 5);
            y = (int) (brutSpeedY / 5);
            _graphics.DrawLine(Pens.Purple, centerX, centerY, centerX + x, centerY + y);
            _graphics.FillEllipse(Brushes.Purple, centerX + x - 5, centerY + y - 5, 10, 10);

            text = string.Format("Final   Speed ( {0},{1} ) pixels/ms", finalSpeedX, finalSpeedY);
            _graphics.DrawString(text, _font, Brushes.Black, 5, 63);
            x = (int)(finalSpeedX / 5);
            y = (int)(finalSpeedY / 5);
            _graphics.DrawLine(Pens.Blue, centerX, centerY, centerX + x, centerY + y);
            _graphics.FillEllipse(Brushes.Blue, centerX + x - 5, centerY + y - 5, 10, 10);

            text = string.Format("Current Speed ( {0},{1} ) pixels/ms", currentSpeedX, currentSpeedY);
            _graphics.DrawString(text, _font, Brushes.Black, 5, 81);
            x = (int)(currentSpeedX / 5);
            y = (int)(currentSpeedY / 5);
            _graphics.FillEllipse(Brushes.LightBlue, centerX + x - 5, centerY + y - 5, 10, 10);

            x = (int)(SpeedMin / 5);
            _graphics.DrawEllipse(Pens.Green, centerX - x, centerY - x, x * 2, x * 2);
            x = (int)(SpeedMax / 5);
            _graphics.DrawEllipse(Pens.LightGreen, centerX - x, centerY - x, x * 2, x * 2);

        }
    }
}

using System;
using System.Collections.Generic;

namespace ProducerConsumer.Core
{
    public class Consumer
    {
        private readonly int _maxDuration;
        private readonly int _minDuration;
        private int _minutesToFinishConsumption;
        private readonly Random _random;
        private bool _needNewRandom;

        public event EventHandler<int> LookForNewTask;

        public Consumer(int min, int max, int minutesToFinish)
        {
            _maxDuration = max;
            _minDuration = min;
            _minutesToFinishConsumption = minutesToFinish;
            _random = new Random();
            _needNewRandom = false;
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
        }

        protected virtual void Instance_OneMinuteIsOver(object sender, DateTime time)
        {
            if (_needNewRandom)
            {
                _minutesToFinishConsumption = _random.Next(_minDuration, _maxDuration);
                _needNewRandom = false;
            }

            if (_minutesToFinishConsumption == 0)
            {
                LookForNewTask.Invoke(this, _minutesToFinishConsumption);
                _needNewRandom = true;
            }
            else
            {
                _minutesToFinishConsumption--;
            }
        }
    }
}

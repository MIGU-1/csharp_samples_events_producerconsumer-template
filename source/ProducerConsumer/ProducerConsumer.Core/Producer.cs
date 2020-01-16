using System;

namespace ProducerConsumer.Core
{
    public class Producer
    {
        private readonly int _maxDuration;
        private readonly int _minDuration;
        private int _minutesToNextProduction;
        private readonly Random _random;
        private int _taskNumber;
        private bool _needNewRandom;

        public event EventHandler<Task> NewTaskProduced;
        protected virtual void Instance_OneMinuteIsOver(object sender, DateTime Time)
        {
            if (_needNewRandom)
            {
                _minutesToNextProduction = _random.Next(_minDuration, _maxDuration);
                _needNewRandom = false;
            }

            if (_minutesToNextProduction == 0)
            {
                NewTaskProduced.Invoke(this, new Task(_taskNumber));
                _taskNumber++;
                _needNewRandom = true;
            }
            else
            {
                _minutesToNextProduction--;
            }
        }
        public Producer(int min, int max)
        {
            _maxDuration = max;
            _minDuration = min;
            _minutesToNextProduction = -1;
            _taskNumber = int.MinValue;
            _random = new Random();
            _needNewRandom = true;
            _taskNumber = 1;
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
        }
    }
}

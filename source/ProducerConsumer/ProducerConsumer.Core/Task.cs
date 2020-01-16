using System;

namespace ProducerConsumer.Core
{
    public class Task
    {
        public DateTime BeginConsumptionTime { get; set; }
        public DateTime CreationTime { get; set; }
        public int TaskNumber { get; set; }

        public Task(int taskNr)
        {
            TaskNumber = taskNr;
            CreationTime = DateTime.Now;
            BeginConsumptionTime = DateTime.MinValue;
        }
    }
}

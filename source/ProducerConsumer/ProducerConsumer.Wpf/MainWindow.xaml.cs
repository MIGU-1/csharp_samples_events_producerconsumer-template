using ProducerConsumer.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ProducerConsumer.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Producer _producer;
        private Consumer _consumer;
        private List<Task> _taskList;

        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Producer, Consumer und Queue erzeugen. Observer anmelden und 
        /// Simulation starten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            TextBlockLog.Text = "";
            _taskList = new List<Task>();
            int min = Convert.ToInt32(TextBoxProducerMinimum.Text);
            int max = Convert.ToInt32(TextBoxProducerMaximum.Text);
            _producer = new Producer(min, max);
            min = Convert.ToInt32(TextBoxConsumerMinimum.Text);
            max = Convert.ToInt32(TextBoxConsumerMaximum.Text);
            _consumer = new Consumer(min, max, 2);
            _producer.NewTaskProduced += Producer_NewTaskProduced;
            _consumer.LookForNewTask += Consumer_LookForNewTask;
            if (CheckBoxIsRunning.IsChecked == true)
            {
                CheckBoxIsRunning.IsChecked = false;
                _producer.NewTaskProduced -= Producer_NewTaskProduced;
                _consumer.LookForNewTask -= Consumer_LookForNewTask;
                System.GC.Collect();
            }
            else
            {
                CheckBoxIsRunning.IsChecked = true;
            }

            FastClock.Instance.IsRunning = CheckBoxIsRunning.IsChecked == true;
        }
        private void Consumer_LookForNewTask(object sender, int minutesToFinish)
        {
            if (_taskList.Count == 0)
            {
                AddLineToTextBox($"FAIL - KEIN TASK IN DER LISTE!!!");
            }
            else
            {
                AddLineToTextBox($"Consum von Task {_taskList[_taskList.Count - 1].TaskNumber} wird gestartet!");
                _taskList.RemoveAt(_taskList.Count - 1);
            }
        }
        private void Producer_NewTaskProduced(object sender, Task newTask)
        {
            _taskList.Add(newTask);
            AddLineToTextBox($"{newTask.CreationTime} - Neuer Task hinzugefügt! | Anzahl Tasks = {_taskList.Count} | Task Nummer = {newTask.TaskNumber}");
        }

        /// <summary>
        /// Fügt eine Zeile zur Textbox hinzu.
        /// Da Timer in eigenem Thread läuft ist ein Threadwechsel mittels Invoke
        /// notwendig
        /// </summary>
        /// <param name="line"></param>
        void AddLineToTextBox(string line)
        {
            StringBuilder text = new StringBuilder(TextBlockLog.Text);
            text.Append(FastClock.Instance.Time.ToShortTimeString() + "\t");
            text.Append(line + "\n");
            TextBlockLog.Text = text.ToString();
        }
        private void CheckBoxIsRunning_Click(object sender, RoutedEventArgs e)
        {
            FastClock.Instance.Factor = 1000;
            FastClock.Instance.IsRunning = CheckBoxIsRunning.IsChecked == true;
        }
    }
}

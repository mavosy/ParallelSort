using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Utilities;

namespace MeasurementApp.ViewModels
{
    abstract class Measure<T> : ViewModelBase where T : ICompute
    {
        public class RunOrNot
        {
            public bool Run {
                get {
                    return run;
                }
                set {
                    run = value;
                    parent.OnPropertyChanged(nameof(AvailableAlgorithms));
                }
            }

            public T Algorithm {
                get; private set;
            }

            internal RunOrNot(Measure<T> parent, T algorithm)
            {
                this.parent = parent;
                Run = true;
                Algorithm = algorithm;
            }

            private Measure<T> parent;
            private bool run;
        }

        abstract protected void ToRun();

        public ObservableCollection<RunOrNot> AvailableAlgorithms {
            get {
                return availableAlgorithms;
            }
        }
        public RelayCommand<object> RunCommand {
            get; protected set;
        }

        public bool RunIsEnabled {
            get {
                return runIsEnabled;
            }
            protected set {
                runIsEnabled = value;
                OnPropertyChanged();
            }
        }

        public string ResultLog {
            get {
                return resultLog;
            }
            set {
                resultLog = value;
                OnPropertyChanged();
            }
        }

        public Measure()
        {
            availableAlgorithms = new ObservableCollection<RunOrNot>();
            RunCommand = new RelayCommand<object>(x => ToRun(), null);
            RunIsEnabled = true;
        }

        public void AddAlgorithms(T[] algorithms)
        {
            foreach (T alg in algorithms) {
                availableAlgorithms.Add(new RunOrNot(this, alg));
            }
        }

        private ObservableCollection<RunOrNot> availableAlgorithms;
        private bool runIsEnabled;
        private string resultLog;
    }
}

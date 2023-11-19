using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Sorting;
using Utilities;

namespace MeasurementApp.ViewModels
{
    class MeasureSorting : Measure<ISort<MyKeyValue<int, string>>>
    {
        #region properties
        public int NumberOfItems {
            get {
                return numberOfItems;
            }
            set {
                numberOfItems = value;
                OnPropertyChanged();
            }
        }

        public string[] AvailableArrayPreparation {
            get {
                return availableArrayPreparation;
            }
        }

        public int ArrayPreparationIndex {
            get {
                return arrayPreparationIndex;
            }
            set {
                arrayPreparationIndex = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MeasureSorting()
        {
            AddAlgorithms(availableSortingAlgorithms);
            NumberOfItems = 10_000;
            ArrayPreparationIndex = 2;
        }

        protected override void ToRun()
        {
            RunIsEnabled = false;
            Task[] startSorting = new Task[AvailableAlgorithms.Count];
            Task[] endSorting = new Task[AvailableAlgorithms.Count];
            for (int i = 0; i < AvailableAlgorithms.Count; i++) {
                if (!AvailableAlgorithms[i].Run) {
                    continue;
                }
                ISort<MyKeyValue<int, string>> s = AvailableAlgorithms[i].Algorithm;
                startSorting[i] = new Task(() => {});
                endSorting[i] = startSorting[i].ContinueWith(t => {
                        // Render the initialization to the UI.
                        ResultLog += "Algorithm " + s.Name + ": Started with " + NumberOfItems +
                            " prepared as '" + AvailableArrayPreparation[ArrayPreparationIndex] +
                            "'." + Environment.NewLine;
                    }, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(t => {
                        // Run in the background a long computation which generates a result.
                        return Run(s, NumberOfItems, ArrayPreparationIndex);
                    }).ContinueWith(t => {
                        // Render the result on the UI.
                        ResultLog += "Algorithm " + s.Name +
                            (t.Result.Item3 ? " sorted ok" : " failed") + ": " +
                            t.Result.Item1.ToString() + " wall clock sec. " +
                            t.Result.Item2.ToString() + " processor user time sec." +
                            Environment.NewLine;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            Task start = new Task(() => {
                    for (int i = 0; i < startSorting.Length; i++) {
                        if (AvailableAlgorithms[i].Run) {
                            startSorting[i].RunSynchronously();
                            endSorting[i].Wait();
                        }
                    }
                });
            start.ContinueWith(t => {
                // Enable the start button.
                RunIsEnabled = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());

            start.Start();
        }

        private static Tuple<double, double, bool> Run(ISort<MyKeyValue<int, string>> sort, int numberOfItems, int arrayPreparationIndex)
        {
            MyKeyValue<int, string>[] numbers = null;
            Stopwatch wallClock = new Stopwatch();
            ProcessUserTime processUserTime = new ProcessUserTime();

            switch (arrayPreparationIndex) {
                case 0:
                    numbers = ArrayExtensions.CreateOrderedMKVArray(numberOfItems);
                    break;
                case 1:
                    numbers = ArrayExtensions.CreateReverseOrderedMKVArray(numberOfItems);
                    break;
                case 2:
                    numbers = ArrayExtensions.CreateRandomMKVArray(numberOfItems, new Random());
                    break;
            }

            // Measure the sorting.
            wallClock.Restart();
            processUserTime.Restart();
            sort.Sort(numbers);
            processUserTime.Stop();
            wallClock.Stop();
            return
                new Tuple<double, double, bool>(wallClock.Elapsed.TotalSeconds,
                                                processUserTime.ElapsedTotalSeconds,
                                                ArrayExtensions.VerifySortedOneToLength(numbers, numberOfItems));
        }

        private readonly ISort<MyKeyValue<int, string>>[] availableSortingAlgorithms = {
                new StandardSort<MyKeyValue<int, string>>(),
                new SelectionSort<MyKeyValue<int, string>>(),
                new ParallelMergeSort<MyKeyValue<int, string>>()
            };
        private readonly string[] availableArrayPreparation = {
                "Ordered",
                "Reverse ordered",
                "Random"
            };
        private int numberOfItems;
        private int arrayPreparationIndex;
    }
}

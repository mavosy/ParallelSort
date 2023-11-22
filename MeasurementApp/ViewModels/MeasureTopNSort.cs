using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Sorting;
using Utilities;

namespace MeasurementApp.ViewModels
{
    class MeasureTopNSort : Measure<ITopNSort<MyKeyValue<int, string>>>
    {
        public int TotalNumberOfItems {
            get {
                return totalNumberOfItems;
            }
            set {
                totalNumberOfItems = value;
                OnPropertyChanged();
            }
        }

        public int SelectedNumberOfItems {
            get {
                return selectedNumberOfItems;
            }
            set {
                selectedNumberOfItems = value;
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

        public MeasureTopNSort()
        {
            AddAlgorithms(availableTopNSortAlgorithms);
            TotalNumberOfItems = 100_000;
            SelectedNumberOfItems = 100;
            ArrayPreparationIndex = 2;
        }

        protected override void ToRun()
        {
            RunIsEnabled = false;

            Task[] startSelecting = new Task[AvailableAlgorithms.Count];
            Task[] endSelecting = new Task[AvailableAlgorithms.Count];
            for (int i = 0; i < AvailableAlgorithms.Count; i++) {
                if (!AvailableAlgorithms[i].Run) {
                    continue;
                }
                ITopNSort<MyKeyValue<int, string>> s = AvailableAlgorithms[i].Algorithm;
                startSelecting[i] = new Task(() => {});
                endSelecting[i] = startSelecting[i].ContinueWith(t => {
                        // Render the initialization to the UI.
                        ResultLog += "Algorithm " + s.Name + ": Started with selecting " + SelectedNumberOfItems +
                            " from total " + TotalNumberOfItems +
                            " prepared as '" + AvailableArrayPreparation[ArrayPreparationIndex] +
                            "'." + Environment.NewLine;
                    }, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(t => {
                        // Run in the background a long computation which generates a result.
                        return Run(s, TotalNumberOfItems, SelectedNumberOfItems, ArrayPreparationIndex);
                    }).ContinueWith(t => {
                        // Render the result on the UI.
                        ResultLog += "Algorithm " + s.Name + " behaved " +
                            (t.Result.Item3 ? "ok" : "failed") + ": " +
                            t.Result.Item1.ToString() + " wall clock sec. " +
                            t.Result.Item2.ToString() + " processor user time sec." +
                            Environment.NewLine;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            Task start = new Task(() => {
                    for (int i = 0; i < startSelecting.Length; i++) {
                        if (AvailableAlgorithms[i].Run) {
                            startSelecting[i].RunSynchronously();
                            endSelecting[i].Wait();
                        }
                    }
                });
            start.ContinueWith(t => {
                // Enable the start button.
                RunIsEnabled = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());

            start.Start();
        }

        private static Tuple<double, double, bool> Run(ITopNSort<MyKeyValue<int, string>> topNSort, int totalNumberOfItems, int selectedNumberOfItems, int arrayPreparationIndex)
        {
            MyKeyValue<int, string>[] numbers = null;
            Stopwatch wallClock = new Stopwatch();
            ProcessUserTime processUserTime = new ProcessUserTime();

            switch (arrayPreparationIndex) {
                case 0:
                    numbers = ArrayExtensions.CreateOrderedMKVArray(totalNumberOfItems);
                    break;
                case 1:
                    numbers = ArrayExtensions.CreateReverseOrderedMKVArray(totalNumberOfItems);
                    break;
                case 2:
                    numbers = ArrayExtensions.CreateRandomMKVArray(totalNumberOfItems, new Random());
                    break;
            }

            // Measure the sorting.
            wallClock.Restart();
            processUserTime.Restart();
            MyKeyValue<int, string>[] result = topNSort.TopNSort(numbers, selectedNumberOfItems);
            processUserTime.Stop();
            wallClock.Stop();
            return
                new Tuple<double, double, bool>(wallClock.Elapsed.TotalSeconds,
                                                processUserTime.ElapsedTotalSeconds,
                                                ArrayExtensions.VerifySortedOneToLength(result, selectedNumberOfItems));
        }

        private readonly ITopNSort<MyKeyValue<int, string>>[] availableTopNSortAlgorithms = {
                new TopNStandardSort<MyKeyValue<int, string>>(),
                new TopNSelectionSort<MyKeyValue<int, string>>(),
                new NewTopNStandardSort<MyKeyValue<int, string>>()
            };
        private readonly string[] availableArrayPreparation = {
                "Ordered",
                "Reverse ordered",
                "Random"
            };
        private int totalNumberOfItems;
        private int selectedNumberOfItems;
        private int arrayPreparationIndex;
    }
}

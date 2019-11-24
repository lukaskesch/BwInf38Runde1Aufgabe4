using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace BwInfAufgabe4
{
    public partial class MainWindow : Window
    {
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // CONSTRUCTOR
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public MainWindow()
        {
            InitializeComponent();
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // GLOBAL VARIABLES
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private int Consumption;
        private int MaxFuel;
        private int MaxDistance;
        private int InitialFuel;
        List<int> GasStation_Distances;
        int[] GasStation_Prices;

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // METHODS
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private void Calculate()
        {
            Stopwatch SW = new Stopwatch();
            SW.Start();

            // Get shortest route
            ShortestRoute SR = new ShortestRoute(GasStation_Distances, MaxDistance, MaxFuel, InitialFuel, Consumption);
            int MaxStops = SR.GetStopCount();

            // Get cheapest route with maximum amount of stops
            CheapestRoute CR = new CheapestRoute(GasStation_Distances, GasStation_Prices, MaxDistance, MaxFuel, InitialFuel, Consumption, MaxStops);
            int[] Route = CR.GetRoute();
            double Costs = CR.GetCosts();

            SW.Stop();

            // Output result
            string Out = String.Empty;
            if (Route == null)
            {
                MessageBox.Show("Keine Route");
                return;
            }

            double[] Amounts = CR.GetAmountsPerStop();
            double[] CostPerStop = CR.GetCostPerStop();

            ListViewStops.Items.Clear();
            for(int I = 0; I < Route.Length; I++)
            {
                int Stop = Route[I];
                ListViewStops.Items.Add(GasStation_Distances[Stop] + "km - " + Amounts[I].ToString("#.00") + "l -> " + CostPerStop[I].ToString("#.00") + "€");
            }

            LabelStops.Content = MaxStops.ToString();
            LabelCost.Content = Costs.ToString("#.00") + "€";
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private void OpenFile()
        {
            // Open file dialog
            Microsoft.Win32.OpenFileDialog Dlg = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "\"Urlaubsfahrt\"-Datei (*.txt)|*.txt|Alle Dateien (*.*)|*.*",
                FilterIndex = 0
            };

            // Return if user cancels selection
            if (Dlg.ShowDialog() != true)
            {
                return;
            }

            // Read file contents
            FileStream File = new FileStream(Dlg.FileName, FileMode.Open);
            string FileString = String.Empty;
            StreamReader Reader = new StreamReader(File);

            try
            {
                FileString = Reader.ReadToEnd();
            }
            catch
            {
                MessageBox.Show("Die Datei konnte nicht eingelesen werden.");
                return;
            }
            finally
            {
                File.Close();
                Reader.Close();
            }

            // Split file into rows
            string[] Rows = FileString.Split('\n');

            try
            {
                Consumption = int.Parse(Rows[0]);
                MaxFuel = int.Parse(Rows[1]);
                InitialFuel = int.Parse(Rows[2]);
                MaxDistance = int.Parse(Rows[3]);

                int GasStationCount = int.Parse(Rows[4]);
                KeyValuePair<int, int>[] GasStations = new KeyValuePair<int, int>[GasStationCount];

                for (int I = 0; I < GasStationCount; I++)
                {
                    string[] ValueStrings = Rows[5 + I].Split(' ');
                    int[] Values = new int[2];
                    int ValCount = 0;
                    foreach (string S in ValueStrings)
                    {
                        if (S.Trim() != String.Empty)
                        {
                            Values[ValCount] = int.Parse(S);
                            ValCount++;
                            if (ValCount > 1) { break; }
                        }
                    }

                    GasStations[I] = new KeyValuePair<int, int>(Values[0], Values[1]);
                }

                // Sort GasStation Array
                Array.Sort(GasStations, (x, y) => x.Key.CompareTo(y.Key));

                // Put in seperate List and Array (Performance reasons)
                GasStation_Distances = new List<int>();
                GasStation_Prices = new int[GasStationCount];

                for(int I = 0; I < GasStations.Length; I++)
                {
                    GasStation_Distances.Add(GasStations[I].Key);
                    GasStation_Prices[I] = GasStations[I].Value;
                }

                ButtonCalc.IsEnabled = true;
            }
            catch
            {
                MessageBox.Show("Die Datei konnte nicht eingelesen werden.");
                ButtonCalc.IsEnabled = false;
            }
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // HANDLERS
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private void ButtonCalc_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    }
}

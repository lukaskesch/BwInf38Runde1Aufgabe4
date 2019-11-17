using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

// TODO: Remove GasStations Array - use GasStation_Distances and GasStation_Prices instead

namespace BwInfAufgabe4
{
    public class CheapestRoute
    {
        List<int> GasStation_Distances;
        int[] GasStation_Prices;
        int[,] GasStations;
        double MaxDistance;
        double MaxFuel;
        double Fuel;
        double Consumption;
        int MaxStops;
        double MaxRange;
        int[] Path;
        int[] BestPath;
        double MaxCost;

        List<string> Log;
        string[] CurrentLog;
        string[] BestLog;

        public CheapestRoute(int[,] _GasStations, int _MaxDistance, int _MaxFuel, int _Fuel, int _Consumption, int _MaxStops, double _MaxCosts)
        {
            Log = new List<string>();

            GasStation_Distances = new List<int>();
            GasStation_Prices = new int[_GasStations.GetLength(0)];
            for (int I = 0; I < _GasStations.GetLength(0); I++)
            {
                GasStation_Distances.Add(_GasStations[I, 0]);
                GasStation_Prices[I] = _GasStations[I, 1];
            }

            GasStations = _GasStations;
            MaxDistance = _MaxDistance;
            MaxFuel = _MaxFuel;
            Fuel = _Fuel;
            Consumption = _Consumption;
            MaxStops = _MaxStops;
            MaxCost = _MaxCosts;

            MaxRange = MaxFuel / Consumption * 100;
            Path = new int[MaxStops];

            Log.Add("MaxDistance: " + MaxDistance + " km");
            Log.Add("MaxFuel: " + MaxFuel + " l");
            Log.Add("Fuel: " + Fuel + " l");
            Log.Add("Consumption: " + Consumption + " l/100km");
            Log.Add("++++++++++++++++++++++++++++++++++++++++++++++++++");
            Log.Add("[Part 1]");
            Log.Add("MaxStops: " + MaxStops);
            Log.Add("MaxCost: " + MaxCost / 100 + " €");
            Log.Add("++++++++++++++++++++++++++++++++++++++++++++++++++");
            Log.Add("[Part 2]");

            CurrentLog = new string[MaxStops + 1];
            BestLog = new string[MaxStops];

            Stopwatch Timer = new Stopwatch();
            Timer.Start();

            Jump1(0, -1, 0, Fuel, 0);

            Timer.Stop();
            MessageBox.Show("Zeit: " + Timer.Elapsed);

            StreamWriter SR = new StreamWriter(Environment.CurrentDirectory + "\\log.txt"); ;
            try
            {
                foreach (string L in Log)
                {
                    SR.WriteLine(L);
                }

                if (BestLog != null)
                {
                    foreach (string L in BestLog)
                    {
                        SR.WriteLine(L);
                    }
                }
            }
            finally
            {
                SR.Flush();
                SR.Close();
            }
        }

        public int[] GetRoute()
        {
            return BestPath;
        }

        public double GetCosts()
        {
            return MaxCost;
        }

        private int GetGasStationIndex(int Position)
        {
            int Index = GasStation_Distances.BinarySearch(Position);

            if (Index < 0)
            {
                Index = ~Index;
                Index -= 1;
            }

            return Index;
        }

        private void Jump1(int _Step, int _Station, double _Distance, double _Fuel, double _Cost)
        {
            double DistanceReachable;

            if (_Step == 0)
            {
                DistanceReachable = _Fuel / Consumption * 100;
            }
            else
            {
                DistanceReachable = _Distance + MaxRange;
                Path[_Step - 1] = _Station;
            }

            if (_Distance + (MaxStops + 1 - _Step) * MaxRange < MaxDistance)
            {
                return;
            }

            if (DistanceReachable >= MaxDistance)
            {
                double NewFuel;
                double NewCost;
                double FuelNeeded = ((MaxDistance - _Distance) / 100 * Consumption);
                // As much as needed
                if (FuelNeeded >= _Fuel)
                {
                    NewCost = _Cost + (FuelNeeded - _Fuel) * GasStations[_Station, 1];
                    NewFuel = 0;

                    /*
                    CurrentLog[_Step] =
                        "Stop: " + _Step + "\t| " +
                        "Station: " + _Station + "\t| " +
                        "Distance: " + _Distance + "\t| " +
                        "Fuel: " + _Fuel + "\t| " +
                        "NewFuel: " + NewFuel + "\t| " +
                        "Amount: " + (FuelNeeded - _Fuel) + "\t| " +
                        "Price: " + GasStations[_Station, 1] + "\t| " +
                        "Costs: " + (NewCost - _Cost) + "\t| " +
                        "AllCosts: " + NewCost;
                    */
                }
                else
                {
                    NewCost = 0;
                    NewFuel = _Fuel - FuelNeeded;

                    /*
                    CurrentLog[_Step] =
                        "Stop: " + _Step + "\t| " +
                        "Station: " + _Station + "\t| " +
                        "Distance: " + _Distance + "\t| " +
                        "Fuel: " + _Fuel + "\t| " +
                        "NewFuel: " + NewFuel + "\t| " +
                        "Amount: " + 0 + "\t| " +
                        "Price: " + GasStations[_Station, 1] + "\t| " +
                        "Costs: " + (NewCost - _Cost) + "\t| " +
                        "AllCosts: " + NewCost;
                    */
                }

                if (NewCost >= MaxCost) { return; }

                // Solution
                int[] FinalPath = new int[MaxStops];
                Array.Copy(Path, FinalPath, MaxStops);

                BestPath = FinalPath;
                MaxCost = NewCost;

                BestLog = new string[CurrentLog.Length];
                Array.Copy(CurrentLog, BestLog, CurrentLog.Length);
            }

            // If step limit is reached without a solution abort
            if (_Step >= MaxStops)
            {
                return;
            }

            int FurthestReachableStation = GetGasStationIndex((int)Math.Floor(DistanceReachable));
            for (int I = FurthestReachableStation; I > _Station; I--)
            {
                double NewFuel;
                double NewCost;
                double FuelNeeded = ((GasStations[I, 0] - _Distance) / 100 * Consumption);

                // As much as needed
                if (FuelNeeded >= _Fuel)
                {
                    NewCost = _Cost + (FuelNeeded - _Fuel) * GasStations[_Station, 1];
                    NewFuel = 0;

                    /*
                    CurrentLog[_Step] =
                        "Stop: " + _Step + "\t| " +
                        "Station: " + _Station + "\t| " +
                        "Distance: " + _Distance + "\t| " +
                        "Fuel: " + _Fuel + "\t| " +
                        "NewFuel: " + NewFuel + "\t| " +
                        "Amount: " + (FuelNeeded - _Fuel) + "\t\t| " +
                        "Price: " + GasStations[_Station, 1] + "\t\t| " +
                        "Costs: " + (NewCost - _Cost) + "\t\t| " +
                        "AllCosts: " + NewCost;
                    */

                    Jump1(_Step + 1, I, GasStations[I, 0], NewFuel, NewCost);
                }
                else
                {
                    NewCost = 0;
                    NewFuel = _Fuel - FuelNeeded;
                    
                    /*
                    if (_Step != 0)
                    {
                        CurrentLog[_Step] =
                        "Stop: " + _Step + "\t| " +
                        "Station: " + _Station + "\t| " +
                        "Distance: " + _Distance + "\t| " +
                        "Fuel: " + _Fuel + "\t| " +
                        "NewFuel: " + NewFuel + "\t| " +
                        "Amount: " + 0 + "\t\t| " +
                        "Price: " + GasStations[_Station, 1] + "\t\t| " +
                        "Costs: " + (NewCost - _Cost) + "\t\t| " +
                        "AllCosts: " + NewCost;
                        
                    }
                    else
                    {
                        CurrentLog[_Step] =
                        "Stop: " + _Step + "\t| " +
                        "Station: " + _Station + "\t| " +
                        "Distance: " + _Distance + "\t| " +
                        "Fuel: " + _Fuel + "\t| " +
                        "NewFuel: " + NewFuel + "\t| " +
                        "Amount: " + 0 + "\t\t| " +
                        "Price: " + "-" + "\t\t| " +
                        "Costs: " + (NewCost - _Cost) + "\t\t| " +
                        "AllCosts: " + NewCost;
                    }
                    */

                    Jump1(_Step + 1, I, GasStations[I, 0], NewFuel, NewCost);
                }

                // As much as possible
                if (_Step == 0) { continue; }
                NewFuel = MaxFuel - FuelNeeded;
                NewCost = _Cost + (MaxFuel - _Fuel) * GasStations[_Station, 1];

                /*
                CurrentLog[_Step] =
                    "Stop: " + _Step + "\t| " +
                    "Station: " + _Station + "\t| " +
                    "Distance: " + _Distance + "\t| " +
                    "Fuel: " + _Fuel + "\t| " +
                    "NewFuel: " + NewFuel + "\t| " +
                    "Amount: " + (MaxFuel - _Fuel) + "\t\t| " +
                    "Price: " + GasStations[_Station, 1] + "\t\t| " +
                    "Costs: " + (NewCost - _Cost) + "\t\t| " +
                    "AllCosts: " + NewCost;
                */

                Jump1(_Step + 1, I, GasStations[I, 0], NewFuel, NewCost);
            }
        }

    }
}
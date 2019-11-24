using System;
using System.Collections.Generic;

namespace BwInfAufgabe4
{
    public class CheapestRoute
    {
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // CONSTRUCTOR
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public CheapestRoute(List<int> _GasStation_Distances, int[] _GasStation_Prices, int _MaxDistance, int _MaxFuel, int _InitialFuel, int _Consumption, int _MaxStops)
        {
            GasStation_Distances = _GasStation_Distances;
            GasStation_Prices = _GasStation_Prices;
            MaxDistance = _MaxDistance;
            MaxFuel = _MaxFuel;
            Fuel = _InitialFuel;
            Consumption = _Consumption;
            MaxStops = _MaxStops;
            MaxCost = double.PositiveInfinity;

            MaxRange = MaxFuel / Consumption * 100;
            Path = new int[MaxStops];
            GasAmounts = new double[MaxStops];

            Drive(0, -1, 0, Fuel, 0);
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // GLOBAL VARIABLES
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private List<int> GasStation_Distances;
        private int[] GasStation_Prices;
        private double MaxDistance;
        private double MaxFuel;
        private double Fuel;
        private double Consumption;
        private int MaxStops;
        private double MaxRange;
        private int[] Path;
        private int[] BestPath;
        private double MaxCost;

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // METHODS
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public int[] GetRoute()
        {
            return BestPath;
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public double GetCosts()
        {
            // Round up to two decimal places
            double Multiplier = Math.Pow(10, 2);
            return Math.Ceiling(MaxCost / 100 * Multiplier) / Multiplier;
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public double[] GetAmountsPerStop()
        {
            return BestGasAmounts;
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public double[] GetCostPerStop()
        {
            double[] EuroAmounts = new double[BestGasAmounts.Length];
            double Multiplier = Math.Pow(10, 2);


            for (int I = 0; I < BestPath.Length; I++)
            {
                int Stop = BestPath[I];
                EuroAmounts[I] = Math.Ceiling(BestGasAmounts[I] * GasStation_Prices[Stop] / 100 * Multiplier) / Multiplier;
            }

            return EuroAmounts;
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private int GetGasStationIndex(int Position)
        {
            // Seach index of Position
            int Index = GasStation_Distances.BinarySearch(Position);

            // Get index in front of position
            if (Index < 0)
            {
                Index = ~Index;
                Index -= 1;
            }

            return Index;
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private double[] GasAmounts;
        private double[] BestGasAmounts;

        private void Drive(int _Step, int _Station, double _Distance, double _Fuel, double _Cost)
        {
            double DistanceReachable;

            // Calcualte reachable distance
            if (_Step == 0)
            {
                DistanceReachable = _Fuel / Consumption * 100;
            }
            else
            {
                DistanceReachable = _Distance + MaxRange;
                Path[_Step - 1] = _Station;
            }

            // Check if goal is even reachable with full refuels
            if (_Distance + (MaxStops + 1 - _Step) * MaxRange < MaxDistance)
            {
                return;
            }

            // If distance is reached save the route
            if (DistanceReachable >= MaxDistance)
            {
                // Last jump
                double NewCost;
                double FuelNeeded = ((MaxDistance - _Distance) / 100 * Consumption);

                // As much as needed
                if (FuelNeeded >= _Fuel)
                {
                    NewCost = _Cost + (FuelNeeded - _Fuel) * (double)GasStation_Prices[_Station];
                    GasAmounts[_Step - 1] = (FuelNeeded - _Fuel);
                }
                else
                {
                    NewCost = 0;
                    GasAmounts[_Step - 1] = 0;
                }

                // Ignore more expensive routes
                if (NewCost >= MaxCost) { return; }

                // Update values
                int[] FinalPath = new int[MaxStops];
                Array.Copy(Path, FinalPath, MaxStops);

                BestGasAmounts = new double[MaxStops];
                Array.Copy(GasAmounts, BestGasAmounts, MaxStops);

                BestPath = FinalPath;
                MaxCost = NewCost;
                return;
            }

            // If step limit is reached without a solution abort
            if (_Step >= MaxStops)
            {
                return;
            }

            // Jump to next station (begin with furthest)
            int FurthestReachableStation = GetGasStationIndex((int)Math.Floor(DistanceReachable));
            for (int I = FurthestReachableStation; I > _Station; I--)
            {
                double NewFuel;
                double NewCost;
                double FuelNeeded = (((double)GasStation_Distances[I] - _Distance) / 100 * Consumption);

                // As much fuel as needed
                if (FuelNeeded >= _Fuel)
                {
                    NewCost = _Cost + (FuelNeeded - _Fuel) * (double)GasStation_Prices[_Station];
                    NewFuel = 0;

                    GasAmounts[_Step - 1] = (FuelNeeded - _Fuel);
                    Drive(_Step + 1, I, (double)GasStation_Distances[I], NewFuel, NewCost);
                }
                else
                {
                    NewCost = 0;
                    NewFuel = _Fuel - FuelNeeded;

                    // Don't set GasAmounts - Prevent Step 0 Exception & Always 0
                    Drive(_Step + 1, I, (double)GasStation_Distances[I], NewFuel, NewCost);
                }

                // As much fuel as possible
                if (_Step == 0) { continue; }
                NewFuel = MaxFuel - FuelNeeded;
                NewCost = _Cost + (MaxFuel - _Fuel) * (double)GasStation_Prices[_Station];

                GasAmounts[_Step - 1] = (MaxFuel - _Fuel);
                Drive(_Step + 1, I, (double)GasStation_Distances[I], NewFuel, NewCost);
            }
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    }
}
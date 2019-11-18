using System;
using System.Collections.Generic;

namespace BwInfAufgabe4
{
    public class ShortestRoute
    {
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // CONSTRUCTOR
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public ShortestRoute(List<int> _GasStation_Distances, int _MaxDistance, int _MaxFuel, int _InitialFuel, int _Consumption)
        {
            GasStation_Distances = _GasStation_Distances;
            MaxDistance = _MaxDistance;
            MaxFuel = _MaxFuel;
            Fuel = _InitialFuel;
            Consumption = _Consumption;

            MaxRange = (MaxFuel * 100) / Consumption;

            Calculate();
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // GLOBAL VARIABLES
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private List<int> GasStation_Distances;
        private double MaxDistance;
        private double MaxFuel;
        private double Fuel;
        private double Consumption;
        private double MaxRange;
        private int Stops;

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // METHODS
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public int GetStopCount()
        {
            return Stops;
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

        private void Calculate()
        {
            // Set initial values
            Stops = 0;
            double CurrentDistance = 0;
            double Range = (Fuel * 100) / Consumption;

            while (true)
            {
                // Calculate reacable distance
                double DistanceReachable = CurrentDistance + Range;

                // Abort if over distance limit
                if(DistanceReachable > MaxDistance)
                {
                    break;
                }

                // Get last station reachable
                int StationReachableIndex = GetGasStationIndex((int)Math.Floor(DistanceReachable));

                // Update values
                CurrentDistance = GasStation_Distances[StationReachableIndex];
                Range = MaxRange;
                Stops++;
            }
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    }
}

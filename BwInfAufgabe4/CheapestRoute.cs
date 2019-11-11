using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BwInfAufgabe4
{
    public class CheapestRoute
    {
        int[,] GasStations;
        int MaxDistance;
        int MaxFuel;
        double Fuel;
        int Consumption;
        int MaxStops;
        double MaxRange;
        int[] Path;
        int[] BestPath;
        double MaxCost;

        public CheapestRoute(int[,] _GasStations, int _MaxDistance, int _MaxFuel, int _Fuel, int _Consumption, int _MaxStops, double _MaxCosts)
        {
            GasStations = _GasStations;
            MaxDistance = _MaxDistance;
            MaxFuel = _MaxFuel;
            Fuel = _Fuel;
            Consumption = _Consumption;
            MaxStops = _MaxStops;
            MaxCost = _MaxCosts;

            MaxRange = (double)MaxFuel / (double)Consumption * 100;
            Path = new int[MaxStops];

            Jump(0, -1, Fuel, 0);
        }

        public int[] GetRoute()
        {
            return BestPath;
        }

        public double GetCosts()
        {
            return MaxCost;
        }

        private void Jump(int _Step, int _StationIndex, double _Fuel, double _Costs)
        {
            int CurrentDistance;
            double Range;

            if (_Step == 0)
            {
                CurrentDistance = 0;
                Range = (double)_Fuel / (double)Consumption * 100;
            }
            else
            {
                Path[_Step - 1] = _StationIndex;
                CurrentDistance = GasStations[_StationIndex, 0];
                Range = MaxRange;
                _Costs += ((double)MaxFuel - _Fuel) * GasStations[_StationIndex, 1];
                _Fuel = MaxFuel;
            }

            double DistanceReachable = CurrentDistance + Range;

            // If cost limit is reached without a solution abort
            if (_Costs > MaxCost)
            {
                return;
            }

            if (CurrentDistance + (MaxStops + 1 - _Step) * MaxRange < MaxDistance)
            {
                return;
            }

            if (DistanceReachable >= MaxDistance)
            {
                // Solution
                int[] FinalPath = new int[MaxStops];
                Array.Copy(Path, FinalPath, MaxStops);

                BestPath = FinalPath;
                MaxCost = _Costs;
                return;
            }

            // If step limit is reached without a solution abort
            if (_Step >= MaxStops)
            {
                return;
            }

            for(int I = GasStations.GetLength(0) - 1; I > _StationIndex; I--)
            {
                if(GasStations[I, 0] <= DistanceReachable)
                {
                    double FuelLeft = _Fuel - ((GasStations[I, 0] - CurrentDistance) / 100 * Consumption);
                    Jump(_Step + 1, I, FuelLeft, _Costs);
                }
            }
        }
    }
}

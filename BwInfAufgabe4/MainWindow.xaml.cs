using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BwInfAufgabe4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int Verbrauch = 0;
        int Tankgroeße = 0;
        int Tankfuellung = 0;
        int Streckenlaenge = 0;
        int GesammtStreckeGefahren = 0;
        int[,] Tankstellen;
        private void ButtonCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string EingabeString = TextBoxEingabe.Text;
                string[] EingabeArray = Regex.Split(EingabeString, "\r\n");

                Verbrauch = int.Parse(EingabeArray[0]);
                Tankgroeße = int.Parse(EingabeArray[1]);
                Tankfuellung = int.Parse(EingabeArray[2]);
                Streckenlaenge = int.Parse(EingabeArray[3]);
                Tankstellen = SetTankstellenArray(EingabeArray);

                //TextBoxAusgabe.Text += Verbrauch + "\n" + Tankgroeße + "\n" + Tankfuellung + "\n" + Streckenlaenge + "\n" + EingabeArray[4];
                //Ausgabe(Tankstellen);
                MessageBox.Show("Es muss insgesammt " + CalculateNumberOfStops().ToString() + " mal getankt werden");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int[,] SetTankstellenArray(string[] EingabeArray)
        {
            int Laenge = int.Parse(EingabeArray[4]);
            int[,] Tankstellen = new int[Laenge, 2];
            string[] EingabeArrayZeile;

            for (int i = 5; i < EingabeArray.Length; i++)
            {
                int index = EingabeArray[i].IndexOf(' ');
                EingabeArray[i] = EingabeArray[i].Insert(index, ",");

                EingabeArrayZeile = EingabeArray[i].Split(',');
                EingabeArrayZeile[0] = EingabeArrayZeile[0].Trim(' ');
                EingabeArrayZeile[1] = EingabeArrayZeile[1].Trim(' ');

                Tankstellen[i - 5, 0] = int.Parse(EingabeArrayZeile[0]);
                Tankstellen[i - 5, 1] = int.Parse(EingabeArrayZeile[1]);
            }
            return Tankstellen;
        }
        private void Ausgabe(int[,] Tankstellen)
        {
            for (int i = 0; i < Tankstellen.GetLength(0); i++)
            {
                TextBoxAusgabe.Text += "\n" + Tankstellen[i, 0] + " " + Tankstellen[i, 1];
            }
        }
        private int CalculateReichweite()
        {
            return (Tankfuellung * 100) / Verbrauch;
        }
        private int CalculateNumberOfStops()
        {
            int NumberOfStops = 0;
            int StreckeGefahren = GesammtStreckeGefahren + CalculateReichweite();

            //MessageBox.Show(StreckeGefahren.ToString());

            while (StreckeGefahren < Streckenlaenge)
            {
                NumberOfStops++;
                GesammtStreckeGefahren = Tankstellen[GetIndexOfNextGasstation(StreckeGefahren), 0];

                //MessageBox.Show(GesammtStreckeGefahren.ToString());

                Tankfuellung = Tankgroeße;
                StreckeGefahren = GesammtStreckeGefahren + CalculateReichweite();
                //MessageBox.Show(StreckeGefahren.ToString());

            }
            return NumberOfStops;
        }
        private int GetIndexOfNextGasstation(int Strecke)
        {
            for (int i = 0; i < Tankstellen.GetLength(0); i++)
            {
                if (Tankstellen[i, 0] > Strecke)
                {
                    return i - 1;
                }
            }
            return 0;
        }
    }
}

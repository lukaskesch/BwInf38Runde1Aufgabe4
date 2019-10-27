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
        double Verbrauch = 0;
        double Tankgroeße = 0;
        double Tankfuellung = 0;
        int Streckenlaenge = 0;
        int[,] Tankstellen;
        private void ButtonCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string EingabeString = TextBoxEingabe.Text;
                string[] EingabeArray = Regex.Split(EingabeString, "\r\n");

                Verbrauch = double.Parse(EingabeArray[0]);
                Tankgroeße = double.Parse(EingabeArray[1]);
                Tankfuellung = double.Parse(EingabeArray[2]);
                Streckenlaenge = int.Parse(EingabeArray[3]);
                Tankstellen = SetTankstellenArray(EingabeArray);

                //TextBoxAusgabe.Text += Verbrauch + "\n" + Tankgroeße + "\n" + Tankfuellung + "\n" + Streckenlaenge + "\n" + EingabeArray[4];
                //Ausgabe(Tankstellen);
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
    }
}

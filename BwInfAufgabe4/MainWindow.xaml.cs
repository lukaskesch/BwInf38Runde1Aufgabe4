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
                Reset();
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
            return Tankstellen.GetLength(0) - 1;
        }
        private void Reset()
        {
            Verbrauch = 0;
            Tankgroeße = 0;
            Tankfuellung = 0;
            Streckenlaenge = 0;
            GesammtStreckeGefahren = 0;
        }
        private void ButtonBeispiel1_Click(object sender, RoutedEventArgs e)
        {
            TextBoxEingabe.Text = "8\r\n55\r\n22\r\n1000\r\n3\r\n100    145\r\n400    140\r\n900    122";
        }
        private void ButtonBeispiel2_Click(object sender, RoutedEventArgs e)
        {
            TextBoxEingabe.Text = "24\r\n274\r\n273\r\n10000\r\n154\r\n24     119\r\n110    116\r\n157    140\r\n256    141\r\n278    144\r\n368    136\r\n416    122\r\n502    127\r\n593    121\r\n666    130\r\n751    127\r\n801    128\r\n827    144\r\n925    136\r\n991    132\r\n1044   139\r\n1118   115\r\n1159   128\r\n1203   132\r\n1296   136\r\n1375   130\r\n1479   135\r\n1546   127\r\n1621   125\r\n1679   143\r\n1695   141\r\n1726   140\r\n1832   131\r\n1922   117\r\n1978   130\r\n1995   137\r\n2041   141\r\n2119   134\r\n2160   144\r\n2240   145\r\n2315   134\r\n2421   135\r\n2493   143\r\n2527   138\r\n2543   122\r\n2559   115\r\n2654   130\r\n2762   119\r\n2813   123\r\n2883   130\r\n2950   142\r\n2982   135\r\n3009   129\r\n3052   121\r\n3103   125\r\n3162   131\r\n3239   143\r\n3346   116\r\n3427   120\r\n3462   125\r\n3480   126\r\n3548   136\r\n3638   127\r\n3670   123\r\n3761   139\r\n3833   118\r\n3898   143\r\n3946   134\r\n3979   141\r\n4051   140\r\n4096   129\r\n4124   127\r\n4219   129\r\n4289   118\r\n4393   126\r\n4455   131\r\n4497   145\r\n4590   119\r\n4695   130\r\n4769   129\r\n4835   135\r\n4874   119\r\n4891   144\r\n4965   139\r\n5065   133\r\n5159   116\r\n5247   138\r\n5323   121\r\n5352   118\r\n5406   137\r\n5493   123\r\n5534   117\r\n5581   123\r\n5687   130\r\n5796   117\r\n5881   131\r\n5982   139\r\n6049   131\r\n6110   135\r\n6200   131\r\n6266   131\r\n6297   144\r\n6312   143\r\n6392   142\r\n6452   118\r\n6501   144\r\n6599   121\r\n6651   119\r\n6734   125\r\n6758   125\r\n6799   143\r\n6859   141\r\n6912   131\r\n6942   122\r\n7049   117\r\n7149   125\r\n7258   129\r\n7296   143\r\n7317   121\r\n7363   124\r\n7446   130\r\n7494   116\r\n7589   138\r\n7653   117\r\n7737   133\r\n7803   143\r\n7820   124\r\n7929   115\r\n8012   129\r\n8117   137\r\n8135   135\r\n8174   139\r\n8242   130\r\n8349   133\r\n8417   130\r\n8511   141\r\n8589   135\r\n8611   129\r\n8643   124\r\n8717   124\r\n8765   122\r\n8795   144\r\n8854   129\r\n8880   128\r\n8936   127\r\n8987   121\r\n9041   139\r\n9100   129\r\n9186   130\r\n9292   143\r\n9398   123\r\n9472   129\r\n9502   124\r\n9598   135\r\n9642   123\r\n9731   127\r\n9789   133\r\n9864   122\r\n9936   119";
        }
        private void ButtonBeispiel3_Click(object sender, RoutedEventArgs e)
        {
            TextBoxEingabe.Text = "30\r\n160\r\n160\r\n800\r\n14\r\n7      142\r\n69     141\r\n164    139\r\n264    136\r\n302    134\r\n344    131\r\n417    129\r\n465    124\r\n552    123\r\n607    122\r\n631    120\r\n647    119\r\n706    117\r\n777    115";
        }
        private void ButtonBeispiel4_Click(object sender, RoutedEventArgs e)
        {
            TextBoxEingabe.Text = "30\r\n120\r\n111\r\n1000\r\n17\r\n7      115\r\n69     117\r\n164    119\r\n264    120\r\n302    122\r\n344    123\r\n417    124\r\n465    124\r\n552    125\r\n607    130\r\n631    131\r\n647    135\r\n706    137\r\n777    140\r\n812    141\r\n896    143\r\n950    145";
        }
        private void ButtonBeispiel5_Click(object sender, RoutedEventArgs e)
        {
            TextBoxEingabe.Text = "21\r\n244\r\n42\r\n10000\r\n171\r\n16     133\r\n45     132\r\n66     140\r\n107    116\r\n136    139\r\n194    131\r\n262    120\r\n285    120\r\n337    141\r\n446    127\r\n463    128\r\n492    132\r\n587    143\r\n627    126\r\n728    116\r\n787    137\r\n807    125\r\n822    136\r\n853    142\r\n891    132\r\n960    124\r\n985    143\r\n1090   120\r\n1198   115\r\n1220   144\r\n1305   141\r\n1377   139\r\n1427   136\r\n1505   132\r\n1523   139\r\n1566   125\r\n1586   142\r\n1646   131\r\n1742   135\r\n1845   123\r\n1890   142\r\n1911   126\r\n1926   126\r\n1987   124\r\n2078   135\r\n2104   118\r\n2168   118\r\n2264   120\r\n2301   122\r\n2344   117\r\n2433   136\r\n2477   145\r\n2565   125\r\n2669   139\r\n2707   129\r\n2744   119\r\n2837   121\r\n2893   132\r\n2984   121\r\n3045   135\r\n3081   118\r\n3127   129\r\n3143   137\r\n3247   116\r\n3281   144\r\n3316   122\r\n3419   130\r\n3454   116\r\n3542   131\r\n3600   122\r\n3633   124\r\n3690   115\r\n3751   136\r\n3780   123\r\n3852   116\r\n3924   125\r\n3975   127\r\n4033   139\r\n4119   133\r\n4189   121\r\n4209   138\r\n4299   116\r\n4314   118\r\n4343   141\r\n4370   139\r\n4459   118\r\n4509   138\r\n4569   117\r\n4642   122\r\n4714   117\r\n4746   131\r\n4806   129\r\n4899   117\r\n5001   142\r\n5098   128\r\n5188   125\r\n5223   141\r\n5323   131\r\n5407   133\r\n5506   137\r\n5531   131\r\n5572   143\r\n5644   143\r\n5678   143\r\n5751   139\r\n5805   137\r\n5892   116\r\n5987   138\r\n6040   131\r\n6100   136\r\n6155   130\r\n6185   129\r\n6245   140\r\n6318   135\r\n6352   142\r\n6449   133\r\n6525   143\r\n6605   145\r\n6692   134\r\n6791   129\r\n6883   127\r\n6905   124\r\n6925   134\r\n6959   132\r\n7031   119\r\n7110   119\r\n7209   123\r\n7252   145\r\n7276   133\r\n7308   118\r\n7347   130\r\n7426   116\r\n7473   131\r\n7535   139\r\n7585   145\r\n7665   144\r\n7698   140\r\n7798   116\r\n7820   125\r\n7851   130\r\n7918   143\r\n7944   138\r\n7997   134\r\n8015   126\r\n8034   132\r\n8063   137\r\n8142   129\r\n8169   122\r\n8232   121\r\n8259   133\r\n8341   138\r\n8419   125\r\n8525   120\r\n8609   118\r\n8672   145\r\n8690   116\r\n8753   124\r\n8844   134\r\n8944   131\r\n9049   125\r\n9140   138\r\n9184   140\r\n9218   124\r\n9264   132\r\n9342   121\r\n9399   134\r\n9448   130\r\n9494   136\r\n9511   126\r\n9547   138\r\n9633   140\r\n9726   118\r\n9790   126\r\n9810   123\r\n9913   132\r\n9945   132";
        }
    }
}

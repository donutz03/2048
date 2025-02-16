using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace game2048cs;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Grid myGrid = new Grid();
        myGrid.Width = 250;
        myGrid.Height = 100;
        myGrid.HorizontalAlignment = HorizontalAlignment.Center;
        myGrid.VerticalAlignment = VerticalAlignment.Center;
        myGrid.ShowGridLines = true;

        for (int i = 0; i < 4; i++)
        {
            ColumnDefinition colDef = new ColumnDefinition();
            myGrid.ColumnDefinitions.Add(colDef);
        }
        
        for (int i = 0; i < 4; i++)
        {
            RowDefinition rowDef = new RowDefinition();
            myGrid.RowDefinitions.Add(rowDef);
        }

        int[] startPosition = randomPosition();
        TextBlock txt1 = new TextBlock();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                TextBlock txt = new TextBlock();
                txt.Text = "0";
                txt.FontSize = 20;
                txt.TextAlignment = TextAlignment.Center;
                txt.VerticalAlignment = VerticalAlignment.Center;
                txt.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(txt, i);
                Grid.SetColumn(txt, j);
                myGrid.Children.Add(txt);
            }
        }

        this.Content = myGrid;
    }

    int[] randomPosition()
    {
        var columnPosition1 = new Random().Next(4);
        var rowPosition1 = new Random().Next(4);
        
        var columnPosition2 = new Random().Next(4);
        var rowPosition2 = new Random().Next(4);
        while (columnPosition1 == columnPosition2 && rowPosition1 == rowPosition2)
        {
            columnPosition2 = new Random().Next(4);
            rowPosition2 = new Random().Next(4);
        }

        var oddsOfGetting2or4 = Get10PercentOddsFor4InStartingPosition();
        var randomPosition1 = new Random().Next(100);
        var randomPosition2 = new Random().Next(100);
        
        while (randomPosition1 == randomPosition2)
        {
            randomPosition2 = new Random().Next(100);
        }
        
        return new int[] { columnPosition1, columnPosition2, 
            rowPosition1, rowPosition2, 
            oddsOfGetting2or4[randomPosition1],
            oddsOfGetting2or4[randomPosition2] };
    }

    private static int[] Get10PercentOddsFor4InStartingPosition()
    {
        int[] oddsOfGetting2Or4 = new int[100];
        var count = 0;
        while (count != 10)
        {
            var poz = new Random().Next(100);
            if (oddsOfGetting2Or4[poz] == 0)
            {
                oddsOfGetting2Or4[poz] = 4;
                count++;
            }
            count++;
        }

        for (int i = 0; i < 100; i++)
        {
            if (oddsOfGetting2Or4[i] == 0)
            {
                oddsOfGetting2Or4[i] = 2;
            }
        }

        return oddsOfGetting2Or4;
    }
}
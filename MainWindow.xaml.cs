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
    private static Grid myGrid;
    public MainWindow()
    {
        
        InitializeComponent();
        this.KeyDown += MainWindow_KeyDown;
        myGrid = new Grid();
        GridInit(myGrid);

        int[] startPosition = randomPosition();
        
        GetTextBlock(startPosition[0], startPosition[2], 
            startPosition[4].ToString(), myGrid);
        GetTextBlock(startPosition[1], startPosition[3], 
            startPosition[5].ToString(), myGrid);
        Content = myGrid;
        

    }
    
    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            MessageBox.Show("Ai apăsat săgeata stânga!");
        }
        else if (e.Key == Key.Right)
        {
            int[,] matrix2048 = Get2048Matrix();
            moveArrayRight(matrix2048);
            // MessageBox.Show("Ai apăsat săgeata dreapta!");
        }
        else if (e.Key == Key.Up)
        {
            MessageBox.Show("Ai apăsat săgeata sus!");
        }
        else if (e.Key == Key.Down)
        {
            MessageBox.Show("Ai apăsat săgeata jos!");
        }
    }

    private static void moveArrayRight(int[,] array)
    {
        for (int i = 0; i < 4; i++)
        {
            int[] currentRow = new int[4];
            for (int j = 0; j < 4; j++)
            {
                currentRow[j] = array[i, j];
            }

            for (int step = 0; step < 3; step++)
            {
                for (int j = 3; j > 0; j--)
                {
                    if (currentRow[j] == 0)
                    {
                        currentRow[j] = currentRow[j - 1];
                        currentRow[j - 1] = 0;
                    }
                }

                for (int j = 0; j < 4; j++)
                {
                    TextBlock tb = myGrid.Children.OfType<TextBlock>()
                        .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
                    tb.Text = currentRow[j] == 0 ? "" : currentRow[j].ToString();
                }
            }

            for (int j = 3; j > 0; j--)
            {
                if (currentRow[j] == currentRow[j - 1] && currentRow[j] != 0)
                {
                    currentRow[j] *= 2;
                    currentRow[j - 1] = 0;

                    for (int k = 0; k < 4; k++)
                    {
                        TextBlock tb = myGrid.Children.OfType<TextBlock>()
                            .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == k);
                        tb.Text = currentRow[k] == 0 ? "" : currentRow[k].ToString();
                    }


                    for (int k = j - 1; k > 0; k--)
                    {
                        if (currentRow[k] == 0)
                        {
                            currentRow[k] = currentRow[k - 1];
                            currentRow[k - 1] = 0;
                        }
                    }
                }
            }

            for (int j = 0; j < 4; j++)
            {
                array[i, j] = currentRow[j];
            }
        }
    }

    private static string getValueFromCell(int i, int j)
    {
        foreach (TextBlock textBlock in myGrid.Children)
        {
            if (Grid.GetRow(textBlock) == i && Grid.GetColumn(textBlock) == j)
            {
                return textBlock.Text;
            }
        }

        return ""
            ;
    }

    private static int[,] Get2048Matrix()
    {
        int[,] matrix2048 = new int[4, 4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                string value = getValueFromCell(i, j);
                if (value.Equals(String.Empty))
                {
                    matrix2048[i, j] = 0;
                }
                else
                {
                    matrix2048[i, j] = int.Parse(getValueFromCell(i, j));
                }
            }
        }

        return matrix2048;
    }
    
    private static void GridInit(Grid myGrid)
    {
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
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GetTextBlock(i, j, "", myGrid);
            }
        }
    }

    int[] randomCoordinatesAndNumber()
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

    private static void GetTextBlock(int i, int j, String numberInPosition, Grid myGrid)
    {
        TextBlock existingTb = myGrid.Children.OfType<TextBlock>()
            .FirstOrDefault(tb => Grid.GetRow(tb) == i && Grid.GetColumn(tb) == j);

        if (existingTb != null)
        {
            existingTb.Text = numberInPosition;
        }
        else
        {
            TextBlock txt = new TextBlock();
            txt.Text = numberInPosition;
            txt.FontSize = 20;
            txt.TextAlignment = TextAlignment.Center;
            txt.VerticalAlignment = VerticalAlignment.Center;
            txt.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(txt, i);
            Grid.SetColumn(txt, j);
            myGrid.Children.Add(txt);
        }
    }

    int[] randomPosition()
    {
        var columnPosition1 = new Random().Next(4);
        var rowPosition1 = new Random().Next(4);
        
        var columnPosition2 = new Random().Next(4);
        var rowPosition2 = new Random().Next(4);
        while (columnPosition1 == columnPosition2) 
        {
            columnPosition2 = new Random().Next(4);
        }

        while (rowPosition1 == rowPosition2)
        {
            rowPosition2 = new Random().Next(4);

        }
        var oddsOfGetting2or4 = Get10PercentOddsFor4InStartingPosition();
        var randomPosition1 = new Random().Next(100);
        var randomPosition2 = new Random().Next(100);
        
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
using System.Windows;
using System.Windows.Controls;

namespace game2048cs;

public class Game2048
{
    private readonly Grid _myGrid;

    public Game2048(Grid grid)
    {
        _myGrid = grid;
        GridInit(_myGrid);
    }
    
    
    public  void MoveArrayRight(int[,] array)
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
                    TextBlock tb = _myGrid.Children.OfType<TextBlock>()
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
                        TextBlock tb = _myGrid.Children.OfType<TextBlock>()
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
    
    public  void MoveArrayLeft(int[,] array)
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
            for (int j = 0; j < 3; j++) 
            {
                if (currentRow[j] == 0)
                {
                    currentRow[j] = currentRow[j + 1];
                    currentRow[j + 1] = 0;
                }
            }

            for (int j = 0; j < 4; j++)
            {
                TextBlock tb = _myGrid.Children.OfType<TextBlock>()
                    .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
                tb.Text = currentRow[j] == 0 ? "" : currentRow[j].ToString();
            }
        }

        for (int j = 0; j < 3; j++) 
        {
            if (currentRow[j] == currentRow[j + 1] && currentRow[j] != 0)
            {
                currentRow[j] *= 2;
                currentRow[j + 1] = 0;

                for (int k = 0; k < 4; k++)
                {
                    TextBlock tb = _myGrid.Children.OfType<TextBlock>()
                        .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == k);
                    tb.Text = currentRow[k] == 0 ? "" : currentRow[k].ToString();
                }

                for (int k = j + 1; k < 3; k++)
                {
                    if (currentRow[k] == 0)
                    {
                        currentRow[k] = currentRow[k + 1];
                        currentRow[k + 1] = 0;
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


    private string GetValueFromCell(int i, int j)
    {
        foreach (TextBlock textBlock in _myGrid.Children)
        {
            if (Grid.GetRow(textBlock) == i && Grid.GetColumn(textBlock) == j)
            {
                return textBlock.Text;
            }
        }

        return ""
            ;
    }

    public int[,] Get2048Matrix()
    {
        int[,] matrix2048 = new int[4, 4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                string value = GetValueFromCell(i, j);
                if (value.Equals(String.Empty))
                {
                    matrix2048[i, j] = 0;
                }
                else
                {
                    matrix2048[i, j] = int.Parse(GetValueFromCell(i, j));
                }
            }
        }

        return matrix2048;
    }

    private void GridInit(Grid myGrid)
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

    public  void GetTextBlock(int i, int j, String numberInPosition, Grid myGrid)
    {
        TextBlock? existingTb = myGrid.Children.OfType<TextBlock>()
            .FirstOrDefault(tb => Grid.GetRow(tb) == i && Grid.GetColumn(tb) == j);

        if (existingTb != null)
        {
            existingTb.Text = numberInPosition;
        }
        else
        {
            TextBlock txt = new TextBlock
            {
                Text = numberInPosition,
                FontSize = 20,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(txt, i);
            Grid.SetColumn(txt, j);
            myGrid.Children.Add(txt);
        }
    }

    public int[] RandomCoordinatesAndNumber()
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

        var oddsOfGetting2Or4 = Get10PercentOddsFor4InStartingPosition();
        var randomPosition1 = new Random().Next(100);
        var randomPosition2 = new Random().Next(100);
        
        while (randomPosition1 == randomPosition2)
        {
            randomPosition2 = new Random().Next(100);
        }
        
        return
        [
            columnPosition1, columnPosition2, 
            rowPosition1, rowPosition2, 
            oddsOfGetting2Or4[randomPosition1],
            oddsOfGetting2Or4[randomPosition2]
        ];
     
    }
    
    public int[] RandomPosition()
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
        var oddsOfGetting2Or4 = Get10PercentOddsFor4InStartingPosition();
        var randomPosition1 = new Random().Next(100);
        var randomPosition2 = new Random().Next(100);
        
        return
        [
            columnPosition1, columnPosition2, 
            rowPosition1, rowPosition2, 
            oddsOfGetting2Or4[randomPosition1],
            oddsOfGetting2Or4[randomPosition2]
        ];
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
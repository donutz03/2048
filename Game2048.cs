using System.Windows;
using System.Windows.Controls;

namespace game2048cs;

public class Game2048
{
    private readonly Grid _myGrid;
    private int[] OddsOfGetting2Or4 { get; }
    private static readonly Random _random = new Random();

    public Game2048(Grid grid)
    {
        _myGrid = grid;
        GridInit(_myGrid);
        OddsOfGetting2Or4 = Get10PercentOddsFor4InStartingPosition();
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
                    // textblock.redraw
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

    public void MoveArrayDown(int[,] array)
    {
        for (int j = 0; j < 4; j++)
        {
            int[] currentColumn = new int[4];
            for (int i = 0; i < 4; i++)
            {
                currentColumn[i] = array[i, j];
            }

            for (int step = 0; step < 3; step++)
            {
                for (int i = 3; i > 0; i--)
                {
                    if (currentColumn[i] == 0)
                    {
                        currentColumn[i] = currentColumn[i - 1];
                        currentColumn[i - 1] = 0;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    TextBlock tb = _myGrid.Children.OfType<TextBlock>()
                        .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
                    tb.Text = currentColumn[i] == 0 ? "" : currentColumn[i].ToString();
                }
            }

            for (int i = 3; i > 0; i--)
            {
                if (currentColumn[i] == currentColumn[i - 1] && currentColumn[i] != 0)
                {
                    currentColumn[i] *= 2;
                    currentColumn[i - 1] = 0;

                    for (int k = 0; k < 4; k++)
                    {
                        TextBlock tb = _myGrid.Children.OfType<TextBlock>()
                            .First(e => Grid.GetRow(e) == k && Grid.GetColumn(e) == j);
                        tb.Text = currentColumn[k] == 0 ? "" : currentColumn[k].ToString();
                    }

                    for (int k = i - 1; k > 0; k--)
                    {
                        if (currentColumn[k] == 0)
                        {
                            currentColumn[k] = currentColumn[k - 1];
                            currentColumn[k - 1] = 0;
                        }
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                array[i, j] = currentColumn[i];
            }
        }
    }

    public void MoveArrayUp(int[,] array)
    {
        for (int j = 0; j < 4; j++)
        {
            int[] currentColumn = new int[4];
            for (int i = 0; i < 4; i++)
            {
                currentColumn[i] = array[i, j];
            }

            for (int step = 0; step < 3; step++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (currentColumn[i] == 0)
                    {
                        currentColumn[i] = currentColumn[i + 1];
                        currentColumn[i + 1] = 0;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    TextBlock tb = _myGrid.Children.OfType<TextBlock>()
                        .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
                    tb.Text = currentColumn[i] == 0 ? "" : currentColumn[i].ToString();
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (currentColumn[i] == currentColumn[i + 1] && currentColumn[i] != 0)
                {
                    currentColumn[i] *= 2;
                    currentColumn[i + 1] = 0;

                    for (int k = 0; k < 4; k++)
                    {
                        TextBlock tb = _myGrid.Children.OfType<TextBlock>()
                            .First(e => Grid.GetRow(e) == k && Grid.GetColumn(e) == j);
                        tb.Text = currentColumn[k] == 0 ? "" : currentColumn[k].ToString();
                    }

                    for (int k = i + 1; k < 3; k++)
                    {
                        if (currentColumn[k] == 0)
                        {
                            currentColumn[k] = currentColumn[k + 1];
                            currentColumn[k + 1] = 0;
                        }
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                array[i, j] = currentColumn[i];
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
        if (!(myGrid.ColumnDefinitions.Count == 4 && myGrid.RowDefinitions.Count == 4))
        {
            myGrid.Width = 250;
            myGrid.Height = 100;
            myGrid.HorizontalAlignment = HorizontalAlignment.Center;
            myGrid.VerticalAlignment = VerticalAlignment.Center;
            myGrid.ShowGridLines = true;
        }
        

        if (myGrid.ColumnDefinitions.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                myGrid.ColumnDefinitions.Add(colDef);
            }
        }
    
        if (myGrid.RowDefinitions.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                myGrid.RowDefinitions.Add(rowDef);
            }
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

    public int[]? RandomCoordinatesAndNumber(int[,] array)
    {
      
        var oddsOfGetting2Or4 = this.OddsOfGetting2Or4;
        List<int[]> emptyPositions = new List<int[]>();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (array[i, j] == 0)
                {
                    emptyPositions.Add(new int[] { i, j });
                }
            }
        }
        
       
        if (emptyPositions.Count != 0)
        {
            int randomIndex = _random.Next(emptyPositions.Count);
            int[] randomPosition = emptyPositions[randomIndex];

        
            int randomValueIndex = _random.Next(100);
        
            return
            [
                randomPosition[0],  
                randomPosition[1], 
                oddsOfGetting2Or4[randomValueIndex],
            ];
        }

        return null;


    }
    
    public int[] RandomPosition()
    {
        var columnPosition1 = _random.Next(4);
        var rowPosition1 = _random.Next(4);
        
        var columnPosition2 = _random.Next(4);
        var rowPosition2 = _random.Next(4);
        while (columnPosition1 == columnPosition2) 
        {
            columnPosition2 = _random.Next(4);
        }

        while (rowPosition1 == rowPosition2)
        {
            rowPosition2 = _random.Next(4);

        }

        var oddsOfGetting2Or4 = OddsOfGetting2Or4;
        var randomPosition1 = _random.Next(100);
        var randomPosition2 = _random.Next(100);
        
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
        while (count < 10)
        {
            var poz = _random.Next(100);
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
    
    public bool IsGameOver(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (matrix[i,j] == 0)
                    return false;
            }
        }


        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols - 1; j++)
            {
                if (matrix[i,j] == matrix[i,j + 1])
                    return false;
            }
        }

        for (int i = 0; i < rows - 1; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (matrix[i,j] == matrix[i + 1,j])
                    return false;
            }
        }

        return true;
    }

}
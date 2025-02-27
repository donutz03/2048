using System.Windows;
using System.Windows.Controls;

namespace game2048cs.model;

public class Game2048
{
    private readonly Grid _myGrid;
    private int[] OddsOfGetting2Or4 { get; }
    private static readonly Random Random = new Random();
    public event EventHandler? StateChanged;
    
    protected virtual void OnStateChanged()
    {
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
    public Game2048(Grid grid)
    {
        _myGrid = grid;
        GridInit(_myGrid);
        OddsOfGetting2Or4 = Get10PercentOddsFor4InStartingPosition();
    }
    
public void MoveArrayRight(int[,] array)
{
    bool moved = false;
    
    for (int i = 0; i < 4; i++)
    {
        for (int j = 2; j >= 0; j--)
        {
            if (array[i, j] != 0)
            {
                int k = j;
                while (k + 1 < 4 && array[i, k + 1] == 0)
                {
                    array[i, k + 1] = array[i, k];
                    array[i, k] = 0;
                    k++;
                    moved = true;
                }
            }
        }
        
        for (int j = 3; j > 0; j--)
        {
            if (array[i, j] != 0 && array[i, j] == array[i, j - 1])
            {
                array[i, j] *= 2;
                array[i, j - 1] = 0;
                moved = true;
                
                for (int k = j - 1; k > 0; k--)
                {
                    if (array[i, k] == 0 && array[i, k - 1] != 0)
                    {
                        array[i, k] = array[i, k - 1];
                        array[i, k - 1] = 0;
                    }
                }
            }
        }
        
        for (int j = 0; j < 4; j++)
        {
            TextBlock tb = _myGrid.Children.OfType<TextBlock>()
                .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
            tb.Text = array[i, j] == 0 ? "" : array[i, j].ToString();
        }
    }
    
    if (moved)
    {
        OnStateChanged();
    }
}

public void MoveArrayLeft(int[,] array)
{
    bool moved = false;
    
    for (int i = 0; i < 4; i++)
    {
        for (int j = 1; j < 4; j++)
        {
            if (array[i, j] != 0)
            {
                int k = j;
                while (k - 1 >= 0 && array[i, k - 1] == 0)
                {
                    array[i, k - 1] = array[i, k];
                    array[i, k] = 0;
                    k--;
                    moved = true;
                }
            }
        }
        
        for (int j = 0; j < 3; j++)
        {
            if (array[i, j] != 0 && array[i, j] == array[i, j + 1])
            {
                array[i, j] *= 2;
                array[i, j + 1] = 0;
                moved = true;
                
                for (int k = j + 1; k < 3; k++)
                {
                    if (array[i, k] == 0 && array[i, k + 1] != 0)
                    {
                        array[i, k] = array[i, k + 1];
                        array[i, k + 1] = 0;
                    }
                }
            }
        }
        
        for (int j = 0; j < 4; j++)
        {
            TextBlock tb = _myGrid.Children.OfType<TextBlock>()
                .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
            tb.Text = array[i, j] == 0 ? "" : array[i, j].ToString();
        }
    }
    
    if (moved)
    {
        OnStateChanged();
    }
}

public void MoveArrayDown(int[,] array)
{
    bool moved = false;
    
    for (int j = 0; j < 4; j++)
    {
        for (int i = 2; i >= 0; i--)
        {
            if (array[i, j] != 0)
            {
                int k = i;
                while (k + 1 < 4 && array[k + 1, j] == 0)
                {
                    array[k + 1, j] = array[k, j];
                    array[k, j] = 0;
                    k++;
                    moved = true;
                }
            }
        }
        
        for (int i = 3; i > 0; i--)
        {
            if (array[i, j] != 0 && array[i, j] == array[i - 1, j])
            {
                array[i, j] *= 2;
                array[i - 1, j] = 0;
                moved = true;
                
                for (int k = i - 1; k > 0; k--)
                {
                    if (array[k, j] == 0 && array[k - 1, j] != 0)
                    {
                        array[k, j] = array[k - 1, j];
                        array[k - 1, j] = 0;
                    }
                }
            }
        }
        
        for (int i = 0; i < 4; i++)
        {
            TextBlock tb = _myGrid.Children.OfType<TextBlock>()
                .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
            tb.Text = array[i, j] == 0 ? "" : array[i, j].ToString();
        }
    }
    
    if (moved)
    {
        OnStateChanged();
    }
}

public void MoveArrayUp(int[,] array)
{
    bool moved = false;
    
    for (int j = 0; j < 4; j++)
    {
        for (int i = 1; i < 4; i++)
        {
            if (array[i, j] != 0)
            {
                int k = i;
                while (k - 1 >= 0 && array[k - 1, j] == 0)
                {
                    array[k - 1, j] = array[k, j];
                    array[k, j] = 0;
                    k--;
                    moved = true;
                }
            }
        }
        
        for (int i = 0; i < 3; i++)
        {
            if (array[i, j] != 0 && array[i, j] == array[i + 1, j])
            {
                array[i, j] *= 2;
                array[i + 1, j] = 0;
                moved = true;
                
                for (int k = i + 1; k < 3; k++)
                {
                    if (array[k, j] == 0 && array[k + 1, j] != 0)
                    {
                        array[k, j] = array[k + 1, j];
                        array[k + 1, j] = 0;
                    }
                }
            }
        }
        
        for (int i = 0; i < 4; i++)
        {
            TextBlock tb = _myGrid.Children.OfType<TextBlock>()
                .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
            tb.Text = array[i, j] == 0 ? "" : array[i, j].ToString();
        }
    }
    
    if (moved)
    {
        OnStateChanged();
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
            myGrid.Style = Application.Current.FindResource("GameGridStyle") as Style;
            // myGrid.Width = 250;
            // myGrid.Height = 100;
            myGrid.HorizontalAlignment = HorizontalAlignment.Center;
            myGrid.VerticalAlignment = VerticalAlignment.Center;
            myGrid.ShowGridLines = false; //was true
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

    public void GetTextBlock(int i, int j, String numberInPosition, Grid myGrid)
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
                Style = Application.Current.FindResource("TileTextBlockStyle") as Style,
                Width = 100,
                Height = 100,
                Margin = new Thickness(8),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            txt.Padding = new Thickness(0, 10, 0, 0);

            Grid.SetRow(txt, i);
            Grid.SetColumn(txt, j);
            myGrid.Children.Add(txt);
        }
        OnStateChanged();
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
                    emptyPositions.Add([i, j]);
                }
            }
        }
        
       
        if (emptyPositions.Count != 0)
        {
            int randomIndex = Random.Next(emptyPositions.Count);
            int[] randomPosition = emptyPositions[randomIndex];

        
            int randomValueIndex = Random.Next(100);
        
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
        var columnPosition1 = Random.Next(4);
        var rowPosition1 = Random.Next(4);
        
        var columnPosition2 = Random.Next(4);
        var rowPosition2 = Random.Next(4);
        while (columnPosition1 == columnPosition2) 
        {
            columnPosition2 = Random.Next(4);
        }

        while (rowPosition1 == rowPosition2)
        {
            rowPosition2 = Random.Next(4);

        }

        var oddsOfGetting2Or4 = OddsOfGetting2Or4;
        var randomPosition1 = Random.Next(100);
        var randomPosition2 = Random.Next(100);
        
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
            var poz = Random.Next(100);
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

    public int CalculateScore(int[,] matrix)
    {
        int score = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                score += matrix[i, j];
            }
        }
        return score;
    }

    public int FindMaxNumber(int[,] matrix)
    {
        int max = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (matrix[i, j] > max)
                {
                    max = matrix[i, j];
                }
            }
        }
        return max;
    }
    
}
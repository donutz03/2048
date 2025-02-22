using System.Windows;
using System.Windows.Controls;
using game2048cs.model;

namespace game2048cs.Hints;

public class HintSystem
{
    private readonly Game2048 _game;
    private readonly Grid _gameGrid;
    private TextBlock _hintTextBlock;
    private Button _toggleHintsButton;
    private bool _hintsEnabled = true;
    private readonly StackPanel _hintPanel;
    private readonly GameAI _ai;
    private readonly Grid _containerGrid;

    public HintSystem(Game2048 game, Grid gameGrid, Grid containerGrid)
    {
        _game = game;
        _gameGrid = gameGrid;
        _containerGrid = containerGrid;
        _ai = new GameAI();
        
        _game.StateChanged += (sender, args) => UpdateHint();

        _hintPanel = new StackPanel
        {
            Margin = new Thickness(20, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        _hintTextBlock = new TextBlock
        {
            FontSize = 18,
            Margin = new Thickness(0, 0, 0, 10),
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            Width = 180 
        };

        _toggleHintsButton = new Button
        {
            Content = "Turn Off Hints",
            HorizontalAlignment = HorizontalAlignment.Center,
            Width = 100,
            Height = 30
        };
        _toggleHintsButton.Click += ToggleHints_Click;

        _hintPanel.Children.Add(_hintTextBlock);
        _hintPanel.Children.Add(_toggleHintsButton);

        Grid.SetColumn(_hintPanel, 2);
        _containerGrid.Children.Add(_hintPanel);
    }

    public void ToggleVisibility()
    {
        _hintPanel.Visibility = _hintPanel.Visibility == Visibility.Visible 
            ? Visibility.Collapsed 
            : Visibility.Visible;
        
        if (_hintPanel.Visibility == Visibility.Visible)
        {
            UpdateHint();
        }
    }
    
    private void ToggleHints_Click(object sender, RoutedEventArgs e)
    {
        _hintsEnabled = !_hintsEnabled;
        _toggleHintsButton.Content = _hintsEnabled ? "Turn Off Hints" : "Turn On Hints";
        if (!_hintsEnabled)
        {
            _hintTextBlock.Text = "";
        }
        else
        {
            UpdateHint();
        }
    }

    public void UpdateHint()
    {
        if (!_hintsEnabled) return;

        var matrix = _game.Get2048Matrix();
        var bestMove = _ai.GetBestMove(matrix);
       var potentialScore = EvaluateMove(matrix, bestMove);
        
        _hintTextBlock.Text = $"Suggested move: {bestMove}\nPotential score: {potentialScore}";
    }


    private string GetBestMove(int[,] matrix)
    {
        var direction = _ai.GetBestMove(matrix);
        return direction.ToString();
    }
    
    private int EvaluateMove(int[,] matrix, GameAI.Direction direction)
    {
        var simulatedMatrix = SimulateMove(matrix, direction);
        return CalculateMoveScore(matrix, simulatedMatrix);
    }

    private int[,] SimulateMove(int[,] originalMatrix, GameAI.Direction direction)
    {
        var matrix = (int[,])originalMatrix.Clone();
        
        switch (direction)
        {
            case GameAI.Direction.Left:
                SimulateMoveLeft(matrix);
                break;
            case GameAI.Direction.Right:
                SimulateMoveRight(matrix);
                break;
            case GameAI.Direction.Up:
                SimulateMoveUp(matrix);
                break;
            case GameAI.Direction.Down:
                SimulateMoveDown(matrix);
                break;
        }

        return matrix;
    }
    private int CalculateMoveScore(int[,] before, int[,] after) 
    {
        int score = 0;
        int maxTile = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (after[i,j] > before[i,j])
                {
                    score += after[i,j];
                }
                maxTile = Math.Max(maxTile, after[i,j]);
            }
        }

        if ((after[0,0] == maxTile || after[0,3] == maxTile || 
             after[3,0] == maxTile || after[3,3] == maxTile))
        {
            score += maxTile;
        }

        score += CountEmptyCells(after) * 100;

        return score;
    }

    private int CountEmptyCells(int[,] matrix)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                if (matrix[i,j] == 0) count++;
        return count;
    }

   

    private void SimulateMoveLeft(int[,] matrix)
    {
        for (int i = 0; i < 4; i++)
        {
            int[] row = new int[4];
            for (int j = 0; j < 4; j++) row[j] = matrix[i,j];
            
            for (int j = 0; j < 3; j++)
            {
                if (row[j] == row[j+1] && row[j] != 0)
                {
                    row[j] *= 2;
                    row[j+1] = 0;
                }
            }
            
            for (int j = 0; j < 3; j++)
            {
                if (row[j] == 0)
                {
                    for (int k = j + 1; k < 4; k++)
                    {
                        if (row[k] != 0)
                        {
                            row[j] = row[k];
                            row[k] = 0;
                            break;
                        }
                    }
                }
            }

            for (int j = 0; j < 4; j++) matrix[i,j] = row[j];
        }
    }

    private void SimulateMoveRight(int[,] matrix)
    {
        for (int i = 0; i < 4; i++)
        {
            int[] row = new int[4];
            for (int j = 0; j < 4; j++) row[j] = matrix[i,j];
            
            for (int j = 3; j > 0; j--)
            {
                if (row[j] == row[j-1] && row[j] != 0)
                {
                    row[j] *= 2;
                    row[j-1] = 0;
                }
            }
            
            for (int j = 3; j > 0; j--)
            {
                if (row[j] == 0)
                {
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (row[k] != 0)
                        {
                            row[j] = row[k];
                            row[k] = 0;
                            break;
                        }
                    }
                }
            }

            for (int j = 0; j < 4; j++) matrix[i,j] = row[j];
        }
    }

    private void SimulateMoveUp(int[,] matrix)
    {
        for (int j = 0; j < 4; j++)
        {
            int[] column = new int[4];
            for (int i = 0; i < 4; i++) column[i] = matrix[i,j];
            
            for (int i = 0; i < 3; i++)
            {
                if (column[i] == column[i+1] && column[i] != 0)
                {
                    column[i] *= 2;
                    column[i+1] = 0;
                }
            }
            
            for (int i = 0; i < 3; i++)
            {
                if (column[i] == 0)
                {
                    for (int k = i + 1; k < 4; k++)
                    {
                        if (column[k] != 0)
                        {
                            column[i] = column[k];
                            column[k] = 0;
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < 4; i++) matrix[i,j] = column[i];
        }
    }

    private void SimulateMoveDown(int[,] matrix)
    {
        for (int j = 0; j < 4; j++)
        {
            int[] column = new int[4];
            for (int i = 0; i < 4; i++) column[i] = matrix[i,j];
            
            for (int i = 3; i > 0; i--)
            {
                if (column[i] == column[i-1] && column[i] != 0)
                {
                    column[i] *= 2;
                    column[i-1] = 0;
                }
            }
            
            for (int i = 3; i > 0; i--)
            {
                if (column[i] == 0)
                {
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (column[k] != 0)
                        {
                            column[i] = column[k];
                            column[k] = 0;
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < 4; i++) matrix[i,j] = column[i];
        }
    }
}
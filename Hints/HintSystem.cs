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

    public HintSystem(Game2048 game, Grid gameGrid, DockPanel mainContainer)
    {
        _game = game;
        _gameGrid = gameGrid;
        
        _hintPanel = new StackPanel
        {
            Margin = new Thickness(20, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center
        };

        _hintTextBlock = new TextBlock
        {
            FontSize = 18,
            Margin = new Thickness(0, 0, 0, 10)
        };

        _toggleHintsButton = new Button
        {
            Content = "Turn Off Hints",
            Width = 100,
            Height = 30
        };
        _toggleHintsButton.Click += ToggleHints_Click;

        _hintPanel.Children.Add(_hintTextBlock);
        _hintPanel.Children.Add(_toggleHintsButton);

        mainContainer.Children.Add(_hintPanel);
        DockPanel.SetDock(_hintPanel, Dock.Right);
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
        var bestMove = GetBestMove(matrix);
        _hintTextBlock.Text = $"Swipe {bestMove}";
    }

    private readonly GameAI _ai = new GameAI();

    private string GetBestMove(int[,] matrix)
    {
        var direction = _ai.GetBestMove(matrix);
        return direction.ToString();
    }

    private int EvaluateMove(int[,] originalMatrix, string move)
    {
        var matrix = (int[,])originalMatrix.Clone();
        
        switch (move)
        {
            case "LEFT":
                SimulateMoveLeft(matrix);
                break;
            case "RIGHT":
                SimulateMoveRight(matrix);
                break;
            case "UP":
                SimulateMoveUp(matrix);
                break;
            case "DOWN":
                SimulateMoveDown(matrix);
                break;
        }

        return CalculateMoveScore(originalMatrix, matrix);
    }

    private int CalculateMoveScore(int[,] before, int[,] after)
    {
        int score = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (after[i,j] > before[i,j])
                {
                    score += after[i,j] * 2;
                    
                    if ((i == 0 || i == 3) && (j == 0 || j == 3))
                    {
                        score += after[i,j];
                    }
                }
            }
        }

        score += CountEmptyCells(after) * 100;

        score += CalculateMonotonicityScore(after) * 50;

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

    private int CalculateMonotonicityScore(int[,] matrix)
    {
        int score = 0;
        
        for (int i = 0; i < 4; i++)
        {
            bool increasing = true;
            bool decreasing = true;
            for (int j = 1; j < 4; j++)
            {
                if (matrix[i,j] < matrix[i,j-1]) increasing = false;
                if (matrix[i,j] > matrix[i,j-1]) decreasing = false;
            }
            if (increasing || decreasing) score++;
        }

        for (int j = 0; j < 4; j++)
        {
            bool increasing = true;
            bool decreasing = true;
            for (int i = 1; i < 4; i++)
            {
                if (matrix[i,j] < matrix[i-1,j]) increasing = false;
                if (matrix[i,j] > matrix[i-1,j]) decreasing = false;
            }
            if (increasing || decreasing) score++;
        }

        return score;
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
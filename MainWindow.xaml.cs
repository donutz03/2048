using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using game2048cs.Context;
using game2048cs.Hints;
using game2048cs.model;
using game2048cs.View;


namespace game2048cs;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow 
{
  
    private Game2048 _game;
    private bool _wasGameOverShown;
    private readonly Grid _myGrid;
    private GameMenu _gameMenu;
    private readonly DockPanel _mainContainer;

    public MainWindow()
    {
        
        InitializeComponent();
        PreviewKeyDown += MainWindow_PreviewKeyDown;


        _mainContainer = new DockPanel();
        var gameContainer = new Grid();
        gameContainer.ColumnDefinitions.Add(
            new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
        gameContainer.ColumnDefinitions.Add(
            new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
        gameContainer.ColumnDefinitions.Add(new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        });
        StackPanel gamePanel = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        _myGrid = new Grid();
        _game = new Game2048(_myGrid);
        _gameMenu = new GameMenu(this, _game, gameContainer);
        
        _mainContainer.Children.Add(_gameMenu.GetMenu());
        DockPanel.SetDock(_gameMenu.GetMenu(), Dock.Top);
        Button newGameButton = new Button
        {
            Content = "New Game",
            Width = 100,
            Height = 30,
            Margin = new Thickness(0, 10, 0, 0) 
        };
        
        newGameButton.Click += NewGame_Click;
        gamePanel.Children.Add(_myGrid);
        gamePanel.Children.Add(newGameButton);
        Grid.SetColumn(gamePanel, 1);
        gameContainer.Children.Add(gamePanel);
        _mainContainer.Children.Add(gameContainer);
        int[] startPosition = _game.RandomPosition();

        _game.GetTextBlock(startPosition[0], startPosition[2], startPosition[4].ToString(), _myGrid);
        _game.GetTextBlock(startPosition[1], startPosition[3], startPosition[5].ToString(), _myGrid);
        Content = _mainContainer;
        

        using (var db = new GameDbContext())
        {
            db.Database.EnsureCreated();
        }
    }

    private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
    {

        if (e.Key == Key.Left || e.Key == Key.Right ||
            e.Key == Key.Up || e.Key == Key.Down)
        {
            e.Handled = true;

            int[,] matrix2048 = _game.Get2048Matrix();

            if (e.Key == Key.Left)
            {
                _game.MoveArrayLeft(matrix2048);
            }
            else if (e.Key == Key.Right)
            {
                _game.MoveArrayRight(matrix2048);
            }
            else if (e.Key == Key.Up)
            {
                _game.MoveArrayUp(matrix2048);
            }
            else if (e.Key == Key.Down)
            {
                _game.MoveArrayDown(matrix2048);
            }

            var randomCoordinates = _game.RandomCoordinatesAndNumber(matrix2048);
            if (randomCoordinates != null)
            {
                _game.GetTextBlock(randomCoordinates[0], randomCoordinates[1], randomCoordinates[2].ToString(),
                    _myGrid);
            }

            if (_game.IsGameOver(matrix2048) && !_wasGameOverShown)
            {
                _wasGameOverShown = true;
            
                int score = CalculateScore(matrix2048);
                int maxNumber = FindMaxNumber(matrix2048);
            
                var gameOverDialog = new GameOverDialog(score, maxNumber);
                gameOverDialog.Owner = this;
                gameOverDialog.ShowDialog();
            }


        }
    }

    public void NewGame_Click(object sender, RoutedEventArgs e)
    {
        _gameMenu.StopSolutionIfPlaying();

        _myGrid.Children.Clear();

        _game = new Game2048(_myGrid);
        if (_gameMenu._hintSystem != null)
        {
            _gameMenu.RecreateHintSystem();
        }
        _wasGameOverShown = false;

        int[] startPosition = _game.RandomPosition();
        _game.GetTextBlock(startPosition[0], startPosition[2], startPosition[4].ToString(), _myGrid);
        _game.GetTextBlock(startPosition[1], startPosition[3], startPosition[5].ToString(), _myGrid);
    }
    
    public KeyEventHandler GetKeyDownHandler()
    {
        return MainWindow_PreviewKeyDown;
    }

    public void DisableKeyHandler()
    {
        PreviewKeyDown -= MainWindow_PreviewKeyDown;
    }

    public void RestoreKeyHandler(KeyEventHandler handler)
    {
        PreviewKeyDown += handler;
    }

    
    private int CalculateScore(int[,] matrix)
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

    private int FindMaxNumber(int[,] matrix)
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
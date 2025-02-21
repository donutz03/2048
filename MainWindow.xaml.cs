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
    private HintSystem _hintSystem;



    public MainWindow()
    {
        
        InitializeComponent();
        KeyDown += MainWindow_KeyDown;

        var mainContainer = new DockPanel();
        StackPanel gamePanel = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        _myGrid = new Grid();
        _game = new Game2048(_myGrid);
        _gameMenu = new GameMenu(this, _game);
        mainContainer.Children.Add(_gameMenu.GetMenu());
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
        mainContainer.Children.Add(gamePanel);
        int[] startPosition = _game.RandomPosition();

        _game.GetTextBlock(startPosition[0], startPosition[2], startPosition[4].ToString(), _myGrid);
        _game.GetTextBlock(startPosition[1], startPosition[3], startPosition[5].ToString(), _myGrid);
        Content = mainContainer;
        
        _hintSystem = new HintSystem(_game, _myGrid, mainContainer);

        using (var db = new GameDbContext())
        {
            db.Database.EnsureCreated();
        }
    }

    public void NewGame_Click(object sender, RoutedEventArgs e)
    {
        
        _myGrid.Children.Clear();

        _game = new Game2048(_myGrid);

        _wasGameOverShown = false;

        int[] startPosition = _game.RandomPosition();
        _game.GetTextBlock(startPosition[0], startPosition[2], startPosition[4].ToString(), _myGrid);
        _game.GetTextBlock(startPosition[1], startPosition[3], startPosition[5].ToString(), _myGrid);
    }

    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
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
            _game.GetTextBlock(randomCoordinates[0], randomCoordinates[1], randomCoordinates[2].ToString(), _myGrid);
        }

        if (_game.IsGameOver(matrix2048) && !_wasGameOverShown)
        {
            MessageBox.Show("Game Over");
            _wasGameOverShown = true;
        }
        _hintSystem.UpdateHint();
    }
}
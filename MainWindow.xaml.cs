using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;


namespace game2048cs;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow 
{
  
    private Game2048 _game;
    private bool _wasGameOverShown = false;

    public MainWindow()
    {
        
        InitializeComponent();
        KeyDown += MainWindow_KeyDown;
        StackPanel gamePanel = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        Grid myGrid = new Grid();
        _game = new Game2048(myGrid);
        Button newGameButton = new Button
        {
            Content = "New Game",
            Width = 100,
            Height = 30,
            Margin = new Thickness(0, 10, 0, 0) 
        };
        newGameButton.Click += NewGame_Click;
        gamePanel.Children.Add(myGrid);
        gamePanel.Children.Add(newGameButton);
        int[] startPosition = _game.RandomPosition();

        _game.GetTextBlock(startPosition[0], startPosition[2], startPosition[4].ToString(), myGrid);
        _game.GetTextBlock(startPosition[1], startPosition[3], startPosition[5].ToString(), myGrid);
        Content = gamePanel;
    }
    
    private void NewGame_Click(object sender, RoutedEventArgs e)
    {
        StackPanel mainPanel = (StackPanel)Content;
        Grid myGrid = (Grid)mainPanel.Children[0];

        _game = new Game2048(myGrid);

        _wasGameOverShown = false;

        int[] startPosition = _game.RandomPosition();
        _game.GetTextBlock(startPosition[0], startPosition[2], startPosition[4].ToString(), myGrid);
        _game.GetTextBlock(startPosition[1], startPosition[3], startPosition[5].ToString(), myGrid);
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
            _game.GetTextBlock(randomCoordinates[0], randomCoordinates[1], randomCoordinates[2].ToString(), (Grid)((StackPanel)Content).Children[0]);
        }

        if (_game.IsGameOver(matrix2048) && !_wasGameOverShown)
        {
            MessageBox.Show("Game Over");
            _wasGameOverShown = true;
        }
    }
}
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;


namespace game2048cs;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow 
{
  
    private readonly Game2048 _game;
    private bool _wasGameOverShown = false;

    public MainWindow()
    {
        
        InitializeComponent();
        KeyDown += MainWindow_KeyDown;
        Grid myGrid = new Grid();
        _game = new Game2048(myGrid);

        int[] startPosition = _game.RandomPosition();

        _game.GetTextBlock(startPosition[0], startPosition[2], startPosition[4].ToString(), myGrid);
        _game.GetTextBlock(startPosition[1], startPosition[3], startPosition[5].ToString(), myGrid);
        Content = myGrid;
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
            _game.GetTextBlock(randomCoordinates[0], randomCoordinates[1], randomCoordinates[2].ToString(), (Grid)Content);
        }

        if (_game.IsGameOver(matrix2048) && !_wasGameOverShown)
        {
            MessageBox.Show("Game Over");
            _wasGameOverShown = true;
        }
    }
}
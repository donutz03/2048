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
            MessageBox.Show("You pressed up arrow key");
        }
        else if (e.Key == Key.Down)
        {
            MessageBox.Show("You pressed up arrow key!");
        }
    }
}
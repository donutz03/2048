
using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using game2048cs.Hints;
using game2048cs.model;
namespace game2048cs.AISolver;


public class SolutionPlayer
{
    private readonly Game2048 _game;
    private readonly GameAI _ai;
    private readonly DispatcherTimer _timer;
    private bool _isPlaying;
    private const int MoveIntervalMs = 400; 

    public SolutionPlayer(Game2048 game)
    {
        _game = game;
        _ai = new GameAI();
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(MoveIntervalMs)
        };
        _timer.Tick += Timer_Tick;
        _isPlaying = false;
    }

    public void StartPlaying()
    {
        if (_isPlaying)
            return;

        _isPlaying = true;
        _timer.Start();
    }

    public void StopPlaying()
    {
        _timer.Stop();
        _isPlaying = false;
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        int[,] matrix = _game.Get2048Matrix();
        
        if (_game.IsGameOver(matrix))
        {
            StopPlaying();
            MessageBox.Show("AI solution completed. Game over!", "2048 Solution", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        GameAI.Direction bestMove = _ai.GetBestMove(matrix);
        
        ApplyMove(matrix, bestMove);
        
        var randomCoordinates = _game.RandomCoordinatesAndNumber(matrix);
        if (randomCoordinates != null)
        {
            _game.GetTextBlock(randomCoordinates[0], randomCoordinates[1], randomCoordinates[2].ToString(), 
                GetGameGrid());
        }
    }

    private void ApplyMove(int[,] matrix, GameAI.Direction direction)
    {
        switch (direction)
        {
            case GameAI.Direction.Left:
                _game.MoveArrayLeft(matrix);
                break;
            case GameAI.Direction.Right:
                _game.MoveArrayRight(matrix);
                break;
            case GameAI.Direction.Up:
                _game.MoveArrayUp(matrix);
                break;
            case GameAI.Direction.Down:
                _game.MoveArrayDown(matrix);
                break;
        }
    }

    private System.Windows.Controls.Grid GetGameGrid()
    {
        return _game.GetType().GetField("_myGrid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_game) as System.Windows.Controls.Grid;
    }
}

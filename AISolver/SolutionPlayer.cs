using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using game2048cs.Hints;
using game2048cs.model;

namespace game2048cs.AISolver
{
    public class SolutionPlayer
    {
        private readonly Game2048 _game;
        private readonly GameAI _ai;
        private readonly DispatcherTimer _timer;
        private bool _isPlaying;
        private const int MoveIntervalMs = 400; 
        private readonly Window _mainWindow;
        private KeyEventHandler _existingKeyHandler;


        public SolutionPlayer(Game2048 game, Window mainWindow)
        {
            _game = game;
            _mainWindow = mainWindow;
            _ai = new GameAI();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(MoveIntervalMs)
            };
            _timer.Tick += Timer_Tick;
            _isPlaying = false;
        }
        
        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right || 
                e.Key == Key.Up || e.Key == Key.Down)
            {
                e.Handled = true;
            }
        }

        public void StartPlaying()
        {
            if (_isPlaying)
                return;

            if (_mainWindow is MainWindow mainWindowInstance)
            {
                _existingKeyHandler = mainWindowInstance.GetKeyDownHandler();
                mainWindowInstance.DisableKeyHandler();
            }

            _mainWindow.PreviewKeyDown += MainWindow_PreviewKeyDown;

            
            ShowAIPlayingOverlay(true);
            
            _isPlaying = true;
            _timer.Start();
        }

        public void StopPlaying()
        {
            _timer.Stop();
            _isPlaying = false;
            
            _mainWindow.PreviewKeyDown -= MainWindow_PreviewKeyDown;
    
            if (_mainWindow is MainWindow mainWindowInstance && _existingKeyHandler != null)
            {
                mainWindowInstance.RestoreKeyHandler(_existingKeyHandler);
            }            
            ShowAIPlayingOverlay(false);
        }
        
        private void ShowAIPlayingOverlay(bool show)
        {
            
            if (show)
            {
                string originalTitle = _mainWindow.Title;
                _mainWindow.Tag = originalTitle; 
                _mainWindow.Title = originalTitle + " - AI is playing...";
            }
            else
            {
                if (_mainWindow.Tag != null)
                {
                    _mainWindow.Title = _mainWindow.Tag.ToString();
                }
            }
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

            try
            {
                GameAI.Direction bestMove = _ai.GetBestMove(matrix);
                
                // Console.WriteLine($"AI move: {bestMove}");
                
                ApplyMove(matrix, bestMove);
                
                var randomCoordinates = _game.RandomCoordinatesAndNumber(matrix);
                if (randomCoordinates != null)
                {
                    _game.GetTextBlock(randomCoordinates[0], randomCoordinates[1], randomCoordinates[2].ToString(), 
                        GetGameGrid());
                }
            }
            catch (Exception ex)
            {
                StopPlaying();
                MessageBox.Show($"Error during AI play: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
}
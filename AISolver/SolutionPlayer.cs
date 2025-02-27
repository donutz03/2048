using System.Windows;
using System.Windows.Controls;
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
        private bool _isPaused;
        private const int MoveIntervalMs = 400; 
        private readonly Window _mainWindow;
        private KeyEventHandler _existingKeyHandler;
        private Button _pauseResumeButton;
        private Button _closeButton;
        private StackPanel _controlPanel;
        private bool _wasHintSystemVisible;
        private View.GameMenu _gameMenu;

        public SolutionPlayer(Game2048 game, Window mainWindow, View.GameMenu gameMenu)
        {
            _game = game;
            _mainWindow = mainWindow;
            _gameMenu = gameMenu;
            _ai = new GameAI();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(MoveIntervalMs)
            };
            _timer.Tick += Timer_Tick;
            _isPlaying = false;
            _isPaused = false;
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

            _wasHintSystemVisible = false;
            if (_gameMenu._hintSystem != null && IsElementVisible(_gameMenu._hintSystem))
            {
                _wasHintSystemVisible = true;
                _gameMenu._hintSystem.ToggleVisibility();
            }
            
            _gameMenu.SetHelpMenuItemsEnabled(false);
            
            CreateControlPanel();
            
            ShowAIPlayingOverlay(true);
            
            _isPlaying = true;
            _isPaused = false;
            _timer.Start();
            
            UpdateButtonStates();
        }

        private bool IsElementVisible(HintSystem hintSystem)
        {
            var fieldInfo = typeof(HintSystem).GetField("_hintPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fieldInfo != null)
            {
                var panel = fieldInfo.GetValue(hintSystem) as UIElement;
                return panel != null && panel.Visibility == Visibility.Visible;
            }
            return false;
        }

        private void CreateControlPanel()
        {
            Grid gameContainer = FindGameContainer();
            if (gameContainer == null) return;

            _controlPanel = new StackPanel
            {
                Margin = new Thickness(20, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            _pauseResumeButton = new Button
            {
                Content = "Pause",
                Width = 100,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 10)
            };
            _pauseResumeButton.Click += PauseResumeButton_Click;

            _closeButton = new Button
            {
                Content = "Close AI Solution",
                Width = 120,
                Height = 30,
                IsEnabled = false
            };
            _closeButton.Click += CloseButton_Click;

            _controlPanel.Children.Add(_pauseResumeButton);
            _controlPanel.Children.Add(_closeButton);

            Grid.SetColumn(_controlPanel, 0);
            gameContainer.Children.Add(_controlPanel);
        }

        private Grid FindGameContainer()
        {
            if (_mainWindow.Content is DockPanel mainContainer)
            {
                foreach (UIElement element in mainContainer.Children)
                {
                    if (element is Grid grid && grid.ColumnDefinitions.Count == 3)
                    {
                        return grid;
                    }
                }
            }
            return null;
        }

        private void PauseResumeButton_Click(object sender, RoutedEventArgs e)
        {
            _isPaused = !_isPaused;
            if (_isPaused)
            {
                _pauseResumeButton.Content = "Resume";
                _timer.Stop();
            }
            else
            {
                _pauseResumeButton.Content = "Pause";
                _timer.Start();
            }
            
            _closeButton.IsEnabled = _isPaused;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            StopPlaying();
        }

        public void StopPlaying()
        {
            _timer.Stop();
            _isPlaying = false;
            _isPaused = false;
            
            _mainWindow.PreviewKeyDown -= MainWindow_PreviewKeyDown;
    
            if (_mainWindow is MainWindow mainWindowInstance && _existingKeyHandler != null)
            {
                mainWindowInstance.RestoreKeyHandler(_existingKeyHandler);
            }            
            
            if (_controlPanel != null)
            {
                Grid gameContainer = FindGameContainer();
                if (gameContainer != null)
                {
                    gameContainer.Children.Remove(_controlPanel);
                }
                _controlPanel = null;
            }
            
            _gameMenu.SetHelpMenuItemsEnabled(true);
            
            if (_wasHintSystemVisible && _gameMenu._hintSystem != null)
            {
                _gameMenu._hintSystem.ToggleVisibility();
            }
            
            ShowAIPlayingOverlay(false);
        }
        
        private void UpdateButtonStates()
        {
            if (_pauseResumeButton != null)
            {
                _pauseResumeButton.Content = _isPaused ? "Resume" : "Pause";
            }
            
            if (_closeButton != null)
            {
                _closeButton.IsEnabled = _isPaused;
            }
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
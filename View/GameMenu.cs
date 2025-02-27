using System.Windows.Input;
using game2048cs.AISolver;
using game2048cs.Hints;

namespace game2048cs.View;

using model;
using System.Windows;
using System.Windows.Controls;

public class GameMenu
{
    private readonly MainWindow _mainWindow;
    private readonly Menu _menu;
    private Game2048 _game;
    public HintSystem _hintSystem;
    private readonly Grid _gameContainer; 
    private SolutionPlayer _solutionPlayer;
    private MenuItem _hintItem;
    private MenuItem _watchSolutionItem;

    public GameMenu(MainWindow mainWindow, Game2048 game, Grid mainContainer)
    {
        _mainWindow = mainWindow;
        _game = game;
        _gameContainer = mainContainer;
        _menu = new Menu
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        _menu.PreviewKeyDown += Menu_PreviewKeyDown;

        InitializeMenu();
    }
    
    private void Menu_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left || e.Key == Key.Right || 
            e.Key == Key.Up || e.Key == Key.Down)
        {
            e.Handled = true;
        }
    }

    private void InitializeMenu()
    {
        var gameMenuItem = new MenuItem { Header = "Game" };
        var newGameItem = new MenuItem { Header = "New Game" };
        newGameItem.Click += (s, e) => _mainWindow.NewGame_Click(s, e);
        gameMenuItem.Items.Add(newGameItem);

        var helpMenuItem = new MenuItem { Header = "Help" };
        _hintItem = new MenuItem { Header = "Get Hints" };
        _hintItem.Click += HintItem_Click;
        _watchSolutionItem = new MenuItem { Header = "Watch Solution" };
        _watchSolutionItem.Click += WatchSolutionItem_Click;
        helpMenuItem.Items.Add(_hintItem);
        helpMenuItem.Items.Add(_watchSolutionItem);

        var scoresMenuItem = new MenuItem { Header = "Scores" };
        var viewScoresItem = new MenuItem { Header = "View Scores" };
        viewScoresItem.Click += ViewScoresItem_Click;
        scoresMenuItem.Items.Add(viewScoresItem);

        _menu.Items.Add(gameMenuItem);
        _menu.Items.Add(helpMenuItem);
        _menu.Items.Add(scoresMenuItem);
    }

    public void SetHelpMenuItemsEnabled(bool enabled)
    {
        _hintItem.IsEnabled = enabled;
        _watchSolutionItem.IsEnabled = enabled;
    }

    public void RecreateHintSystem()
    {
        if (_hintSystem != null)
        {
            var wasVisible = IsHintSystemVisible();

            var gameGrid = _gameContainer.Children.OfType<StackPanel>()
                .FirstOrDefault()?.Children.OfType<Grid>().FirstOrDefault();

            if (gameGrid != null)
            {
                var hintPanelElement = _gameContainer.Children.OfType<StackPanel>()
                    .FirstOrDefault(sp => Grid.GetColumn(sp) == 2);

                if (hintPanelElement != null)
                {
                    _gameContainer.Children.Remove(hintPanelElement);
                }

                _hintSystem = new HintSystem(_game, gameGrid, _gameContainer);

                if (!wasVisible)
                {
                    _hintSystem.ToggleVisibility();
                }
            }
        }
    }

    private void HintItem_Click(object sender, RoutedEventArgs e)
    {
        if (_hintSystem == null)
        {
            var gameGrid = _gameContainer.Children.OfType<StackPanel>()
                .FirstOrDefault()?.Children.OfType<Grid>().FirstOrDefault();

            if (gameGrid != null)
            {
                _hintSystem = new HintSystem(_game, gameGrid, _gameContainer);
            }
        }
        else
        {
            _hintSystem?.ToggleVisibility();
        }
    }

    private void WatchSolutionItem_Click(object sender, RoutedEventArgs e)
    {
        if (_solutionPlayer == null)
        {
            _solutionPlayer = new SolutionPlayer(_game, _mainWindow, this);
        }
    
        var result = MessageBox.Show(
            "Do you want to watch the AI solve this game from the current state?", 
            "Watch Solution", 
            MessageBoxButton.YesNo, 
            MessageBoxImage.Question);
    
        if (result == MessageBoxResult.Yes)
        {
            _solutionPlayer.StartPlaying();
        }
    }
    
    public void StopSolutionIfPlaying()
    {
        _solutionPlayer?.StopPlaying();
    }

    private void ViewScoresItem_Click(object sender, RoutedEventArgs e)
    {
        var scoresWindow = new ScoresWindow();
        scoresWindow.Show();
    }

    public Menu GetMenu()
    {
        return _menu;
    }
    
    public void UpdateGameReference(Game2048 game)
    {
        _game = game;
    
        if (_hintSystem != null)
        {
            var hintPanelElement = _gameContainer.Children.OfType<StackPanel>()
                .FirstOrDefault(sp => Grid.GetColumn(sp) == 2);
            
            if (hintPanelElement != null)
            {
                _gameContainer.Children.Remove(hintPanelElement);
            }
        
            var gameGrid = _gameContainer.Children.OfType<StackPanel>()
                .FirstOrDefault()?.Children.OfType<Grid>().FirstOrDefault();
            
            if (gameGrid != null)
            {
                _hintSystem = new HintSystem(_game, gameGrid, _gameContainer);
            
                
                if (!IsHintSystemVisible())
                {
                    _hintSystem.ToggleVisibility();
                }
            }
        }
    }
    
    private bool IsHintSystemVisible()
    {
        if (_hintSystem == null) return false;
    
        var fieldInfo = typeof(HintSystem).GetField("_hintPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (fieldInfo != null)
        {
            var panel = fieldInfo.GetValue(_hintSystem) as UIElement;
            return panel != null && panel.Visibility == Visibility.Visible;
        }
        return false;
    }
}
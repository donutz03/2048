namespace game2048cs.View;


using game2048cs.model;


using System.Windows;
using System.Windows.Controls;


public class GameMenu
{
    private readonly MainWindow _mainWindow;
    private readonly Menu _menu;
    private readonly Game2048 _game;
    
    public GameMenu(MainWindow mainWindow, Game2048 game)
    {
        _mainWindow = mainWindow;
        _game = game;
        _menu = new Menu
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        InitializeMenu();
    }

    private void InitializeMenu()
    {
        var gameMenuItem = new MenuItem { Header = "Game" };
        var newGameItem = new MenuItem { Header = "New Game" };
        newGameItem.Click += (s, e) => _mainWindow.NewGame_Click(s, e);
        gameMenuItem.Items.Add(newGameItem);

        var helpMenuItem = new MenuItem { Header = "Help" };
        var hintItem = new MenuItem { Header = "Hint" };
        hintItem.Click += HintItem_Click;
        var watchSolutionItem = new MenuItem { Header = "Watch Solution" };
        watchSolutionItem.Click += WatchSolutionItem_Click;
        helpMenuItem.Items.Add(hintItem);
        helpMenuItem.Items.Add(watchSolutionItem);

        var scoresMenuItem = new MenuItem { Header = "Scores" };
        var viewScoresItem = new MenuItem { Header = "View Scores" };
        viewScoresItem.Click += ViewScoresItem_Click;
        scoresMenuItem.Items.Add(viewScoresItem);

        _menu.Items.Add(gameMenuItem);
        _menu.Items.Add(helpMenuItem);
        _menu.Items.Add(scoresMenuItem);
    }

    private void HintItem_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement hint logic
        MessageBox.Show("Hint feature coming soon!");
    }

    private void WatchSolutionItem_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement solution watching logic
        MessageBox.Show("Watch solution feature coming soon!");
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
}

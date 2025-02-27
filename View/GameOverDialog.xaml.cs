using System.Windows;
using game2048cs.Context;
using game2048cs.model;

namespace game2048cs.View;

public partial class GameOverDialog : Window
{
    private readonly int _score;
    private readonly int _maxNumber;

    public GameOverDialog(int score, int maxNumber)
    {
        InitializeComponent();
        _score = score;
        _maxNumber = maxNumber;
            
        ScoreTextBlock.Text = $"Your Score: {_score}";
        MaxNumberTextBlock.Text = $"Highest Tile: {_maxNumber}";
    }
    
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
    
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PlayerNameTextBox.Text))
        {
            MessageBox.Show("Please enter your name", "Name Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        using (var db = new GameDbContext())
        {
            var gameScore = new GameScore
            {
                PlayerName = PlayerNameTextBox.Text,
                TotalScore = _score,
                MaxNumber = _maxNumber,
                Date = DateTime.Now
            };

            db.Scores.Add(gameScore);
            db.SaveChanges();
        }

        DialogResult = true;
        Close();
    }
}
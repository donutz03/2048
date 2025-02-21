using System.Linq;
using System.Windows;
using game2048cs.Context;

namespace game2048cs
{
    public partial class ScoresWindow : Window
    {
        public ScoresWindow()
        {
            InitializeComponent();
            LoadScores();
        }

        private void LoadScores()
        {
            using (var db = new GameDbContext())
            {
                ScoresGrid.ItemsSource = db.Scores
                    .OrderByDescending(s => s.Date)
                    .ToList();
            }
        }
    }
}
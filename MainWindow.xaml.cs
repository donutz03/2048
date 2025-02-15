using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace game2048cs;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Grid myGrid = new Grid();
        myGrid.Width = 250;
        myGrid.Height = 100;
        myGrid.HorizontalAlignment = HorizontalAlignment.Center;
        myGrid.VerticalAlignment = VerticalAlignment.Center;
        myGrid.ShowGridLines = true;

        for (int i = 0; i < 4; i++)
        {
            ColumnDefinition colDef = new ColumnDefinition();
            myGrid.ColumnDefinitions.Add(colDef);
        }
        
        for (int i = 0; i < 4; i++)
        {
            RowDefinition rowDef = new RowDefinition();
            myGrid.RowDefinitions.Add(rowDef);
        }
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                TextBlock txt = new TextBlock();
                txt.Text = "0";
                txt.FontSize = 20;
                txt.TextAlignment = TextAlignment.Center;
                txt.VerticalAlignment = VerticalAlignment.Center;
                txt.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(txt, i);
                Grid.SetColumn(txt, j);
                myGrid.Children.Add(txt);
            }
        }

        this.Content = myGrid;
    }
}
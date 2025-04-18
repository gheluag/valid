using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace PlantTracker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
    private ObservableCollection<Plant> _filteredPlants = new ObservableCollection<Plant>();

    public MainWindow()
    {
        InitializeComponent();

        plantsListView.ItemsSource = _filteredPlants;

        _plants.Add(new Plant
        {
            Name = "Фикус",
            WaterFrequency = 7,
            LastWateredDate = DateTime.Now.AddDays(-5)
        });

        _plants.Add(new Plant
        {
            Name = "Кактус",
            WaterFrequency = 14,
            LastWateredDate = DateTime.Now.AddDays(-15)
        });

        _plants.Add(new Plant
        {
            Name = "Орхидея",
            WaterFrequency = 5,
            LastWateredDate = DateTime.Now.AddDays(-3)
        });

        UpdateFilteredPlants();

        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromHours(1)
        };
        timer.Tick += (s, e) => UpdateFilteredPlants();
        timer.Start();
    }

    private void MarkAsWatered_Click(object sender, RoutedEventArgs e)
    {
        if (plantsListView.SelectedItem is Plant selectedPlant)
        {
            selectedPlant.LastWateredDate = DateTime.Now;
        }
    }

    private void AddPlant_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(plantNameTextBox.Text))
        {
            MessageBox.Show("Введите название растения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(waterFrequencyTextBox.Text, out int frequency) || frequency <= 0)
        {
            MessageBox.Show("Введите корректную частоту полива (число больше 0)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var newPlant = new Plant
        {
            Name = plantNameTextBox.Text,
            WaterFrequency = frequency,
            LastWateredDate = lastWateredDatePicker.SelectedDate ?? DateTime.Now
        };

        _plants.Add(newPlant);

        // Очищаем поля ввода
        plantNameTextBox.Clear();
        waterFrequencyTextBox.Clear();
        lastWateredDatePicker.SelectedDate = DateTime.Now;

        UpdateFilteredPlants();
    }

    private void UpdateFilteredPlants()
    {
        _filteredPlants.Clear();

        var query = _plants.AsEnumerable();

        // Фильтрация по поиску
        if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
        {
            query = query.Where(p => p.Name.Contains(searchTextBox.Text, StringComparison.OrdinalIgnoreCase));
        }

        if (needsWateringCheckBox.IsChecked == true)
        {
            query = query.Where(p => p.NeedsWatering);
        }

        foreach (var plant in query)
        {
            _filteredPlants.Add(plant);
        }
    }

    private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        UpdateFilteredPlants();
    }

    private void NeedsWateringCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        UpdateFilteredPlants();
    }
}


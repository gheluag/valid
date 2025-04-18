using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlantTracker
{
    public class Plant : INotifyPropertyChanged
    {
        private string _name;
        private int _waterFrequency;
        private DateTime _lastWateredDate;
        private bool _needsWatering;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int WaterFrequency
        {
            get => _waterFrequency;
            set
            {
                _waterFrequency = value;
                OnPropertyChanged(nameof(WaterFrequency));
                UpdateWateringStatus();
            }
        }

        public DateTime LastWateredDate
        {
            get => _lastWateredDate;
            set
            {
                _lastWateredDate = value;
                OnPropertyChanged(nameof(LastWateredDate));
                OnPropertyChanged(nameof(NextWateringDate));
                UpdateWateringStatus();
            }
        }

        public DateTime NextWateringDate => LastWateredDate.AddDays(WaterFrequency);

        public string WateringStatus => _needsWatering ? "Хочеца воды" : "Кайфует";

        public Brush StatusColor => _needsWatering ? Brushes.Red : Brushes.Green;

        public bool NeedsWatering
        {
            get => _needsWatering;
            private set
            {
                _needsWatering = value;
                OnPropertyChanged(nameof(WateringStatus));
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        private void UpdateWateringStatus()
        {
            NeedsWatering = DateTime.Now >= NextWateringDate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

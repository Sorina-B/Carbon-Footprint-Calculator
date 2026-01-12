using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Proiect2
{
    public class CarbonViewModel : INotifyPropertyChanged
    {
        protected readonly DatabaseService _dbService;

        // --- 1. TRAVEL INPUTS ---
        private double _kmDriven;
        public double KmDriven { get => _kmDriven; set { _kmDriven = value; OnPropertyChanged(); } }

        private int _vehicleTypeIndex = 0;
        public int VehicleTypeIndex { get => _vehicleTypeIndex; set { _vehicleTypeIndex = value; OnPropertyChanged(); } }

        private int _flightsCount;
        public int FlightsCount { get => _flightsCount; set { _flightsCount = value; OnPropertyChanged(); } }

        // RESTORED: Public Transit Properties
        private int _transitFreqIndex = 0;
        public int TransitFreqIndex { get => _transitFreqIndex; set { _transitFreqIndex = value; OnPropertyChanged(); } }

        private int _transitTypeIndex = 0;
        public int TransitTypeIndex { get => _transitTypeIndex; set { _transitTypeIndex = value; OnPropertyChanged(); } }

        // --- 2. HOME INPUTS ---
        private double _monthlyKwh;
        public double MonthlyKwh { get => _monthlyKwh; set { _monthlyKwh = value; OnPropertyChanged(); } }

        private double _renewableEnergyPercent;
        public double RenewableEnergyPercent { get => _renewableEnergyPercent; set { _renewableEnergyPercent = value; OnPropertyChanged(); } }

        private int _newGarments;
        public int NewGarments { get => _newGarments; set { _newGarments = value; OnPropertyChanged(); } }

        // RESTORED: Gadget Properties
        private int _gadgetFreqIndex = 0;
        public int GadgetFreqIndex { get => _gadgetFreqIndex; set { _gadgetFreqIndex = value; OnPropertyChanged(); } }

        // --- 3. FOOD INPUTS ---
        private int _dietTypeIndex = 0;
        public int DietTypeIndex { get => _dietTypeIndex; set { _dietTypeIndex = value; OnPropertyChanged(); } }

        private double _compostPercent;
        public double CompostPercent { get => _compostPercent; set { _compostPercent = value; OnPropertyChanged(); } }

        // --- RESULTS VISUALIZATION ---
        private double _totalTonsResult;
        public double TotalTonsResult { get => _totalTonsResult; set { _totalTonsResult = value; OnPropertyChanged(); } }

        private bool _resultVisible = false;
        public bool ResultVisible { get => _resultVisible; set { _resultVisible = value; OnPropertyChanged(); } }

        private double _footprintProgress;
        public double FootprintProgress { get => _footprintProgress; set { _footprintProgress = value; OnPropertyChanged(); } }

        private Color _resultColor;
        public Color ResultColor { get => _resultColor; set { _resultColor = value; OnPropertyChanged(); } }

        private string _comparisonMessage;
        public string ComparisonMessage { get => _comparisonMessage; set { _comparisonMessage = value; OnPropertyChanged(); } }

        // --- COMMANDS ---
        public ICommand CalculateCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand ToggleHistoryCommand { get; }

        public CarbonViewModel()
        {
            _dbService = new DatabaseService();

            CalculateCommand = new Command(async () => await CalculateFootprint());
            ResetCommand = new Command(ResetFields);
            ToggleHistoryCommand = new Command(async () => await ToggleHistory());

            ResetFields();
        }

        private void ResetFields()
        {
            KmDriven = 0;
            FlightsCount = 0;
            MonthlyKwh = 0;
            RenewableEnergyPercent = 0;
            NewGarments = 0;
            CompostPercent = 0;

            VehicleTypeIndex = 0;
            TransitFreqIndex = 0; // Reset
            TransitTypeIndex = 0; // Reset
            GadgetFreqIndex = 0;  // Reset
            DietTypeIndex = 0;

            ResultVisible = false;
            FootprintProgress = 0;
        }

        private async Task CalculateFootprint()
        {
            double totalCo2 = 0;

            // 1. Travel Calculation
            double carFactor = VehicleTypeIndex switch
            {
                0 => 0.20, // Gas
                1 => 0.12, // Hybrid
                2 => 0.05, // Electric
                _ => 0.0   // Bike/Walk
            };
            totalCo2 += KmDriven * carFactor;
            totalCo2 += FlightsCount * 400;

            // RESTORED: Transit Logic
            double transitBase = 0;
            if (TransitFreqIndex == 1) transitBase = 300;       // Weekly
            else if (TransitFreqIndex == 2) transitBase = 1200; // Daily

            // Adjust for Train (more efficient than bus)
            if (TransitTypeIndex == 1) transitBase *= 0.6;
            totalCo2 += transitBase;

            // 2. Home Calculation
            double annualKwh = MonthlyKwh * 12;
            double renewableFactor = 1.0 - (RenewableEnergyPercent / 100.0);
            totalCo2 += annualKwh * 0.417 * renewableFactor;
            totalCo2 += NewGarments * 15.0;

            // RESTORED: Gadget Logic
            if (GadgetFreqIndex == 1) totalCo2 += 150;
            else if (GadgetFreqIndex == 2) totalCo2 += 400;

            // 3. Food Calculation
            double baseDietEmission = DietTypeIndex switch
            {
                0 => 2000, // Meat
                1 => 1200, // Veg
                _ => 1000  // Vegan
            };
            double compostFactor = (CompostPercent / 100.0) * 0.25;
            totalCo2 += baseDietEmission * (1.0 - compostFactor);

            // Final Result in Tons
            TotalTonsResult = totalCo2 / 1000.0;

            // --- VISUAL LOGIC ---
            double maxScale = 8.0;
            FootprintProgress = TotalTonsResult / maxScale;
            if (FootprintProgress > 1.0) FootprintProgress = 1.0;

            if (TotalTonsResult < 2.5)
            {
                ResultColor = Colors.SeaGreen;
                ComparisonMessage = "Fantastic! You are living sustainably. 🌱";
            }
            else if (TotalTonsResult < 4.5)
            {
                ResultColor = Color.FromArgb("#E07A5F");
                ComparisonMessage = "You are near the global average. ⚖️";
            }
            else
            {
                ResultColor = Colors.Firebrick;
                ComparisonMessage = "High Footprint. Consider reducing emissions. ⚠️";
            }

            ResultVisible = true;
            await _dbService.AddRecord(TotalTonsResult);
        }

        // --- HISTORY LOGIC ---
        public ObservableCollection<CarbonRecord> HistoryList { get; } = new ObservableCollection<CarbonRecord>();

        private bool _isHistoryVisible = false;
        public bool IsHistoryVisible
        {
            get => _isHistoryVisible;
            set
            {
                _isHistoryVisible = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCalculatorVisible));
            }
        }

        public bool IsCalculatorVisible => !IsHistoryVisible;

        private async Task ToggleHistory()
        {
            if (IsHistoryVisible)
            {
                IsHistoryVisible = false;
            }
            else
            {
                var records = await _dbService.GetHistory();
                HistoryList.Clear();
                foreach (var record in records)
                {
                    HistoryList.Add(record);
                }
                IsHistoryVisible = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
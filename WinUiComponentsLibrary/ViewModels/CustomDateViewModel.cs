using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WinUiComponentsLibrary.ViewModels
{
    public class CustomDateViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected byte _Day;
        public byte Day
        {
            get => this._Day;
            set
            {
                if (this._Day != value)
                {
                    this._Day = value;
                    this.OnPropertyChanged();
                }
            }
        }

        protected byte _Month;
        public byte Month
        {
            get => this._Month;
            set
            {
                if (this._Month != value)
                {
                    this._Month = value;
                    this.OnPropertyChanged();
                }
            }
        }

        protected ushort _Year;
        public ushort Year
        {
            get => this._Year;
            set
            {
                if (this._Year != value)
                {
                    this._Year = value;
                    this.OnPropertyChanged();
                }
            }
        }

        protected string? _DateParution;
        public string? DateParution
        {
            get => _DateParution;
            set
            {
                if (_DateParution != value)
                {
                    _DateParution = value;
                    OnPropertyChanged();
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using AppHelpers.Dates;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace WinUiComponentsLibrary.ViewModels
{
    public class CustomDateViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public DateTime? Date { get; private set; }

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

        protected string _DateParution;
        public string DateParution
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

        public string GetDate(out string messageError)
        {
            bool isDateCorrect = DateTime.TryParseExact($"{Day:00}/{Month:00}/{Year:0000}", "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out DateTime result);
            if (!isDateCorrect)
            {
                messageError = $"La date renseignée n'est pas valide.";
                Date = null;
                return DateHelpers.NoAnswer;
            }
            else
            {
                messageError = null;
                Date = result;
                return result.ToShortDateString();
            }
        }


        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

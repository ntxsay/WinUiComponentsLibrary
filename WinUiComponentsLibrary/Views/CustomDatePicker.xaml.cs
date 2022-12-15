// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using AppHelpersStd20.Dates;
using AppHelpersStd20.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiComponentsLibrary.Views
{
    public sealed partial class CustomDatePicker : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public readonly string[] chooseMonths;
        public readonly string[] chooseYear;
        public CustomDatePicker()
        {
            this.InitializeComponent();
            List<string> months = new()
            {
                DateHelpers.NoAnswer
            };
            months.AddRange(DateTimeFormatInfo.CurrentInfo.MonthNames);
            chooseMonths = months.ToArray();

            List<string> years = new ()
            {
                DateHelpers.NoAnswer
            };
            years.AddRange(Enumerable.Range(1900, 130).OrderByDescending(o => o).Select(s => s.ToString()));
            chooseYear= years.ToArray();
        }

        public ObservableCollection<string> _DaysInMonth = new();
        public ObservableCollection<string> DaysInMonth
        {
            get => _DaysInMonth;
            set
            {
                if (this._DaysInMonth != value)
                {
                    this._DaysInMonth = value;
                    this.OnPropertyChanged();
                }
            }
        }


        private bool _IsMonthEnabled;
        public bool IsMonthEnabled
        {
            get => this._IsMonthEnabled;
            set
            {
                if (this._IsMonthEnabled != value)
                {
                    this._IsMonthEnabled = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private bool _IsDayEnabled;
        public bool IsDayEnabled
        {
            get => this._IsDayEnabled;
            set
            {
                if (this._IsDayEnabled != value)
                {
                    this._IsDayEnabled = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private string _Day;
        public string Day
        {
            get => this._Day;
            set
            {
                if (this._Day != value)
                {
                    this._Day = value;
                    this.OnPropertyChanged();
                }

                //if (this.Jour != value)
                //{
                //    this.Jour = value;
                //}
            }
        }

        private string _Month;
        public string Month
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

        private string _Year;
        public string Year
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

        public string GetDate(out string messageError, out DateTime? exactDate)
        {
            try
            {
                if (!Month.IsStringNullOrEmptyOrWhiteSpace() && Month != DateHelpers.NoAnswer &&
                    Year.IsStringNullOrEmptyOrWhiteSpace() || Year == DateHelpers.NoAnswer)
                {
                    messageError = $"Vous devez spécifier l'année avant de valider le mois.";
                    exactDate = null;
                    return DateHelpers.NoAnswer;
                }
                else if (!Day.IsStringNullOrEmptyOrWhiteSpace() && Day != DateHelpers.NoAnswer &&
                    Month.IsStringNullOrEmptyOrWhiteSpace() || Month == DateHelpers.NoAnswer)
                {
                    messageError = $"Vous devez spécifier le mois avant de valider le jour.";
                    exactDate = null;
                    return DateHelpers.NoAnswer;
                }
                else
                {
                    if (!Day.IsStringNullOrEmptyOrWhiteSpace() && Day != DateHelpers.NoAnswer &&
                   !Month.IsStringNullOrEmptyOrWhiteSpace() && Month != DateHelpers.NoAnswer &&
                   !Year.IsStringNullOrEmptyOrWhiteSpace() && Year != DateHelpers.NoAnswer)
                    {
                        var day = Convert.ToInt32(Day);
                        var month = DateHelpers.ChooseMonth().ToList().IndexOf(Month);
                        var year = Convert.ToInt32(Year);
                        var isDateCorrect = DateTime.TryParseExact($"{day:00}/{month:00}/{year:0000}", "dd/MM/yyyy", new CultureInfo("fr-FR"), DateTimeStyles.AssumeLocal, out DateTime date);
                        if (!isDateCorrect)
                        {
                            messageError = $"La date renseignée n'est pas valide.";
                            exactDate = null;
                            return DateHelpers.NoAnswer;
                        }
                        else
                        {
                            messageError = null;
                            exactDate = date;
                            return date.ToShortDateString();
                        }
                    }
                    else if (!Month.IsStringNullOrEmptyOrWhiteSpace() && Month != DateHelpers.NoAnswer &&
                            !Year.IsStringNullOrEmptyOrWhiteSpace() && Year != DateHelpers.NoAnswer)
                    {
                        var month = DateHelpers.ChooseMonth().ToList().IndexOf(Month);
                        var year = Convert.ToInt32(Year);
                        var result = $"--/{month:00}/{year:0000}";

                        messageError = null;
                        exactDate = null;
                        return result;
                    }
                    else if (!Year.IsStringNullOrEmptyOrWhiteSpace() && Year != DateHelpers.NoAnswer)
                    {
                        var result = $"--/--/{Year:0000}";

                        messageError = null;
                        exactDate = null;
                        return result;
                    }
                }

                messageError = null;
                exactDate = null;
                return null;
            }
            catch (Exception)
            {
                messageError = $"Une erreur inconnue s'est produite.";
                exactDate = null;
                return DateHelpers.NoAnswer;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is string value)
            {
                if (comboBox.Tag.ToString() == "xMonth")
                {
                    if (value.IsStringNullOrEmptyOrWhiteSpace() || value == DateHelpers.NoAnswer)
                    {
                        if (Mois != 0)
                            Mois = 0;
                    }
                    else 
                    {
                        int month = DateTime.ParseExact(value, "MMMM", CultureInfo.CurrentCulture).Month;
                        if (Mois != month)
                            Mois = Convert.ToByte(month);
                    }
                }
                else if (comboBox.Tag.ToString() == "xYear")
                {
                    if (value.IsStringNullOrEmptyOrWhiteSpace() || value == DateHelpers.NoAnswer)
                    {
                        if (Annee != 0)
                            Annee = 0;
                    }
                    else if (value.All(a => char.IsNumber(a)))
                    {
                        ushort year = Convert.ToUInt16(value);
                        if (Annee != year)
                            Annee = year;
                    }
                }
                else if (comboBox.Tag.ToString() == "xDay")
                {
                    if (value.IsStringNullOrEmptyOrWhiteSpace() || value == DateHelpers.NoAnswer)
                    {
                        if (Jour != 0)
                            Jour = 0;
                    }
                    else if (value.All(a => char.IsNumber(a)))
                    {
                        byte day = Convert.ToByte(value);
                        if (Jour != day)
                            Jour = day;
                    }
                }
            }
        }

        #region Jour
        public byte Jour
        {
            get { return (byte)GetValue(JourProperty); }
            set { SetValue(JourProperty, value); }
        }

        public static readonly DependencyProperty JourProperty = DependencyProperty.Register(nameof(Jour), typeof(byte),
                                                                typeof(CustomDatePicker), new PropertyMetadata(null, new PropertyChangedCallback(OnJourChanged)));

        private static void OnJourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomDatePicker parent && e.NewValue is byte value)
            {
                parent.Day = value > 0 && value <= 31 ? value.ToString() : DateHelpers.NoAnswer;
            }
        }
        #endregion

        #region Mois
        public byte Mois
        {
            get { return (byte)GetValue(MoisProperty); }
            set => SetValue(MoisProperty, value);
        }

        public static readonly DependencyProperty MoisProperty = DependencyProperty.Register(nameof(Mois), typeof(byte),
                                                                typeof(CustomDatePicker), new PropertyMetadata(null, new PropertyChangedCallback(OnMoisChanged)));

        private static void OnMoisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomDatePicker parent && e.NewValue is byte value)
            {
                if (value > 0 && value <= 12)
                {
                    parent.Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(value);
                    parent.IsDayEnabled = true;
                    int daysNb = DateTime.DaysInMonth(parent.Annee, value);
                    List<string> days = new()
                    {
                        DateHelpers.NoAnswer,
                    };
                    days.AddRange(Enumerable.Range(1, daysNb).Select(s => s.ToString()));

                    parent.DaysInMonth.Clear();

                    days.ForEach(f => parent.DaysInMonth.Add(f));
                }
                else
                {
                    parent.Month = DateHelpers.NoAnswer;
                    parent.IsDayEnabled = false;
                }
            }
        }
        #endregion

        #region Annee
        public ushort Annee
        {
            get { return (ushort)GetValue(AnneeProperty); }
            set { SetValue(AnneeProperty, value); }
        }

        public static readonly DependencyProperty AnneeProperty = DependencyProperty.Register(nameof(Annee), typeof(ushort),
                                                                typeof(CustomDatePicker), new PropertyMetadata(null, new PropertyChangedCallback(OnAnneeChanged)));

        private static void OnAnneeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomDatePicker parent && e.NewValue is ushort value)
            {
                if (value >= 1900 && value < 2100)
                {
                    parent.Year = value.ToString();
                    parent.IsMonthEnabled = true;
                }
                else
                {
                    parent.Year = DateHelpers.NoAnswer;
                    parent.IsMonthEnabled = false;
                }
            }
        }
        #endregion

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

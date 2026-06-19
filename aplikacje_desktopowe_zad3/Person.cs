using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace aplikacje_desktopowe_zad3
{
    public class Person : INotifyPropertyChanged
    {
        private string _imieNazwisko = string.Empty;
        private string _pierwszeImie = string.Empty;
        private string _nazwisko = string.Empty;
        private string[] _imiona = Array.Empty<string>();
        private DateTime? _dataUrodzin;
        private int? _wiek;

        public string ImieNazwisko
        {
            get => _imieNazwisko;
            set
            {
                _imieNazwisko = value;
                OnPropertyChanged();

                var wyrazy = (value ?? string.Empty)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (wyrazy.Length > 0)
                {
                    Imiona = wyrazy.Length > 1 ? wyrazy[..^1] : wyrazy;
                    PierwszeImie = wyrazy[0];
                    Nazwisko = wyrazy.Length > 1 ? wyrazy[^1] : string.Empty;
                }
                else
                {
                    Imiona = Array.Empty<string>();
                    PierwszeImie = string.Empty;
                    Nazwisko = string.Empty;
                }
            }
        }

        public string[] Imiona
        {
            get => _imiona;
            private set
            {
                _imiona = value;
                OnPropertyChanged();
            }
        }

        public string PierwszeImie
        {
            get => _pierwszeImie;
            private set
            {
                _pierwszeImie = value;
                OnPropertyChanged();
            }
        }

        public string Nazwisko
        {
            get => _nazwisko;
            private set
            {
                _nazwisko = value;
                OnPropertyChanged();
            }
        }

        public DateTime? DataUrodzin
        {
            get => _dataUrodzin;
            set
            {
                _dataUrodzin = value;
                OnPropertyChanged();

                Wiek = ObliczWiek(value);
            }
        }

        public int? Wiek
        {
            get => _wiek;
            private set
            {
                _wiek = value;
                OnPropertyChanged();
            }
        }

        private static int? ObliczWiek(DateTime? dataUrodzin)
        {
            if (dataUrodzin is null)
            {
                return null;
            }

            var dzis = DateTime.Today;
            var wiek = dzis.Year - dataUrodzin.Value.Year;
            if (dataUrodzin.Value.Date > dzis.AddYears(-wiek))
            {
                wiek--;
            }

            return Math.Max(wiek, 0);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

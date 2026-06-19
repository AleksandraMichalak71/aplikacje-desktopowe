using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace aplikacje_desktopowe_zad4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ErrorText = "Błąd";

        private double? _accumulator;
        private string? _pendingOp;
        private string? _lastOp;
        private double _lastRightOperand;
        private bool _newEntry = true;
        private bool _hasError;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            var digit = ((Button)sender).Content.ToString()!;

            if (_hasError)
            {
                Reset();
            }

            if (_newEntry)
            {
                DisplayText.Text = digit == "0" ? "0" : digit;
                _newEntry = false;
            }
            else
            {
                DisplayText.Text = DisplayText.Text == "0" ? digit : DisplayText.Text + digit;
            }
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                Reset();
            }

            if (_newEntry)
            {
                DisplayText.Text = "0,";
                _newEntry = false;
                return;
            }

            if (!DisplayText.Text.Contains(','))
            {
                DisplayText.Text += ",";
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                return;
            }

            var op = (string)((Button)sender).Tag;
            var currentValue = ParseDisplay();

            if (_accumulator.HasValue && _pendingOp != null && !_newEntry)
            {
                var result = Calculate(_accumulator.Value, currentValue, _pendingOp);
                if (!TrySetResult(result))
                {
                    return;
                }
                _accumulator = result;
            }
            else
            {
                _accumulator = currentValue;
            }

            _pendingOp = op;
            _newEntry = true;
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                return;
            }

            var currentValue = ParseDisplay();

            if (_pendingOp != null && _accumulator.HasValue)
            {
                var right = _newEntry ? _lastRightOperand : currentValue;
                var result = Calculate(_accumulator.Value, right, _pendingOp);

                _lastOp = _pendingOp;
                _lastRightOperand = right;

                if (!TrySetResult(result))
                {
                    return;
                }

                _accumulator = result;
                _pendingOp = null;
                _newEntry = true;
            }
            else if (_lastOp != null)
            {
                var result = Calculate(currentValue, _lastRightOperand, _lastOp);
                if (!TrySetResult(result))
                {
                    return;
                }

                _accumulator = result;
                _newEntry = true;
            }
        }

        private void Percent_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                return;
            }

            var currentValue = ParseDisplay();
            double result;

            if (_accumulator.HasValue && (_pendingOp == "+" || _pendingOp == "-"))
            {
                result = _accumulator.Value * (currentValue / 100.0);
            }
            else
            {
                result = currentValue / 100.0;
            }

            TrySetResult(result);
            _newEntry = true;
        }

        private void Sqrt_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                return;
            }

            var currentValue = ParseDisplay();
            if (currentValue < 0)
            {
                ShowError();
                return;
            }

            TrySetResult(Math.Sqrt(currentValue));
            _newEntry = true;
        }

        private void Invert_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                return;
            }

            var currentValue = ParseDisplay();
            if (currentValue == 0)
            {
                ShowError();
                return;
            }

            TrySetResult(1.0 / currentValue);
            _newEntry = true;
        }

        // Funkcja dodatkowa 1: silnia (n!) - iloczyn liczb od 2 do x;
        // dziala tylko dla nieujemnych liczb calkowitych (i nie wiekszych niz 170, bo dalej wynik wychodzi poza zakres double)
        private void Factorial_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                return;
            }

            var currentValue = ParseDisplay();
            if (currentValue < 0 || currentValue != Math.Floor(currentValue) || currentValue > 170)
            {
                ShowError();
                return;
            }

            double result = 1;
            for (var i = 2; i <= currentValue; i++)
            {
                result *= i;
            }

            TrySetResult(result);
            _newEntry = true;
        }

        // Funkcja dodatkowa 2: losowanie liczby zmiennoprzecinkowej z przedzialu [0, x] -
        // wpisana wartosc x staje sie gornym ograniczeniem losowania, wynik zastepuje wyswietlana liczbe
        private void Random_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                return;
            }

            var currentValue = ParseDisplay();
            var result = Random.Shared.NextDouble() * currentValue;
            TrySetResult(result);
            _newEntry = true;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            DisplayText.Text = "0";
            _accumulator = null;
            _pendingOp = null;
            _lastOp = null;
            _lastRightOperand = 0;
            _newEntry = true;
            _hasError = false;
        }

        private double Calculate(double left, double right, string op)
        {
            switch (op)
            {
                case "+": return left + right;
                case "-": return left - right;
                case "*": return left * right;
                case "/":
                    if (right == 0)
                    {
                        ShowError();
                        return 0;
                    }
                    return left / right;
                case "^": return Math.Pow(left, right);
                case "mod":
                    if (right == 0)
                    {
                        ShowError();
                        return 0;
                    }
                    return left % right;
                default: return right;
            }
        }

        private bool TrySetResult(double value)
        {
            if (_hasError)
            {
                return false;
            }

            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                ShowError();
                return false;
            }

            DisplayText.Text = value.ToString("G15", CultureInfo.InvariantCulture).Replace('.', ',');
            return true;
        }

        private void ShowError()
        {
            DisplayText.Text = ErrorText;
            _hasError = true;
        }

        private double ParseDisplay()
        {
            return double.Parse(DisplayText.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calculator.Logic;
using Newtonsoft.Json;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        private UserSettings settings;
        private int numericBase;
        private int openParenthesesCount;
        private string CalculationMode;
        private Memory memory = new Memory();

        public MainWindow()
        {
            InitializeComponent();
            settings = UserSettings.LoadSettings();
            ApplySettings();
        }
        private void ApplySettings()
        {
            CalculationMode = settings.CalculationMode;
            numericBase = settings.NumericBase;
            if (settings.Mode == "Standard")
                SetStandardMode(null, null);
            else
                SetProgrammerMode(null, null);
            switch (numericBase)
            {
                case 2: Bin_Click(null, null); break;
                case 8: Oct_Click(null, null); break;
                case 10: Dec_Click(null, null); break;
                case 16: Hex_Click(null, null); break;
            }
        }
        private void AppendToDisplay(string value)
        {
            if (Display.Text == "0")
            {
                Display.Text = value;
            }
            else if (CalculationMode == "AfterEqual")
            {
                Display.Text += value;
            }
            else if (IsValidCalculation(Display.Text + value))
            {
                string expressionWithoutLastOperator = (Display.Text + value).TrimEnd('+', '-', '×', '÷');
                string result = ArithmeticLogic.CalculateExpression(expressionWithoutLastOperator);

                if (numericBase == 10 && result.Contains("."))
                    Display.Text = result;
                else
                    Display.Text = FormatNumber(result) + value;
            }
            else
            {
                Display.Text = FormatNumber(Display.Text + value);
            }
        }
        private bool IsValidCalculation(string text)
        {
            string validNumberPattern;

            if (numericBase == 16)
            {
                // În baza 16, permitem și literele A-F (majuscule și minuscule)
                validNumberPattern = @"[0-9A-Fa-f]+";
            }
            else
            {
                // În bazele 2, 8 și 10, permitem doar cifrele
                validNumberPattern = @"\d+";
            }

            // Regex pentru a verifica expresii de forma "-număr operator număr operator"
            return System.Text.RegularExpressions.Regex.IsMatch(text,
                $@"^\-?{validNumberPattern}(\.\d+)?([\+\-\×\÷]\-?{validNumberPattern}(\.\d+)?)+[\+\-\×\÷]$");
        }


        private void Zero_Click(object sender, RoutedEventArgs e) => AppendToDisplay("0");
        private void One_Click(object sender, RoutedEventArgs e) => AppendToDisplay("1");
        private void Two_Click(object sender, RoutedEventArgs e) { if (numericBase > 2) AppendToDisplay("2"); }
        private void Three_Click(object sender, RoutedEventArgs e) { if (numericBase > 2) AppendToDisplay("3"); }
        private void Four_Click(object sender, RoutedEventArgs e) { if (numericBase >= 8) AppendToDisplay("4"); }
        private void Five_Click(object sender, RoutedEventArgs e) { if (numericBase >= 8) AppendToDisplay("5"); }
        private void Six_Click(object sender, RoutedEventArgs e) { if (numericBase >= 8) AppendToDisplay("6"); }
        private void Seven_Click(object sender, RoutedEventArgs e) { if (numericBase >= 8) AppendToDisplay("7"); }
        private void Eight_Click(object sender, RoutedEventArgs e) { if (numericBase > 8) AppendToDisplay("8"); }
        private void Nine_Click(object sender, RoutedEventArgs e) { if (numericBase > 8) AppendToDisplay("9"); }
        private void A_Click(object sender, RoutedEventArgs e) { if (numericBase == 16) AppendToDisplay("A"); }
        private void B_Click(object sender, RoutedEventArgs e) { if (numericBase == 16) AppendToDisplay("B"); }
        private void C_Click(object sender, RoutedEventArgs e) { if (numericBase == 16) AppendToDisplay("C"); }
        private void D_Click(object sender, RoutedEventArgs e) { if (numericBase == 16) AppendToDisplay("D"); }
        private void E_Click(object sender, RoutedEventArgs e) { if (numericBase == 16) AppendToDisplay("E"); }
        private void F_Click(object sender, RoutedEventArgs e) { if (numericBase == 16) AppendToDisplay("F"); }



        private void Plus_Click(object sender, RoutedEventArgs e) => AppendToDisplay("+");
        private void Minus_Click(object sender, RoutedEventArgs e) => AppendToDisplay("-");
        private void Multiply_Click(object sender, RoutedEventArgs e) => AppendToDisplay("×");
        private void Divide_Click(object sender, RoutedEventArgs e) => AppendToDisplay("÷");
        private void Percent_Click(object sender, RoutedEventArgs e) => AppendToDisplay("%");


        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = ArithmeticLogic.CalculateExpression(Display.Text);
        }
        private void Inversion_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = ArithmeticLogic.Inversion(Display.Text);
        }
        private void Square_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = ArithmeticLogic.Square(Display.Text);
        }
        private void Root_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = ArithmeticLogic.SquareRoot(Display.Text);
        }
        private void OppositeSign_Click(object sender, RoutedEventArgs e)
        {
            double number = Convert.ToDouble(Display.Text);
            Display.Text = (-number).ToString();
        }
        private void LeftBracket_Click(object sender, RoutedEventArgs e)
        {
            AppendToDisplay("(");
            openParenthesesCount++;
        }
        private void RightBracket_Click(object sender, RoutedEventArgs e)
        {
            if (openParenthesesCount > 0) 
            {
                AppendToDisplay(")");
                openParenthesesCount--;
            }
        }
        private void Point_Click(object sender, RoutedEventArgs e)
        {
            if (numericBase > 2)
                AppendToDisplay(".");
        }


        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            openParenthesesCount = 0;
        }
        private void ClearLast_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length > 1)
            {
                Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
                if (Display.Text.EndsWith("."))
                {
                    Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
                }
            }
            else
            {
                Display.Text = "0";
            }
        }
        private void ClearEntery_Click(object sender, RoutedEventArgs e)
        {
            string text = Display.Text;
            int lastOperatorIndex = text.LastIndexOfAny(new char[] { '+', '-', '×', '÷', '%' });

            if (lastOperatorIndex != -1)
                Display.Text = text.Substring(0, lastOperatorIndex + 1);
            else
                Display.Text = "0";
        }


        private void MemoryRecall_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = memory.RecallMemory().ToString();
        }
        private void MemoryClear_Click(object sender, RoutedEventArgs e)
        {
            memory.ClearMemory();
        }
        private void MemoryAdd_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, out double value))
            {
                memory.AddToMemory(value);
            }
        }
        private void MemorySubstract_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, out double value))
            {
                memory.SubtractFromMemory(value);
            }
        }
        private void MemoryStore_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, out double value))
            {
                memory.StoreMemory(value);
            }
        }
        private void Memory_Click(object sender, RoutedEventArgs e)
        {
            List<double> values = memory.GetMemoryStack();

            if (values.Count == 0)
            {
                MessageBox.Show("Memoria este goală!", "Memorie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string memoryContent = string.Join("\n", values);
                MessageBox.Show($"Valori în memorie:\n{memoryContent}", "Memorie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void SetStandardMode(object sender, RoutedEventArgs e)
        {
            StandardModePanel.Visibility = Visibility.Visible;
            ProgrammerModePanel.Visibility = Visibility.Collapsed;
            numericBase = 10;
            settings.Mode = "Standard";
            settings.SaveSettings();
            ConversionsButton.Visibility = Visibility.Collapsed;
            Dec_Click(null, null);
            ClearAll_Click(null, null);
            EnableValidButtons();
        }
        private void SetProgrammerMode(object sender, RoutedEventArgs e)
        {
            StandardModePanel.Visibility = Visibility.Collapsed;
            ProgrammerModePanel.Visibility = Visibility.Visible;
            numericBase = settings.NumericBase;
            settings.Mode = "Programmer";
            settings.SaveSettings();
            ConversionsButton.Visibility = Visibility.Visible;
            switch (numericBase)
            {
                case 2: Bin_Click(null, null); break;
                case 8: Oct_Click(null, null); break;
                case 10: Dec_Click(null, null); break;
                case 16: Hex_Click(null, null); break;
                default: Dec_Click(null, null); break;
            }
            ClearAll_Click(null, null);
            EnableValidButtons();
        }


        private void Bin_Click(object sender, RoutedEventArgs e)
        {
            numericBase = 2;
            ArithmeticLogic.SetBase(2);
            settings.NumericBase = 2;
            settings.SaveSettings();
            ClearAll_Click(null, null);
            EnableValidButtons();
        }
        private void Oct_Click(object sender, RoutedEventArgs e)
        {
            numericBase = 8;
            ArithmeticLogic.SetBase(8);

            settings.NumericBase = 8;
            settings.SaveSettings();
            ClearAll_Click(null, null);
            EnableValidButtons();
        }
        private void Dec_Click(object sender, RoutedEventArgs e)
        {
            numericBase = 10;
            ArithmeticLogic.SetBase(10);

            settings.NumericBase = 10;
            settings.SaveSettings();
            ClearAll_Click(null, null);
            EnableValidButtons();
        }
        private void Hex_Click(object sender, RoutedEventArgs e)
        {
            numericBase = 16;
            ArithmeticLogic.SetBase(16);

            settings.NumericBase = 16;
            settings.SaveSettings();
            ClearAll_Click(null, null);
            EnableValidButtons();
        }


        private void EnableValidButtons()
        {
            foreach (var child in ProgrammerModePanel.Children)
            {
                if (child is Button button)
                {
                    string text = button.Content.ToString();
                    button.IsEnabled = IsValidForBase(text);
                }
            }
        }
        private bool IsValidForBase(string buttonText)
        {
            if ("+-*/%()=".Contains(buttonText)) return true;
            if (numericBase == 2) return "01".Contains(buttonText);
            if (numericBase == 8) return "01234567".Contains(buttonText);
            if (numericBase == 10) return "0123456789".Contains(buttonText);
            if (numericBase == 16) return "0123456789ABCDEF".Contains(buttonText);
            return false;
        }


        private void AfterEqualMode(object sender, RoutedEventArgs e)
        {
            CalculationMode = "AfterEqual";
            settings.CalculationMode = "AfterEqual";
            settings.SaveSettings();
        }
        private void WhileTypingMode(object sender, RoutedEventArgs e)
        {
            CalculationMode = "WhileTyping";
            settings.CalculationMode = "WhileTyping";
            settings.SaveSettings();
        }


        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Display.Text) && Display.Text != "0")
            {
                Clipboard.SetText(Display.Text);
            }
        }
        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Display.Text) && Display.Text != "0")
            {
                Clipboard.SetText(Display.Text);
                Display.Text = "0";
            }
        }
        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText().Trim();
                if (IsValidPaste(clipboardText))
                {
                    AppendToDisplay(clipboardText);
                }
                else
                {
                    MessageBox.Show("Invalid input from clipboard!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private bool IsValidPaste(string text)
        {
            string validChars;

            switch (numericBase)
            {
                case 2:
                    validChars = "01";
                    break;
                case 8:
                    validChars = "01234567";
                    break;
                case 10:
                    validChars = "0123456789";
                    break;
                case 16:
                    validChars = "0123456789ABCDEF";
                    break;
                default:
                    validChars = "0123456789";
                    break;
            }
            return System.Text.RegularExpressions.Regex.IsMatch(text, $"^[{validChars}+\\-*/().%×÷]+$");
        }


        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Stoica Andra Stefania, grupa 10LF234", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            string key = e.Key.ToString();
            if (key.Length == 2 && key.StartsWith("D"))
            {
                string digit = key[1].ToString();
                if (IsValidForBase(digit))
                    AppendToDisplay(digit);
            }
            else if (key.Length == 3 && key.StartsWith("NumPad"))
            {
                string digit = key[6].ToString();
                if (IsValidForBase(digit))
                    AppendToDisplay(digit);
            }
            else
            {
                switch (key)
                {
                    case "Add":
                    case "OemPlus":
                        AppendToDisplay("+");
                        break;

                    case "Subtract":
                    case "OemMinus":
                        AppendToDisplay("-");
                        break;

                    case "Multiply":
                    case "OemAsterisk":
                        AppendToDisplay("×");
                        break;

                    case "Divide": 
                    case "Oem2":   
                        AppendToDisplay("÷");
                        break;

                    case "Decimal":
                    case "OemPeriod":
                        AppendToDisplay(".");
                        break;

                    case "Return": 
                    case "Enter":
                        Equals_Click(sender, e);
                        break;

                    case "Back": 
                        ClearLast_Click(sender, e);
                        break;

                    case "Escape": 
                        ClearAll_Click(sender, e);
                        break;

                    case "OemOpenBrackets": 
                        AppendToDisplay("(");
                        openParenthesesCount++;
                        break;

                    case "OemCloseBrackets": 
                        if (openParenthesesCount > 0)
                        {
                            AppendToDisplay(")");
                            openParenthesesCount--;
                        }
                        break;

                    case "Oem5":
                        AppendToDisplay("%");
                        break;

                    default:
                        if (numericBase == 16 && "ABCDEF".Contains(key))
                        {
                            AppendToDisplay(key);
                        }
                        break;
                }
            }
        }


        private string FormatNumber(string number)
        {
            if (!settings.DigitGrouping) return number; 

            if (numericBase == 10) 
            {
                if (double.TryParse(number, out double num))
                {
                    return num.ToString("#,##0");
                }
            }
            else if (numericBase == 2 || numericBase == 8 || numericBase == 16)
            {
                
                return GroupByBase(number, numericBase);
            }

            return number;
        }
        private string GroupByBase(string number, int numericBase)
        {
            int groupSize;
            if (numericBase == 2)
                groupSize= 4;  
            else if (numericBase == 8)
                groupSize=3;
            else if (numericBase == 16)
                groupSize=4;
            else
                groupSize= 3;

            number = number.Replace(",", "");

            int len = number.Length;
            if (len <= groupSize) return number;

            List<string> groups = new List<string>();
            for (int i = len; i > 0; i -= groupSize)
            {
                int start = Math.Max(0, i - groupSize);
                groups.Insert(0, number.Substring(start, i - start));
            }

            return string.Join(",", groups);
        }
        private void Grouping_Click(object sender, RoutedEventArgs e)
        {
            settings.DigitGrouping = !settings.DigitGrouping;
            settings.SaveSettings();
            string message = settings.DigitGrouping ? "Digit Grouping Enabled" : "Digit Grouping Disabled";
            MessageBox.Show(message, "Digit Grouping", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void Show_Json_click(object sender, RoutedEventArgs e)
        {
            UserSettings settings = UserSettings.LoadSettings();
            string jsonSettings = JsonConvert.SerializeObject(settings, Formatting.Indented);
            MessageBox.Show(jsonSettings, "User Settings", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Show_Conversions(object sender, RoutedEventArgs e)
        {
            if (!long.TryParse(Display.Text, out long number))
            {
                MessageBox.Show("Invalid input! Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string binary = Convert.ToString(number, 2);
            string octal = Convert.ToString(number, 8);
            string decimalValue = number.ToString();
            string hex = Convert.ToString(number, 16).ToUpper();

            string message = $"Binary: {binary}\nOctal: {octal}\nDecimal: {decimalValue}\nHexadecimal: {hex}";

            MessageBox.Show(message, "Number Conversions", MessageBoxButton.OK, MessageBoxImage.Information);
        }


    }
}

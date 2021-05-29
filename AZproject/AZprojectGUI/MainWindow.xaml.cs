using Common;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AZprojectGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum InputState
    {
        Start,
        FirstSymbol,
        SecondSymbol,
        Solved
    }

    public partial class MainWindow : Window
    {
        InputState state;
        bool[] results;
        Algorithm solver = new Algorithm();

        public MainWindow()
        {
            InitializeComponent();
            state = InputState.Start;
            FormulaTextbox.Text = "(";

            
            
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]"))
            {              

                switch (state)
                {
                    case InputState.Start:
                        FormulaTextbox.Text += e.Text + "|";
                        state = InputState.SecondSymbol;
                        break;

                    case InputState.FirstSymbol:
                        FormulaTextbox.Text += e.Text + "|";
                        state = InputState.SecondSymbol;
                        break;

                    case InputState.SecondSymbol:
                        FormulaTextbox.Text += e.Text + ")&(";
                        state = InputState.FirstSymbol;
                        break;

                    default:
                        break;
                }
            }

            refocusTextbox();
            e.Handled = true;
        }

        private void FormulaTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FormulaTextbox.Text.Length == 0)
            {
                FormulaTextbox.Text = "(";
                state = InputState.Start;
                refocusTextbox();
            }
        }

        private void FormulaTextbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Right || e.Key == Key.Left)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Back )
            {
                switch (state)
                {
                    case InputState.Start:

                        break;
                    case InputState.FirstSymbol:
                        if (FormulaTextbox.Text.Length > 2)
                        {
                            FormulaTextbox.Text = FormulaTextbox.Text[FormulaTextbox.Text.Length - 1] == '~'
                                ? FormulaTextbox.Text.Remove(FormulaTextbox.Text.Length - 5, 5)
                                : FormulaTextbox.Text.Remove(FormulaTextbox.Text.Length - 4, 4);

                            state = InputState.SecondSymbol;
                            FormulaTextbox.CaretIndex = FormulaTextbox.Text.Length;
                        }
                        break;

                    case InputState.SecondSymbol:
                        FormulaTextbox.Text = FormulaTextbox.Text[FormulaTextbox.Text.Length - 1] == '~'
                                ? FormulaTextbox.Text.Remove(FormulaTextbox.Text.Length - 3, 3)
                                : FormulaTextbox.Text.Remove(FormulaTextbox.Text.Length - 2, 2);
                        state = FormulaTextbox.Text.Length<=2 ? InputState.Start : InputState.FirstSymbol;

                        FormulaTextbox.CaretIndex = FormulaTextbox.Text.Length;
                        break;

                    default:
                        break;
                }
                e.Handled = true;
            }
        }

        private void FormulaTextbox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            refocusTextbox();
        }

        private void negateButton_Click(object sender, RoutedEventArgs e)
        {
            switch (state)
            {
                case InputState.FirstSymbol:
                    if (FormulaTextbox.Text[FormulaTextbox.Text.Length - 1] != '~')
                    {
                        FormulaTextbox.Text += "~";
                    }
                    else
                    {
                        FormulaTextbox.Text = FormulaTextbox.Text.Remove(FormulaTextbox.Text.Length - 1, 1);
                    }
                    refocusTextbox();
                    break;

                case InputState.SecondSymbol:
                    if (FormulaTextbox.Text[FormulaTextbox.Text.Length - 1] != '~')
                    {
                        FormulaTextbox.Text += "~";
                    }
                    else
                    {
                        FormulaTextbox.Text = FormulaTextbox.Text.Remove(FormulaTextbox.Text.Length - 1, 1);
                    }
                    refocusTextbox();
                    break;

                default:
                    break;
            }

        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            if(state== InputState.FirstSymbol)
            {
                FormulaTextbox.Text = FormulaTextbox.Text[FormulaTextbox.Text.Length - 1] == '~'
                                ? FormulaTextbox.Text.Remove(FormulaTextbox.Text.Length - 3, 3)
                                : FormulaTextbox.Text.Remove(FormulaTextbox.Text.Length - 2, 2);

                var solvability = solver.Perform2SAT(FormulaTextbox.Text, out results);

                state = InputState.Solved;

                if(solvability)
                {
                    resultsTextBlock.Text = "Formula is solvable for: \n";

                    foreach (KeyValuePair<string, int> entry in solver.namesInd)
                    {
                        resultsTextBlock.Text += entry.Key + "=" + results[entry.Value] + ", ";
                    }

                }
                else
                {
                    resultsTextBlock.Text = "Formula is not solvable! \n";
                }

            }
        }

        private void refocusTextbox()
        {
            FormulaTextbox.Focus();
            FormulaTextbox.CaretIndex = FormulaTextbox.Text.Length;
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            FormulaTextbox.Text = "(";
            resultsTextBlock.Text = "";
            state = InputState.Start;
        }
    }
}

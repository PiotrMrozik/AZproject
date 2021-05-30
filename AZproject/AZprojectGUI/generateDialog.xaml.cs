using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AZprojectGUI
{
    /// <summary>
    /// Interaction logic for generateDialog.xaml
    /// </summary>
    public partial class generateDialog : Window
    {
        Regex regex = new Regex("[^0-9]+");

        public generateDialog()
        {
            InitializeComponent();
        }

        private void symbolsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            
            e.Handled = regex.IsMatch(e.Text);
        }

        private void clausesTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            string result = Generator.GenerateFormula(Int32.Parse(symbolsTextBox.Text), Int32.Parse(clausesTextBox.Text));
            ((MainWindow)Application.Current.MainWindow).FormulaTextbox.Text = result + "&(";
            ((MainWindow)Application.Current.MainWindow).state = InputState.FirstSymbol;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

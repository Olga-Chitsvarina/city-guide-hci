using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CityAttractionsAndEvents
{
    /// <summary>
    /// Interaction logic for AskQuestion.xaml
    /// </summary>
    public partial class AskQuestion : UserControl
    {
        static Boolean boxClicked = false;
        public AskQuestion()
        {
            InitializeComponent();
            askTextBox.GotFocus += onSearchFocus;
            submitQuestButton.Click += SubmitQuestButton_Click;
            this.Height = 40;
        }

        private void SubmitQuestButton_Click(object sender, RoutedEventArgs e)
        {
            ((InfoExpander)((Canvas)((StackPanel)this.Parent).Parent).Parent).askQuestion(askTextBox.Text);
        }

        private void onSearchFocus(object sender, RoutedEventArgs e)
        {
            if (!boxClicked)
            {
                this.askTextBox.Text = "";
                this.askTextBox.TextAlignment = TextAlignment.Right;
                boxClicked = true;
            }
        }
    }
}

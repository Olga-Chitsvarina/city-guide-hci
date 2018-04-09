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
    /// Interaction logic for AnswetQuestion.xaml
    /// </summary>
    public partial class AnswerQuestion : UserControl
    {
        static Boolean boxClicked = false;
        public AnswerQuestion(String question)
        {
            InitializeComponent();
            quesText.Text = question;
            ansTextBox.GotFocus += onSearchFocus;
            submitAnswerButton.Click += SubmitAnswerButton_Click;
            this.Height = 130;
        }

        private void SubmitAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            ((InfoExpander)((Canvas)((StackPanel)this.Parent).Parent).Parent).answerQuestion(quesText.Text, ansTextBox.Text);
        }

        private void onSearchFocus(object sender, RoutedEventArgs e)
        {
            if (!boxClicked)
            {
                this.ansTextBox.Text = "";
                this.ansTextBox.TextAlignment = TextAlignment.Right;
                boxClicked = true;
            }
        }
    }
}

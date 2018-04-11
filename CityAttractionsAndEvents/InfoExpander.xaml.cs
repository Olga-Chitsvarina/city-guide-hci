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
    /// Interaction logic for InfoExpander.xaml
    /// </summary>
    public partial class InfoExpander : UserControl
    {
        List<InfoAnswered> answeredQuestions;
        List<AnswerQuestion> questionsAsked;
        Boolean expanded = false;
        public InfoExpander(string type)
        {
            InitializeComponent();
            answeredQuestions = new List<InfoAnswered>();
            questionsAsked = new List<AnswerQuestion>();
            backdropRect.Height = 56;
            infoTypeText.Text = type;
            expandInfoButton.Click += ExpandInfoButton_Click;
            for (int i = 0; i < 4; i++)
            {
                InfoAnswered ia = new InfoAnswered();
                expandedCanvas.Children.Add(ia);
                answeredQuestions.Add(ia);
            }
            AskQuestion aq = new AskQuestion();
            expandedCanvas.Children.Add(aq);
        }

        private void ExpandInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (expanded)
            {
                backdropRect.Height = 56;
                this.Height = 56;
                expandInfoButton.Content = "+";
                expandedCanvas.Visibility = Visibility.Hidden;
                expanded = false;
            } else
            {
                backdropRect.Height = 56 + (70*expandedCanvas.Children.Count);
                this.Height = 56 + (50 * expandedCanvas.Children.Count);
                expandedCanvas.Visibility = Visibility.Visible;
                expandInfoButton.Content = "-";
                expanded = true;
            }
            ((MainWindow)((Canvas)((Canvas)((ScrollViewer)((DockPanel)((StackPanel)this.Parent).Parent).Parent).Parent).Parent).Parent).renderPlaceProfile();
        }

        public void askQuestion(String question)
        {
            AnswerQuestion aq = new AnswerQuestion(question);
            questionsAsked.Add(aq);
            updateElements();
        }

        public void answerQuestion(String question, String answer)
        {
            InfoAnswered ia = new InfoAnswered();
            ia.answerText.Text = answer;
            ia.questionText.Text = question;
            answeredQuestions.Add(ia);
            List<AnswerQuestion> tempAQ = new List<AnswerQuestion>();
            foreach(AnswerQuestion aq in questionsAsked)
            {
                tempAQ.Add(aq);
            }
            foreach(AnswerQuestion aq in tempAQ)
            {
                if (aq.quesText.Text == question)
                {
                    questionsAsked.Remove(aq);
                }
            }
            updateElements();
        }

        private void updateElements()
        {
            expandedCanvas.Children.Clear();
            foreach (InfoAnswered ia in answeredQuestions)
            {
                expandedCanvas.Children.Add(ia);
            }
            foreach (AnswerQuestion anq in questionsAsked)
            {
                expandedCanvas.Children.Add(anq);
            }
            AskQuestion aq = new AskQuestion();
            expandedCanvas.Children.Add(aq);
        }
    }
}

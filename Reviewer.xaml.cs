using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // For ObservableCollection
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace QuizMaker
{
    public partial class Reviewer : ContentPage
    {
        // Define a class to hold the Question and Answer
        public class QuestionAnswer
        {
            public string Question { get; set; }
            public string Answer { get; set; }
        }

        // ObservableCollection to automatically update the UI
        public ObservableCollection<QuestionAnswer> QuestionsAndAnswers { get; set; } = new ObservableCollection<QuestionAnswer>();

        // Parameterless constructor
        public Reviewer()
        {
            InitializeComponent();
            BindingContext = this;  // Set BindingContext for data binding
            LoadQuestionsFromDatabase(); // Fetch questions and answers from the database
        }

        // Method to load questions and answers from the database
        private async Task LoadQuestionsFromDatabase()
        {
            try
            {
                // Show the loading indicator
                Device.BeginInvokeOnMainThread(() =>
                {
                    LoadingIndicator.IsRunning = true;
                    LoadingIndicator.IsVisible = true;
                });

                // Assuming the service method GetEntities() returns a list of questions
                var service = new Services();
                var questionsAndAnswers = await service.GetEntities();  // Fetch data from the database

                // Clear existing data and load new questions
                QuestionsAndAnswers.Clear();

                if (questionsAndAnswers != null && questionsAndAnswers.Count > 0)
                {
                    foreach (var item in questionsAndAnswers)
                    {
                        QuestionsAndAnswers.Add(new QuestionAnswer
                        {
                            Question = item.Question,
                            Answer = item.Answer ?? "No answer provided"
                        });
                    }
                }
                else
                {
                    QuestionsAndAnswers.Add(new QuestionAnswer
                    {
                        Question = "No questions found in the database",
                        Answer = "No answers available"
                    });
                }

                // Update UI on the main thread after loading data
                Device.BeginInvokeOnMainThread(() =>
                {
                    LoadingIndicator.IsRunning = false;
                    LoadingIndicator.IsVisible = false;
                });
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the data fetching process
                Device.BeginInvokeOnMainThread(() =>
                {
                    LoadingIndicator.IsRunning = false;
                    LoadingIndicator.IsVisible = false;
                    DisplayAlert("Error", "An error occurred while loading data: " + ex.Message, "OK");
                });
            }
        }

        // Refresh Button Click Handler - Reloads the questions from the database
        private async void RefreshBtn_Clicked(object sender, EventArgs e)
        {
            // Optionally, show loading indicator here
            await LoadQuestionsFromDatabase();  // Reload the questions from the database
        }
    }
}

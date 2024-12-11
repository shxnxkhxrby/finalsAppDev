using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace QuizMaker
{
    public partial class Answer : ContentPage
    {
        private List<Entry> answerEntries = new List<Entry>();  // List to hold the entry fields for answers
        private List<string> correctAnswers = new List<string>(); // List to store the correct answers
        private List<Label> resultLabels = new List<Label>(); // List to store the result labels (correct/incorrect)

        public Answer()
        {
            InitializeComponent();
            LoadQuestions();  // Load questions when the page is created
        }

        // Change the return type of LoadQuestions to Task
        private async Task LoadQuestions()
        {
            var service = new Services();
            var questionsAndAnswers = await service.GetEntities();

            if (questionsAndAnswers == null || questionsAndAnswers.Count == 0)
            {
                await DisplayAlert("No Questions", "There are no questions available to answer.", "OK");
                return;
            }

            // Clear previous questions, answers, and results if any
            QuestionAnswerStackLayout.Children.Clear();
            answerEntries.Clear();
            correctAnswers.Clear();
            resultLabels.Clear();

            // Load the questions dynamically
            foreach (var item in questionsAndAnswers)
            {
                var questionLabel = new Label
                {
                    Text = $"Question: {item.Question}",
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    TextColor = Color.FromArgb("#000000")
                };

                var frame = new Frame
                {
                    Padding = new Thickness(10),
                    CornerRadius = 20,
                    BackgroundColor = Color.FromArgb("#FFFFFF"),
                    Content = new Entry
                    {
                        Placeholder = "Your answer here",
                        TextColor = Color.FromArgb("#000000"),
                        HorizontalTextAlignment = TextAlignment.Center,
                        HeightRequest = 45,
                        Margin = new Thickness(0, 5)
                    }
                };

                var resultLabel = new Label
                {
                    FontSize = 16,
                    TextColor = Color.FromArgb("#00796b"),
                    IsVisible = false
                };

                QuestionAnswerStackLayout.Children.Add(questionLabel);
                QuestionAnswerStackLayout.Children.Add(frame);
                QuestionAnswerStackLayout.Children.Add(resultLabel);

                var answerEntry = (Entry)((Frame)frame).Content;
                answerEntries.Add(answerEntry);
                resultLabels.Add(resultLabel);

                correctAnswers.Add(item.Answer);
            }
        }


        private async void OnSubmitAnswersClicked(object sender, EventArgs e)
        {
            if (answerEntries == null || answerEntries.Count == 0)
            {
                await DisplayAlert("No Answers", "Please answer all the questions before submitting.", "OK");
                return; // Exit if there are no answers
            }

            int score = 0; // Variable to track the user's score

            // Check all user-provided answers
            for (int i = 0; i < answerEntries.Count; i++)
            {
                var userAnswer = answerEntries[i]?.Text?.Trim();  // Safe null check for user answer
                var correctAnswer = correctAnswers[i]?.Trim();   // Safe null check for correct answer

                // Check if userAnswer is empty or null and treat it as incorrect
                if (string.IsNullOrEmpty(userAnswer))
                {
                    resultLabels[i].Text = "Incorrect"; // Show "Incorrect" if no answer is provided
                    resultLabels[i].TextColor = Color.FromArgb("#F44336"); // Red color for Incorrect
                }
                else if (userAnswer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    score++;  // Increment score if the answer is correct
                    resultLabels[i].Text = "Correct";  // Show "Correct"
                    resultLabels[i].TextColor = Color.FromArgb("#00ff00"); // Green color for Correct
                }
                else
                {
                    resultLabels[i].Text = "Incorrect"; // Show "Incorrect" if the answer is wrong
                    resultLabels[i].TextColor = Color.FromArgb("#F44336"); // Red color for Incorrect
                }

                // Make the result label visible after submission
                resultLabels[i].IsVisible = true;
            }

            // Display the user's score
            ScoreLabel.Text = $"Your Score: {score}/{answerEntries.Count}";

            // Show "View Results" button after submission
            ViewResultBtn.IsVisible = true;
            SubmitBtn.IsVisible = false;
            AnswerAgainBtn.IsVisible = true;
        }


        private async void OnViewResultsClicked(object sender, EventArgs e)
        {
            var resultSummary = string.Empty;

            for (int i = 0; i < resultLabels.Count; i++)
            {
                var question = ((Label)QuestionAnswerStackLayout.Children[i * 3]).Text;
                var result = resultLabels[i].Text;

                resultSummary += $"{question}: {result}\n";
            }

            await DisplayAlert("Your Results", resultSummary, "OK");
        }

        private async void OnAnswerAgainClicked(object sender, EventArgs e)
        {
            // Reset the answers and results
            for (int i = 0; i < answerEntries.Count; i++)
            {
                answerEntries[i].Text = string.Empty;
                resultLabels[i].IsVisible = false;
                resultLabels[i].Text = string.Empty;
            }

            ScoreLabel.Text = string.Empty;

            // Hide the results and answer buttons, and show the Submit button
            SubmitBtn.IsVisible = true;
            AnswerAgainBtn.IsVisible = false;
            ViewResultBtn.IsVisible = false;

            // Reload the questions again
            await LoadQuestions();  // Await the LoadQuestions method to reload the questions asynchronously
        }
    }
}

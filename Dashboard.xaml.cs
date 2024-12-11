namespace QuizMaker
{
    public partial class Dashboard : ContentPage
    {
        public List<Entry> createdQuestions = new List<Entry>();
        public List<Entry> createdAnswers = new List<Entry>();

        public Dashboard(List<string> questions)
        {
            InitializeComponent();

            // Loop through the questions and dynamically create frames for each one
            for (int i = 0; i < questions.Count; i++)
            {
                // Create a VerticalStackLayout for each question-answer pair
                var questionLayout = new VerticalStackLayout
                {
                    Spacing = 10,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };

                // Create a label for the question number
                var label1 = new Label
                {
                    Text = $"{i + 1}.",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Color.FromArgb("#000000"),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start
                };

                // Create an entry for the question text
                var entry1 = new Entry
                {
                    Placeholder = $"Question {i + 1}",
                    BackgroundColor = Color.FromArgb("#00000000"),
                    HorizontalTextAlignment = TextAlignment.Center,
                    WidthRequest = 200,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    PlaceholderColor = Color.FromArgb("#E0E0E0")
                };

                // Create an entry for the answer text
                var entry2 = new Entry
                {
                    Placeholder = $"Answer to question {i + 1}",
                    BackgroundColor = Color.FromArgb("#00000000"),
                    HorizontalTextAlignment = TextAlignment.Center,
                    WidthRequest = 200,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    PlaceholderColor = Color.FromArgb("#E0E0E0")
                };

                // Add the label and entries to the layout
                questionLayout.Children.Add(label1);
                questionLayout.Children.Add(entry1);
                questionLayout.Children.Add(entry2);

                // Add the question layout to the container
                QuestionsContainer.Children.Add(questionLayout);

                // Save the entries for later reference
                createdQuestions.Add(entry1);
                createdAnswers.Add(entry2);
            }
        }

        private async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            var questions = new List<string>();
            var answers = new List<string>();

            bool allFieldsFilled = true;  // Flag to check if all fields are filled

            // Collect all questions and answers from the dynamic entry fields
            foreach (var qentry in createdQuestions)
            {
                if (string.IsNullOrWhiteSpace(qentry.Text)) // Check if question is empty
                {
                    allFieldsFilled = false;
                    break;
                }
                questions.Add(qentry.Text);
            }

            foreach (var aentry in createdAnswers)
            {
                if (string.IsNullOrWhiteSpace(aentry.Text)) // Check if answer is empty
                {
                    allFieldsFilled = false;
                    break;
                }
                answers.Add(aentry.Text);
            }

            if (!allFieldsFilled)
            {
                // Show an alert if any field is empty
                await DisplayAlert("Empty fields", "Please fill out all questions and answers.", "OK");
            }
            else
            {
                // Save the collected questions and answers
                var service = new Services();
                await service.SaveQuestionsAndAnswers(questions, answers);
                await Navigation.PushAsync(new Answer());
            }
        }


        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page
            Navigation.PopAsync();
        }
    }
}

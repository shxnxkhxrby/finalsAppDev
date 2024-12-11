namespace QuizMaker
{
    public partial class CreateQuiz : ContentPage
    {
        // Store questions in a shared list
        public List<string> Questions { get; set; } = new List<string>();

        public CreateQuiz()
        {
            InitializeComponent();
            UpdateExistingQuestionsLabel(); // Update label on page load
        }

        private async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(NumberOfQuestions.Text))
            {
                await DisplayAlert("Error", "Entries must be filled", "OK");
                return;
            }

            if (!int.TryParse(NumberOfQuestions.Text, out int result))
            {
                await DisplayAlert("Error", "Please input a valid number", "OK");
                return;
            }

            if (result > 50)
            {
                await DisplayAlert("Error", "The maximum number of questions is 50", "OK");
                return;
            }

            Questions.Clear();
            int numberOfQuestions = int.Parse(NumberOfQuestions.Text);

            // Generate mock data if no questions exist in the database
            for (int i = 0; i < numberOfQuestions; i++)
            {
                Questions.Add($"Mock Question {i + 1}");
            }

            // Now update the label with the number of questions in the database
            UpdateExistingQuestionsLabel();

            // Pass the Questions list to the Dashboard page
            await Navigation.PushAsync(new Dashboard(Questions));
        }


        private void Clear(object sender, EventArgs e)
        {
            NumberOfQuestions.Text = "";
            Questions.Clear(); // Reset the questions list
            UpdateExistingQuestionsLabel(); // Update label when clearing questions
        }

        private void AddNumber(object sender, EventArgs e)
        {
            if (int.TryParse(NumberOfQuestions.Text, out int number))
            {
                number--;
                if (number >= 0)
                {
                    NumberOfQuestions.Text = number.ToString();
                }
            }

            UpdateExistingQuestionsLabel(); // Update label after changing number
        }

        private void SubNumber(object sender, EventArgs e)
        {
            if (int.TryParse(NumberOfQuestions.Text, out int number))
            {
                number++;
                NumberOfQuestions.Text = number.ToString();
            }

            UpdateExistingQuestionsLabel(); // Update label after changing number
        }

        private async void UpdateExistingQuestionsLabel()
        {
            var service = new Services();
            var questionsAndAnswers = await service.GetEntities();  // Retrieve saved questions from SQLite
            ExistingQuestionsLabel.Text = $"{questionsAndAnswers.Count}";
        }
    }
}

namespace QuizMaker
{
    public partial class EditQuiz : ContentPage
    {
        private List<Entity> allQuestionsAndAnswers;
        private Services service;

        public EditQuiz()
        {
            InitializeComponent();
            service = new Services();
        }

        // Override OnAppearing to reload the questions when the page appears
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadQuestionsAndAnswers();
        }

        // Load questions and answers from the database
        private async Task LoadQuestionsAndAnswers()
        {
            allQuestionsAndAnswers = await service.GetEntities();  // Fetch questions from SQLite
            QuestionsCollectionView.ItemsSource = allQuestionsAndAnswers;  // Update CollectionView data
        }

        // Handle the update button click
        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var entity = button?.BindingContext as Entity;
            if (entity != null)
            {
                var questionEntry = (button?.Parent.FindByName<Entry>("QuestionEntry"));
                var answerEntry = (button?.Parent.FindByName<Entry>("AnswerEntry"));

                // Update entity and database
                entity.Question = questionEntry.Text;
                entity.Answer = answerEntry.Text;

                await service.Update(entity);
                await LoadQuestionsAndAnswers(); // Refresh list after update
            }
        }

        // Handle the delete button click
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var entity = button?.BindingContext as Entity;
            if (entity != null)
            {
                await service.Delete(entity);
                await LoadQuestionsAndAnswers();  // Refresh list after deletion
            }
        }
    }
}

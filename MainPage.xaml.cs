namespace QuizMaker
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

        }


        private async void Create(object sender, EventArgs e)
        {
            // Use Shell navigation to go to the CreateQuiz page
            await Shell.Current.GoToAsync("//CreateQuiz");
        }


        private async void Exit(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Exit", "Are you sure you want to exit?", "Yes", "No");
            if (confirm)
            {
                if (Application.Current != null)
                {
                    Application.Current.Quit();
                }

            }
        }
    }

}

using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.App.Views.QuestionViews;

public partial class BooleanQuestionView : ContentView
{
    private readonly Action<int> _answerSelectedCallback;

    private readonly string _groupName =
    Guid.NewGuid().ToString();


    public BooleanQuestionView(
        QuizQuestion question,
        Action<int> answerSelectedCallback)
    {
        InitializeComponent();

        _answerSelectedCallback =
            answerSelectedCallback
            ?? throw new ArgumentNullException(nameof(answerSelectedCallback));

        Render(question);
    }

    private void Render(QuizQuestion question)
    {
        QuestionLabel.Text = question.QuestionText;

        OptionsContainer.Children.Clear();

        for (int i = 0; i < question.Options.Count; i++)
        {
            var index = i;

            var button = new RadioButton
            {
                Content = question.Options[i],
                GroupName = _groupName,
                FontSize = 18
            };

            button.CheckedChanged += (_, args) =>
            {
                if (args.Value)
                {
                    _answerSelectedCallback(index);
                }
            };

            OptionsContainer.Children.Add(button);
        }
    }
}

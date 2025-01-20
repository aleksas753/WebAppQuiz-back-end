using WebAppQuiz.Data;
using System.Text.Json.Serialization;

namespace WebAppQuiz.Data
{
    public class MultiAnswerQuestion : Question
    {
        private List<Option> _options = new List<Option>();
        private List<Answer> _answers = new List<Answer>();

        public ICollection<Option> Options => _options;
        public ICollection<Answer> Answers => _answers;

        public MultiAnswerQuestion() : base() { }

        public MultiAnswerQuestion(int id, string description, QuestionType type)
            : base(id, description, type)
        {

        }

        public int GetAnswerCount()
        {
            return Answers?.Count ?? 0;
        }

        public void AddOptions(string option)
        {
            Option newOption = new Option(option);
            _options.Add(newOption);
        }

        public void AddAnswer(string answer)
        {
            Option newOption = new Option(answer);
            _options.Add(newOption);
            Answer newAnswer = new Answer(answer);
            _answers.Add(newAnswer);
        }

        public override int Accept(IQuestionVisitor visitor, object userAnswer)
        {
            return visitor.Visit(this, userAnswer as List<string>);
        }
    }
}

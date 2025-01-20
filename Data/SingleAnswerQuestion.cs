namespace WebAppQuiz.Data
{
    public class SingleAnswerQuestion : Question
    {
        private List<Option> _options = new List<Option>();

        public IReadOnlyList<Option> Options => _options.AsReadOnly();

        //public IReadOnlyList<string> Options => _options.AsReadOnly();
        public Answer CorrectAnswer { get; set; }

        public SingleAnswerQuestion() : base() { }

        public SingleAnswerQuestion(int id, string description, QuestionType type, string anwser) : base(id, description, type)
        {

            this.CorrectAnswer = new Answer(anwser);
            _options.Add(new Option(anwser));
        }

        public void AddOptions(string option)
        {
            Option newOption = new Option(option);
            _options.Add(newOption);
        }

        public override int Accept(IQuestionVisitor visitor, object userAnswer)
        {
            if (userAnswer != null)
            {
                return visitor.Visit(this, userAnswer as string);
            }
            return 0;
        }


    }
}

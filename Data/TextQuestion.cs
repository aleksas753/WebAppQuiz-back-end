namespace WebAppQuiz.Data
{
    public class TextQuestion : Question
    {
        public string CorrectAnswer { get; set; }

        public TextQuestion() : base() { }

        public TextQuestion(int id, string description, QuestionType type, string Anwser) : base(id, description, type)
        {
            this.CorrectAnswer = Anwser;
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

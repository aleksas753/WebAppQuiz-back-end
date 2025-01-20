namespace WebAppQuiz.Data
{
    public interface IQuestionVisitor
    {
        int Visit(SingleAnswerQuestion question, string userAnswer);
        int Visit(MultiAnswerQuestion question, List<string> userAnswer);

        int Visit(TextQuestion question, string userAnswer);
    }
}

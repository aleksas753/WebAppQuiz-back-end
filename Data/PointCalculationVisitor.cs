using WebAppQuiz.Data;

namespace WebAppQuiz.Data
{
    public class PointCalculationVisitor : IQuestionVisitor
    {
        public int Visit(SingleAnswerQuestion question, string userAnswer)
        {
            return question.CorrectAnswer.Value.Equals(userAnswer, StringComparison.OrdinalIgnoreCase) ? question.MaxValue : 0;
        }

        public int Visit(MultiAnswerQuestion question, List<string> userAnswer)
        {
            var answers = question.Answers.Select(answer => answer.Value).ToList();
            int goodAnswers = question.GetAnswerCount();
            if (question.GetAnswerCount() == 0)
                throw new InvalidOperationException("The correct answers list cannot be empty.");
            

            // Count correctly checked answers
            int correctlyChecked = userAnswer.Intersect(answers).Count();

            // Calculate the score
            double rawScore = (100.0 / goodAnswers) * correctlyChecked;

            // Round up and return
            return (int)Math.Ceiling(rawScore);

        }

        public int Visit(TextQuestion question, string userAnswer)
        {
            return question.CorrectAnswer.Equals(userAnswer, StringComparison.OrdinalIgnoreCase) ? question.MaxValue : 0;
        }

    }
}

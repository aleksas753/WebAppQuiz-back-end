using System.ComponentModel.DataAnnotations;

namespace WebAppQuiz.Data
{
    public class HighScore
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public int Score { get; set; }

        public DateTime StartDate { get; set; }

        public HighScore() { }

        public HighScore(string email, int score, DateTime dateTime)
        {
            Email = email;
            Score = score;
            StartDate = dateTime;
        }
    }

    public record HighScoreDto(string email, Dictionary<int, Object> answers);
}

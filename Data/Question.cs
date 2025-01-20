using System.Numerics;
using System.Text.Json.Serialization;

namespace WebAppQuiz.Data
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$Type")]
    [JsonDerivedType(typeof(SingleAnswerQuestion), "SingleAnswerQuestion")]
    [JsonDerivedType(typeof(MultiAnswerQuestion), "MultiAnswerQuestion")]
    [JsonDerivedType(typeof(TextQuestion), "TextQuestion")]
    public abstract class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public QuestionType Type { get; set; }

        public int MaxValue = 100;


        public Question() { }

        public Question(int id, string description, QuestionType type)
        {
            this.Id = id;
            this.Description = description;
            this.Type = type;
        }

        //public abstract int CheckAnswer(T answer);
        public abstract int Accept(IQuestionVisitor visitor, object userAnswer);

    }

    public enum QuestionType
    {
        Single,
        Multi,
        Text
    }
}

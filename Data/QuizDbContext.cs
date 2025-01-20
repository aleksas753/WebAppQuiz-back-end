using Microsoft.EntityFrameworkCore;

namespace WebAppQuiz.Data
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

        public DbSet<HighScore> HighScores { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure discriminator for the Question hierarchy
            modelBuilder.Entity<Question>()
                .HasDiscriminator<QuestionType>("Type") // Discriminator column name
                .HasValue<SingleAnswerQuestion>(QuestionType.Single)
                .HasValue<MultiAnswerQuestion>(QuestionType.Multi)
                .HasValue<TextQuestion>(QuestionType.Text);

            // Owned entities for MultiAnswerQuestion
            modelBuilder.Entity<MultiAnswerQuestion>(builder =>
            {
                builder.OwnsMany(q => q.Options, opt =>
                {
                    opt.WithOwner().HasForeignKey("QuestionId");
                    opt.Property<int>("Id").ValueGeneratedOnAdd();
                    opt.HasKey("Id");
                });

                builder.OwnsMany(q => q.Answers, ans =>
                {
                    ans.WithOwner().HasForeignKey("QuestionId");
                    ans.Property<int>("Id").ValueGeneratedOnAdd();
                    ans.HasKey("Id");
                });
            });

            // Owned entities for SingleAnswerQuestion
            modelBuilder.Entity<SingleAnswerQuestion>(builder =>
            {
                builder.OwnsMany(q => q.Options, opt =>
                {
                    opt.WithOwner().HasForeignKey("QuestionId");
                    opt.Property<int>("Id").ValueGeneratedOnAdd();
                    opt.HasKey("Id");
                });

                builder.OwnsOne(q => q.CorrectAnswer, ans =>
                {
                    ans.WithOwner().HasForeignKey("QuestionId");
                    ans.Property<int>("Id").ValueGeneratedOnAdd();
                    ans.HasKey("Id");
                });
            });

            modelBuilder.Entity<SingleAnswerQuestion>()
                .OwnsMany(q => q.Options).Property(o => o.Value);

            modelBuilder.Entity<SingleAnswerQuestion>()
                .OwnsOne(q => q.CorrectAnswer).Property(o => o.Value);

            // Seed SingleAnswerQuestions
            modelBuilder.Entity<SingleAnswerQuestion>().HasData(
                new { Id = 1, Description = "What is the capital of France?", Type = QuestionType.Single },
                new { Id = 2, Description = "Which planet is known as the Red Planet?", Type = QuestionType.Single },
                new { Id = 3, Description = "What is the largest ocean on Earth?", Type = QuestionType.Single },
                new { Id = 4, Description = "Who wrote the play 'Romeo and Juliet'?", Type = QuestionType.Single }
            );

            // Seeding SingleAnswerQuestions answers
            modelBuilder.Entity<SingleAnswerQuestion>()
                .OwnsOne(q => q.CorrectAnswer).HasData(
                    new { Id = 1, QuestionId = 1, Value = "Paris" },
                    new { Id = 2, QuestionId = 2, Value = "Mars" },
                    new { Id = 3, QuestionId = 3, Value = "Pacific Ocean" },
                    new { Id = 4, QuestionId = 4, Value = "William Shakespeare" }
                );

            // Seeding SingleAnswerQuestions options
            modelBuilder.Entity<SingleAnswerQuestion>()
                .OwnsMany(q => q.Options).HasData(
                    new { Id = 5, QuestionId = 1, Value = "Berlin" },
                    new { Id = 6, QuestionId = 1, Value = "Madrid" },
                    new { Id = 7, QuestionId = 1, Value = "Paris" },
                    new { Id = 8, QuestionId = 1, Value = "Rome" },
                    

                    new { Id = 9, QuestionId = 2, Value = "Earth" },
                    new { Id = 10, QuestionId = 2, Value = "Venus" },
                    new { Id = 11, QuestionId = 2, Value = "Jupiter" },
                    new { Id = 12, QuestionId = 2, Value = "Mars" },

                    new { Id = 13, QuestionId = 3, Value = "Pacific Ocean" },
                    new { Id = 14, QuestionId = 3, Value = "Atlantic Ocean" },
                    new { Id = 15, QuestionId = 3, Value = "Indian Ocean" },
                    new { Id = 16, QuestionId = 3, Value = "Arctic Ocean" },

                    new { Id = 18, QuestionId = 4, Value = "Charles Dickens" },
                    new { Id = 19, QuestionId = 4, Value = "George Orwell" },
                    new { Id = 20, QuestionId = 4, Value = "Mark Twain" },
                    new { Id = 21, QuestionId = 4, Value = "William Shakespeare" }
                );

            // Seeding TextQuestions
            var textQuestion1 = new TextQuestion(5, "What is the largest continent?", QuestionType.Text, "Asia");
            modelBuilder.Entity<TextQuestion>().HasData(textQuestion1);

            var textQuestion2 = new TextQuestion(6, "Which country has the largest population?", QuestionType.Text, "China");
            modelBuilder.Entity<TextQuestion>().HasData(textQuestion2);

            // Seed MultiAnswerQuestions
            modelBuilder.Entity<MultiAnswerQuestion>().HasData(
                new { Id = 7, Description = "Which of these are fruits?", Type = QuestionType.Multi },
                new { Id = 8, Description = "Which animals are mammals?", Type = QuestionType.Multi },
                new { Id = 9, Description = "Which of these are programming languages?", Type = QuestionType.Multi },
                new { Id = 10, Description = "Which of these are car brands?", Type = QuestionType.Multi }
            );

            // Seed Options
            modelBuilder.Entity<MultiAnswerQuestion>()
                .OwnsMany(q => q.Options).HasData(
                    // Fruits (Question 7)
                    new { Id = 1, QuestionId = 7, Value = "Apple" },
                    new { Id = 2, QuestionId = 7, Value = "Banana" },
                    new { Id = 4, QuestionId = 7, Value = "Carrot" },
                    new { Id = 5, QuestionId = 7, Value = "Cucumber" },
                    new { Id = 3, QuestionId = 7, Value = "Orange" },

                    // Mammals (Question 8)
                    new { Id = 9, QuestionId = 8, Value = "Fish" },
                    new { Id = 6, QuestionId = 8, Value = "Dog" },
                    new { Id = 7, QuestionId = 8, Value = "Cat" },
                    new { Id = 10, QuestionId = 8, Value = "Bird" },
                    new { Id = 8, QuestionId = 8, Value = "Elephant" },

                    // Programming Languages (Question 9)
                    new { Id = 11, QuestionId = 9, Value = "C#" },
                    new { Id = 12, QuestionId = 9, Value = "Python" },
                    new { Id = 15, QuestionId = 9, Value = "Magic Draw" },
                    new { Id = 13, QuestionId = 9, Value = "Java" },
                    new { Id = 14, QuestionId = 9, Value = "Visual Studio Code" },

                    // Car brands (Question 10)
                    new { Id = 16, QuestionId = 10, Value = "Avensis " },
                    new { Id = 18, QuestionId = 10, Value = "Prius" },
                    new { Id = 19, QuestionId = 10, Value = "Lenovo" },
                    new { Id = 17, QuestionId = 10, Value = "Volkswagen" },
                    new { Id = 20, QuestionId = 10, Value = "Passat" }
                );

            // Seed Answers
            modelBuilder.Entity<MultiAnswerQuestion>()
                .OwnsMany(q => q.Answers).HasData(
                    // Fruits (Question 7)
                    new { Id = 1, QuestionId = 7, Value = "Apple" },
                    new { Id = 2, QuestionId = 7, Value = "Banana" },
                    new { Id = 3, QuestionId = 7, Value = "Orange" },

                    // Mammals (Question 8)
                    new { Id = 4, QuestionId = 8, Value = "Dog" },
                    new { Id = 5, QuestionId = 8, Value = "Cat" },
                    new { Id = 6, QuestionId = 8, Value = "Elephant" },

                    // Programming Languages (Question 9)
                    new { Id = 7, QuestionId = 9, Value = "C#" },
                    new { Id = 8, QuestionId = 9, Value = "Python" },
                    new { Id = 9, QuestionId = 9, Value = "Java" },

                    // Car brands (Question 10)
                    new { Id = 10, QuestionId = 10, Value = "Volkswagen" }
                );

            // Seed HighScores
            modelBuilder.Entity<HighScore>().HasData(
                new HighScore { Id = 1, Email = "test1@gmail.com", Score = 0, StartDate = DateTime.UtcNow.AddDays(-10) },
                new HighScore { Id = 2, Email = "test2@gmail.com", Score = 100, StartDate = DateTime.UtcNow.AddDays(-9) },
                new HighScore { Id = 3, Email = "test3@gmail.com", Score = 200, StartDate = DateTime.UtcNow.AddDays(-8) },
                new HighScore { Id = 4, Email = "test4@gmail.com", Score = 300, StartDate = DateTime.UtcNow.AddDays(-7) },
                new HighScore { Id = 5, Email = "test5@gmail.com", Score = 400, StartDate = DateTime.UtcNow.AddDays(-6) },
                new HighScore { Id = 6, Email = "test6@gmail.com", Score = 500, StartDate = DateTime.UtcNow.AddDays(-5) },
                new HighScore { Id = 7, Email = "test7@gmail.com", Score = 600, StartDate = DateTime.UtcNow.AddDays(-4) },
                new HighScore { Id = 8, Email = "test8@gmail.com", Score = 700, StartDate = DateTime.UtcNow.AddDays(-3) },
                new HighScore { Id = 9, Email = "test9@gmail.com", Score = 800, StartDate = DateTime.UtcNow.AddDays(-2) },
                new HighScore { Id = 10, Email = "test10@gmail.com", Score = 900, StartDate = DateTime.UtcNow.AddDays(-1) }
            );

        }
    }

}

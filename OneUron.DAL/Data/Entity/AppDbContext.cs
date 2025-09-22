using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Token> Tokens { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<UserQuizAttempt> UserQuizAttempts { get; set; }

        public DbSet<QuestionChoice> QuestionChoices { get; set; }

        public DbSet<QuizHistory> QuizHistories { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<EnRoll> Enrollments { get; set; }

        public DbSet<CourseDetail> CourseDetails { get; set; }

        public DbSet<Acknowledge> Acknowledges { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<Method> Methods { get; set; }

        public DbSet<StudyMethod> StudyMethods { get; set; }

        public DbSet<MethodPro> MethodPros {  get; set; } 

        public DbSet<MethodCon> MethodCons { get; set; }

        public DbSet<Technique> Techniques { get; set; }

        public DbSet<MethodRule> MethodRules { get; set; }

        public DbSet<MethodRuleCondition> MethodRuleConditions { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }

        public DbSet<EvaluationQuestion> EvaluationQuestions { get; set; }

        public DbSet<Choice> Choices { get; set; }

        public DbSet<UserAnswer> UserAnswers { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Process> Processes { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<ProcessTask> ProcessTasks { get; set; }

        public DbSet<MemberShip> MemberShips { get; set; }

        public DbSet<MemberShipPlan> MemberShipPlans { get; set; }

        public DbSet<Features> Features { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quiz>().Property(q => q.Type)
                .HasConversion(new EnumToStringConverter<QuizType>());

            modelBuilder.Entity<Resource>().Property(r => r.Type)
                .HasConversion(new EnumToStringConverter<ResourceType>());

            modelBuilder.Entity<MemberShip>().Property(ms => ms.Status)
                .HasConversion(new EnumToStringConverter<MemberShipStatus>());

            modelBuilder.Entity<Method>().Property(m => m.Difficulty)
                .HasConversion(new EnumToStringConverter<MethodType>());

            modelBuilder.Entity<EvaluationQuestion>().Property(eq => eq.Type)
                .HasConversion(new EnumToStringConverter<EvaluationQuestionType>());

            // one to one User and Token
            modelBuilder.Entity<User>()
                .HasOne(u => u.Token)
                .WithOne(t => t.User)
                .HasForeignKey<Token>(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // one to one User and Profile
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(t => t.User)
                .HasForeignKey<Profile>(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // many to many with User and Quiz
            modelBuilder.Entity<UserQuizAttempt>()
                .HasOne(uqa => uqa.User)
                .WithMany(u => u.UserQuizAttempts)
                .HasForeignKey(uqa => uqa.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Quiz UserQuizAttemps
            modelBuilder.Entity<UserQuizAttempt>()
                .HasOne(uqa => uqa.Quiz)
                .WithMany(q => q.UserQuizAttempts)
                .HasForeignKey(uqa => uqa.QuizId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Quiz and Question 
            modelBuilder.Entity<Question>()
                .HasOne(qt => qt.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(qt => qt.QuizId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Question and QuestionChoice
            modelBuilder.Entity<QuestionChoice>()
                .HasOne(qc => qc.Question)
                .WithMany(q => q.QuestionChoices)
                .HasForeignKey(qc => qc.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many  UserQuizAttempts QuizHistory
            modelBuilder.Entity<QuizHistory>()
                .HasOne(uqa => uqa.UserQuizAttempts)
                .WithMany(qh => qh.QuizHistories)
                .HasForeignKey(qh => qh.UserQuizAttemptId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Question QuizHistory
            modelBuilder.Entity<QuizHistory>()
                .HasOne(q => q.Question)
                .WithMany(qh => qh.QuizHistories)
                .HasForeignKey(qh => qh.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many choice QuizHistory
            modelBuilder.Entity<QuizHistory>()
               .HasOne(qc => qc.Choice)
               .WithMany(qh => qh.QuizHistories)
               .HasForeignKey(qh => qh.ChoiceId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many User Enroll
            modelBuilder.Entity<EnRoll>()
                .HasOne(er => er.User)
                .WithMany(u => u.EnRolls)
                .HasForeignKey(er => er.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Resouce Enroll
            modelBuilder.Entity<EnRoll>()
                .HasOne(er => er.Resource)
                .WithMany(r => r.EnRolls)
                .HasForeignKey(er => er.ResourceId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to one Resource CourseDetail 
            modelBuilder.Entity<Resource>()
                .HasOne(r => r.CourseDetail)
                .WithOne(cd => cd.Resource)
                .HasForeignKey<CourseDetail>(cd => cd.ResourceId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Resource Acknowledge
            modelBuilder.Entity<Acknowledge>()
                .HasOne(al => al.Resource)
                .WithMany(r => r.Acknowledges)
                .HasForeignKey(al => al.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            

            // one to many Resource Skills
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.Resource)
                .WithMany(r => r.Skills)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Resource Instructor
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Resource)
                .WithMany(r => r.Instructors)
                .HasForeignKey(i => i.CourseId)
                .OnDelete(DeleteBehavior.Restrict);


            // One to many User StudyMethod
            modelBuilder.Entity<StudyMethod>()
                .HasOne(sm => sm.User)
                .WithMany(u => u.StudyMethods)
                .HasForeignKey(sm => sm.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to one Method StudyMethod
            modelBuilder.Entity<Method>()
                .HasOne(m => m.StudyMethod)
                .WithOne(sm => sm.Method)
                .HasForeignKey<StudyMethod>(sm => sm.MethodId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to Many Method MethodPro
            modelBuilder.Entity<MethodPro>()
                .HasOne(mp => mp.Method)
                .WithMany(m => m.MethodPros)
                .HasForeignKey(mp => mp.MethodId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Method MethodCon
            modelBuilder.Entity<MethodCon>()
                .HasOne(mc => mc.Method)
                .WithMany(m => m.MethodCons)
                .HasForeignKey (mc => mc.MethodId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Method Technique
            modelBuilder.Entity<Technique>()
                .HasOne(t => t.Method)
                .WithMany(m => m.Techniques)
                .HasForeignKey(t => t.MethodId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Method MethodRule
            modelBuilder.Entity<MethodRule>()
                .HasOne(mr => mr.Method)
                .WithMany(m => m.MethodRules)
                .HasForeignKey(mr => mr.MethodId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many MethodRuleCondition MethodRule
            modelBuilder.Entity<MethodRule>()
                .HasOne(mr => mr.MethodRuleCondition)
                .WithMany(mrc => mrc.MethodRules)
                .HasForeignKey(mr => mr.MethodRuleConditionId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Evaluation MethodRuleCondition
            modelBuilder.Entity<MethodRuleCondition>()
                .HasOne(mrc => mrc.Evaluation)
                .WithMany(e => e.MethodRuleConditions)
                .HasForeignKey(mrc => mrc.EvaluationId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Evaluation EvaluationQuestion
            modelBuilder.Entity<EvaluationQuestion>()
                .HasOne(eq => eq.Evaluation)
                .WithMany(e => e.EvaluationQuestions)
                .HasForeignKey(eq => eq.EvaluationId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many EvaluationQuestion MethodRuleCondition
            modelBuilder.Entity<MethodRuleCondition>()
                .HasOne(mrc => mrc.EvaluationQuestion)
                .WithMany(eq => eq.MethodRuleConditions)
                .HasForeignKey(mrc => mrc.EvaluationQuestionId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Choice MethodRuleCondition
            modelBuilder.Entity<MethodRuleCondition>()
                .HasOne(mrc => mrc.Choice)
                .WithMany(c => c.MethodRuleConditions)
                .HasForeignKey(mrc => mrc.ChoiceId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many EvaluationQuestion Choice
            modelBuilder.Entity<Choice>()
                .HasOne(c => c.EvaluationQuestion)
                .WithMany(eq => eq.Choices)
                .HasForeignKey(c => c.EvaluationQuestionId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<UserAnswer>()
            .HasKey(ua => new { ua.UserId, ua.EvaluationQuestionId, ua.ChoiceId });

            // one to many EvaluationQuestion UserAnswer
            modelBuilder.Entity<UserAnswer>()
                .HasOne(ua => ua.EvaluationQuestion)
                .WithMany(eq => eq.UserAnswers)
                .HasForeignKey(ua => ua.EvaluationQuestionId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Choice UserAnswer
            modelBuilder.Entity<UserAnswer>()
                .HasOne(ua => ua.Choice)
                .WithMany(c => c.UserAnswers)
                .HasForeignKey(ua => ua.ChoiceId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many User UserAnswer
            modelBuilder.Entity<UserAnswer>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAnswers)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many User Schedule
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.User)
                .WithMany(u => u.Schedules)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Schedule Process
            modelBuilder.Entity<Process>()
                .HasOne(p => p.Schedule)
                .WithMany(s => s.Processes)
                .HasForeignKey(p => p.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Schedule Subject
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Schedule)
                .WithMany(sc => sc.Subjects)
                .HasForeignKey(s => s.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Process Subject
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Process)
                .WithMany(p => p.Subjects)
                .HasForeignKey(s => s.ProcessId)
                .OnDelete(DeleteBehavior.Restrict);


            // one to many Process Task
            modelBuilder.Entity<ProcessTask>()
                .HasOne(t => t.Process)
                .WithMany(s => s.ProcessTasks)
                .HasForeignKey(t => t.ProcessId)
                .OnDelete(DeleteBehavior.Restrict);

            // one to many User MemberShip
            modelBuilder.Entity<MemberShip>()
                .HasOne(ms => ms.User)
                .WithMany(u => u.MemberShips)
                .HasForeignKey(ms => ms.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // one to many MemberShipPlan MemberShip
            modelBuilder.Entity<MemberShip>()
                .HasOne(ms => ms.MemberShipPlan)
                .WithMany(msp => msp.MemberShips)
                .HasForeignKey(ms => ms.MemberShipPlanId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

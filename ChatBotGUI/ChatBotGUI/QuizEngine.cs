using System;
using System.Collections.Generic;

namespace ChatBotGUI
{
    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }

        public QuizQuestion(string questionText, List<string> options, string correctAnswer, string explanation)
        {
            QuestionText = questionText;
            Options = options;
            CorrectAnswer = correctAnswer.Trim().ToUpper();
            Explanation = explanation;
        }
    }

    public class QuizEngine
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex = 0;
        private int score = 0;
        private bool isQuizActive = false;

        public bool IsQuizActive => isQuizActive;
        public int TotalQuestions => questions.Count;

        public QuizEngine()
        {
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion("Question 1: What should you do if you receive an email asking for your password?", new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" }, "C", "Reporting phishing emails helps prevent scams and alerts IT security."),
                new QuizQuestion("Question 2: Which of the following makes a password strong?", new List<string> { "A) Using your birthday", "B) 12+ chars with letters, numbers & symbols", "C) Reusing your email password", "D) '12345678'" }, "B", "Long passwords with mixed characters are significantly harder for hackers to crack."),
                new QuizQuestion("Question 3 (True/False): Urgent requests for money or credentials from a colleague via email should always be trusted immediately.", new List<string> { "A) True", "B) False" }, "B", "Scammers often impersonate colleagues or authority figures to create a false sense of urgency."),
                new QuizQuestion("Question 4: What does the 'HTTPS' prefix in a website address signify?", new List<string> { "A) Free internet access", "B) High-speed server", "C) Encrypted communication", "D) Verified government site" }, "C", "HTTPS encrypts data between your browser and the server."),
                new QuizQuestion("Question 5 (True/False): Multi-Factor Authentication (MFA) adds an extra layer of security beyond just your password.", new List<string> { "A) True", "B) False" }, "A", "MFA requires two or more verification factors, making unauthorized access much harder."),
                new QuizQuestion("Question 6: What is 'Baiting' in cybersecurity?", new List<string> { "A) Leaving infected USB drives in public", "B) Sending malware attachments", "C) Calling victims pretending to be support", "D) Hacking Wi-Fi routers" }, "A", "Baiting relies on curiosity by dropping malware-loaded USB drives."),
                new QuizQuestion("Question 7 (True/False): Connecting to free public Wi-Fi without a VPN is completely safe for online banking.", new List<string> { "A) True", "B) False" }, "B", "Public Wi-Fi networks can be intercepted by attackers."),
                new QuizQuestion("Question 8: What is 'Spear Phishing'?", new List<string> { "A) Automated spam", "B) Targeted phishing directed at a specific individual or team", "C) Malware via SMS", "D) Ransomware encryption" }, "B", "Spear phishing targets specific individuals using personalized info."),
                new QuizQuestion("Question 9: What is the primary purpose of Ransomware?", new List<string> { "A) Display pop-up ads", "B) Encrypt files and demand payment for decryption", "C) Speed up your CPU", "D) Steal Wi-Fi passwords" }, "B", "Ransomware locks files until a ransom is paid."),
                new QuizQuestion("Question 10 (True/False): System updates only contain cosmetic visual features and can be ignored indefinitely.", new List<string> { "A) True", "B) False" }, "B", "Software updates patch critical security vulnerabilities."),
                new QuizQuestion("Question 11: Where is the safest place to store complex unique passwords?", new List<string> { "A) On a sticky note on your monitor", "B) In a text file on your desktop", "C) In a trusted Password Manager", "D) Shared with a friend" }, "C", "Password managers securely store and encrypt credentials.")
            };
        }

        public string StartQuiz()
        {
            isQuizActive = true;
            currentQuestionIndex = 0;
            score = 0;
            ActivityLogger.Log("Quiz started - 5+ questions active.");
            return "🎯 **Cybersecurity Quiz Started!**\nAnswer the questions by typing the letter (A, B, C, D) or True/False.\n\n" + GetCurrentQuestionFormatted();
        }

        public string GetCurrentQuestionFormatted()
        {
            if (currentQuestionIndex >= questions.Count) return "";
            var q = questions[currentQuestionIndex];
            return q.QuestionText + "\n" + string.Join("\n", q.Options);
        }

        public string ProcessAnswer(string userChoice)
        {
            if (!isQuizActive) return "";

            string cleanAnswer = userChoice.Trim().ToUpper();
            if (cleanAnswer == "TRUE") cleanAnswer = "A";
            if (cleanAnswer == "FALSE") cleanAnswer = "B";
            if (cleanAnswer.Length > 1 && cleanAnswer.Contains(")"))
            {
                cleanAnswer = cleanAnswer.Substring(0, 1);
            }

            var currentQ = questions[currentQuestionIndex];
            string feedback = "";

            if (cleanAnswer == currentQ.CorrectAnswer)
            {
                score++;
                feedback = $" **Correct!** {currentQ.Explanation}";
            }
            else
            {
                feedback = $" **Incorrect.** (Correct answer was {currentQ.CorrectAnswer}). {currentQ.Explanation}";
            }

            currentQuestionIndex++;

            if (currentQuestionIndex >= questions.Count)
            {
                isQuizActive = false;
                ActivityLogger.Log($"Quiz completed with final score: {score}/{questions.Count}.");
                string finalGrade = GetFinalFeedback(score, questions.Count);
                return $"{feedback}\n\n **Quiz Completed!**\nYour Final Score: {score}/{questions.Count}\nFeedback: {finalGrade}";
            }

            return $"{feedback}\n\n---\n" + GetCurrentQuestionFormatted();
        }

        private string GetFinalFeedback(int score, int total)
        {
            double percentage = (double)score / total * 100;
            if (percentage >= 80) return "Great job! You're a cybersecurity pro! ";
            else if (percentage >= 50) return "Good effort! Keep learning to stay safe online! ";
            else return "Keep learning to stay safe online! Review password and phishing basics.";
        }
    }
}

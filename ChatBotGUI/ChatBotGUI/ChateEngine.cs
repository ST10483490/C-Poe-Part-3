using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ChatBotGUI
{
    public class ChatEngine
    {
        private string userName = "";
        private string lastTopic = "";
        private string favoriteTopic = "";
        private Random random = new Random();

        private bool waitingForTaskReminder = false;
        private string pendingTaskTitle = "";
        private QuizEngine quizEngine = new QuizEngine();

        public bool IsNameRegistered => !string.IsNullOrWhiteSpace(userName);

        public string SetUserName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Name cannot be empty. Please enter your name:";

            userName = input.Trim();
            ActivityLogger.Log($"User registered name: {userName}.");
            return $"Hello, {userName}! Welcome to your Cybersecurity Awareness GUI Assistant.\n" +
                   $"You can ask about passwords, scams, privacy, type 'quiz', or ask 'Show activity log'.";
        }

        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type a question or topic so I can assist you!";

            string cleanInput = input.Trim();
            string lowerInput = cleanInput.ToLower();

            // 1. Handle Active Quiz Interception
            if (quizEngine.IsQuizActive)
            {
                return quizEngine.ProcessAnswer(cleanInput);
            }

            // 2. Start Quiz Command
            if (lowerInput.Contains("quiz") || lowerInput.Contains("play") || lowerInput.Contains("game"))
            {
                return quizEngine.StartQuiz();
            }

            // 3. Activity Log Command ("Show activity log" or "What have you done for me?")[cite: 17, 18]
            if (lowerInput.Contains("show activity log") || lowerInput.Contains("what have you done") || lowerInput.Contains("recent actions") || lowerInput.Contains("history"))
            {
                bool showAll = lowerInput.Contains("all") || lowerInput.Contains("more");
                ActivityLogger.Log("Viewed activity log summary.");
                return ActivityLogger.GetSummary(showAll);
            }

            // 4. Handle Pending Task Reminder State
            if (waitingForTaskReminder)
            {
                waitingForTaskReminder = false;
                string reminder = cleanInput;

                DatabaseHelper.AddTask(pendingTaskTitle, "Task created via conversation.", reminder);
                ActivityLogger.Log($"Reminder set: '{pendingTaskTitle}' on {reminder}.");

                return $"Got! Reminder set for '{pendingTaskTitle}' on {reminder}.";
            }

            // 5. NLP: "Remind me to..."[cite: 17]
            if (lowerInput.StartsWith("remind me to") || lowerInput.StartsWith("set a reminder"))
            {
                string taskDetail = Regex.Replace(cleanInput, @"^(remind me to|set a reminder to|set a reminder)\s*", "", RegexOptions.IgnoreCase).Trim();
                pendingTaskTitle = taskDetail;

                DatabaseHelper.AddTask(pendingTaskTitle, "Task created via NLP reminder.", "Tomorrow");
                ActivityLogger.Log($"Reminder set: '{pendingTaskTitle}' on tomorrow's date.");

                return $"Reminder set for '{pendingTaskTitle}' on tomorrow's date.";
            }

            // 6. NLP: "Add a task to..."[cite: 17]
            if (lowerInput.Contains("add task") || lowerInput.Contains("add a task") || lowerInput.Contains("create task"))
            {
                string taskTitle = Regex.Replace(cleanInput, @"^(add a task to|add task to|add task|create task)\s*", "", RegexOptions.IgnoreCase).Trim();
                if (string.IsNullOrEmpty(taskTitle)) taskTitle = "General Security Task";

                pendingTaskTitle = taskTitle;
                waitingForTaskReminder = true;

                ActivityLogger.Log($"Task added: '{pendingTaskTitle}' (reminder pending).");

                return $"Task added: '{pendingTaskTitle}.' Would you like to set a reminder for this task?";
            }

            // 7. Sentiment & Topics
            string sentimentPrefix = DetectSentiment(lowerInput);

            if (lowerInput.Contains("password"))
            {
                lastTopic = "password";
                ActivityLogger.Log("NLP Interaction: Explored password safety guidance.");
                return sentimentPrefix + GetTopicGuidance("password");
            }
            if (lowerInput.Contains("scam") || lowerInput.Contains("phishing"))
            {
                lastTopic = "scam";
                ActivityLogger.Log("NLP Interaction: Explored scam & phishing defense.");
                return sentimentPrefix + GetTopicGuidance("scam");
            }
            if (lowerInput.Contains("privacy") || lowerInput.Contains("data"))
            {
                lastTopic = "privacy";
                ActivityLogger.Log("NLP Interaction: Explored data privacy guidelines.");
                return sentimentPrefix + GetTopicGuidance("privacy");
            }

            if (lowerInput.Contains("help") || lowerInput.Contains("topics"))
            {
                return $"{sentimentPrefix}Here are topics you can explore, {userName}:\n" +
                       $"• Password Safety\n• Scam & Phishing Defense\n• Data Privacy\n• Cybersecurity Quiz (Type 'quiz')\n• Task Assistant (Type 'add task ...')\n• Activity Log (Ask 'Show activity log')";
            }

            return sentimentPrefix + "I didn't quite catch that. Try asking about 'password', 'scam', 'privacy', type 'quiz', or ask 'Show activity log'.";
        }

        private string GetTopicGuidance(string topic)
        {
            if (topic == "password")
                return $"[Password Guidance for {userName}]:\n1. Use strong passwords (12+ chars).\n2. Mix letters, numbers, symbols.\n3. Use a password manager.";
            if (topic == "scam")
                return $"[Scam Defense for {userName}]:\n1. Check sender email addresses.\n2. Beware of urgency.\n3. Never click unverified links.";
            if (topic == "privacy")
                return $"[Privacy Guidance for {userName}]:\n1. Review app permissions.\n2. Clear cookies regularly.\n3. Keep software updated.";
            return "";
        }

        private string DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("hack"))
                return "It's completely understandable to feel that way. Staying informed is your best defense! ";
            if (input.Contains("confused") || input.Contains("stuck") || input.Contains("hard"))
                return "Don't worry, cybersecurity can be tricky, but I'm here to help! ";
            return "";
        }
    }
}
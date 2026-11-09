using System;
using System.Collections.Generic;

namespace ChatBotGUI
{
    public static class ActivityLogger
    {
        private static List<string> activityLogs = new List<string>();

        public static void Log(string description)
        {
            activityLogs.Add(description);
        }

        public static string GetSummary(bool showAll = false)
        {
            if (activityLogs.Count == 0)
                return "Here's a summary of recent actions:\nNo actions recorded in this session yet.";

            int limit = showAll ? activityLogs.Count : Math.Min(activityLogs.Count, 5);
            string summary = "Here's a summary of recent actions:\n";

            int startIndex = showAll ? 0 : Math.Max(0, activityLogs.Count - limit);
            int displayNum = 1;

            for (int i = startIndex; i < activityLogs.Count; i++)
            {
                summary += $"{displayNum}. {activityLogs[i]}\n";
                displayNum++;
            }

            return summary.TrimEnd();
        }
    }
}
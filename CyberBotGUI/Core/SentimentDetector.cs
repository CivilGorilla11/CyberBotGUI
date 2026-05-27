using System;
using System.Collections.Generic;
using System.Text;

namespace CyberBotGUI.Core
{
    public enum Sentiment { Positive, Negative, Confused, Urgent, Neutral }
    public class SentimentDetector 
    {
        private static readonly List<string> PositiveWords = new List<string>
        { "thanks", "great", "helpful", "awesome", "love", "good", "excellent", "perfect" };

        private static readonly List<string> NegativeWords = new List<string>
        { "worried", "scared", "afraid", "hacked", "breached", "attacked",
          "stolen", "compromised", "angry", "frustrated", "terrible", "awful" };

        private static readonly List<string> ConfusedWords = new List<string>
        { "confused", "don't understand", "not sure", "what is", "what does",
          "explain", "how does", "what do you mean", "unclear", "lost" };

        private static readonly List<string> UrgentWords = new List<string>
        { "help", "urgent", "emergency", "immediately", "asap", "right now",
          "critical", "serious", "danger", "problem", "issue" };

        public Sentiment Detect(string input)
        {
            string lower = input.ToLower();

            foreach (var word in UrgentWords)
                if (lower.Contains(word)) return Sentiment.Urgent;

            foreach (var word in NegativeWords)
                if (lower.Contains(word)) return Sentiment.Negative;

            foreach (var word in ConfusedWords)
                if (lower.Contains(word)) return Sentiment.Confused;

            foreach (var word in PositiveWords)
                if (lower.Contains(word)) return Sentiment.Positive;

            return Sentiment.Neutral;
        }
        public string GetEmpathyPrefix(Sentiment sentiment, string name = "")
        {
            string n = string.IsNullOrEmpty(name) ? "" : $", {name}";

            return sentiment switch
            {
                Sentiment.Urgent => $" I can hear that this is urgent{n}. Let me help you right away.\n\n",
                Sentiment.Negative => $" I understand this can feel overwhelming{n}. You are not alone — let me help.\n\n",
                Sentiment.Confused => $"No worries at all{n} — cybersecurity can be complex. Let me break this down clearly.\n\n",
                Sentiment.Positive => $" Glad to hear that{n}! ", 
                _ => ""
            };
        }
    }
}

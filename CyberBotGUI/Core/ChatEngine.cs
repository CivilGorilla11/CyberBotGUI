using CyberBotGUI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace CyberBotGUI.Core
{

    public delegate string ResponseChat(string userInput);

    public class ChatEngine
    {

        private readonly Dictionary<string, ResponseChat> _keywordHandlers;
       
        private readonly SentimentDetector _sentiment;

        public List<ChatMessage> History { get; } = new List<ChatMessage>();

        public ChatEngine()
        {
           
            _sentiment = new SentimentDetector();

            _keywordHandlers = new Dictionary<string, ResponseChat>
        {
            {"password",   HandlePassword},
            {"phishing",   HandlePhishing},
            {"scam",       HandleScam},
            {"malware",    HandleMalware},
            {"ransomware", HandleRansomware},
            {"firewall",   HandleFirewall},
            {"vpn",        HandleVpn},
            {"hacker",     HandleHacker},
            {"encryption", HandleEncryption},
            {"backup",     HandleBackup},
        };
        }

        public string ProcessInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please enter a message";

            MemoryStore.IncrementMessage();

            History.Add(new ChatMessage(userInput, MessageSender.User));

            string lower = userInput.ToLower();
            string response;
            

            try
            {

                //User Name capturing 
                if (lower.Contains("my name is") || lower.Contains("i am") || lower.Contains("i'm"))
                {
                   string name = ExtractName(userInput);
                    if (!string.IsNullOrEmpty(name))
                    {
                        MemoryStore.SetName(name);
                        response = $"Great to meet you, {name}! I am CyberBot your cybersecurity assistant";
                        History.Add(new ChatMessage(response, MessageSender.Bot));
                        return response;
                    }
                }
                //Greeting Detection

                string[] greetings = { "hello", "hi", "hey" };
                if (greetings.Any(g => lower.Split(' ').Contains(g)))
                {
                    response = MemoryStore.GetGreeting();
                    History.Add(new ChatMessage(response, MessageSender.Bot));
                    return response;
                }

                //Sentiment detection
                var detectedSentiment = _sentiment.Detect(lower);
                string empathyPrefix = _sentiment.GetEmpathyPrefix(detectedSentiment, MemoryStore.UserName);

                //Keyword recognition via delegate dictionary
               
                foreach (var entry in _keywordHandlers)
                {
                    if (Regex.IsMatch(lower, $@"\b{Regex.Escape(entry.Key)}\b"))
                    {
                        MemoryStore.SetLastTopic(entry.Key);
                        string baseResponse = entry.Value.Invoke(userInput);
                        response = empathyPrefix + baseResponse;

                        //Conversational Follow up after 2 or more messages

                        if (MemoryStore.MessageCount > 2)
                            response += BuildFollowUp();

                        History.Add(new ChatMessage(response, MessageSender.Bot));
                        return response; 
                    }
                }
                //Short Input to ask for clarity 
                if (userInput.Trim().Length < 15)
                {
                    response = empathyPrefix + ResponseLibrary.GetRandom(ResponseLibrary.ClarificationResponses);
                    History.Add(new ChatMessage(response, MessageSender.Bot));
                    return response;
                }
                response = empathyPrefix + ResponseLibrary.GetRandom(ResponseLibrary.FallbackResponses);
                History.Add(new ChatMessage(response, MessageSender.Bot));
                return response;


            }
            catch (Exception ex)
            {
                response = $"Something went wrong: {ex.Message}. Please try again";
                return response;

            }
        }
        private string BuildFollowUp()
        {
            if (string.IsNullOrEmpty(MemoryStore.LastTopic))
                return "";
            return $"\n\nWant to go deeper on **{MemoryStore.LastTopic}** or explore a new topic?";
        }

        private string ExtractName(string userInput)
        {
            try
            {
                string lower = userInput.ToLowerInvariant();
                string[] triggers = { "my name is", "i am", "i'm" };

                foreach (var trigger in triggers)
                {
                    int index = lower.IndexOf(trigger);

                    if (index >= 0)
                    {
                        string after = userInput.Substring(index + trigger.Length).Trim();

                        string[] words = after.Split(' ');

                        if (words.Length > 0)
                        {
                            return words[0].Trim('.', ',', '!', '?');
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return string.Empty;
        }

        //Delegate handler methods


        private string HandlePassword(string i)
            => ResponseLibrary.GetRandom(ResponseLibrary.PasswordResponses);

        private string HandlePhishing(string i)
            => ResponseLibrary.GetRandom(ResponseLibrary.PhishingResponses);

        private string HandleScam(string i) =>
          ResponseLibrary.GetRandom(ResponseLibrary.ScamResponses);

        private string HandleMalware(string i) =>
            ResponseLibrary.GetRandom(ResponseLibrary.MalwareResponses);

        private string HandleRansomware(string i) =>
            ResponseLibrary.GetRandom(ResponseLibrary.RansomwareResponses);

        private string HandleFirewall(string i) =>
            ResponseLibrary.GetRandom(ResponseLibrary.FirewallResponses);

        private string HandleVpn(string i) =>
            ResponseLibrary.GetRandom(ResponseLibrary.VpnResponses);

        private string HandleHacker(string i) =>
            ResponseLibrary.GetRandom(ResponseLibrary.HackerResponses);

        private string HandleEncryption(string i) =>
            ResponseLibrary.GetRandom(ResponseLibrary.EncryptionResponses);

        private string HandleBackup(string i) =>
            ResponseLibrary.GetRandom(ResponseLibrary.BackupResponses);

        private string HandleUnrecognised(string i) =>
            ResponseLibrary.GetRandom(ResponseLibrary.FallbackResponses);
    }

}




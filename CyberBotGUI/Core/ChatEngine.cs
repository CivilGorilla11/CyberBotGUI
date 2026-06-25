using CyberBotGUI.Models;
using CyberBotGUI.Data;
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

        private readonly QuizEngine _quiz = new QuizEngine();

        public List<ChatMessage> History { get; } = new List<ChatMessage>();

        public ChatEngine()
        {
           
            _sentiment = new SentimentDetector();

            DatabaseService.Initialize();

            ActivityLog.Add("Bot started", "CyberBotGui launched and database initialized", "SYSTEM");
            ActivityLog.Add("Voice Greeting", "Welcome voice message played on startup", "VOICE");

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

            ActivityLog.Add("User Message", $"Input recieved: \"{userInput.Substring(0, Math.Min(40, userInput.Length))}...\"","USER");

            string lower = userInput.ToLower();
            string response;

            if(_quiz.IsActive && _quiz.AwaitingAnswer)
            {
                response = _quiz.SubmitAnswer(userInput);
                History.Add(new ChatMessage(response, MessageSender.Bot));
                return response;
            }

            var intent = NLPRouter.Detect(userInput);
            bool looksLikeCyberQuestion = lower.Contains("about") || lower. Contains("what is") || lower.Contains("tell me");

            if (intent == Intent.Greeting && looksLikeCyberQuestion)
            {
                intent = Intent.CyberKeyword;
            }
            switch(intent) 
            {
                case Intent.NameIntro:
                    string name = ExtractName(userInput);
                    if(!string.IsNullOrEmpty(name))
                    {
                        MemoryStore.SetName(name);
                        ActivityLog.Add("Name Captured", $"User name set to: {name}", "USER");
                        response = $"Great to meet you, {name}! I am CyberBot your cybersecurity assistant";
                        
                        return Log(response);
                    }

                    break;

                case Intent.Greeting:
                    response = MemoryStore.GetGreeting();
                    return Log(response);

                case Intent.ShowTasks:
                    response = TaskManager.ShowAllTasks();
                    return Log(response);

                    case Intent.AddTask:
                    response = TaskManager.QuickAddFromInput(userInput);
                    return Log(response);

                case Intent.CompleteTask:
                   int cId = NLPRouter.ExtractTaskId(userInput);
                    response = cId > 0
                        ? TaskManager.CompleteTask(cId)
                        : "Which task number would you like to complete?" +
                        " Say 'show tasks' to see all tasks.";
                    return Log(response);

                    case Intent.DeleteTask:
                    int dId = NLPRouter.ExtractTaskId(userInput);
                    response = dId > 0
                        ? TaskManager.DeleteTask(dId)
                        : "Which task number would you like to delete?" +
                        " Say 'show tasks' to see all tasks.";
                       return Log(response);

                    case Intent.SetReminder:
                    response = "To set a reminder say : add task [task title] at "+
                        "and I will prompt you for a date and time for the reminder.";
                    return Log(response);


                case Intent.StartQuiz:
                    _quiz.Start();
                    response = "🧠 Starting the Cybersecurity Awareness Quiz!\n\n" +
                                   "12 questions — multiple choice and true/false.\n" +
                                   "Answer with A, B, C or D, or True/False.\n\n" +
                                   "━━━━━━━━━━━━━━━━━━━━\n\n" +
                                   _quiz.FormatQuestion();
                    return Log(response);



                case Intent.ShowLog:
                    ActivityLog.Add("Log requested and viewed",
                        "User requested to view the activity log", "SYSTEM");
                    response = ActivityLog.FormatForChat();
                    return Log(response);


                case Intent.Exit:
                    ActivityLog.Add("Session Ended!",
                        "User has closed the chat session", "SYSTEM");
                    response = "Goodbye! Remember to stay safe online!";
                       return Log(response);
            }


            try
            {

                //User Name capturing 
                if (lower.Contains("my name is"))
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

                      ActivityLog.Add("Keyword Detected", $"Keyword: {entry.Key} detected in user input", "KEYWORD"); 
                        return Log (response); 
                    }
                }
                //Short Input to ask for clarity 
                if (userInput.Trim().Length < 15)
                {
                    response = empathyPrefix + ResponseLibrary.GetRandom(ResponseLibrary.ClarificationResponses);
                    History.Add(new ChatMessage(response, MessageSender.Bot));
                    return Log(response);
                }
                response = empathyPrefix + ResponseLibrary.GetRandom(ResponseLibrary.FallbackResponses) +
                    "\n\n Tip: Try saying 'show tasks' or 'start quiz', " +
                    "or ask me about cybersecurity topics like 'phishing', 'malware'.";


                return Log(response);


            }
            catch (Exception ex)

            {
                ActivityLog.Add("Error", $"Exception occurred: {ex.Message}", "ERROR");
                response = $"Something went wrong: {ex.Message}. Please try again";
                return Log(response);

            }
        }

        private string Log(string response)
        {
            History.Add(new ChatMessage(response, MessageSender.Bot));
           
            return response;
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
                string[] triggers = { "my name is" };

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

            return "";
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




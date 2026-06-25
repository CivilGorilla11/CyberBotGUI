using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using CyberBotGUI.Models;

namespace CyberBotGUI.Core
{
    public class QuizEngine
    {
        private int _current = 0;
        private int _score = 0;
        public bool IsActive { get; private set; } = false;
        public bool AwaitingAnswer { get; private set; } = false;

        public readonly List<QuizQuestion> Questions = new List<QuizQuestion>
{
    new QuizQuestion { Number=1,  QuestionTyp=QuestionType.TrueFalse,
        Question="TRUE or FALSE: A phishing email always contains obvious spelling mistakes.",
        Options=new List<string>{"A) True","B) False"},
        CorrectOptionIndex=1,
        Explanation="FALSE — Modern phishing emails are often highly polished and may look identical to legitimate company emails. Never judge safety by spelling alone." },

    new QuizQuestion { Number=2,  QuestionTyp=QuestionType.MultipleChoice,
        Question="What does the 3-2-1 backup rule mean?",
        Options=new List<string>{"A) 3 backups, 2 locations, 1 cloud","B) 3 copies, 2 different media types, 1 stored offsite","C) 3 passwords, 2 devices, 1 USB","D) 3 files, 2 folders, 1 drive"},
        CorrectOptionIndex=1,
        Explanation="CORRECT answer is B — 3 copies of data, on 2 different media types, with 1 copy stored offsite or in the cloud protects against hardware failure, theft and disaster." },

    new QuizQuestion { Number=3,  QuestionTyp=QuestionType.MultipleChoice,
        Question="Which of the following is the STRONGEST password?",
        Options=new List<string>{"A) password123","B) MyDog2010!","C) Tr0ub4dor&3#Gx9","D) QWERTY"},
        CorrectOptionIndex=2,
        Explanation="C is correct — Long passwords mixing uppercase, lowercase, numbers and symbols with no dictionary words are exponentially harder to crack via brute force." },

    new QuizQuestion { Number=4,  QuestionTyp=QuestionType.TrueFalse,
        Question="TRUE or FALSE: Using public WiFi with a VPN is completely safe.",
        Options=new List<string>{"A) True","B) False"},
        CorrectOptionIndex=1,
        Explanation="FALSE — A VPN greatly reduces risk by encrypting traffic, but no solution is 100% safe. Avoid sensitive transactions on public WiFi even with a VPN." },

    new QuizQuestion { Number=5,  QuestionTyp=QuestionType.MultipleChoice,
        Question="What is ransomware?",
        Options=new List<string>{"A) Software that speeds up your computer","B) Malware that encrypts files and demands payment","C) A firewall bypass tool","D) An antivirus program"},
        CorrectOptionIndex=1,
        Explanation="B is correct — Ransomware encrypts your data and demands payment for the decryption key. The best defence is maintaining regular offline backups." },

    new QuizQuestion { Number=6,  QuestionTyp=QuestionType.TrueFalse,
        Question="TRUE or FALSE: Two-factor authentication makes your account significantly more secure even if your password is stolen.",
        Options=new List<string>{"A) True","B) False"},
        CorrectOptionIndex=0,
        Explanation="TRUE — 2FA requires a second factor (phone code, authenticator app) that an attacker does not have, making stolen passwords insufficient to gain access." },

    new QuizQuestion { Number=7,  QuestionTyp=QuestionType.MultipleChoice,
        Question="What does HTTPS mean for a website?",
        Options=new List<string>{"A) The website is government approved","B) Data between your browser and the site is encrypted","C) The site has no viruses","D) Your identity is hidden"},
        CorrectOptionIndex=1,
        Explanation="B is correct — HTTPS uses TLS encryption to protect data in transit. However, HTTPS does not mean the website itself is trustworthy or legitimate." },

    new QuizQuestion { Number=8,  QuestionTyp=QuestionType.MultipleChoice,
        Question="What is social engineering in cybersecurity?",
        Options=new List<string>{"A) Building security systems","B) Writing malware code","C) Manipulating people into revealing sensitive information","D) Analysing network traffic"},
        CorrectOptionIndex=2,
        Explanation="C is correct — Social engineering exploits human psychology rather than technical vulnerabilities. Examples include phishing, pretexting and baiting." },

    new QuizQuestion { Number=9,  QuestionTyp=QuestionType.TrueFalse,
        Question="TRUE or FALSE: You should reuse the same strong password across multiple websites to save time.",
        Options=new List<string>{"A) True","B) False"},
        CorrectOptionIndex=1,
        Explanation="FALSE — Password reuse is one of the most dangerous habits. If one site is breached, attackers try those credentials on all other sites — known as credential stuffing." },
     
    new QuizQuestion { Number=10, QuestionTyp=QuestionType.MultipleChoice,
        Question="Which of these is a sign your device may be infected with malware?",
        Options=new List<string>{"A) Your battery charges faster","B) Unexpected pop-ups, slow performance and unknown processes","C) Websites load quicker than usual","D) Your WiFi signal improves"},
        CorrectOptionIndex=1,
        Explanation="B is correct — Unexpected pop-ups, sluggish performance and unknown processes running in the background are classic malware indicators." },

    new QuizQuestion { Number=11, QuestionTyp=QuestionType.TrueFalse,
        Question="TRUE or FALSE: A firewall alone is enough to protect your computer from all cyber threats.",
        Options=new List<string>{"A) True","B) False"},
        CorrectOptionIndex=1,
        Explanation="FALSE — A firewall is one layer of defence. Comprehensive security requires antivirus software, regular updates, strong passwords, backups and user awareness." },

    new QuizQuestion { Number=12, QuestionTyp=QuestionType.MultipleChoice,
        Question="What is the safest way to verify a suspicious email from your bank?",
        Options=new List<string>{"A) Click the link in the email to check","B) Reply to the email asking if it is real","C) Ignore it completely","D) Go directly to your bank's official website or call their number"},
        CorrectOptionIndex=3,
        Explanation="D is correct — Never click links in suspicious emails. Go directly to the official website by typing it yourself, or call the official number to verify." },
};

        public void Start()
        {
            _current = 0;
            _score = 0;
            IsActive = true;
            AwaitingAnswer = true;
            ActivityLog.Add("Quiz has begun", "User launched the 12 set question cybersecurity quiz", "QUIZ");
        }

        // Fix 1 — QuizQuestions → QuizQuestion, Currrent → Current
        public QuizQuestion Current =>
            _current < Questions.Count ? Questions[_current] : null;

        public string FormatQuestion()
        {
            var q = Current;
            if (q == null)
            {
                return "";
            }
            var sb = new System.Text.StringBuilder();

            // Fix 2 — clean interpolated string
            sb.AppendLine($"Question {q.Number} of 12 — " +
                (q.QuestionTyp == QuestionType.TrueFalse ? "True or False" : "Multiple Choice"));
            sb.AppendLine();
            sb.AppendLine(q.Question);
            sb.AppendLine();
            foreach (var option in q.Options)
            {
                sb.AppendLine(option);
            }

            return sb.ToString().Trim();
        }

        public string SubmitAnswer(string input)
        {
            // Fix 3 — use Current (the question object), not _current (the int)
            var q = Current;
            if (q == null)
            {
                return EndQuiz();
            }

            string lower = input.ToLower().Trim();
            int chosen = -1;

            // Fix 4 — compare string to string
            if (lower.StartsWith("a") || lower == "true")
            {
                chosen = 0;
            }
            else if (lower.StartsWith("b") || lower == "false")
            {
                chosen = 1;
            }
            else if (lower.StartsWith("c"))
            {
                chosen = 2;
            }
            else if (lower.StartsWith("d"))
            {
                chosen = 3;
            }

            // Fix 5 — check against -1, not 1
            if (chosen == -1)
            {
                return "Please answer with A, B, C or D (or True/False for the current question).";
            }

            bool correct = chosen == q.CorrectOptionIndex;
            if (correct) _score++;

            // Fix 6 — explanation included on incorrect answers too
            string feedback = correct
                ? $"Correct!\n\n{q.Explanation}"
                : $"Incorrect. The correct answer is {(char)('A' + q.CorrectOptionIndex)}.\n\n{q.Explanation}";

            ActivityLog.Add($"Quiz Question {q.Number} Answered",
                $"User answered {(char)('A' + chosen)} - {(correct ? "CORRECT" : "INCORRECT")}", "QUIZ");

            _current++;

            if (_current >= Questions.Count)
            {
                AwaitingAnswer = false;
                IsActive = false;
                return feedback + "\n\n" + EndQuiz();
            }
            return feedback + $"\n\n━━━━━━━━━━━━━━━━━━━━\n\n" + FormatQuestion();
        }

        private string EndQuiz()
        {
            string grade;
            string advice;

            if (_score >= 11) { grade = "Outstanding"; advice = "Excellent work! You have a strong understanding of cybersecurity best practices."; }
            else if (_score >= 9) { grade = "Very Good"; advice = "Great job! You have a solid grasp of cybersecurity concepts."; }
            else if (_score >= 7) { grade = "Good"; advice = "Good effort! Consider reviewing some areas to strengthen your knowledge."; }
            else if (_score >= 5) { grade = "Fair"; advice = "You have some understanding, but there's room for improvement. Review key concepts."; }
            else { grade = "Needs Improvement"; advice = "Consider revisiting cybersecurity basics and best practices to improve your knowledge."; }

            ActivityLog.Add("Quiz Completed", $"User completed the quiz with a score of {_score}/12 - Grade: {grade}", "QUIZ");
            return $"Quiz Completed!\n\nYour Score: {_score}/12\nGrade: {grade}\n\n{advice}";
        } 
    }
}
using System.Collections.Generic;

namespace CyberBotGUI.Models
{
    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
    }

    public class QuizQuestion
    {
        public int Number { get; set; }
        public string Question { get; set; }
        public QuestionType QuestionTyp { get; set; }
        public List<string> Options { get; set; }
        public int CorrectOptionIndex { get; set; }
        public string Explanation { get; set; }
    }
}

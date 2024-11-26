using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedAdventure.Classes
{
    public class Riddle
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        public Riddle(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }
}

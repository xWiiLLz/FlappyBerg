using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBerg.ScoreLogic
{
    public class ScoreEntry
    {
        #region Properties
        public string Name { get; private set; }
        public int Score { get; private set; }
        public string Date { get; set; }
        #endregion
        public ScoreEntry(string name, int score, string date)
        {
            Name = name;
            Score = score;
            Date = date;
        }
    }
}

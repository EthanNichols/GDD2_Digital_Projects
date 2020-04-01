using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    class ScoreManager:Singleton<ScoreManager>
    {
        int score = 0;

        public int GetScore { get => score; }
        
        /// <summary>
        /// Sets score back to 0
        /// </summary>
        public void ResetScore()
        {
            score = 0;
        }

        /// <summary>
        /// Changes score by value, positive or negative. 
        /// Resets score back to 0 if it falls below 0
        /// </summary>
        /// <param name="value"></param>
        public void ChangeScoreBy(int value)
        {
            score += value;

            if (score < 0)
            {
                score = 0;
            }
        }



    }
}

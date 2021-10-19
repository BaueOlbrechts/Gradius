using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class Score
    {
        private int HighScore { get; set; }
        private int PreviousHighScore { get; set; }
        private int CurrentScore { get; set; } = 0;
        private bool IsNewHighscoreAchieved { get; set; } = false;
        private SpriteFont Font { get; set; }
        private string FileName { get; set; } = "HighScore.txt";
        public Score(SpriteFont font)
        {
            Font = font;
        }

        public void UpdateScores()
        {
            if(IsNewHighscoreAchieved == false && CurrentScore > HighScore)
            {
                PreviousHighScore = HighScore;
                IsNewHighscoreAchieved = true;
            }

            if(IsNewHighscoreAchieved == true)
            {
                HighScore = CurrentScore;
            }
        }

        public void DrawControlScreenScore(SpriteBatch spriteBatch)
        {
            if(GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Hard)
            {
                spriteBatch.DrawString(Font, $"Current Highscore: {HighScore} \nTrying Hardmode?\nYou'll need more than good luck!",
                new Vector2(GameSettings.WINDOWWIDTH / 10 * 3, GameSettings.WINDOWHEIGHT / 2), Color.White);
            }
            else
            {
                spriteBatch.DrawString(Font, $"Current Highscore: {HighScore} \nGood luck trying to beat it!",
                new Vector2(GameSettings.WINDOWWIDTH / 10 * 3, GameSettings.WINDOWHEIGHT / 2), Color.White);
            }
        }
        public void DrawPlayingScreenScore(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, $"Highscore: {HighScore}  Current score: {CurrentScore}",
                Vector2.Zero, Color.White);
        }
        public void DrawGameOverScreenScore(SpriteBatch spriteBatch)
        {
            if(IsNewHighscoreAchieved == false && GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Normal)
            {
                spriteBatch.DrawString(Font, $"Current Highscore: {HighScore}\nYour score: {CurrentScore}\nBetter luck next time!",
                    new Vector2(GameSettings.WINDOWWIDTH / 10 * 3, GameSettings.WINDOWHEIGHT / 2), Color.White);
            }

            if (IsNewHighscoreAchieved == false && GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Hard)
            {
                spriteBatch.DrawString(Font, $"Current Highscore: {HighScore}\nYour score: {CurrentScore}\nMaybe hardmode just isn't for you.\nBetter luck next time!",
                    new Vector2(GameSettings.WINDOWWIDTH / 10 * 3, GameSettings.WINDOWHEIGHT / 2), Color.White);
            }

            if (IsNewHighscoreAchieved == true && GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Normal)
            {
                spriteBatch.DrawString(Font, $"New Highscore: {HighScore}!\nPrevious HighScore: {PreviousHighScore}\nNow go for an even higher score!\nPress 2 on the startscreen to try hardmode",
                    new Vector2(GameSettings.WINDOWWIDTH / 10 * 3, GameSettings.WINDOWHEIGHT / 2), Color.White);
            }

            if (IsNewHighscoreAchieved == true && GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Hard)
            {
                spriteBatch.DrawString(Font, $"New Highscore: {HighScore}!\nPrevious HighScore: {PreviousHighScore}\nLooks like you can handle hardmode, huh.\nNow go for an even higher score!",
                    new Vector2(GameSettings.WINDOWWIDTH / 10 * 3, GameSettings.WINDOWHEIGHT / 2), Color.White);
            }
        }

        public void IncreaseScore()
        {
            CurrentScore += 100;
            if (GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Hard)
                CurrentScore += 50;
        }

        public void ResetScore()
        {
            IsNewHighscoreAchieved = false;
            CurrentScore = 0;
        }

        public void GetHighScoreFromTxt()
        {
            using(StreamReader sr = new StreamReader(FileName))
            {
                HighScore = int.Parse(sr.ReadLine());
            }
        }

        public void SetNewHighScoreIfNecessary()
        {
            if(IsNewHighscoreAchieved == true)
            {
                using (StreamWriter sw = new StreamWriter(FileName))
                {
                    sw.WriteLine(HighScore);
                }
            }
        }
    }
}

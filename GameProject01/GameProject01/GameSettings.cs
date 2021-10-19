using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class GameSettings
    {
        public enum GameState
        {
            ControlScreen = 1,
            Playing = 2,
            GameOver = 3
        }

        public enum Difficulty
        {
            Normal = 1,
            Hard = 2
        }

        public static KeyboardState PreviousKeyboardState, CurrentKeyboardState;
        public static Random Random = new Random();

        //Overall settings
        public static GameState CURRENTGAMESTATE = GameState.ControlScreen;
        public static Difficulty CURRENTDIFFICULTY = Difficulty.Normal;
        public const int WINDOWWIDTH = 800;
        public const int WINDOWHEIGHT = 600;
        public const int TIMEBETWEENWAVES = 170;

        //Avatar Ship settings
        public static Vector2 AVATARSHIPSIZE = new Vector2(90, 42); //30-14 ratio
        public const float AVATARSHIPSPEED = 7;
        public const int EXPLOSIONDURATION = 30;
        public const int NUMBEROFSHIPSPRITES = 7;

        //Projectile settings
        public static Vector2 PROJECTILESIZE = new Vector2(24, 9); //8-3 ratio
        public const float PROJECTILESPEED = 9;
        public const int NUMBEROFPROJECTILESPRITES = 1;

        //Alien projectile settings
        public static Vector2 ALIENPROJECTILESIZE = new Vector2(24, 24); //8-8 ratio
        public const float ALIENPROJECTILESPEED = 5;
        public const int NUMBEROFALIENPROJECTILESPRITES = 2;

        //Alien settings
        public const float DISTANCEBETWEENALIENSINWAVE = 6;

        //Alien01 settings
        public static Vector2 ALIENSHIP01SIZE = new Vector2(42, 60); //14- 20 ratio
        public const float ALIENSHIP01FORWARDSPEED = 6;
        public const float ALIENSHIP01UPDOWNSPEED = 2;
        public const int PLAYONHONING = 40;
        public const int NUMBEROFALIEN01SPRITES = 9;
        public const int NUMBEROFALIEN01COLSINWAVE = 4;
        public const int ALIENSHIP01EXPLOSIONSPRITESTART = 6;

        //Alien02 settings
        public static Vector2 ALIENSHIP02SIZE = new Vector2(60, 56); //20-18 ratio
        public const float ALIENSHIP02FORWARDSPEED = 5;
        public const int NUMBEROFALIEN02SPRITES = 11;
        public const int ALIENSHIP02EXPLOSIONSPRITESTART = 8;
        public const float SINWAVEAMPLITUDE = (float)WINDOWHEIGHT / 7;
        public const float SINWAVEFREQUENCY = (float)WINDOWWIDTH / 100000;
        public const int NUMBEROFALIEN02COLSINWAVE = 2;
        public const int NUMBEROFALIEN02PERCOL = 2;
        public const int TIMEBETWEENALIEN02SPRITECHANGE = 15;

        //Alien03 settings
        public static Vector2 ALIENSHIP03SIZE = new Vector2(45, 48); //15- 16 ratio
        public const float ALIENSHIP03FORWARDSPEED = 8;
        public const int NUMBEROFALIEN03SPRITES = 6;
        public const int ALIENSHIP03EXPLOSIONSPRITESTART = 3;
        public const int NUMBEROFALIEN03PERROW = 3;
        public const int TIMEBETWEENALIEN03SPRITECHANGE = 6;

        //Alien turret settings
        public static Vector2 ALIENTURRETSIZE = new Vector2(45, 48); //15- 16 ratio
        public const float ALIENTURRETFORWARDSPEED = BACKGROUNDSPEED;
        public const int NUMBEROFALIENTURRETSPRITES = 15;
        public const int ALIENTURRETEXPLOSIONSPRITESTART = 12;
        public const int PROJECTILEFIRETIMER = 80;


        //Background settings
        public const int BACKGROUNDSTARTINGDISTANCE = WINDOWWIDTH * 2;
        public const float BACKGROUNDSPEED = 2;
        public static Rectangle BACKGROUND01SPRITERECTANLGE = new Rectangle(3, 0, 192, 152);
        public static Rectangle BACKGROUND02SPRITERECTANLGE = new Rectangle(201, 0, 144, 152);
        public static Rectangle BACKGROUND03SPRITERECTANLGE = new Rectangle(351, 0, 144, 152);
        public static Rectangle BACKGROUND04SPRITERECTANLGE = new Rectangle(501, 0, 192, 152);

        public static Vector2 FORESTSIZE = new Vector2(96,45); //32-15 ratio
        public const int FORESTSTARTINGDISTANCE = BACKGROUNDSTARTINGDISTANCE * 4 / 3;



        public static void SetCurrentStates()
        {
            CurrentKeyboardState = Keyboard.GetState();
        }

        public static void SetPreviousStates()
        {
            PreviousKeyboardState = CurrentKeyboardState;
        }

        public static bool IsSpaceDownFirstFrame()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.Space) && PreviousKeyboardState.IsKeyUp(Keys.Space);
        }

        public static void SetGameStateToControlScreen()
        {
            CURRENTGAMESTATE = GameState.ControlScreen;
        }

        public static void SetGameStateToPlaying()
        {
            CURRENTGAMESTATE = GameState.Playing;
        }

        public static void SetGameStateTogameOver()
        {
            CURRENTGAMESTATE = GameState.GameOver;
        }
    }
}

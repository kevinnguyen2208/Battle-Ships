using System;
using System.Collections.Generic;
using System.IO;
using SwinGameSDK;

public static class HighScoreController {
    private const int NAME_WIDTH = 3;

    private const int SCORES_LEFT = 490;

    private struct Score : IComparable {

        public string Name;

        public int Value;

        public int CompareTo (object obj) {
            if (obj is Score) {
                Score other = ((Score) (obj));
                return (other.Value - Value);
            } else {
                return 0;
            }

        }
    }

    private static List<Score> _Scores = new List<Score> ();

    private static void LoadScores () {
        string filename;
        filename = SwinGame.PathToResource ("highscores.txt");
        StreamReader input;
        input = new StreamReader (filename);

        int numScores;
        numScores = Convert.ToInt32 (input.ReadLine ());
        _Scores.Clear ();
        int i;
        for (i = 1;
            (i <= numScores); i++) {
            Score s;
            string line;
            line = input.ReadLine ();
            s.Name = line.Substring (0, NAME_WIDTH);
            s.Value = Convert.ToInt32 (line.Substring (NAME_WIDTH));
            _Scores.Add (s);
        }

        input.Close ();
    }

    private static void SaveScores () {
        string filename;
        filename = SwinGame.PathToResource ("highscores.txt");
        StreamWriter output;
        output = new StreamWriter (filename);
        output.WriteLine (_Scores.Count);
        foreach (Score s in _Scores) {
            output.WriteLine ((s.Name + s.Value));
        }

        output.Close ();
    }

    public static void DrawHighScores () {
        const int SCORES_HEADING = 40;
        const int SCORES_TOP = 80;
        const int SCORE_GAP = 30;
        if ((_Scores.Count == 0)) {
            HighScoreController.LoadScores ();
        }

        SwinGame.DrawText ("   High Scores   ", Color.White, GameResources.GameFont ("Courier"), SCORES_LEFT, SCORES_HEADING);

        for (int i = 0;
            (i <= (_Scores.Count - 1)); i++) {
            Score s;
            s = _Scores[i];

            if ((i < 9)) {
                SwinGame.DrawText ((" " + ((i + 1) + (":   " + (s.Name + ("   " + s.Value))))), Color.White, GameResources.GameFont ("Courier"), SCORES_LEFT, (SCORES_TOP + (i * SCORE_GAP)));
            } else {
                SwinGame.DrawText (((i + 1) + (":   " + (s.Name + ("   " + s.Value)))), Color.White, GameResources.GameFont ("Courier"), SCORES_LEFT, (SCORES_TOP + (i * SCORE_GAP)));
            }

        }

    }

    public static void HandleHighScoreInput () {
        if ((SwinGame.MouseClicked (MouseButton.LeftButton) || (SwinGame.KeyTyped (KeyCode.EscapeKey) || SwinGame.KeyTyped (KeyCode.ReturnKey)))) {
            GameController.EndCurrentState ();
        }
    }

    public static void ReadHighScore (int value) {
        const int ENTRY_TOP = 500;
        if ((_Scores.Count == 0)) {
            LoadScores ();
        }

        if ((value > _Scores[(_Scores.Count - 1)].Value)) {
            Score s = new Score ();
            s.Value = value;
            GameController.AddNewState (GameState.ViewingHighScores);
            int x;
            x = (SCORES_LEFT + SwinGame.TextWidth (GameResources.GameFont ("Courier"), "Name: "));
            SwinGame.StartReadingText (Color.White, NAME_WIDTH, GameResources.GameFont ("Courier"), x, ENTRY_TOP);

            while (SwinGame.ReadingText ()) {
                SwinGame.ProcessEvents ();
                UtilityFunctions.DrawBackground ();
                HighScoreController.DrawHighScores ();
                SwinGame.DrawText ("Name: ", Color.White, GameResources.GameFont ("Courier"), SCORES_LEFT, ENTRY_TOP);
                SwinGame.RefreshScreen ();
            }

            s.Name = SwinGame.TextReadAsASCII ();
            if ((s.Name.Length < 3)) {
                s.Name = (s.Name + new string (' ', 3 - s.Name.Length));
            }

            _Scores.RemoveAt ((_Scores.Count - 1));
            _Scores.Add (s);
            _Scores.Sort ();
            GameController.EndCurrentState ();
        }

    }
}
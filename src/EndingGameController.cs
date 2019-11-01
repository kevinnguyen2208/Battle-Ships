using SwinGameSDK;

public static class EndingGameController {

    public static void DrawEndOfGame () {
        Rectangle toDraw = new Rectangle ();
        UtilityFunctions.DrawField (GameController.ComputerPlayer.PlayerGrid, GameController.ComputerPlayer, true);
        UtilityFunctions.DrawSmallField (GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
        toDraw.X = 0;
        toDraw.Y = 250;
        toDraw.Width = SwinGame.ScreenWidth ();
        toDraw.Height = SwinGame.ScreenHeight ();

        string selectedText = GameController.HumanPlayer.IsDestroyed ? "YOU LOSE!" : "-- WINNER --";
        SwinGame.DrawText (selectedText, Color.White, Color.Transparent, GameResources.GameFont ("ArialLarge"), FontAlignment.AlignCenter, toDraw);
    }

    public static void HandleEndOfGameInput () {
        if ((SwinGame.MouseClicked (MouseButton.LeftButton) || (SwinGame.KeyTyped (KeyCode.ReturnKey) || SwinGame.KeyTyped (KeyCode.EscapeKey)))) {
            HighScoreController.ReadHighScore (GameController.HumanPlayer.Score);
            GameController.EndCurrentState ();
        }
    }
}
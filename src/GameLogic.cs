using System;
using SwinGameSDK;

public static class GameLogic {

    public static void Main () {

        SwinGame.OpenGraphicsWindow ("Battle Ships", 800, 600);

        GameResources.LoadResources ();

        GameController.Init ();

        do {
            GameController.HandleUserInput ();
            GameController.DrawScreen ();
        } while ((((SwinGame.WindowCloseRequested () == true) || (GameController.CurrentState == GameState.Quitting)) == false));

        SwinGame.StopMusic ();

        GameResources.FreeResources ();
    }

}
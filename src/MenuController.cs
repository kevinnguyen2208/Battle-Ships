using System;
using SwinGameSDK;

public static class MenuController {
    private static string[][] _menuStructure = new string[][] {
        new string[] { "PLAY", "SETUP", "SCORES", "QUIT" },
        new string[] { "RETURN", "SURRENDER", "QUIT" },
        new string[] { "EASY", "MEDIUM", "HARD" },
    };

    private const int MENU_TOP = 575;

    private const int MENU_LEFT = 30;

    private const int MENU_GAP = 0;

    private const int BUTTON_WIDTH = 75;

    private const int BUTTON_HEIGHT = 15;

    private const int BUTTON_SEP = (BUTTON_WIDTH + MENU_GAP);

    private const int TEXT_OFFSET = 0;

    private const int MAIN_MENU = 0;

    private const int GAME_MENU = 1;

    private const int SETUP_MENU = 2;

    private const int MAIN_MENU_PLAY_BUTTON = 0;

    private const int MAIN_MENU_SETUP_BUTTON = 1;

    private const int MAIN_MENU_TOP_SCORES_BUTTON = 2;

    private const int MAIN_MENU_QUIT_BUTTON = 3;

    private const int SETUP_MENU_EASY_BUTTON = 0;

    private const int SETUP_MENU_MEDIUM_BUTTON = 1;

    private const int SETUP_MENU_HARD_BUTTON = 2;

    private const int SETUP_MENU_EXIT_BUTTON = 3;

    private const int GAME_MENU_RETURN_BUTTON = 0;

    private const int GAME_MENU_SURRENDER_BUTTON = 1;

    private const int GAME_MENU_QUIT_BUTTON = 2;

    private static Color MENU_COLOR = SwinGame.RGBAColor (2, 167, 252, 255);

    private static Color HIGHLIGHT_COLOR = SwinGame.RGBAColor (1, 57, 86, 255);

    public static void HandleMainMenuInput () {
        HandleMenuInput (MAIN_MENU, 0, 0);
    }

    public static void HandleSetupMenuInput () {
        bool handled;
        handled = HandleMenuInput (SETUP_MENU, 1, 1);
        if (!handled) {
            HandleMenuInput (MAIN_MENU, 0, 0);
        }

    }

    public static void HandleGameMenuInput () {
        HandleMenuInput (GAME_MENU, 0, 0);
    }

    private static bool HandleMenuInput (int menu, int level, int xOffset) {
        if (SwinGame.KeyTyped (KeyCode.EscapeKey)) {
            GameController.EndCurrentState ();
            return true;
        }

        if (SwinGame.MouseClicked (MouseButton.LeftButton)) {
            for (int i = 0;
                (i <= (_menuStructure[menu].Length - 1)); i++) {

                if (IsMouseOverMenu (i, level, xOffset)) {
                    PerformMenuAction (menu, i);
                    return true;
                }
            }

            if ((level > 0)) {

                GameController.EndCurrentState ();
            }
        }

        return false;
    }

    public static void DrawMainMenu () {

        DrawButtons (MAIN_MENU);
    }

    public static void DrawGameMenu () {

        DrawButtons (GAME_MENU);
    }

    public static void DrawSettings () {

        DrawButtons (MAIN_MENU);
        DrawButtons (SETUP_MENU, 1, 1);
    }

    private static void DrawButtons (int menu) {
        DrawButtons (menu, 0, 0);
    }

    private static void DrawButtons (int menu, int level, int xOffset) {
        int btnTop = (MENU_TOP - ((MENU_GAP + BUTTON_HEIGHT) * level));
        Rectangle toDraw = new Rectangle ();

        for (int i = 0; i <= (_menuStructure[menu].Length - 1); i++) {
            int btnLeft = (MENU_LEFT + (BUTTON_SEP * (i + xOffset)));
            toDraw.X = (btnLeft + TEXT_OFFSET);
            toDraw.Y = (btnTop + TEXT_OFFSET);
            toDraw.Width = BUTTON_WIDTH;
            toDraw.Height = BUTTON_HEIGHT;

            SwinGame.DrawText (_menuStructure[menu][i], Color.White, btnLeft, btnTop);

            if ((SwinGame.MouseDown (MouseButton.LeftButton) && IsMouseOverMenu (i, level, xOffset))) {
                SwinGame.DrawRectangle (HIGHLIGHT_COLOR, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
            }
        }
    }

    private static bool IsMouseOverButton (int button) {
        return IsMouseOverMenu (button, 0, 0);
    }

    private static bool IsMouseOverMenu (int button, int level, int xOffset) {
        int btnTop = (MENU_TOP - ((MENU_GAP + BUTTON_HEIGHT) * level));
        int btnLeft = (MENU_LEFT + (BUTTON_SEP * (button + xOffset)));
        return UtilityFunctions.IsMouseInRectangle (btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
    }

    private static void PerformMenuAction (int menu, int button) {
        switch (menu) {
            case MAIN_MENU:
                PerformMainMenuAction (button);
                break;
            case SETUP_MENU:
                PerformSetupMenuAction (button);
                break;
            case GAME_MENU:
                PerformGameMenuAction (button);
                break;
        }
    }

    private static void PerformMainMenuAction (int button) {
        switch (button) {
            case MAIN_MENU_PLAY_BUTTON:
                GameController.StartGame ();
                break;
            case MAIN_MENU_SETUP_BUTTON:
                GameController.AddNewState (GameState.AlteringSettings);
                break;
            case MAIN_MENU_TOP_SCORES_BUTTON:
                GameController.AddNewState (GameState.ViewingHighScores);
                break;
            case MAIN_MENU_QUIT_BUTTON:
                GameController.EndCurrentState ();
                break;
        }
    }

    private static void PerformSetupMenuAction (int button) {
        switch (button) {
            case SETUP_MENU_EASY_BUTTON:
                GameController.SetDifficulty (AIOption.Hard);
                break;
            case SETUP_MENU_MEDIUM_BUTTON:
                GameController.SetDifficulty (AIOption.Hard);
                break;
            case SETUP_MENU_HARD_BUTTON:
                GameController.SetDifficulty (AIOption.Hard);
                break;
        }

        GameController.EndCurrentState ();
    }

    private static void PerformGameMenuAction (int button) {
        switch (button) {
            case GAME_MENU_RETURN_BUTTON:
                GameController.EndCurrentState ();
                break;
            case GAME_MENU_SURRENDER_BUTTON:
                GameController.EndCurrentState ();

                GameController.EndCurrentState ();

                break;
            case GAME_MENU_QUIT_BUTTON:
                GameController.AddNewState (GameState.Quitting);
                break;
        }
    }
}
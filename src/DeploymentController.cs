using System;
using SwinGameSDK;

public static class DeploymentController {
    private const int SHIPS_TOP = 98;

    private const int SHIPS_LEFT = 20;

    private const int SHIPS_HEIGHT = 90;

    private const int SHIPS_WIDTH = 300;

    private const int TOP_BUTTONS_TOP = 72;

    private const int TOP_BUTTONS_HEIGHT = 46;

    private const int PLAY_BUTTON_LEFT = 693;

    private const int PLAY_BUTTON_WIDTH = 80;

    private const int UP_DOWN_BUTTON_LEFT = 410;

    private const int LEFT_RIGHT_BUTTON_LEFT = 350;

    private const int RANDOM_BUTTON_LEFT = 547;

    private const int RANDOM_BUTTON_WIDTH = 51;

    private const int DIR_BUTTONS_WIDTH = 47;

    private const int TEXT_OFFSET = 5;

    private static Direction _currentDirection = Direction.UpDown;

    private static ShipName _selectedShip = ShipName.Tug;

    public static void HandleDeploymentInput () {
        if (SwinGame.KeyTyped (KeyCode.EscapeKey)) {
            GameController.AddNewState (GameState.ViewingGameMenu);
        }

        if ((SwinGame.KeyTyped (KeyCode.UpKey) || SwinGame.KeyTyped (KeyCode.DownKey))) {
            _currentDirection = Direction.UpDown;
        }

        if ((SwinGame.KeyTyped (KeyCode.LeftKey) || SwinGame.KeyTyped (KeyCode.RightKey))) {
            _currentDirection = Direction.LeftRight;
        }

        if (SwinGame.KeyTyped (KeyCode.RKey)) {
            GameController.HumanPlayer.RandomizeDeployment ();
        }

        if (SwinGame.MouseClicked (MouseButton.LeftButton)) {
            ShipName selected;
            selected = GetShipMouseIsOver ();
            if ((selected != ShipName.None)) {
                _selectedShip = selected;
            } else {
                DoDeployClick ();
            }

            if ((GameController.HumanPlayer.ReadyToDeploy && UtilityFunctions.IsMouseInRectangle (PLAY_BUTTON_LEFT, TOP_BUTTONS_TOP, PLAY_BUTTON_WIDTH, TOP_BUTTONS_HEIGHT))) {
                GameController.EndDeployment ();
            } else if (UtilityFunctions.IsMouseInRectangle (UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT)) {
                _currentDirection = Direction.UpDown;
            } else if (UtilityFunctions.IsMouseInRectangle (LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT)) {
                _currentDirection = Direction.LeftRight;
            } else if (UtilityFunctions.IsMouseInRectangle (RANDOM_BUTTON_LEFT, TOP_BUTTONS_TOP, RANDOM_BUTTON_WIDTH, TOP_BUTTONS_HEIGHT)) {
                GameController.HumanPlayer.RandomizeDeployment ();
            }

        }

    }

    private static void DoDeployClick () {
        Point2D mouse = SwinGame.MousePosition ();

        int row = Convert.ToInt32 (Math.Floor (((mouse.Y - UtilityFunctions.FIELD_TOP) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP))));
        int col = Convert.ToInt32 (Math.Floor (((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP))));

        if (((row >= 0) && (row < GameController.HumanPlayer.PlayerGrid.Height))) {
            if (((col >= 0) && (col < GameController.HumanPlayer.PlayerGrid.Width))) {

                try {
                    GameController.HumanPlayer.PlayerGrid.MoveShip (row, col, _selectedShip, _currentDirection);
                } catch (Exception ex) {
                    Audio.PlaySoundEffect (GameResources.GameSound ("Error"));
                    UtilityFunctions.Message = ex.Message;
                }

            }

        }

    }

    public static void DrawDeployment () {
        UtilityFunctions.DrawField (GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer, true);

        if ((_currentDirection == Direction.LeftRight)) {
            SwinGame.DrawBitmap (GameResources.GameImage ("LeftRightButton"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP);

        } else {
            SwinGame.DrawBitmap (GameResources.GameImage ("UpDownButton"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP);

        }

        foreach (ShipName sn in Enum.GetValues (typeof (ShipName))) {
            int i = (int) (sn) - 1;

            if ((i >= 0)) {
                if ((sn == _selectedShip)) {
                    SwinGame.DrawBitmap (GameResources.GameImage ("SelectedShip"), SHIPS_LEFT, (SHIPS_TOP + (i * SHIPS_HEIGHT)));
                }

                SwinGame.DrawText (sn.ToString (), Color.White, GameResources.GameFont ("Courier"), SHIPS_LEFT + TEXT_OFFSET, SHIPS_TOP + i * SHIPS_HEIGHT);
            }

        }

        if (GameController.HumanPlayer.ReadyToDeploy) {
            SwinGame.DrawBitmap (GameResources.GameImage ("PlayButton"), PLAY_BUTTON_LEFT, TOP_BUTTONS_TOP);

        }

        SwinGame.DrawBitmap (GameResources.GameImage ("RandomButton"), RANDOM_BUTTON_LEFT, TOP_BUTTONS_TOP);
        UtilityFunctions.DrawMessage ();
    }

    private static ShipName GetShipMouseIsOver () {
        foreach (ShipName sn in Enum.GetValues (typeof (ShipName))) {
            int i = (int) (sn) - 1;
            if (UtilityFunctions.IsMouseInRectangle (SHIPS_LEFT, (SHIPS_TOP + (i * SHIPS_HEIGHT)), SHIPS_WIDTH, SHIPS_HEIGHT)) {
                return sn;
            }
        }

        return ShipName.None;
    }
}
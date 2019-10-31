using System;
using System.Collections.Generic;
using SwinGameSDK;

public static class GameController {
    private static BattleShipsGame _theGame;

    private static Player _human;

    private static AIPlayer _ai;

    private static Stack<GameState> _state = new Stack<GameState> ();

    private static AIOption _aiSetting;

    public static GameState CurrentState {
        get {
            return _state.Peek ();
        }
    }

    public static Player HumanPlayer {
        get {
            return _human;
        }
    }

    public static Player ComputerPlayer {
        get {
            return _ai;
        }
    }

    public static void Init () {
        Audio.PlaySoundEffect (GameResources.GameSound ("Music"));

        _state.Push (GameState.Quitting);

        _state.Push (GameState.ViewingMainMenu);
    }

    public static void StartGame () {

        if (_theGame != null) {
            EndGame ();
        }

        _theGame = new BattleShipsGame ();

        switch (_aiSetting) {
            case AIOption.Medium:
                _ai = new AIMediumPlayer (_theGame);
                break;
            case AIOption.Hard:
                _ai = new AIHardPlayer (_theGame);
                break;
            default:
                _ai = new AIHardPlayer (_theGame);
                break;
        }
        _human = new Player (_theGame);

        _human.PlayerGrid.Changed += new EventHandler (GridChanged);
        _ai.PlayerGrid.Changed += new EventHandler (GridChanged);

        _theGame.AttackCompleted += new BattleShipsGame.AttackCompletedHandler (AttackCompleted);
        AddNewState (GameState.Deploying);

    }

    private static void EndGame () {

        _ai.PlayerGrid.Changed -= GridChanged;
        _theGame.AttackCompleted -= AttackCompleted;
    }

    public static void GridChanged (object sender, EventArgs args) {
        GameController.DrawScreen ();
        SwinGame.RefreshScreen ();
    }

    private static void PlayHitSequence (int row, int column, bool showAnimation) {
        if (showAnimation) {
            UtilityFunctions.AddExplosion (row, column);
        }

        Audio.PlaySoundEffect (GameResources.GameSound ("Hit"));
        UtilityFunctions.DrawAnimationSequence ();
    }

    private static void PlayMissSequence (int row, int column, bool showAnimation) {
        if (showAnimation) {
            UtilityFunctions.AddSplash (row, column);
        }

        Audio.PlaySoundEffect (GameResources.GameSound ("Miss"));
        UtilityFunctions.DrawAnimationSequence ();
    }

    private static void AttackCompleted (object sender, AttackResult result) {
        bool isHuman;
        isHuman = (_theGame.Player == HumanPlayer);
        if (isHuman) {
            UtilityFunctions.Message = ("You " + result.ToString ());
        } else {
            UtilityFunctions.Message = ("The AI " + result.ToString ());
        }

        switch (result.Value) {
            case ResultOfAttack.Destroyed:
                PlayHitSequence (result.Row, result.Column, isHuman);
                Audio.PlaySoundEffect (GameResources.GameSound ("Sink"));
                break;
            case ResultOfAttack.GameOver:
                PlayHitSequence (result.Row, result.Column, isHuman);
                Audio.PlaySoundEffect (GameResources.GameSound ("Sink"));
                while (Audio.SoundEffectPlaying (GameResources.GameSound ("Sink"))) {

                    SwinGame.RefreshScreen ();
                }

                if (HumanPlayer.IsDestroyed) {
                    Audio.PlaySoundEffect (GameResources.GameSound ("Lose"));
                } else {
                    Audio.PlaySoundEffect (GameResources.GameSound ("Winner"));
                }

                break;
            case ResultOfAttack.Hit:
                PlayHitSequence (result.Row, result.Column, isHuman);
                break;
            case ResultOfAttack.Miss:
                PlayMissSequence (result.Row, result.Column, isHuman);
                break;
            case ResultOfAttack.ShotAlready:
                Audio.PlaySoundEffect (GameResources.GameSound ("Error"));
                break;
        }
    }

    public static void EndDeployment () {

        _theGame.AddDeployedPlayer (_human);
        _theGame.AddDeployedPlayer (_ai);
        SwitchState (GameState.Discovering);
    }

    public static void Attack (int row, int col) {
        AttackResult result = _theGame.Shoot (row, col);
        CheckAttackResult (result);
    }

    private static void AIAttack () {
        AttackResult result = _theGame.Player.Attack ();
        CheckAttackResult (result);
    }

    private static void CheckAttackResult (AttackResult result) {
        switch (result.Value) {
            case ResultOfAttack.Miss:
                if ((_theGame.Player == ComputerPlayer)) {
                    AIAttack ();
                }

                break;
            case ResultOfAttack.GameOver:
                SwitchState (GameState.EndingGame);
                break;
        }
    }

    public static void HandleUserInput () {

        SwinGame.ProcessEvents ();
        switch (CurrentState) {
            case GameState.ViewingMainMenu:
                MenuController.HandleMainMenuInput ();
                break;
            case GameState.ViewingGameMenu:
                MenuController.HandleGameMenuInput ();
                break;
            case GameState.AlteringSettings:
                MenuController.HandleSetupMenuInput ();
                break;
            case GameState.Deploying:
                DeploymentController.HandleDeploymentInput ();
                break;
            case GameState.Discovering:
                DiscoveryController.HandleDiscoveryInput ();
                break;
            case GameState.EndingGame:
                EndingGameController.HandleEndOfGameInput ();
                break;
            case GameState.ViewingHighScores:
                HighScoreController.HandleHighScoreInput ();
                break;
        }
        UtilityFunctions.UpdateAnimations ();
    }

    public static void DrawScreen () {
        UtilityFunctions.DrawBackground ();
        switch (CurrentState) {
            case GameState.ViewingMainMenu:
                MenuController.DrawMainMenu ();
                break;
            case GameState.ViewingGameMenu:
                MenuController.DrawGameMenu ();
                break;
            case GameState.AlteringSettings:
                MenuController.DrawSettings ();
                break;
            case GameState.Deploying:
                DeploymentController.DrawDeployment ();
                break;
            case GameState.Discovering:
                DiscoveryController.DrawDiscovery ();
                break;
            case GameState.EndingGame:
                EndingGameController.DrawEndOfGame ();
                break;
            case GameState.ViewingHighScores:
                HighScoreController.DrawHighScores ();
                break;
        }
        UtilityFunctions.DrawAnimations ();
        SwinGame.RefreshScreen ();
    }

    public static void AddNewState (GameState state) {
        _state.Push (state);
        UtilityFunctions.Message = "";
    }

    public static void SwitchState (GameState newState) {
        GameController.EndCurrentState ();
        GameController.AddNewState (newState);
    }

    public static void EndCurrentState () {
        _state.Pop ();
    }

    public static void SetDifficulty (AIOption setting) {
        _aiSetting = setting;
    }
}
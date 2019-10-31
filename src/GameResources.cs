using System;
using System.Collections.Generic;
using SwinGameSDK;

public static class GameResources {

    private static void LoadFonts () {
        NewFont ("ArialLarge", "Bungee-Regular.otf", 80);
        NewFont ("Courier", "Bungee-Regular.otf", 14);
        NewFont ("CourierSmall", "Bungee-Regular.otf", 8);
        NewFont ("Menu", "Bungee-Regular.otf", 8);
    }

    private static void LoadImages () {

        NewImage ("Menu", "main_page.jpg");
        NewImage ("Discovery", "discover.jpg");
        NewImage ("Deploy", "deploy.jpg");

        NewImage ("LeftRightButton", "deploy_dir_button_horiz.png");
        NewImage ("UpDownButton", "deploy_dir_button_vert.png");
        NewImage ("SelectedShip", "deploy_button_hl.png");
        NewImage ("PlayButton", "deploy_play_button.png");
        NewImage ("RandomButton", "deploy_randomize_button.png");

        int i;
        for (i = 1;
            (i <= 5); i++) {
            NewImage (("ShipLR" + i), ("ship_deploy_horiz_" +
                (i + ".png")));
            NewImage (("ShipUD" + i), ("ship_deploy_vert_" +
                (i + ".png")));
        }

        NewImage ("Explosion", "explosion.png");
        NewImage ("Splash", "splash.png");
    }

    private static void LoadSounds () {
        NewSound ("Error", "error.wav");
        NewSound ("Hit", "hit.wav");
        NewSound ("Sink", "sink.wav");

        NewSound ("Miss", "watershot.wav");
        NewSound ("Winner", "winner.wav");
        NewSound ("Lose", "lose.wav");
        NewSound ("Music", "music.wav");
    }

    private static void LoadMusic () {
        NewMusic ("Background", "horrordrone.mp3");
    }

    public static Font GameFont (string font) {
        return _Fonts[font];
    }

    public static Bitmap GameImage (string image) {
        return _Images[image];
    }

    public static SoundEffect GameSound (string sound) {
        return _Sounds[sound];
    }

    public static Music GameMusic (string music) {
        return _Music[music];
    }

    private static Dictionary<string, Bitmap> _Images = new Dictionary<string, Bitmap> ();

    private static Dictionary<string, Font> _Fonts = new Dictionary<string, Font> ();

    private static Dictionary<string, SoundEffect> _Sounds = new Dictionary<string, SoundEffect> ();

    private static Dictionary<string, Music> _Music = new Dictionary<string, Music> ();

    private static Bitmap _Background;

    private static Bitmap _Animation;

    private static Bitmap _LoaderFull;

    private static Bitmap _LoaderEmpty;

    private static Font _LoadingFont;

    private static SoundEffect _StartSound;

    public static void LoadResources () {
        int width;
        int height;
        width = SwinGame.ScreenWidth ();
        height = SwinGame.ScreenHeight ();
        SwinGame.ChangeScreenSize (800, 600);
        ShowLoadingScreen ();
        ShowMessage ("Loading fonts...", 0);
        LoadFonts ();

        ShowMessage ("Loading images...", 1);
        LoadImages ();

        ShowMessage ("Loading sounds...", 2);
        LoadSounds ();

        ShowMessage ("Loading music...", 3);
        LoadMusic ();

        ShowMessage ("Game loaded...", 5);

        EndLoadingScreen (width, height);
    }

    private static void ShowLoadingScreen () {
        _Background = SwinGame.LoadBitmap (SwinGame.PathToResource ("SplashBack.png", ResourceKind.BitmapResource));
        SwinGame.DrawBitmap (_Background, 0, 0);
        SwinGame.RefreshScreen ();
        SwinGame.ProcessEvents ();
        _Animation = SwinGame.LoadBitmap (SwinGame.PathToResource ("SwinGameAni.jpg", ResourceKind.BitmapResource));
        _LoadingFont = SwinGame.LoadFont (SwinGame.PathToResource ("arial.ttf", ResourceKind.FontResource), 12);
        _StartSound = Audio.LoadSoundEffect (SwinGame.PathToResource ("SwinGameStart.ogg", ResourceKind.SoundResource));
        _LoaderFull = SwinGame.LoadBitmap (SwinGame.PathToResource ("loader_full.png", ResourceKind.BitmapResource));
        _LoaderEmpty = SwinGame.LoadBitmap (SwinGame.PathToResource ("loader_empty.png", ResourceKind.BitmapResource));
        PlaySwinGameIntro ();
    }

    private static void PlaySwinGameIntro () {
        const int ANI_CELL_COUNT = 11;
        Audio.PlaySoundEffect (_StartSound);

        int i;
        for (i = 0;
            (i <= (ANI_CELL_COUNT - 1)); i++) {
            SwinGame.DrawBitmap (_Background, 0, 0);

            SwinGame.RefreshScreen ();
            SwinGame.ProcessEvents ();
        }

    }

    private static void ShowMessage (string message, int number) {
        const int BG_Y = 453;
        const int TX = 310;
        const int TY = 493;
        const int TW = 200;
        const int TH = 25;

        const int BG_X = 279;

        SwinGame.DrawBitmap (_LoaderEmpty, BG_X, BG_Y);
        SwinGame.DrawCell (_LoaderFull, 0, BG_X, BG_Y);

        Rectangle toDraw = new Rectangle ();
        toDraw.X = TX;
        toDraw.Y = TY;
        toDraw.Width = TW;
        toDraw.Height = TH;

        SwinGame.DrawText (message, Color.White, Color.Transparent, _LoadingFont, FontAlignment.AlignCenter, toDraw);
        SwinGame.DrawText (message, Color.White, Color.Transparent, _LoadingFont, FontAlignment.AlignCenter, toDraw);

        SwinGame.RefreshScreen ();
        SwinGame.ProcessEvents ();
    }

    private static void EndLoadingScreen (int width, int height) {
        SwinGame.ProcessEvents ();

        SwinGame.ClearScreen ();
        SwinGame.RefreshScreen ();
        SwinGame.FreeFont (_LoadingFont);
        SwinGame.FreeBitmap (_Background);
        SwinGame.FreeBitmap (_Animation);
        SwinGame.FreeBitmap (_LoaderEmpty);
        SwinGame.FreeBitmap (_LoaderFull);

        SwinGame.ChangeScreenSize (width, height);
    }

    private static void NewFont (string fontName, string filename, int size) {
        _Fonts.Add (fontName, SwinGame.LoadFont (SwinGame.PathToResource (filename, ResourceKind.FontResource), size));
    }

    private static void NewImage (string imageName, string filename) {
        _Images.Add (imageName, SwinGame.LoadBitmap (SwinGame.PathToResource (filename, ResourceKind.BitmapResource)));
    }

    private static void NewTransparentColorImage (string imageName, string fileName, Color transColor) {
        _Images.Add (imageName, SwinGame.LoadBitmap (SwinGame.PathToResource (fileName, ResourceKind.BitmapResource)));
    }

    private static void NewTransparentColourImage (string imageName, string fileName, Color transColor) {
        NewTransparentColorImage (imageName, fileName, transColor);
    }

    private static void NewSound (string soundName, string filename) {
        _Sounds.Add (soundName, Audio.LoadSoundEffect (SwinGame.PathToResource (filename, ResourceKind.SoundResource)));
    }

    private static void NewMusic (string musicName, string filename) {
        _Music.Add (musicName, Audio.LoadMusic (SwinGame.PathToResource (filename, ResourceKind.SoundResource)));
    }

    private static void FreeFonts () {
        foreach (Font obj in _Fonts.Values) {
            SwinGame.FreeFont (obj);
        }
    }

    private static void FreeImages () {
        foreach (Bitmap obj in _Images.Values) {
            SwinGame.FreeBitmap (obj);
        }
    }

    private static void FreeSounds () {

    }

    private static void FreeMusic () {
        foreach (Music obj in _Music.Values) {
            Audio.FreeMusic (obj);
        }
    }

    public static void FreeResources () {
        FreeFonts ();
        FreeImages ();
        FreeMusic ();
        FreeSounds ();
        SwinGame.ProcessEvents ();
    }
}
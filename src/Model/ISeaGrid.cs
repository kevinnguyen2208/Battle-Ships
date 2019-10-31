using System;

public interface ISeaGrid {
    int Width {
        get;
    }

    int Height {
        get;
    }

    event EventHandler Changed;

    TileView this [int x, int y] {
        get;
    }

    AttackResult HitTile (int row, int col);
}
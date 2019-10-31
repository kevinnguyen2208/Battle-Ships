using System;

public class SeaGridAdapter : ISeaGrid {

    private SeaGrid _MyGrid;

    public SeaGridAdapter (SeaGrid grid) {
        _MyGrid = grid;
        _MyGrid.Changed += new EventHandler (MyGrid_Changed);

        Changed += new EventHandler (GameController.GridChanged);
    }

    private void MyGrid_Changed (object sender, EventArgs e) {
        Changed (this, e);
    }

    public TileView this [int x, int y] {
        get {
            TileView result = _MyGrid[x, y];

            if (result == TileView.Ship) {
                return TileView.Sea;
            } else {
                return result;
            }
        }
    }

    public event EventHandler Changed;

    public int Width {
        get {
            return _MyGrid.Width;
        }
    }

    public int Height {
        get {
            return _MyGrid.Height;
        }
    }

    public AttackResult HitTile (int row, int col) {
        return _MyGrid.HitTile (row, col);
    }

}
using System;
using System.Collections.Generic;

public class SeaGrid : ISeaGrid {

    private const int _WIDTH = 10;

    private const int _HEIGHT = 10;

    private Tile[, ] _GameTiles;

    private Dictionary<ShipName, Ship> _Ships;

    private int _ShipsKilled = 0;

    public event EventHandler Changed;

    public int Width {
        get {
            return _WIDTH;
        }
    }

    public int Height {
        get {
            return _HEIGHT;
        }
    }

    public int ShipsKilled {
        get {
            return _ShipsKilled;
        }
    }

    public TileView this [int x, int y] {
        get {
            return _GameTiles[x, y].View;
        }
    }

    public bool AllDeployed {
        get {
            foreach (Ship s in _Ships.Values) {
                if (!s.IsDeployed) {
                    return false;
                }
            }
            return true;
        }
    }

    public SeaGrid (Dictionary<ShipName, Ship> ships) {

        _GameTiles = new Tile[Width, Height];
        for (int i = 0;
            (i <= (Width - 1)); i++) {
            for (int j = 0;
                (j <= (Height - 1)); j++) {
                _GameTiles[i, j] = new Tile (i, j, null);
            }
        }

        _Ships = ships;
    }

    public void MoveShip (int row, int col, ShipName ship, Direction direction) {
        Ship newShip = _Ships[ship];
        newShip.Remove ();
        AddShip (row, col, direction, newShip);
    }

    private void AddShip (int row, int col, Direction direction, Ship newShip) {
        try {
            int size = newShip.Size;
            int currentRow = row;
            int currentCol = col;
            int dRow;
            int dCol;

            if ((direction == Direction.LeftRight)) {
                dRow = 0;
                dCol = 1;
            } else {
                dRow = 1;
                dCol = 0;
            }

            for (int i = 0;
                (i <= (size - 1)); i++) {
                if (((currentRow < 0) || ((currentRow >= Width) || ((currentCol < 0) || (currentCol >= Height))))) {
                    throw new InvalidOperationException ("Ship can\'t fit on the board");
                }
                _GameTiles[currentRow, currentCol].Ship = newShip;
                currentCol = (currentCol + dCol);
                currentRow = (currentRow + dRow);
            }

            newShip.Deployed (direction, row, col);
        } catch (Exception e) {
            newShip.Remove ();

            throw new ApplicationException (e.Message);
        } finally {

        }
    }

    public AttackResult HitTile (int row, int col) {
        try {

            if (_GameTiles[row, col].Shot) {
                return new AttackResult (ResultOfAttack.ShotAlready, ("have already attacked [" +
                    (col + ("," +
                        (row + "]!")))), row, col);
            }

            _GameTiles[row, col].Shoot ();

            if ((_GameTiles[row, col].Ship == null)) {
                return new AttackResult (ResultOfAttack.Miss, "missed", row, col);
            }

            if (_GameTiles[row, col].Ship.IsDestroyed) {
                _GameTiles[row, col].Shot = true;
                _ShipsKilled++;
                return new AttackResult (ResultOfAttack.Destroyed, _GameTiles[row, col].Ship, "destroyed the enemy\'s", row, col);
            }

            return new AttackResult (ResultOfAttack.Hit, "hit something!", row, col);
        } finally {
            Changed (this, EventArgs.Empty);
        }
    }

}
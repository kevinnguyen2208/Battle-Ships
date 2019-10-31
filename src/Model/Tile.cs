using System;

public class Tile {

    private int _RowValue;

    private int _ColumnValue;

    private Ship _Ship;

    private bool _Shot;

    public bool Shot {
        get {
            return _Shot;
        }
        set {
            _Shot = value;
        }
    }

    public int Row {
        get {
            return _RowValue;
        }
    }

    public int Column {
        get {
            return _ColumnValue;
        }
    }

    public Ship Ship {
        get {
            return _Ship;
        }
        set {
            if (_Ship == null) {
                _Ship = value;
                if (value != null) {
                    _Ship.AddTile (this);
                }

            } else {
                throw new InvalidOperationException (("There is already a ship at [" + (Row + (", " + (Column + "]")))));
            }

        }
    }

    public Tile (int row, int col, Ship ship) {
        _RowValue = row;
        _ColumnValue = col;
        _Ship = ship;
    }

    public void ClearShip () {
        _Ship = null;
    }

    public TileView View {
        get {

            if (_Ship == null) {

                if (_Shot) {
                    return TileView.Miss;
                } else {

                    return TileView.Sea;
                }
            } else {

                if (_Shot) {
                    return TileView.Hit;
                } else {

                    return TileView.Ship;
                }
            }
        }
    }

    internal void Shoot () {
        if (false == Shot) {
            Shot = true;
            if (_Ship != null) {
                _Ship.Hit ();
            }

        } else {
            throw new ApplicationException ("You have already shot this square");
        }

    }
}
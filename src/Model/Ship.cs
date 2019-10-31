using System.Collections.Generic;

public class Ship {
    private ShipName _shipName;

    private int _sizeOfShip;

    private int _hitsTaken = 0;

    private List<Tile> _tiles;

    private int _row;

    private int _col;

    private Direction _direction;

    public string Name {
        get {
            if ((_shipName == ShipName.AircraftCarrier)) {
                return "Aircraft Carrier";
            }

            return _shipName.ToString ();
        }
    }

    public int Size {
        get {
            return _sizeOfShip;
        }
    }

    public int Hits {
        get {
            return _hitsTaken;
        }
    }

    public int Row {
        get {
            return _row;
        }
    }

    public int Column {
        get {
            return _col;
        }
    }

    public Direction Direction {
        get {
            return _direction;
        }
    }

    public Ship (ShipName ship) {
        _shipName = ship;
        _tiles = new List<Tile> ();

        _sizeOfShip = (int) (_shipName);
    }

    public void AddTile (Tile tile) {
        _tiles.Add (tile);
    }

    public void Remove () {
        foreach (Tile tile in _tiles) {
            tile.ClearShip ();
        }

        _tiles.Clear ();
    }

    public void Hit () {
        _hitsTaken = (_hitsTaken + 1);
    }

    public bool IsDeployed {
        get {
            return (_tiles.Count > 0);
        }
    }

    public bool IsDestroyed {
        get {
            return Hits == Size;
        }
    }

    internal void Deployed (Direction direction, int row, int col) {
        _row = row;
        _col = col;
        _direction = direction;
    }
}
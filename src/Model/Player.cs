using System;
using System.Collections;
using System.Collections.Generic;

public class Player : IEnumerable {
    protected static Random _Random = new Random ();

    private Dictionary<ShipName, Ship> _Ships = new Dictionary<ShipName, Ship> ();

    private SeaGrid _playerGrid;

    private ISeaGrid _enemyGrid;

    protected BattleShipsGame _game;

    private int _shots;

    private int _hits;

    private int _misses;

    public BattleShipsGame Game {
        get {
            return _game;
        }
        set {
            _game = value;
        }
    }

    public ISeaGrid Enemy {
        set {
            _enemyGrid = value;
        }
    }

    public Player (BattleShipsGame controller) {
        _playerGrid = new SeaGrid (_Ships);

        _game = controller;

        foreach (ShipName name in Enum.GetValues (typeof (ShipName))) {
            if ((name != ShipName.None)) {
                _Ships.Add (name, new Ship (name));
            }

        }

        RandomizeDeployment ();
    }

    public ISeaGrid EnemyGrid {
        get {
            return _enemyGrid;
        }
        set {
            _enemyGrid = value;
        }
    }

    public SeaGrid PlayerGrid {
        get {
            return _playerGrid;
        }
    }

    public bool ReadyToDeploy {
        get {
            return _playerGrid.AllDeployed;
        }
    }

    public bool IsDestroyed {
        get {

            return _playerGrid.ShipsKilled == Enum.GetValues (typeof (ShipName)).Length - 1;
        }
    }

    public Ship Ship (ShipName name) {
        if ((name == ShipName.None)) {
            return null;
        }

        return _Ships[name];
    }

    public int Shots {
        get {
            return _shots;
        }
    }

    public int Hits {
        get {
            return _hits;
        }
    }

    public int Missed {
        get {
            return _misses;
        }
    }

    public int Score {
        get {
            if (IsDestroyed) {
                return 0;
            } else {
                return ((Hits * 12) - (Shots - (PlayerGrid.ShipsKilled * 20)));
            }
        }
    }

    public IEnumerator<Ship> GetShipEnumerator () {
        Ship[] result = new Ship[_Ships.Values.Count];
        _Ships.Values.CopyTo (result, 0);
        List<Ship> lst = new List<Ship> ();
        lst.AddRange (result);
        return lst.GetEnumerator ();
    }

    public IEnumerator GetEnumerator () {
        Ship[] result = new Ship[_Ships.Values.Count];
        _Ships.Values.CopyTo (result, 0);
        List<Ship> lst = new List<Ship> ();
        lst.AddRange (result);
        return lst.GetEnumerator ();
    }

    public virtual AttackResult Attack () {

        return null;
    }

    internal AttackResult Shoot (int row, int col) {
        _shots++;
        AttackResult result = EnemyGrid.HitTile (row, col);
        switch (result.Value) {
            case ResultOfAttack.Destroyed:
            case ResultOfAttack.Hit:
                _hits++;
                break;
            case ResultOfAttack.Miss:
                _misses++;
                break;
        }
        return result;
    }

    public virtual void RandomizeDeployment () {
        bool placementSuccessful;
        Direction heading;

        foreach (ShipName shipToPlace in Enum.GetValues (typeof (ShipName))) {
            if ((shipToPlace == ShipName.None)) {
                continue;
            }

            placementSuccessful = false;
            while (!placementSuccessful) {
                int dir = _Random.Next (2);
                int x = _Random.Next (0, 11);
                int y = _Random.Next (0, 11);
                if ((dir == 0)) {
                    heading = Direction.UpDown;
                } else {
                    heading = Direction.LeftRight;
                }

                try {
                    PlayerGrid.MoveShip (x, y, shipToPlace, heading);
                    placementSuccessful = true;
                } catch {
                    placementSuccessful = false;
                }
            }

        }
    }

}
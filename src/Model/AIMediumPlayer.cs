using System;
using System.Collections.Generic;

public class AIMediumPlayer : AIPlayer {

    private enum AIStates {
        Searching,
        TargetingShip,
    }

    private AIStates _CurrentState = AIStates.Searching;

    private Stack<Location> _Targets = new Stack<Location> ();

    public AIMediumPlayer (BattleShipsGame controller) : base (controller) { }

    protected override void GenerateCoords (ref int row, ref int column) {
        while (((row < 0) || ((column < 0) || ((row >= EnemyGrid.Height) || ((column >= EnemyGrid.Width) || (EnemyGrid[row, column] != TileView.Sea)))))) {
            switch (_CurrentState) {
                case AIStates.Searching:
                    SearchCoords (ref row, ref column);
                    break;
                case AIStates.TargetingShip:
                    TargetCoords (ref row, ref column);
                    break;
                default:
                    throw new ApplicationException ("AI has gone in an imvalid state");
            }
        }
    }

    private void TargetCoords (ref int row, ref int column) {
        Location l = _Targets.Pop ();
        if ((_Targets.Count == 0)) {
            _CurrentState = AIStates.Searching;
        }

        row = l.Row;
        column = l.Column;
    }

    private void SearchCoords (ref int row, ref int column) {
        row = _Random.Next (0, EnemyGrid.Height);
        column = _Random.Next (0, EnemyGrid.Width);
    }

    protected override void ProcessShot (int row, int col, AttackResult result) {
        if (result.Value == ResultOfAttack.Hit) {
            _CurrentState = AIStates.TargetingShip;
            AddTarget ((row - 1), col);
            AddTarget (row, (col - 1));
            AddTarget ((row + 1), col);
            AddTarget (row, (col + 1));
        } else if ((result.Value == ResultOfAttack.ShotAlready)) {
            throw new ApplicationException ("Error in AI");
        }
    }

    private void AddTarget (int row, int column) {
        if (((row >= 0) && ((column >= 0) && ((row < EnemyGrid.Height) && ((column < EnemyGrid.Width) && (EnemyGrid[row, column] == TileView.Sea)))))) {
            _Targets.Push (new Location (row, column));
        }

    }
}
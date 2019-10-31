using SwinGameSDK;

public abstract class AIPlayer : Player {

    protected class Location {
        private int _Row;
        private int _Column;

        public int Row {
            get {
                return _Row;
            }
            set {
                _Row = value;
            }
        }

        public int Column {
            get {
                return _Column;
            }
            set {
                _Column = value;
            }
        }

        public Location (int row, int column) {
            _Column = column;
            _Row = row;
        }

        public static bool operator == (Location left, Location right) {
            if (object.ReferenceEquals (left, right)) {
                return true;
            }
            if (object.ReferenceEquals (left, null) || object.ReferenceEquals (right, null)) {
                return false;
            }

            return left.Row == right.Row && left.Column == right.Column;
        }

        public static bool operator != (Location left, Location right) {
            return !(left == right);
        }

    }

    public AIPlayer (BattleShipsGame game) : base (game) { }

    protected abstract void GenerateCoords (ref int row, ref int column);

    protected abstract void ProcessShot (int row, int column, AttackResult result);

    public override AttackResult Attack () {
        AttackResult result;
        int row = 0;
        int column = 0;

        do {
            Delay ();

            GenerateCoords (ref row, ref column);
            result = _game.Shoot (row, column);
            ProcessShot (row, column, result);
        } while (result.Value != ResultOfAttack.Miss && result.Value != ResultOfAttack.GameOver && !SwinGame.WindowCloseRequested ());

        return result;
    }

    private void Delay () {

        if (SwinGame.WindowCloseRequested ()) return;

        SwinGame.ProcessEvents ();
        SwinGame.RefreshScreen ();

    }

}
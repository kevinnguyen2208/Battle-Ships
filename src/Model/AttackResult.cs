public class AttackResult {
    private ResultOfAttack _Value;

    private Ship _Ship;

    private string _Text;

    private int _Row;

    private int _Column;

    public ResultOfAttack Value {
        get {
            return _Value;
        }
    }

    public Ship Ship {
        get {
            return _Ship;
        }
    }

    public string Text {
        get {
            return _Text;
        }
    }

    public int Row {
        get {
            return _Row;
        }
    }

    public int Column {
        get {
            return _Column;
        }
    }

    public AttackResult (ResultOfAttack value, string text, int row, int column) {
        _Value = value;
        _Text = text;
        _Ship = null;
        _Row = row;
        _Column = column;
    }

    public AttackResult (ResultOfAttack value, Ship ship, string text, int row, int column):
        this (value, text, row, column) {
            _Ship = ship;
        }

    public override string ToString () {
        if ((_Ship == null)) {
            return Text;
        }

        return (Text + (" " + _Ship.Name));
    }
}
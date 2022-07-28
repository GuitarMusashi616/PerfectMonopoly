using System;
using System.Collections;
using System.Collections.Generic;

public class PostRollState : State {
	public PostRollState(Board board) : base(board) {

	}

	public override void Roll ()
	{
		if (!_board.Dice.Doubles) {
			throw new Exception("Already Rolled");
		}

		if (_board.Dice.DoubleCount > 3) {
			_board.Teleport (30);
			throw new Exception ("Go To Jail");
		}
		defaultRoll ();
	}

	public override void EndTurn ()
	{
		if (_board.Dice.Doubles) {
			throw new Exception ("Got Doubles, Roll Again");
		}
		_board.WhoseTurn += 1;
		_board.State = new PreRollState (_board);
	}

	public override void Buy() {
		if (!(_board.CurrentSquare is OwnableSquare)) {
			throw new MemberAccessException ("Current Player's Square is not Ownable");
		}

		OwnableSquare ownable = ((OwnableSquare)_board.CurrentSquare);

		ownable.BoughtBy (_board.CurrentPlayer);
		// return _board.CurrentSquare.Name + " bought by " + _board.CurrentPlayer.Name;
	}
}

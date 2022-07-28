using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {
	protected Board _board;

	public State(Board board) {
		_board = board;
	}

	protected void defaultRoll() {
		_board.Dice.Roll ();
		_board.Move (_board.Dice.Total);
	}

	public abstract void Roll ();

	public abstract void EndTurn ();

	public abstract void Buy ();
}

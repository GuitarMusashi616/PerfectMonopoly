using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreRollState : State {

	public PreRollState(Board board) : base(board) {

	}

	public override void Roll ()
	{
		defaultRoll ();
		_board.State = new PostRollState (_board);
	}

	public override void EndTurn ()
	{
		throw new Exception("Must Roll First");
	}

	public override void Buy() {
		throw new Exception("Must Roll First");
	}
}

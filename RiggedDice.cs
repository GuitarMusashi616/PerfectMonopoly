using System;
using System.Collections;
using System.Collections.Generic;

public class RiggedDice : Dice {
	private int[] _expectedRolls;
	private int _index;

	public RiggedDice( int[] expectedRolls) {
		_expectedRolls = expectedRolls;
	}

	public override void Roll ()
	{
		try {
			_faces [0] = _expectedRolls [_index++];
			_faces [1] = _expectedRolls [_index++];
		} catch (IndexOutOfRangeException) {
			base.Roll ();
		}
	}
}

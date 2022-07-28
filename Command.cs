using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command {
	// subclass sandbox with access to board as receiver
	private static Board _board;

	public static void SetBoard(Board board) {
		_board = board;
	}

	public abstract void Undo ();

	public abstract void Execute ();
}

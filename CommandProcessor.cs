using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor {
	private List<Command> _history;
	// keeps history of commands, has execute and undo

	public CommandProcessor() {
		_history = new List<Command> ();
	}

	public void Execute (Command command) {
		if (command is NullCommand) {
			return;
		}
		command.Execute ();
		_history.Add (command);
	}

	public void Undo() {
		if (_history.Count > 0) {
			int index = _history.Count - 1;
			_history [index].Undo ();
			_history.RemoveAt (index);
		}
	}
}

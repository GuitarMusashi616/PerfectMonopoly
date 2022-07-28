using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandAloneGame : MonoBehaviour, IPlayerListener {
	public InputField[] _inputField;
	[SerializeField]
	private Board _board;
	private string _string;

	public void OnPlayerUpdate(Player player) {
		int playerNum = Int32.Parse(player.Name.Substring (7))-1;
		_inputField[playerNum].text = _board.Squares[player.Position].Name;
	}
	// Use this for initialization
	void Start () {
		_board = new Board (4);
		//_inputField = _playerView.GetComponent<InputField> ();
		// var scriptable = GameObject.Find ("View").GetComponent<View>();
		foreach (Player player in _board.Players) {
			player.Register (this);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Roll() {
		_board.Roll ();
	}

	public void EndTurn() {
		_board.EndTurn ();
	}

	public void Buy() {
		_board.Buy ();
	}

	public void Undo() {
		_board.Undo ();
	}
}

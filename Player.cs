using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player {
	private int _number;
	private int _boardLength;
	private string _name;
	private int _position;
	private int _cash;
	private List<OwnableSquare> _ownedSquares;
	private List<IPlayerListener> _subscribers;

	public Player(int number, int boardLength) {
		_number = number;
		_boardLength = boardLength;
		_name = "Player " + (_number+1).ToString ();
		_position = 0;
		_cash = 1500;
		_subscribers = new List<IPlayerListener> ();
		_ownedSquares = new List<OwnableSquare> ();
	}

	public int Cash {
		set {
			_cash = value;
			notify ();
		}
		get {
			return _cash;
		}
	}

	public int Position {
		set {
			_position = value % _boardLength;
			notify ();
		}
		get {
			return _position;
		}
	}

	public string Name {
		get {
			return _name;
		}
	}

	public void AddOwnable(OwnableSquare os) {
		_ownedSquares.Add (os);
	}

	public void Register(IPlayerListener ipl) {
		_subscribers.Add (ipl);
	}

	private void notify() {
		foreach (IPlayerListener ipl in _subscribers) {
			ipl.OnPlayerUpdate (this);
		}
	}
}

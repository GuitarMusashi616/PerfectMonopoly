using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Square {
	[SerializeField]
	private string _name;
	// private List<Player> _visitors;

	public int PassCount{get; set;}
	public int VisitCount{get; set;}

	public Square(string name) {
		_name = name;
	}

	public string Name {
		get {
			return _name;
		}
	}

	public virtual Command PassedBy (Player player) {
		PassCount += 1;
		return new NullCommand ();
	}

	public virtual Command VisitedBy (Player player) {
		VisitCount += 1;
		return new NullCommand ();
	}
}

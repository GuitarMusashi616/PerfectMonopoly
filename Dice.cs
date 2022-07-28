using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dice {
	
	[SerializeField]
	protected int[] _faces;
	private System.Random _rand;

	public Dice() {
		_faces = new int[2];
		_rand = new System.Random ();
	}

	public object this[int i] {
		get {
			return _faces[i];
		}
	}

	public int Total {
		get {
			return _faces [0] + _faces [1];
		}
	}

	public bool Doubles {
		get {
			return _faces [0] == _faces [1];
		}
	}

	public int DoubleCount{get; set;}

	public virtual void Roll() {
		// non unity engine implementation
		_faces [0] = _rand.Next(5) + 1;
		_faces [1] = _rand.Next(5) + 1;
		if (Doubles) {
			DoubleCount += 1;
		} else {
			DoubleCount = 0;
		}

	}
}

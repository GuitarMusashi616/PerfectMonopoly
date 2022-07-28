using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RailroadSquare : OwnableSquare {
	private static List<RailroadSquare> _railroads = new List<RailroadSquare>();

	private int[] _rents;

	public RailroadSquare(string name, int price, int[] rents) : base(name, price) {
		_railroads.Add (this);
		_rents = rents;
	}

	public int OwnedRailroads {
		get {
			int count = 0;
			foreach (RailroadSquare railroad in _railroads) {
				if (railroad.Owner == Owner) {
					count += 1;
				}
			}
			return count;
		}
	}

	public override int Rent {
		get {
			return _rents[OwnedRailroads];
		}
	}

	public static void ResetRailroads() {
		_railroads.Clear ();
	}
}

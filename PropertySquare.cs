using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color {
	Brown, 
	Teal,
	Pink,
	Orange,
	Red,
	Yellow,
	Green,
	Blue
}


public class PropertySquare : OwnableSquare {
	private int[] _rents;
	private int _housePrice;
	private int _houses;
	private Color _color;

	private static Dictionary<Color, List<PropertySquare>> _propertyGroups = new Dictionary<Color, List<PropertySquare>> ();

	public static Dictionary<Color, List<PropertySquare>> PropertyGroup {
		get {
			return _propertyGroups;
		}
	}

	public static void ResetPropertyGroups() {
		_propertyGroups.Clear ();
	}

	public PropertySquare(string name, int price, int housePrice, int[] rents, Color color) : base(name, price) {
		if (rents.Length != 6) {
			throw new ArgumentOutOfRangeException ("rents", "Rent array must be of length 6");
		}
		_rents = rents;
		_housePrice = housePrice;
		_color = color;

		if (!_propertyGroups.ContainsKey(_color)) {
			_propertyGroups.Add (_color, new List<PropertySquare> ());
		}
		_propertyGroups[_color].Add(this);
	}

	public int Houses {
		set {
			if (value > 5) {
				throw new IndexOutOfRangeException ("Cannot have more than 5 houses");
			}
			_houses = value;
		}

		get {
			return _houses;
		}
	}

	public override int Rent {
		get {
			int rent = _rents [_houses];
			if (_houses == 0 && Monopoly) {
				return rent * 2;
			}
			return rent;
		}
	}

	public bool Monopoly {
		get {
			if (!_propertyGroups.ContainsKey (_color)) {
				throw new MemberAccessException ("Property Group has not been Initialized");
			}

			foreach (PropertySquare ps in _propertyGroups[_color]) {
				if (ps.Owner != Owner) {
					return false;
				}
			}

			return true;
		}
	}
}

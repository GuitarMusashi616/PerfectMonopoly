using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class OwnableSquare : Square {
	[SerializeField]
	private Player _owner;
	[SerializeField]
	private int _price;
	private bool _isMortgaged;

	public OwnableSquare(string name, int price) : base(name) {
		_price = price;
	}

	public Player Owner {
		set {
			_owner = value;
		}

		get {
			return _owner;
		}
	}

	public abstract int Rent{ get;}

	public int MortgageValue {
		get {
			return _price / 2;
		}
	}

	public void Mortgage() {
		if (_owner != null) {
			_isMortgaged = true;
			Owner.Cash += MortgageValue;
		}
	}

	public void BoughtBy(Player player) {
		if (Owner != null) {
			throw new Exception ("Already Bought");
		}
		if (player.Cash < _price) {
			throw new ArgumentException ("Player doesn't have enough money", "player");
		}
		player.Cash -= _price;
		player.AddOwnable (this);
		Owner = player;
	}

	public override Command VisitedBy (Player player) {
		base.VisitedBy (player);
		if (Owner == null) {
			return new NullCommand ();
		}
		return new TransactionCommand (player, _owner, Rent);
	}
}

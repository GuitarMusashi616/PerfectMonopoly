using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoSquare : Square {
	public GoSquare() : base("GO") {
	}

	public override Command PassedBy(Player player) {
		base.PassedBy (player);
		return new CashCommand (player, 200);
		// CommandQueue.push (new CashCommand (player, 200));
	}
}

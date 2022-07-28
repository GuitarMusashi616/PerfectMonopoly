using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashCommand : Command {
	private Player _player;
	private int _amount;

	public CashCommand(Player player, int amount) {
		_player = player;
		_amount = amount;
	}

	public override void Execute ()
	{
		_player.Cash += _amount;
	}

	public override void Undo ()
	{
		_player.Cash -= _amount;
	}
}

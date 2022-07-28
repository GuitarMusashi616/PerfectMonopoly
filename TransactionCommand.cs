using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionCommand : Command {
	private Player _sender;
	private Player _receiver;
	private int _amount;

	public TransactionCommand(Player sender, Player receiver, int amount) {
		_sender = sender;
		_receiver = receiver;
		_amount = amount;
	}

	public override void Execute () {
		_sender.Cash -= _amount;
		_receiver.Cash += _amount;
	}

	public override void Undo() {
		_sender.Cash += _amount;
		_receiver.Cash -= _amount;
	}
}

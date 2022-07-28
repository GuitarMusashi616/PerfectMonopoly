using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class TestStandAloneGame {
	[Test]
	public void PlayerFieldsAreWorking() {
		Player player = new Player (0, 40);
		Assert.AreEqual (1500, player.Cash);
		Assert.AreEqual (0, player.Position);
		Assert.AreEqual ("Player 1", player.Name);
	}

	[Test]
	public void SquareFieldsAreWorking() {
		Square square = new Square("GO");
		Assert.AreEqual (square.Name, "GO");
	}

	[Test]
	public void GameSetupWorking() {
		var board = new Board (4);
		Assert.AreEqual ("Player 1", board.CurrentPlayer.Name);
		board.State = new PostRollState (board);
		board.WhoseTurn += 1;
		Assert.AreEqual ("Player 2", board.CurrentPlayer.Name);
		board.State = new PostRollState (board);
		board.WhoseTurn += 1;
		Assert.AreEqual ("Player 3", board.CurrentPlayer.Name);
		board.State = new PostRollState (board);
		board.WhoseTurn += 1;
		Assert.AreEqual ("Player 4", board.CurrentPlayer.Name);
		board.State = new PostRollState (board);
		board.WhoseTurn += 1;
		Assert.AreEqual ("Player 1", board.CurrentPlayer.Name);
	}


	[Test]
	public void MoveShouldAffectSquares() {
		var board = new Board (4);
		Assert.AreEqual (40, board.Squares.Count);

		Assert.AreEqual ("GO", board.CurrentSquare.Name);
		Assert.AreSame (board.Squares[0], board.CurrentSquare);

		board.Move (3);
		Assert.AreEqual (0, board.Squares [0].PassCount);
		Assert.AreEqual (0, board.Squares [0].VisitCount);

		Assert.AreEqual (1, board.Squares [1].PassCount);
		Assert.AreEqual (0, board.Squares [1].VisitCount);

		Assert.AreEqual (1, board.Squares [2].PassCount);
		Assert.AreEqual (0, board.Squares [2].VisitCount);

		Assert.AreEqual (0, board.Squares [3].PassCount);
		Assert.AreEqual (1, board.Squares [3].VisitCount);

		Assert.AreEqual (3, board.CurrentPlayer.Position);
		Assert.AreSame (board.Players [0], board.CurrentPlayer);

		Assert.AreEqual ("Baltic Avenue", board.CurrentSquare.Name);
		Assert.AreSame (board.Squares[3], board.CurrentSquare);
	}

	[Test]
	public void PassGoTest() {
		var board = new Board (4);
		board.Squares [0].PassedBy (board.CurrentPlayer).Execute();
		Assert.AreEqual ("Player 1", board.CurrentPlayer.Name);
		Assert.AreEqual (1700, board.CurrentPlayer.Cash);
	}

	[Test]
	public void UnownedPropertyShouldntChargePlayer() {
		var ps = new PropertySquare ("Baltic Avenue", 60, 50, new int[]{ 4, 20, 60, 180, 320, 450 }, Color.Brown);
		Assert.AreEqual (30, ps.MortgageValue);
		var p1 = new Player (0, 40);
		ps.VisitedBy (p1);
		Assert.AreEqual (1500, p1.Cash);
		ps.Houses += 1;

		ps.VisitedBy (p1);
		Assert.AreEqual (1500, p1.Cash);
	}

	[Test]
	public void OwnedPropertyShouldChargePlayer() {
		PropertySquare.ResetPropertyGroups();
		var ps = new PropertySquare ("Baltic Avenue", 60, 50, new int[]{ 4, 20, 60, 180, 320, 450 }, Color.Brown);
		var ps2 = new PropertySquare ("Mediterranean Avenue", 60, 50, new int[]{ 4, 10, 30, 90, 160, 250 }, Color.Brown);

		Assert.AreEqual (2, PropertySquare.PropertyGroup[Color.Brown].Count);
		Assert.AreSame (ps, PropertySquare.PropertyGroup [Color.Brown] [0]);
		Assert.AreSame (ps2, PropertySquare.PropertyGroup [Color.Brown] [1]);

		var p1 = new Player (0, 40);
		var p2 = new Player (1, 40);
		ps.Owner = p2;

		ps.VisitedBy (p1).Execute();
		Assert.AreEqual (1496, p1.Cash);
		Assert.AreEqual (1504, p2.Cash);

		ps2.Owner = p2; 
		p1.Cash = 1500;
		p2.Cash = 1500;

		Assert.AreEqual (true, ps2.Monopoly);

		ps2.VisitedBy (p2).Execute();
		Assert.AreEqual (1500, p2.Cash);

		ps2.VisitedBy (p1).Execute();
		Assert.AreEqual (1492, p1.Cash);
		Assert.AreEqual (1508, p2.Cash);

		ps.Houses += 1;
		ps.VisitedBy (p1).Execute();
		Assert.AreEqual (1472, p1.Cash);
		Assert.AreEqual (1528, p2.Cash);
	}

	[Test]
	public void RailroadsChargeCorrectAmount() {
		var board = new Board (2);
		Assert.AreSame (board.Players[1], board.PreviousPlayer);
		board.Teleport (5);
		board.State = new PostRollState (board);
		board.Buy ();
		Assert.AreEqual (board.CurrentPlayer, ((RailroadSquare)board.Squares [5]).Owner);
		Assert.AreEqual (1300, board.CurrentPlayer.Cash);
		board.WhoseTurn += 1;

		board.Teleport (5);
		Assert.AreEqual(1450, board.CurrentPlayer.Cash);
		Assert.AreEqual(1350, board.PreviousPlayer.Cash);
	}

	[Test]
	public void PropertiesChargeCorrectAmount() {
		PropertySquare.ResetPropertyGroups ();
		var board = new Board (2);
		var p1 = board.CurrentPlayer;
		var p2 = board.PreviousPlayer;
		board.Move (6);
		board.State = new PostRollState (board);
		board.Buy ();
		Assert.AreEqual (1400, p1.Cash);
		board.WhoseTurn += 1;

		board.Move (6);
		board.State = new PostRollState (board);
		Assert.AreEqual (1494, p2.Cash);
		Assert.AreEqual (1406, p1.Cash);
		board.WhoseTurn += 1;

		board.Move (2);
		board.State = new PostRollState (board);
		board.Buy ();
		Assert.AreEqual (1306, p1.Cash);
		board.Move (1);
		board.Buy ();
		Assert.AreEqual (1186, p1.Cash);
		board.WhoseTurn += 1;

		board.Move (2);
		board.State = new PostRollState (board);
		Assert.AreEqual (3, PropertySquare.PropertyGroup [Color.Teal].Count);
		Assert.AreEqual (true, ((PropertySquare)board.CurrentSquare).Monopoly);
		Assert.AreEqual (1482, p2.Cash);
		Assert.AreEqual (1198, p1.Cash);
		board.WhoseTurn += 1;

		((PropertySquare)board.CurrentSquare).Houses += 1;
		board.Move (1);
		board.State = new PostRollState (board);
		board.WhoseTurn += 1;

		board.Move (1);
		board.State = new PostRollState (board);
		//Assert.AreEqual (1400, p2.Cash);
		//Assert.AreEqual (1600, p1.Cash);
		board.WhoseTurn += 1;
	}

	[Test]
	public void SampleGame() {
		var board = new Board (4);
		board.Move (5);
		// make sure each square does stuff to player

	}

	[Test]
	public void SampleGameThroughButtons() {
		// test the buttons (end to end testing)
		var board = new Board (4);
		board.Dice = new RiggedDice (new int[]{ 5, 5, 3, 2 });
		Assert.Throws<Exception> (board.EndTurn, "Must Roll First");
		Assert.Throws<Exception> (board.Buy , "Must Roll First");

		board.Roll ();
		Assert.IsTrue (board.Dice.Doubles);
		Assert.Throws<Exception> (board.EndTurn, "Got Doubles, Roll Again");

		board.Roll ();
		Assert.IsFalse (board.Dice.Doubles);

		Assert.Throws<Exception> (board.Roll, "Already Rolled");

		board.Buy ();
		Assert.Throws<Exception> (board.Buy, "Already Bought");
		Assert.Throws<Exception> (board.Roll, "Already Rolled");
		board.EndTurn ();


		//board.Buy(); //not enough cash

		//board.BuyHouse(); // must do at beginning of turn
		//board.BuyHouse(); // must own that property
		//board.BuyHouse(); // must have monopoly
		//board.BuyHouse(); // success
	}

	[Test]
	public void RiggedDiceShouldRollPredeterminedIntegers() {
		var board = new Board (4);
		board.Dice = new RiggedDice (new int[]{5, 4, 1, 1, 3, 3, 4, 4, 1, 1, 2, 3});

		board.Dice.Roll ();
		Assert.AreEqual (9, board.Dice.Total);
		Assert.AreEqual (false, board.Dice.Doubles);

		board.Dice.Roll ();
		Assert.AreEqual (2, board.Dice.Total);
		Assert.AreEqual (true, board.Dice.Doubles);

		board.Dice.Roll ();
		Assert.AreEqual (6, board.Dice.Total);
		Assert.AreEqual (true, board.Dice.Doubles);

		board.Dice.Roll ();
		Assert.AreEqual (8, board.Dice.Total);
		Assert.AreEqual (true, board.Dice.Doubles);

		board.Dice.Roll ();
		Assert.AreEqual (2, board.Dice.Total);
		Assert.AreEqual (true, board.Dice.Doubles);

		board.Dice.Roll ();
		Assert.AreEqual (5, board.Dice.Total);
		Assert.AreEqual (false, board.Dice.Doubles);

		board.Dice.Roll ();
	}

	[Test]
	public void BoardShouldUndo() {
		Board board = new Board (4);
		board.Dice = new RiggedDice (new int[] {5, 5, 2, 2, 1, 3, 4, 4, 1, 3 });
		board.Roll ();
		Assert.AreEqual (10, board.CurrentPlayer.Position);
		board.Undo ();
		Assert.AreEqual (0, board.CurrentPlayer.Position);
	}
}

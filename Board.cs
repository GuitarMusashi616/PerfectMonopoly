using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Board
{
	private int _whoseTurn;
	private List<Player> _players;
	private List<Square> _squares;
	private CommandProcessor _commandProcessor;
	private State _state;
	private Dice _dice;

	public Board (int numPlayers)
	{
		PropertySquare.ResetPropertyGroups ();
		RailroadSquare.ResetRailroads ();
		Command.SetBoard (this);

		InitSquares ();
		InitPlayers (numPlayers);

		_commandProcessor = new CommandProcessor ();
		_state = new PreRollState (this);
		_dice = new Dice ();
	}

	public Dice Dice {
		get {
			return _dice;
		}
		set {
			_dice = value;
		}
	}

	public State State {
		set {
			_state = value;
		}
	}

	public List<Player> Players 
	{
		get {
			return _players;
		}
	}

	public List<Square> Squares 
	{
		get {
			return _squares;
		}
	}

	public Player PreviousPlayer 
	{
		get {
			return _players [(_whoseTurn + _players.Count - 1) % _players.Count];
		}
	}

	public Player CurrentPlayer {
		get {
			return _players [_whoseTurn];
		}
	}

	public Square CurrentSquare {
		get {
			return _squares [CurrentPlayer.Position];
		}
	}

	public int WhoseTurn {
		get {
			return _whoseTurn;
		}
		set {
			_whoseTurn = (_whoseTurn + 1) % _players.Count;
		}
	}
		
	private void landOnSquare() {
		Command command = CurrentSquare.VisitedBy (CurrentPlayer);
		_commandProcessor.Execute (command);
	}

	public void Move (int spaces)
	{
		// negative should work too
		for (int i = 1; i < spaces; i++) {
			_squares [CurrentPlayer.Position + i].PassedBy (CurrentPlayer);
		}
		CurrentPlayer.Position += spaces;
		landOnSquare ();
	}

	public void Teleport(int iSquare) 
	{
		CurrentPlayer.Position = iSquare;
		landOnSquare ();
	}

	public void Roll () 
	{
		_state.Roll ();
	}

	public void Buy ()
	{
		_state.Buy ();
	}

	public void EndTurn ()
	{
		_state.EndTurn ();
	}

	public void Undo() 
	{
		_commandProcessor.Undo ();
	}

	void InitPlayers (int numPlayers)
	{
		_players = new List<Player> ();
		for (int i = 0; i < numPlayers; i++) {
			_players.Add (new Player (i, _squares.Count));
		}
	}

	private void InitSquares ()
	{
		_squares = new List<Square> ();
		_squares.Add (new GoSquare ());
		_squares.Add (new PropertySquare ("Mediterranean Avenue", 60, 50, new int[] {4,10,30,90,160,250}, Color.Brown));
		_squares.Add (new Square ("Community"));
		_squares.Add (new PropertySquare ("Baltic Avenue", 60, 50, new int[] {4,20,60,180,320,450}, Color.Brown));
		_squares.Add (new Square ("Income Tax"));
		_squares.Add (new RailroadSquare ("Reading Railroad", 200, new int[] {25,50,100,200}));
		_squares.Add (new PropertySquare ("Oriental Avenue", 100, 50, new int[] {6,30,90,270,400,550}, Color.Teal));
		_squares.Add (new Square ("Chance"));
		_squares.Add (new PropertySquare ("Vermont Avenue", 100, 50, new int[] {6,30,90,270,400,550}, Color.Teal));
		_squares.Add (new PropertySquare ("Connecticut Avenue", 120, 50, new int[] {8,40,100,300,450,600}, Color.Teal));
		_squares.Add (new Square ("In Jail/Just Visiting"));
		_squares.Add (new PropertySquare ("St. Charles Place", 140, 100, new int[] {10,50,150,450,625,750}, Color.Pink));
		_squares.Add (new Square ("Electric Company"));
		_squares.Add (new PropertySquare ("States Avenue", 140, 100, new int[] {10,50,150,450,625,750}, Color.Pink));
		_squares.Add (new PropertySquare ("Virginia Avenue", 160, 100, new int[] { 12, 60, 180, 500, 700, 900 }, Color.Pink));
		_squares.Add (new RailroadSquare ("Pennsylvania Railroad", 200, new int[] { 25, 50, 100, 200 }));
		_squares.Add (new PropertySquare ("St. James Place", 180, 100, new int[] { 14, 70, 200, 550, 750, 950 }, Color.Orange));
		_squares.Add (new Square ("Community Chest"));
		_squares.Add (new PropertySquare ("Tennessee Avenue", 180, 100, new int[] { 14, 70, 200, 550, 750, 950 }, Color.Orange));
		_squares.Add (new PropertySquare ("New York Avenue", 200, 100, new int[] { 16, 80, 220, 600, 800, 1000 }, Color.Orange));
		_squares.Add (new Square ("Free Parking"));
		_squares.Add (new PropertySquare ("Kentucky Avenue", 220, 150, new int[] { 18, 90, 250, 700, 875, 1050 }, Color.Red));
		_squares.Add (new Square ("Chance"));
		_squares.Add (new PropertySquare ("Indiana Avenue", 220, 150, new int[] { 18, 90, 250, 700, 875, 1050 }, Color.Red));
		_squares.Add (new PropertySquare ("Illinois Avenue", 240, 150, new int[] { 20, 100, 300, 750, 925, 1100 }, Color.Red));
		_squares.Add (new RailroadSquare ("B&O Railroad", 200, new int[] { 25, 50, 100, 200 }));
		_squares.Add (new PropertySquare ("Atlantic Avenue", 260, 150, new int[] { 22, 110, 330, 800, 975, 1150 }, Color.Yellow));
		_squares.Add (new PropertySquare ("Ventnor Avenue", 260, 150, new int[] { 22, 110, 330, 800, 975, 1150 }, Color.Yellow));
		_squares.Add (new Square ("Water Works"));
		_squares.Add (new PropertySquare ("Marvin Gardens", 280, 150, new int[] { 24, 120, 360, 850, 1025, 1200 }, Color.Yellow));
		_squares.Add (new Square ("Go To Jail"));
		_squares.Add (new PropertySquare ("Pacific Avenue", 300, 200, new int[] { 26, 130, 390, 900, 1100, 1275 }, Color.Green));
		_squares.Add (new PropertySquare ("North Carolina Avenue", 300, 200, new int[] { 26, 130, 390, 900, 1100, 1275 }, Color.Green));
		_squares.Add (new Square ("Community Chest"));
		_squares.Add (new PropertySquare ("Pennsylvania Avenue", 320, 200, new int[] { 28, 150, 450, 1000, 1200, 1400 }, Color.Green));
		_squares.Add (new RailroadSquare ("Short Line", 200, new int[] { 25, 50, 100, 200 }));
		_squares.Add (new Square ("Chance"));
		_squares.Add (new PropertySquare ("Park Place", 350, 200, new int[] { 35, 175, 500, 1100, 1300, 1500 }, Color.Blue));
		_squares.Add (new Square ("Luxury Tax"));
		_squares.Add (new PropertySquare ("Boardwalk", 400, 200, new int[] { 50, 200, 600, 1400, 1700, 2000 }, Color.Blue));
	}
}

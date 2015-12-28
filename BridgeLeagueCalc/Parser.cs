using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BridgeLeagueCalc
{
	public class Score
	{
		public int Position { get; set; }
		public float Points { get; set; }
		public DateTime DateTime { get; set; }
	}

	public class Player
	{
		public string Name { get; set; }
		public List<Score> Scores { get; set; }

		public Player()
		{
			Scores = new List<Score>();
		}
	}

	public class Parser
	{
		private struct ScratchPad
		{
			public DateTime DateTime { get; set; }

			public int PositionColumn { get; set; }
			public int Player1Column { get; set; }
			public int Player2Column { get; set; }
			public int ScoreColumn { get; set; }

			public List<Player> Players { get; set; }
		}

		public List<Player> Players { get; private set; }

		enum State
		{
			General,
			DateTime,
			ScoreHeaders,
			Scores
		}

		public void Parse( string file, char delimiter )
		{
			Debug.Print( "Parsing file '{0}' using delimiter: '{1}'", file, delimiter );

			State state = State.General;
			ScratchPad scratchPad = new ScratchPad()
			{
				Players = new List<Player>()
			};

			string[] lines = File.ReadAllLines( System.IO.Path.ChangeExtension( file, ".csv" ) );

			foreach ( string line in lines )
			{
				ParseLine( ref state, line, delimiter, ref scratchPad );
			}

			foreach ( Player player in scratchPad.Players )
			{
				Debug.Print( "Found player {0} with score {1} (Position {2}) for date: {3}", player.Name, player.Scores[ 0 ].Points, player.Scores[ 0 ].Position, player.Scores[ 0 ].DateTime );
			}
		}

		private void ParseLine( ref State state, string line, char delimiter, ref ScratchPad scratchPad )
		{
			switch ( state )
			{
			case State.General:
				state = General( line, delimiter, ref scratchPad );
				break;

			case State.DateTime:
				state = DateTime( line, delimiter, ref scratchPad );
				break;

			case State.ScoreHeaders:
				state = ScoreHeaders( line, delimiter, ref scratchPad );
				break;

			case State.Scores:
				state = Scores( line, delimiter, ref scratchPad );
				break;
			}
		}

		private State General( string line, char delimiter, ref ScratchPad scratchPad )
		{
			if ( line.StartsWith( @"#Date" ) )
			{
				return DateTime( line, delimiter, ref scratchPad );
			}
			else if ( line == @"#Scores" )
			{
				return State.ScoreHeaders;
			}

			return State.General;
		}

		private State DateTime( string line, char delimiter, ref ScratchPad scratchPad )
		{
			string[] tokens = line.Split( delimiter );

			if ( tokens[ 0 ] != @"#Date" )
			{
				//TODO: Throw exception
			}

			scratchPad.DateTime = System.DateTime.Parse( tokens[ 1 ] );

			return State.General;
		}

		private State ScoreHeaders( string line, char delimiter, ref ScratchPad scratchPad )
		{
			string[] tokens = line.Split( delimiter );

			for ( int i = 0; i < tokens.Length; ++i )
			{
				if ( tokens[ i ] == "Position" )
				{
					scratchPad.PositionColumn = i;
					Debug.Print( "Found position column: {0}", scratchPad.PositionColumn );
				}
				else if ( tokens[ i ] == "Name1" )
				{
					scratchPad.Player1Column = i;
					Debug.Print( "Found player 1 column: {0}", scratchPad.Player1Column );
				}
				else if ( tokens[ i ] == "Name2" )
				{
					scratchPad.Player2Column = i;
					Debug.Print( "Found player 2 column: {0}", scratchPad.Player2Column );
				}
				else if ( tokens[ i ] == "Score" )
				{
					scratchPad.ScoreColumn = i;
					Debug.Print( "Found score column: {0}", scratchPad.ScoreColumn );
				}
			}

			Debug.Print( "Header search complete" );
			return State.Scores;
		}

		private State Scores( string line, char delimiter, ref ScratchPad scratchPad )
		{
			if ( line[ 0 ] == '#' )
			{
				State state = State.General;
				ParseLine( ref state, line, delimiter, ref scratchPad );
				return state;
			}

			string[] tokens = line.Split( delimiter );

			Score score = new Score()
			{
				DateTime = scratchPad.DateTime,
				Points = float.Parse( tokens[ scratchPad.ScoreColumn ] ),
				Position = int.Parse( tokens[ scratchPad.PositionColumn ] )
			};

			Player player1 = new Player()
			{
				Name = tokens[ scratchPad.Player1Column ]
			};

			Player player2 = new Player()
			{
				Name = tokens[ scratchPad.Player2Column ]
			};

			player1.Scores.Add( score );
			player2.Scores.Add( score );

			scratchPad.Players.Add( player1 );
			scratchPad.Players.Add( player2 );

			return State.Scores;
		}
	}
}

using System;
using System.IO;

namespace Scacchi {

	class Values {

		public void Configuration () { }

		public static string LOG = ""; // NON MODIFICARE, contiene la lista delle mosse effettuate

		public const int MIN_WIDTH = 10; // Dimensione minima per considerare un pedone reale (in pixel)
		public const int MIN_HEIGHT = 10;

		public const int WHITE_VALUE = 110; // Valori minimi per cui classificare bianco o nero
		public const int BLACK_VALUE = 100;

		public const int RESIZE_WIDTH = 150; // Dimensioni per il redimensionamento delle immagini (in pixel)
		public const int RESIZE_HEIGHT = 150;

		public static string CCE_PATH = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "stockfish-7-mac", "Mac", "stockfish-7-64");

		public static string BMP1_PATH = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1.jpg");
		public static string BMP2_PATH = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2.jpg");

		public static string BMP3_PATH = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img3.jpg");


	}

	class MainClass {

		public static void Main (string [] args) {

			string HumanMove;
			string ComputerMove;

			HumanMove = ImageManipulator.HumanTurn ();
			Values.LOG += HumanMove + " ";

			ComputerMove = CCEManipulator.GetBestMove (1000, true);
			Values.LOG += ComputerMove + " ";

			HumanMove = "g1f3";
			Values.LOG += HumanMove + " ";

			ComputerMove = CCEManipulator.GetBestMove (1000, true);
			Values.LOG += ComputerMove + " ";

			Console.WriteLine (Values.LOG);

			Console.WriteLine ("Processo terminato");
			Console.Read ();


		}




	}
}
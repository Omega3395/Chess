using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Security.Cryptography;

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

	}

	class MainClass {

		public static void Main (string [] args) {

			// Avvia il Chess Engine
			CCEManipulator.StartCCE (Values.CCE_PATH);


			// Prendi e ridimensiona le immagini
			Bitmap bmp1 = Utils.ResizeImages (new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1.jpg")), Values.RESIZE_WIDTH, Values.RESIZE_HEIGHT);
			Bitmap bmp2 = Utils.ResizeImages (new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2.jpg")), Values.RESIZE_WIDTH, Values.RESIZE_HEIGHT);

			// Crea la immagine "differenza" e ricavane la mossa
			Bitmap bmp3 = ImageManipulator.getDifferenceBitmap (bmp2, bmp1);

			string HumanMove = ImageManipulator.GetHumanMove (bmp3, bmp1);

			//bmp3.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img3.png"), ImageFormat.Png);

			// Chiudi le immagini
			bmp1.Dispose ();
			bmp2.Dispose ();
			bmp3.Dispose ();



			Console.WriteLine (HumanMove);

			// Chiudi il Chess Engine
			CCEManipulator.CloseCCE ();

			Console.WriteLine ("Processo terminato");
			Console.Read ();


		}




	}
}
using System;
using System.Drawing;

namespace Scacchi {
    public class ImageManipulator {
        public static int x1 = 0, x2 = 0, y1 = 0, y2 = 0;

        static bool IsRed(Color pixel) {

            float Hue = pixel.GetHue();
            float Saturation = pixel.GetSaturation();
            float Brightness = pixel.GetBrightness();

            // Hue >= 325
            // Saturation >=0.25
            // 0.125 < Brightness < 0.6875   before: 0.2916666
            //if (Hue >= 325 && Saturation >= 0.25 && 0.26 <= Brightness && Brightness <= 0.6875)

            if (Hue >= 345 && Saturation >= 0.3 && 0.3 <= Brightness && Brightness <= 0.6) {
                return true;

                // Hue < 22.59414
                //Saturation >= 0.25
                // 0.125 < Brightness < 0.6875 before: 0.2916666
            } else if (Hue < 20 && Saturation >= 0.3 && 0.3 < Brightness && Brightness < 0.6) {
                return true;
            }

            return false;

        }


        public static void DrawLines(Bitmap bmp) {

            for (int i = 0; i < 152; i++) {
                for (int j = 1; j < 8; j++) {
                    bmp.SetPixel(i, j * 19, Color.Black);
                    bmp.SetPixel(j * 19, i, Color.Black);
                }
            }

        }


        static string GetCoordinates(int x, int y, int w, int h) {

            char[] Columns = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

            //Get the column
            int Column = 0;
            do {
                Column++;
            } while (w * Column / 8 < x);

            //Get the Row
            int Row = 0;
            do {
                Row++;
            } while (h * Row / 8 < y);
            int FRow = 9 - Row;

            return Convert.ToString(Columns[Column - 1]) + Convert.ToString(FRow);
        }

        static Bitmap AdjustColors(Bitmap bmp) {
            int Base = bmp.Size.Width / 8;
            int Altezza = bmp.Size.Height / 8;
            int x = 0, y = 0, cont;
            for (int i = 0; i < 64; i++) {
                cont = 0;
                for (int x1 = 0; x1 < Base; x1++) {
                    for (int y1 = 0; y1 < Altezza; y1++) {
                        if (IsRed(bmp.GetPixel(x + x1, y + y1))) {
                            cont++;
                        }

                        bmp.SetPixel(x + x1, y + y1, Color.Gray);

                    }
                }

                if (cont > Values.MIN_RED_PIXELS) {
                    for (int x1 = 5; x1 < Base - 5; x1++) {
                        for (int y1 = 5; y1 < Altezza - 5; y1++) {
                            bmp.SetPixel(x + x1, y + y1, Color.Red);
                        }
                    }
                }

                if (x == Base * 7) {
                    x = 0;
                    y += Altezza;
                } else {
                    x += Base;
                }
            }

            return bmp;

        }

        /// <summary>
        /// Simulates a complete houman turn
        /// </summary>
        /// <returns>Dirty human move.</returns>
        public static string HumanTurn(bool save_bmp3, bool cleaned = true) {

            // Prendi e ridimensiona le immagini
            Bitmap bmp1 = Utils.ResizeImages(new Bitmap(Values.BMP1_PATH), Values.RESIZE_WIDTH, Values.RESIZE_HEIGHT);
            Bitmap bmp2 = Utils.ResizeImages(new Bitmap(Values.BMP2_PATH), Values.RESIZE_WIDTH, Values.RESIZE_HEIGHT);

            // Crea la immagine "differenza" e ricavane la mossa
            Bitmap bmp3 = ImageManipulator.getDifferenceBitmap(bmp2, bmp1);

            string HumanMove = ImageManipulator.GetHumanMove(bmp3, bmp1, save_bmp3);

            // Chiudi le immagini
            bmp1.Dispose();
            bmp2.Dispose();
            bmp3.Dispose();

            return (cleaned) ? Utils.ToEssential(HumanMove) : HumanMove;

        }

        /// <summary>
        /// Gets the human move from 2 given images.
        /// </summary>
        /// <returns>The move relevated.</returns>
        /// <param name="bmp">The "difference" image.</param>
        /// <param name="bmp1">The first image.</param>
        public static string GetHumanMove(Bitmap bmp, Bitmap bmp1, bool save_bmp3 = false) {

            int[,] corners = GetCorners(bmp);

            x1 = corners[0, 0];
            y1 = corners[0, 1];
            x2 = corners[1, 0];
            y2 = corners[1, 1];

            bmp.SetPixel(x1, y1, Color.Gold);
            bmp.SetPixel(x2, y2, Color.Gold);

            DrawLines(bmp);

            if (save_bmp3)
                bmp.Save(Values.BMP3_PATH);

            string CoordFrom, CoordTo;
            int W = bmp.Size.Width;
            int H = bmp.Size.Height;

            if (bmp1.GetPixel(x1, y1).ToArgb().Equals(Color.Red.ToArgb())) {
                CoordFrom = GetCoordinates(x1, y1, W, H);
                CoordTo = GetCoordinates(x2, y2, W, H);
            } else {
                CoordTo = GetCoordinates(x1, y1, W, H);
                CoordFrom = GetCoordinates(x2, y2, W, H);
            }

            return CoordFrom + CoordTo;
        }

        /// <summary>
        /// Gets the differences between 2 images.
        /// </summary>
        /// <returns>The difference bitmap.</returns>
        /// <param name="bmp1">First image.</param>
        /// <param name="bmp2">Second image.</param>
        public static Bitmap getDifferenceBitmap(Bitmap bmp1, Bitmap bmp2, bool save_bmp3 = false) {

            Size s1 = bmp1.Size;
            Size s2 = bmp2.Size;

            Bitmap bmp3 = new Bitmap(s1.Width, s1.Height);

            bmp1 = AdjustColors(bmp1);
            bmp2 = AdjustColors(bmp2);

            for (int y = 0; y < bmp3.Size.Height; y++) {
                for (int x = 0; x < bmp3.Size.Width; x++) {

                    bool Red1 = IsRed(bmp1.GetPixel(x, y));
                    bool Red2 = IsRed(bmp2.GetPixel(x, y));

                    if (bmp1.GetPixel(x, y).Equals(bmp2.GetPixel(x, y))) {
                        bmp3.SetPixel(x, y, bmp1.GetPixel(x, y));
                    } else {
                        if ((!Red1 && Red2) || (Red1 && !Red2))
                            bmp3.SetPixel(x, y, Color.Purple);
                    }
                }
            }

            if (save_bmp3)
                bmp3.Save(Values.BMP3_PATH, System.Drawing.Imaging.ImageFormat.Png);

            return bmp3;
        }

        internal static int[,] GetCorners(Bitmap bmp) {

            GetOrizontalLines(bmp);

            int[,] Corners = { { x1, y1 }, { x2, y2 } };
            return Corners;

        }

        static void GetOrizontalLines(Bitmap bmp) {

            bool first = true, second = false;

            Size s1 = bmp.Size;
            Size s2 = bmp.Size;
            if (s1 != s2) {
                return;
            }

            for (int y = 0; y < s1.Height; y++) {
                for (int x = 0; x < s1.Width; x++) {

                    Color InitialPixel = bmp.GetPixel(x, y);

                    if (InitialPixel.ToArgb().Equals(Color.Purple.ToArgb())) {

                        if (first) {
                            x1 = x;
                            y1 = y;
                        }
                        if (second) {
                            x2 = x;
                            y2 = y;
                        }
                        first = false;
                        second = true;
                    }
                }
            }
        }

    }
}
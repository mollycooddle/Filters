using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WindowsFormsApp1;


namespace WindowsFormsApp1
{
    abstract class Filter
    {
        protected abstract Color calculateNewPixelColor(Bitmap sourceimage, int x, int y);

        public int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        public Bitmap Execute(Bitmap sourceimage)
        {
            Bitmap resultimage = new Bitmap(sourceimage.Width, sourceimage.Height);

            for (int x = 0; x < sourceimage.Width; x++)
            {
                for (int y = 0; y < sourceimage.Height; y++)
                {
                    resultimage.SetPixel(x, y, calculateNewPixelColor(sourceimage, x, y));
                }
            }

            return resultimage;
        }
    }

    class InvertFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            Color sourcecolor = sourceimage.GetPixel(x, y);
            Color resultcolor = Color.FromArgb(255 - sourcecolor.R, 255 - sourcecolor.G, 255 - sourcecolor.B);
            return resultcolor;
        }
    }

    class GrayScaleFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            Color sourcecolor = sourceimage.GetPixel(x, y);
            int intensity = (int)(0.299 * sourcecolor.R + 0.587 * sourcecolor.G + 0.114 * sourcecolor.B);
            Color resultcolor = Color.FromArgb(intensity, intensity, intensity);
            return resultcolor;
        }
    }

    class Sepiya : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            Color sourcecolor = sourceimage.GetPixel(x, y);
            int intensity = (int)(0.299 * sourcecolor.R + 0.587 * sourcecolor.G + 0.114 * sourcecolor.B);
            int k = 20;
            int R = (int)(intensity + 2 * k);
            int G = (int)(intensity + 0.5 * k);
            int B = (int)(intensity - 1 * k);

            R = Clamp(R, 0, 255);
            G = Clamp(G, 0, 255);
            B = Clamp(B, 0, 255);

            Color resultcolor = Color.FromArgb(R, G, B);
            return resultcolor;
        }
    }

    class Brightness : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            Color sourcecolor = sourceimage.GetPixel(x, y);
            int R = sourcecolor.R + 20;
            int G = sourcecolor.G + 20;
            int B = sourcecolor.B + 20;

            R = Clamp(R, 0, 255);
            G = Clamp(G, 0, 255);
            B = Clamp(B, 0, 255);

            Color resultcolor = Color.FromArgb(R, G, B);
            return resultcolor;
        }
    }

    class Shift : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            Color sourcecolor = sourceimage.GetPixel(x, y);
            return sourcecolor;
        }

        public Bitmap Execute(Bitmap sourceimage)
        {
            Bitmap resultimage = new Bitmap(sourceimage.Width, sourceimage.Height);

            for (int x = 50; x < sourceimage.Width; x++)
            {
                for (int y = 0; y < sourceimage.Height; y++)
                {
                    resultimage.SetPixel(x, y, calculateNewPixelColor(sourceimage, x - 50, y));
                }
            }

            return resultimage;
        }
    }

    class Embossing : Filter {

        public float[,] kernel = { { 0, 1, 0 }, { -1, 0, 1 }, { 0, -1, 0 } };
        public Embossing() { }
        public Embossing(float[,] kernel)
        {
            this.kernel = kernel;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            for (int l = -radiusY; l <= radiusY; l++) {
                for (int k = -radiusX; k <= radiusX; k++) {
                    int idX = Clamp(x + k, 0, sourceimage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceimage.Height - 1);

                    Color neighorColor = sourceimage.GetPixel(idX, idY);
                    resultR += neighorColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighorColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighorColor.B * kernel[k + radiusX, l + radiusY];
                }
            }

            resultR = Clamp((int)resultR, 0, 255);
            resultG = Clamp((int)resultG, 0, 255);
            resultB = Clamp((int)resultB, 0, 255);

            resultR += 100;
            resultG += 100;
            resultB += 100;

            int intensity = (int)(0.299 * resultR + 0.587 * resultG + 0.114 * resultB);
            return Color.FromArgb(
                Clamp((int)intensity, 0, 255),
                Clamp((int)intensity, 0, 255),
                Clamp((int)intensity, 0, 255));

        }

        public Bitmap Execute(Bitmap sourceimage)
        {
            Bitmap resultimage = new Bitmap(sourceimage.Width, sourceimage.Height);

            for (int x = 0; x < sourceimage.Width; x++)
            {
                for (int y = 0; y < sourceimage.Height; y++)
                {
                    resultimage.SetPixel(x, y, calculateNewPixelColor(sourceimage, x, y));
                }
            }

            return resultimage;
        }
    }

    class Blur : Filter {

        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            int kernelsize = 9;
            int radius = kernelsize / 2;

            for (int l = -radius; l <= radius; l++)
            {
                int idX = Clamp(x + l, 0, sourceimage.Width - 1);
                int idY = Clamp(y + l, 0, sourceimage.Height - 1);

                Color neighorColor = sourceimage.GetPixel(idX, idY);

                resultR += neighorColor.R;
                resultG += neighorColor.G;
                resultB += neighorColor.B;
            }

            resultR /= (float)kernelsize;
            resultG /= (float)kernelsize;
            resultB /= (float)kernelsize;

            return Color.FromArgb(
                Clamp((int)resultR, 0, 255),
                Clamp((int)resultG, 0, 255),
                Clamp((int)resultB, 0, 255));


            /*            float[,] kernel = { { 0, 0, 0, 0, 0, 0, 0, 0 ,1.0f / (float)9},
                                            { 0, 0, 0, 0, 0, 0, 0, 1.0f / (float)9 ,0},
                                            { 0, 0, 0, 0, 0, 0, 1.0f / (float)9, 0 ,0},
                                            { 0, 0, 0, 0, 0, 1.0f / (float)9, 0, 0 ,0},
                                            { 0, 0, 0, 0, 1.0f / (float)9, 0, 0, 0 ,0},
                                            { 0, 0, 0, 1.0f / (float)9, 0, 0, 0, 0 ,0},
                                            { 0, 0, 1.0f / (float)9, 0, 0, 0, 0, 0 ,0},
                                            { 0, 1.0f / (float)9, 0, 0, 0, 0, 0, 0, 0},
                                            { 1.0f / (float)9, 0, 0, 0, 0, 0, 0, 0 , 0}};

                        int radiusX = kernel.GetLength(0) / 2;
                        int radiusY = kernel.GetLength(1) / 2;

                        float resultR = 0;
                        float resultG = 0;
                        float resultB = 0;
                        for (int l = -radiusY; l <= radiusY; l++)
                        {
                            for (int k = -radiusX; k <= radiusX; k++)
                            {
                                int idX = Clamp(x + k, 0, sourceimage.Width - 1);
                                int idY = Clamp(y + l, 0, sourceimage.Height - 1);

                                Color neighorColor = sourceimage.GetPixel(idX, idY);
                                resultR += neighorColor.R * kernel[k + radiusX, l + radiusY];
                                resultG += neighorColor.G * kernel[k + radiusX, l + radiusY];
                                resultB += neighorColor.B * kernel[k + radiusX, l + radiusY];
                            }

                        }


                        return Color.FromArgb(
                    Clamp((int)resultR, 0, 255),
                    Clamp((int)resultG, 0, 255),
                    Clamp((int)resultB, 0, 255));*/
        }    

        public Bitmap Execute(Bitmap sourceimage)
        {
            /*Bitmap resultimage = new Bitmap(sourceimage.Width, sourceimage.Height);

            for (int y = 0; y < sourceimage.Height; y++)
            {
                for (int x = 0; x < sourceimage.Width; x++)
                {
                    resultimage.SetPixel(x, y, calculateNewPixelColor(sourceimage, x, y));
                }
            }

            return resultimage;*/

            Bitmap resultimage = new Bitmap(sourceimage.Width, sourceimage.Height);

            for (int y = 0; y < sourceimage.Height; y++)
            {
                for (int x = 0; x < sourceimage.Width; x++)
                {
                    resultimage.SetPixel(x, y, calculateNewPixelColor(sourceimage, x, y));
                }
            }

            return resultimage;
        }
    }

    class GreyWorld : Filter {
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            Color color = sourceimage.GetPixel(x, y);
            return color;
        }

        public Bitmap Execute(Bitmap sourceimage) {
            Bitmap resultimage = new Bitmap(sourceimage.Width, sourceimage.Height);
            float R = 0;
            float G = 0;
            float B = 0;
            float k = sourceimage.Width * sourceimage.Height;

            for (int x = 0; x < sourceimage.Width; x++)
            {
                for (int y = 0; y < sourceimage.Height; y++)
                {
                    Color sourcecolor1 = sourceimage.GetPixel(x, y);
                    R += sourcecolor1.R;   //сумма кр всех пикселей
                    G += sourcecolor1.G;   //сумма гр всех пикселей
                    B += sourcecolor1.B;   //сумма бл всех пикселей
                }
            }

            R = (1.0f / (float)k) * R;    //not R
            G = (1.0f / (float)k) * G;    //not G
            B = (1.0f / (float)k) * B;    //not B

            float avg = (R + G + B) / (float)3;


            for (int x = 0; x < sourceimage.Width; x++)
            {
                for (int y = 0; y < sourceimage.Height; y++)
                {
                    Color sourcecolor = sourceimage.GetPixel(x, y);
                    float resultR = (float)sourcecolor.R * avg / R;
                    float resultG = (float)sourcecolor.G * avg / G;
                    float resultB = (float)sourcecolor.B * avg / B;

                    resultimage.SetPixel(x, y, Color.FromArgb(Clamp((int)resultR, 0, 255), Clamp((int)resultG, 0, 255), Clamp((int)resultB, 0, 255)));
                }
            }

            return resultimage;
        }
    }

    class Autolevels : Filter {
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            Color color = sourceimage.GetPixel(x, y);
            return color;
        }

        public Bitmap Execute(Bitmap sourceimage)
        {
            Bitmap resultimage = new Bitmap(sourceimage.Width, sourceimage.Height);
            float R = 0;
            float G = 0;
            float B = 0;
            float minR = 255; float maxR = 0;
            float minG = 255; float maxG = 0;
            float minB = 255; float maxB = 0;

            for (int x = 0; x < sourceimage.Width; x++)
            {
                for (int y = 0; y < sourceimage.Height; y++)
                {
                    Color sourcecolor1 = sourceimage.GetPixel(x, y);
                    if (sourcecolor1.R < minR) minR = sourcecolor1.R;
                    if (sourcecolor1.G < minG) minG = sourcecolor1.G;
                    if (sourcecolor1.B < minB) minB = sourcecolor1.B;
                    if (sourcecolor1.R > maxR) maxR = sourcecolor1.R;
                    if (sourcecolor1.G > maxG) maxG = sourcecolor1.G;
                    if (sourcecolor1.B > maxB) maxB = sourcecolor1.B;
                }
            }

            for (int x = 0; x < sourceimage.Width; x++)
            {
                for (int y = 0; y < sourceimage.Height; y++)
                {
                    Color sourcecolor = sourceimage.GetPixel(x, y);

                    R = (sourcecolor.R - minR) * 255 / (maxR - minR);
                    G = (sourcecolor.G - minG) * 255 / (maxG - minG);
                    B = (sourcecolor.B - minB) * 255 / (maxB - minB);

                    resultimage.SetPixel(x, y, Color.FromArgb(Clamp((int)R, 0, 255), Clamp((int)G, 0, 255), Clamp((int)B, 0, 255)));
                }
            }

            return resultimage;
        }
    }

    class PerfectReflector : Filter {
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            Color color = sourceimage.GetPixel(x, y);
            return color;
        }

        public Bitmap Execute(Bitmap sourceimage)
        {
            Bitmap resultimage = new Bitmap(sourceimage.Width, sourceimage.Height);
            float R = 0;
            float G = 0;
            float B = 0;
            float maxR = 0;
            float maxG = 0;
            float maxB = 0;

            for (int x = 0; x < sourceimage.Width; x++)
            {
                for (int y = 0; y < sourceimage.Height; y++)
                {
                    Color sourcecolor1 = sourceimage.GetPixel(x, y);
                    if (sourcecolor1.R > maxR) maxR = sourcecolor1.R;
                    if (sourcecolor1.G > maxG) maxG = sourcecolor1.G;
                    if (sourcecolor1.B > maxB) maxB = sourcecolor1.B;
                }
            }

            for (int x = 0; x < sourceimage.Width; x++)
            {
                for (int y = 0; y < sourceimage.Height; y++)
                {
                    Color sourcecolor = sourceimage.GetPixel(x, y);

                    R = sourcecolor.R * 255 / maxR;
                    G = sourcecolor.G * 255 / maxG;
                    B = sourcecolor.B * 255 / maxB;

                    resultimage.SetPixel(x, y, Color.FromArgb(Clamp((int)R, 0, 255), Clamp((int)G, 0, 255), Clamp((int)B, 0, 255)));
                }
            }

            return resultimage;
        }
    }

    class Expansion : Filter {
            protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
            {
            int[,] kernel = {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 }
            };

            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            int maxR = 0, maxG = 0, maxB = 0;

            for (int l = -radiusY; l <= radiusY; l++)
            {
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    if (kernel[k + radiusX, l + radiusY] == 1)
                    {
                        int idX = Clamp(x + k, 0, sourceimage.Width - 1);
                        int idY = Clamp(y + l, 0, sourceimage.Height - 1);

                        Color neighborColor = sourceimage.GetPixel(idX, idY);

                        if (neighborColor.R > maxR) maxR = neighborColor.R;
                        if (neighborColor.G > maxG) maxG = neighborColor.G;
                        if (neighborColor.B > maxB) maxB = neighborColor.B;
                    }
                }
            }

            return Color.FromArgb(maxR, maxG, maxB);
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }

    class Narrowing : Filter {
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            int[,] kernel = {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 }
            };

            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            int minR = 255, minG = 255, minB = 255;

            for (int l = -radiusY; l <= radiusY; l++)
            {
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    if (kernel[k + radiusX, l + radiusY] == 1)
                    {
                        int idX = Clamp(x + k, 0, sourceimage.Width - 1);
                        int idY = Clamp(y + l, 0, sourceimage.Height - 1);

                        Color neighborColor = sourceimage.GetPixel(idX, idY);

                        if (neighborColor.R < minR) minR = neighborColor.R;
                        if (neighborColor.G < minG) minG = neighborColor.G;
                        if (neighborColor.B < minB) minB = neighborColor.B;
                    }
                }
            }

            return Color.FromArgb(minR, minG, minB);
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
    class Median : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            int radiusX = 1;
            int radiusY = 1;
            List<float> arrR = new List<float>();
            List<float> arrG = new List<float>();
            List<float> arrB = new List<float>();

            for (int l = -radiusY; l <= radiusY; l++)
            {
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceimage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceimage.Height - 1);

                    Color neighborColor = sourceimage.GetPixel(idX, idY);
                    arrR.Add(neighborColor.R);
                    arrG.Add(neighborColor.G);
                    arrB.Add(neighborColor.B);
                }
            }

            arrR.Sort();
            arrG.Sort(); 
            arrB.Sort();



            return Color.FromArgb(Clamp((int)arrR[4], 0, 255), 
                                  Clamp((int)arrG[4], 0, 255), 
                                  Clamp((int)arrB[4], 0, 255));
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    
    }

    class SobelFilter : Filter {


        float[,] kernelX = {
            { -1.0f, 0, 1.0f },
            { -2.0f, 0, 2.0f },
            { -1.0f, 0, 1.0f }
        };

        float[,] kernelY = {
            { -1.0f, -2.0f, -1.0f },
            {  0,  0,  0 },
            {  1.0f,  2.0f,  1.0f }
        };

        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            Color color = sourceimage.GetPixel(x, y);
            return color;
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        Color GetPixelWithMirrorPadding(Bitmap image, int x, int y)
        {
            x = Clamp(x, 0, image.Width - 1);
            y = Clamp(y, 0, image.Height - 1);
            return image.GetPixel(x, y);
        }

        public Bitmap Execute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    float gradientXR = 0, gradientYR = 0;
                    float gradientXG = 0, gradientYG = 0;
                    float gradientXB = 0, gradientYB = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Color neighborColor = GetPixelWithMirrorPadding(sourceImage, x + i, y + j);

                            gradientXR += neighborColor.R * kernelX[i + 1, j + 1];
                            gradientYR += neighborColor.R * kernelY[i + 1, j + 1];

                            gradientXG += neighborColor.G * kernelX[i + 1, j + 1];
                            gradientYG += neighborColor.G * kernelY[i + 1, j + 1];

                            gradientXB += neighborColor.B * kernelX[i + 1, j + 1];
                            gradientYB += neighborColor.B * kernelY[i + 1, j + 1];
                        }
                    }

                    int gradientR = (int)Math.Sqrt(gradientXR * gradientXR + gradientYR * gradientYR);
                    int gradientG = (int)Math.Sqrt(gradientXG * gradientXG + gradientYG * gradientYG);
                    int gradientB = (int)Math.Sqrt(gradientXB * gradientXB + gradientYB * gradientYB);

                    gradientR = Clamp(gradientR, 0, 255);
                    gradientG = Clamp(gradientG, 0, 255);
                    gradientB = Clamp(gradientB, 0, 255);

                    resultImage.SetPixel(x, y, Color.FromArgb(gradientR, gradientG, gradientB));
                }
            }

            return resultImage;
        }
    }

    class ScharraFilter : Filter {
        float[,] kernelX = {
            { -3.0f, 0, 3.0f },
            { -10.0f, 0, 10.0f },
            { -3.0f, 0, 3.0f }
        };

        float[,] kernelY = {
            { -3.0f, -10.0f, -3.0f },
            {  0,  0,  0 },
            {  3.0f,  10.0f,  3.0f }
        };
        protected override Color calculateNewPixelColor(Bitmap sourceimage, int x, int y)
        {
            Color color = sourceimage.GetPixel(x, y);
            return color;
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        Color GetPixelWithMirrorPadding(Bitmap image, int x, int y)
        {
            x = Clamp(x, 0, image.Width - 1);
            y = Clamp(y, 0, image.Height - 1);
            return image.GetPixel(x, y);
        }

        public Bitmap Execute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    float gradientXR = 0, gradientYR = 0;
                    float gradientXG = 0, gradientYG = 0;
                    float gradientXB = 0, gradientYB = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Color neighborColor = GetPixelWithMirrorPadding(sourceImage, x + i, y + j);

                            gradientXR += neighborColor.R * kernelX[i + 1, j + 1];
                            gradientYR += neighborColor.R * kernelY[i + 1, j + 1];

                            gradientXG += neighborColor.G * kernelX[i + 1, j + 1];
                            gradientYG += neighborColor.G * kernelY[i + 1, j + 1];

                            gradientXB += neighborColor.B * kernelX[i + 1, j + 1];
                            gradientYB += neighborColor.B * kernelY[i + 1, j + 1];
                        }
                    }

                    int gradientR = (int)Math.Sqrt(gradientXR * gradientXR + gradientYR * gradientYR);
                    int gradientG = (int)Math.Sqrt(gradientXG * gradientXG + gradientYG * gradientYG);
                    int gradientB = (int)Math.Sqrt(gradientXB * gradientXB + gradientYB * gradientYB);

                    gradientR = Clamp(gradientR, 0, 255);
                    gradientG = Clamp(gradientG, 0, 255);
                    gradientB = Clamp(gradientB, 0, 255);

                    resultImage.SetPixel(x, y, Color.FromArgb(gradientR, gradientG, gradientB));
                }
            }

            return resultImage;
        }
    }
}
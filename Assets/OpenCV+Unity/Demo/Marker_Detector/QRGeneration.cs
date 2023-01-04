namespace OpenCvSharp.Demo
{
    using UnityEngine;
    using OpenCvSharp;
    using OpenCvSharp.Aruco;
    using System.IO;
    using System;

    public class QRGeneration : MonoBehaviour
    {
        public static void GenerateQR(out Mat M1, out Mat M2, out Mat M3, out Mat M4)
        {
            // Uygulamamýzý Çalaný Sikeriz Bu yüzden üst 2 QR kodu 34 06 yaptýk aq (pis siqerler)
            Dictionary dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict4X4_1000);
            Mat UstSolQR = new();
            Mat UstSagQR = new();

            CvAruco.DrawMarker(dictionary, 34, 60, UstSolQR);
            CvAruco.DrawMarker(dictionary, 6, 60, UstSagQR);

            //Alttaki 2 QR kod ise albümün QR kodlarý olacak burda SQL kullanacaz
            Mat SolAltQR = new();
            Mat SagAltQR = new();

            var id1 = 293;  //TODO: Veri Tabanýndan alýnan ID atanacak
            var id2 = 31;   //TODO: Veri Tabanýndan alýnan ID atanacak

            CvAruco.DrawMarker(dictionary, id1, 60, SolAltQR);
            CvAruco.DrawMarker(dictionary, id2, 60, SagAltQR);

            GeneratePaddedImage(Unity.MatToTexture(UstSolQR), out Texture2D UstSolT);
            GeneratePaddedImage(Unity.MatToTexture(UstSagQR), out Texture2D UstSagT);
            GeneratePaddedImage(Unity.MatToTexture(SagAltQR), out Texture2D AltSagT);
            GeneratePaddedImage(Unity.MatToTexture(SolAltQR), out Texture2D AltSolT);

            M1 = Unity.TextureToMat(UstSolT);
            M2 = Unity.TextureToMat(UstSagT);
            M3 = Unity.TextureToMat(AltSolT);
            M4 = Unity.TextureToMat(AltSagT);
        }
        public static void GeneratePaddedImage(Texture2D input, out Texture2D image)
        {
            var PaddedImage = input.Clone();

            PaddedImage.Reinitialize(input.width + 8, input.height + 8);
            
            var borderColor = new UnityEngine.Color(1, 1, 1, 1);
            var borderWidth = 4;

            for (var x = 0; x < PaddedImage.width; x++) {
                for (var y = 0; y < PaddedImage.height; y++) {
                    if (x < borderWidth || x > PaddedImage.width - 1 - borderWidth) PaddedImage.SetPixel(x, y, borderColor);
                    else if (y < borderWidth || y > PaddedImage.height - 1 - borderWidth) PaddedImage.SetPixel(x, y, borderColor);
                    else
                    {
                        PaddedImage.SetPixel(x, y, input.GetPixel(x - borderWidth, y - borderWidth));
                    }
                }
            }

            PaddedImage.Apply();

            PaddedImage.ApplyPadding(PaddingMode.Diagonal);
            PaddedImage.Apply();

            image = PaddedImage;
        }
        public static void EmbedQRIntoImage(Mat solUst, Mat sagUst, Mat solAlt, Mat sagAlt, Texture2D image)
        {
            var solUstT = Unity.MatToTexture(solUst);
            var sagUstT = Unity.MatToTexture(sagUst);
            var solAltT = Unity.MatToTexture(solAlt);
            var sagAltT = Unity.MatToTexture(sagAlt);

            var x1 = 0;
            var y1 = 0;

            for (var x = 2; x < 70; x++) {
                y1 = 0;
                for (var y = 2; y < 70; y++) {
                    image.SetPixel(x, y, solAltT.GetPixel(x1, y1));
                    y1++;
                }
                x1++;
            }

            for (var x = image.width - 70; x < image.width - 2; x++)
            {
                y1 = 0;
                for (var y = 2; y < 70; y++)
                {
                    image.SetPixel(x, y, sagAltT.GetPixel(x1, y1));
                    y1++;
                }
                x1++;
            }

            x1 = 0;
            for (var x = 2; x < 70; x++)    //Sol Üstü Kontrol Ediyo
            {
                y1 = 0;
                for (var y = image.height - 70; y < image.height - 2; y++)
                {
                    image.SetPixel(x, y, solUstT.GetPixel(x1, y1));
                    y1++;
                }
                x1++;
            }

            x1 = 0;
            for (var x = image.width - 70; x < image.width - 2; x++)
            {
                y1 = 0;
                for (var y = image.height - 70; y < image.height - 2; y++)
                {
                    image.SetPixel(x, y, sagUstT.GetPixel(x1, y1));
                    y1++;
                }
                x1++;
            }

            image.Apply();
            ImageSaving(image);
        }
        public static void ImageSaving(Texture2D input)
        {
            var dirPath = Application.persistentDataPath + "/../Assets/OpenCV+Unity/Demo/Marker_Detector/SaveImages/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            var a = ImageConversion.EncodeToJPG(input);
            File.WriteAllBytes("C:/Users/90542/Desktop/Bum/" + timeStamp + ".png", a);
        }
    }
}
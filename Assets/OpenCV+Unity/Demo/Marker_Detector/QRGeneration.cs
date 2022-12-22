namespace OpenCvSharp.Demo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using OpenCvSharp;
    using OpenCvSharp.Aruco;
    using UnityEngine.UI;
    using System.Drawing;
    using SixLabors.ImageSharp;
    using System.IO;
    using System;

    public class QRGeneration : MonoBehaviour
    {
        
        public static void GenerateQR()
        {
            // Uygulamamýzý Çalaný Sikeriz Bu yüzden üst 2 QR kodu 34 06 yaptýk aq (pis siqerler)
            Dictionary dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.DictArucoOriginal);
            Mat UstSolQR = new();
            Mat UstSagQR = new();

            CvAruco.DrawMarker(dictionary, 34, 50, UstSolQR);
            CvAruco.DrawMarker(dictionary, 06, 50, UstSagQR);

            //Alttaki 2 QR kod ise albümün QR kodlarý olacak burda SQL kullanacaz
            Mat AlbumSolAltQR = new();
            Mat AlbumSagAltQR = new();

            var id1 = 293;  //TODO: Veri Tabanýndan alýnan ID atanacak
            var id2 = 31;   //TODO: Veri Tabanýndan alýnan ID atanacak

            //CvAruco.DrawMarker(dictionary, id1, 50, AlbumSolAltQR);
            //CvAruco.DrawMarker(dictionary, id2, 50, AlbumSagAltQR);

            //ImageSaving(Unity.MatToTexture(UstSolQR));
            //ImageSaving(Unity.MatToTexture(UstSagQR));
            //ImageSaving(Unity.MatToTexture(AlbumSolAltQR));
            //ImageSaving(Unity.MatToTexture(AlbumSagAltQR));
        }
        public static void GeneratePaddedImage(Texture2D input)
        {
            var PaddedImage = input.Clone();

            PaddedImage.Reinitialize(input.width + 100, input.height + 100);
            
            var borderColor = new UnityEngine.Color(1, 1, 1, 1);
            var borderWidth = 50;

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

            //PaddedImage.ApplyPadding(PaddingMode.Diagonal);
            //PaddedImage.Apply();

            ImageSaving(PaddedImage);
        }
        private static void ImageSaving(Texture2D input)
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
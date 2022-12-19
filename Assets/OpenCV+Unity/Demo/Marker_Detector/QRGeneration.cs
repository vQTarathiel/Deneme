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
            Mat Identifier1 = new Mat();
            Mat Identifier2 = new Mat();

            CvAruco.DrawMarker(dictionary, 34, 50, Identifier1);
            CvAruco.DrawMarker(dictionary, 06, 50, Identifier2);

            //Alttaki 2 QR kod ise albümün QR kodlarý olacak burda SQL kullanacaz
            Mat AlbumSolAltQR = new Mat();
            Mat AlbumSagAltQR = new Mat();

            var id1 = 293;  //TODO: Veri Tabanýndan alýnan ID atanacak
            var id2 = 31;   //TODO: Veri Tabanýndan alýnan ID atanacak

            CvAruco.DrawMarker(dictionary, id1, 50, AlbumSolAltQR);
            CvAruco.DrawMarker(dictionary, id2, 50, AlbumSagAltQR);

            Mat abc = new Mat();
            Cv2.Add(AlbumSolAltQR, AlbumSagAltQR, abc);

            ImageSaving(Unity.MatToTexture(abc));
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
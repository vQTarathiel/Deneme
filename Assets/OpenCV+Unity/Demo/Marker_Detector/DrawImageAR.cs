namespace OpenCvSharp.Demo
{
    using UnityEngine;
    using OpenCvSharp;
    using OpenCvSharp.Aruco;
    using System.IO;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DrawImageAR : MonoBehaviour
    {
        private static void DrawImageOnMarker(Point2f[][] corners)
        {
            List<Point2f> avgOfCornersList = new List<Point2f> ();
            foreach (var x in corners)
            {
                var avaragePoint = new Point2f((int)Math.Round((x[0].X + x[1].X + x[2].X + x[3].X) / 4), (int)Math.Round((x[0].Y + x[1].Y + x[2].Y + x[3].Y) / 4));
                avgOfCornersList.Add(avaragePoint);
            }

            var avgOfCorners = avgOfCornersList.ToArray();

            //Burada cornerlar sıralanacak ve resim üzerine yazılaacak fonk. yazılacak aqqqqqqqq
        }
        public static void DrawAR(Mat mat, out Point2f[][] foundCorners, out int[] foundIds)
        {
            Point2f[][] corners;
            int[] ids;
            Point2f[][] rejectedImgPoints;
            DetectorParameters detectorParameters = DetectorParameters.Create();
            Dictionary dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict4X4_1000);

            Mat grayMat = new Mat();
            Cv2.CvtColor(mat, grayMat, ColorConversionCodes.BGR2GRAY);


            CvAruco.DetectMarkers(grayMat, dictionary, out corners, out ids, detectorParameters, out rejectedImgPoints);

            if (AppIDAuthentication(ids))
            {
                DrawImageOnMarker(corners);
                CvAruco.DrawDetectedMarkers(mat, corners, ids);
            }

            foundCorners = corners;
            foundIds = ids;
        }
        private static bool AppIDAuthentication(int[] idList)       //Uygulamaya ait özel ID kontrolü
        {
            
            if (idList.Length == 4)
            {
                if (Array.Find(idList, x => x == 34) == 34 && Array.Find(idList, x => x == 6) == 6)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
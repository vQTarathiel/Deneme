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
        private static void DrawImageOnMarker(Point2f[][] corners, int[] ids)
        {
            List<Point2f> avgOfCornersList = new List<Point2f> ();
            List<Point2f> SortedCorners = new List<Point2f>();
            foreach (var x in corners)
            {
                var avaragePoint = new Point2f((int)Math.Round((x[0].X + x[1].X + x[2].X + x[3].X) / 4), (int)Math.Round((x[0].Y + x[1].Y + x[2].Y + x[3].Y) / 4));
                avgOfCornersList.Add(avaragePoint);
            }

            image = cv2.imread('image.jpg');
            gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY);

            //QR kodlu resmin kordinatları (warped)
            points_A = np.float32([[320, 15], [700, 215], [85, 610], [530, 780]]);

            //QR içine koyulacak resmin kordinatları
            points_B = np.float32([[0, 0], [420, 0], [0, 594], [420, 594]]);

            //Perspektifi hesaplayan fonksiyon
            M = cv2.getPerspectiveTransform(points_A, points_B);
            
            //İçeri koyulacak resmin warp edilmesi
            warped = cv2.warpPerspective(gray, M, (420, 594));

            //Overlay edilmesi iki resmin
            cv2.imshow('Original', image);
            cv2.imshow('Warped', warped);
            cv2.waitKey(0);
            cv2.destroyAllWindows();


            //resim üzerine yazılaacak fonk. yazılacak aqqqqqqqq

            int xOrt, yOrt;
            xOrt = (int) Math.Round(avgOfCornersList[0].X + avgOfCornersList[1].X + avgOfCornersList[2].X + avgOfCornersList[3].X) / 4;
            yOrt = (int) Math.Round(avgOfCornersList[0].Y + avgOfCornersList[1].Y + avgOfCornersList[2].Y + avgOfCornersList[3].Y) / 4;

            SortedCorners.Add(avgOfCornersList.Find(x => x.X < xOrt && x.Y > yOrt));
            SortedCorners.Add(avgOfCornersList.Find(x => x.X > xOrt && x.Y > yOrt));
            SortedCorners.Add(avgOfCornersList.Find(x => x.X < xOrt && x.Y < yOrt));
            SortedCorners.Add(avgOfCornersList.Find(x => x.X > xOrt && x.Y < yOrt));


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
                DrawImageOnMarker(corners, ids);
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
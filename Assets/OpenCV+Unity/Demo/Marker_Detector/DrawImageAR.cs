namespace OpenCvSharp.Demo
{
    using UnityEngine;
    using OpenCvSharp;
    using OpenCvSharp.Aruco;
    using System.IO;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.Networking;
    using System.Threading.Tasks;

    public class DrawImageAR : MonoBehaviour
    {
        private static async void DrawImageOnMarker(Mat mat, Point2f[][] corners, int[] ids)
        {
            PicturePlane picturePlane = new();
            List<Point2f> avgOfCornersList = new List<Point2f> ();
            List<Point2f> SortedCorners = new List<Point2f>();

            var obj = GameObject.Find("PicturePlane").GetComponent<Renderer>().material;

            foreach (var x in corners)
            {
                var avaragePoint = new Point2f((int)Math.Round((x[0].X + x[1].X + x[2].X + x[3].X) / 4), (int)Math.Round((x[0].Y + x[1].Y + x[2].Y + x[3].Y) / 4));
                avgOfCornersList.Add(avaragePoint);
            }

            int xOrt, yOrt;
            xOrt = (int)Math.Round(avgOfCornersList[0].X + avgOfCornersList[1].X + avgOfCornersList[2].X + avgOfCornersList[3].X) / 4;
            yOrt = (int)Math.Round(avgOfCornersList[0].Y + avgOfCornersList[1].Y + avgOfCornersList[2].Y + avgOfCornersList[3].Y) / 4;

            SortedCorners.Add(avgOfCornersList.Find(x => x.X < xOrt && x.Y > yOrt));
            SortedCorners.Add(avgOfCornersList.Find(x => x.X > xOrt && x.Y > yOrt));
            SortedCorners.Add(avgOfCornersList.Find(x => x.X < xOrt && x.Y < yOrt));
            SortedCorners.Add(avgOfCornersList.Find(x => x.X > xOrt && x.Y < yOrt));

            picturePlane.MoveToPosition(SortedCorners);

            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://i.imgur.com/VSmMqZm.jpeg"))
            {
                // begin request:
                var asyncOp = www.SendWebRequest();

                // await until it's done: 
                while (asyncOp.isDone == false)
                    await Task.Delay(3000 / 30);//30 hertz

                // read results:
                if (www.isNetworkError || www.isHttpError)
                {
                    // log error:
                    Debug.Log($"{www.error}, URL:{www.url}");

                }
                else
                {
                    obj.mainTexture = DownloadHandlerTexture.GetContent(www);                           //BURADAN DEVAMKE FOTO FLIPLENECEK
                }
            }


            
            // Enable tesselation in the material
            Material materialInstance = new Material(obj.shader);
            materialInstance.CopyPropertiesFromMaterial(obj);
            materialInstance.EnableKeyword("_TESSELLATION");

            // Set the tesselation factor
            materialInstance.SetFloat("_TessellationFactor", 4f);

            // Set the tesselation mode (Quad or Phong)
            materialInstance.SetFloat("_TessellationMode", 1f); // 0 for quad, 1 for phong

            // Apply the material to the mesh renderer
            obj = materialInstance;
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
                DrawImageOnMarker(mat, corners, ids);
                //CvAruco.DrawDetectedMarkers(mat, corners, ids);
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
        private static Mat matImageFile(string filePath)
        {
            Mat matResult = null;
            if (File.Exists(filePath))
            {
                // load into Mat type. Hack: workaround for Cv2.ImRead() being broke.
                byte[] fileData = File.ReadAllBytes(filePath);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); // this will auto-resize the 2,2 texture dimensions.
                matResult = OpenCvSharp.Unity.TextureToMat(tex);
                Cv2.CvtColor(matResult, matResult, ColorConversionCodes.BGR2GRAY);
            }
            return matResult;
        }
    }
}
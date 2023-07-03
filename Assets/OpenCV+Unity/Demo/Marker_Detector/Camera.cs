
using OpenCvSharp;
using OpenCvSharp.Aruco;
using OpenCvSharp.Demo;
using UnityEngine;
using UnityEngine.UI;

public class Camera : WebCamera
{
    private Mat image;
    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        image = OpenCvSharp.Unity.TextureToMat(input);
        Resources.UnloadUnusedAssets();

        if (output == null)
        {
            output = OpenCvSharp.Unity.MatToTexture(image);
            var obj = GameObject.Find("PicturePlane").GetComponent<Renderer>().material;
            obj.mainTextureScale = new Vector2(obj.mainTextureScale.x, -obj.mainTextureScale.y);
        }
        else
        {
            output.hideFlags = HideFlags.HideAndDontSave;
            
            // Create default parameres for detection
            DetectorParameters detectorParameters = DetectorParameters.Create();

            // Dictionary holds set of all available markers
            Dictionary dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict4X4_1000);

            // Variables to hold results
            Point2f[][] corners;
            int[] ids;
            Point2f[][] rejectedImgPoints;

            // Create Opencv image from unity texture
            Mat mat = image;

            // Convert image to grasyscale
            //Mat grayMat = new Mat();

            //Cv2.CvtColor(mat, grayMat, ColorConversionCodes.BGR2GRAY);

            // Detect and draw markers
            //CvAruco.DetectMarkers(grayMat, dictionary, out corners, out ids, detectorParameters, out rejectedImgPoints);
            //CvAruco.DrawDetectedMarkers(mat, corners, ids);

            DrawImageAR.DrawAR(mat, out corners, out ids);

            // Create Unity output texture with detected markers
            Texture2D outputTexture = OpenCvSharp.Unity.MatToTexture(mat);

            // Set texture to see the result
            RawImage rawImage = gameObject.GetComponent<RawImage>();
            rawImage.texture = outputTexture;

            OpenCvSharp.Unity.MatToTexture(mat, output);

            //QRGeneration.GeneratePaddedImage(output, out Texture2D padded);
            
            //QRGeneration.GenerateQR(out Mat SolUstQR, out Mat SagUstQR, out Mat SolAltQR, out Mat SagAltQR);
            //QRGeneration.EmbedQRIntoImage(SolUstQR, SagUstQR, SolAltQR, SagAltQR, output);
        }

        return true;
    }
}

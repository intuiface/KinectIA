            // ****************************************************************************
            // <copyright file="Feedback.cs" company="IntuiLab">
            // INTUILAB CONFIDENTIAL
			//_____________________
			// [2002] - [2015] IntuiLab SA
			// All Rights Reserved.
			// NOTICE: All information contained herein is, and remains
			// the property of IntuiLab SA. The intellectual and technical
			// concepts contained herein are proprietary to IntuiLab SA
			// and may be covered by U.S. and other country Patents, patents
			// in process, and are protected by trade secret or copyright law.
			// Dissemination of this information or reproduction of this
			// material is strictly forbidden unless prior written permission
			// is obtained from IntuiLab SA.
            // </copyright>
            // ****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Windows.Media;
using IntuiLab.Kinect.DataUserTracking;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Globalization;

namespace IntuiLab.Kinect.Utils
{
    internal static class Feedback
    {
        private static KinectSensor m_refKinectSensor;

        private static Brush m_refBrushPointingMode = Brushes.Red;

        private static Brush m_refBrushGesturesMode = Brushes.Green;

        /// <summary>
        /// Draw feedback ColorFrame in a correct pixel format (Pbgra32)
        /// </summary>
        /// <param name="colorFrame">Byte array corresponding to the pixel data</param>
        /// <param name="nWidthImage">Image Width</param>
        /// <param name="nHeightImage">Image Height</param>
        /// <returns></returns>
        public static byte[] FeedbackColorFrame(byte[] colorFrame, int nWidthImage, int nHeightImage)
        {
            try
            {
                // Create BitmapSource with the data pixel (pixel format : Bgr32)
                BitmapSource sourceColor = BitmapSource.Create(nWidthImage, nHeightImage, 96, 96,
                                                   PixelFormats.Bgr32, null, colorFrame, nWidthImage * 4);

                DrawingVisual refDrawingVisual = new DrawingVisual();

                // Draw the BitmapSource in DrawingVisual
                using (DrawingContext dc = refDrawingVisual.RenderOpen())
                {
                    dc.DrawImage(sourceColor, new Rect(0.0, 0.0, nWidthImage, nHeightImage));
                    DrawKinectModeOnStream(dc);
                }

                // Convert the pixel format to the Pbgra32
                var rtb = new RenderTargetBitmap(nWidthImage, nHeightImage, 96d, 96d, PixelFormats.Pbgra32);
                rtb.Render(refDrawingVisual);

                // return a byte array with a correct pixel format
                return ConvertBitmapToByteArray(rtb, nWidthImage, nHeightImage);
            }
            catch (Exception ex)
            {
                DebugLog.DebugTraceLog("Error reading color frame " + ex.ToString(), true);
                return null;
            }
            
        }

        /// <summary>
        /// Draw feedback ColorFrame with the skeleton in a correct pixel format (Pbgra32)
        /// </summary>
        /// <param name="refKinect">the kinect sensor reference</param>
        /// <param name="refUsers">List of User Data (for the skeleton)</param>
        /// <param name="colorFrame">Byte array corresponding to the pixel data</param>
        /// <param name="nWidthImage">Image Width</param>
        /// <param name="nHeightImage">Image Height</param>
        /// <returns></returns>
        public static byte[] FeedbackColorFrameWithSkeleton(KinectSensor refKinect, List<UserData> refUsers, byte[] colorFrame, int nWidthImage, int nHeightImage)
        {
            try
            {
                m_refKinectSensor = refKinect;

                // Create BitmapSource with the data pixel (pixel format : Bgr32)
                BitmapSource sourceColor = BitmapSource.Create(nWidthImage, nHeightImage, 96, 96,
                                                               PixelFormats.Bgr32, null, colorFrame, nWidthImage*4);

                DrawingVisual refDrawingVisual = new DrawingVisual();

                // Draw the BitmapSource in DrawingVisual
                using (DrawingContext dc = refDrawingVisual.RenderOpen())
                {
                    dc.DrawImage(sourceColor, new Rect(0.0, 0.0, nWidthImage, nHeightImage));

                    if (refUsers.Count > 0)
                    {
                        // Draww skeletons
                        foreach (UserData user in refUsers)
                        {
                            DrawBonesAndJoints(user.UserSkeleton.refSkeleton, dc);
                            //DrawHand(user, dc);
                        }
                    }

                    DrawKinectModeOnStream(dc);
                }

                // Convert the pixel format to the Pbgra32
                var rtb = new RenderTargetBitmap(nWidthImage, nHeightImage, 96d, 96d, PixelFormats.Pbgra32);
                rtb.Render(refDrawingVisual);

                // return a byte array with a correct pixel format
                return ConvertBitmapToByteArray(rtb, nWidthImage, nHeightImage);
            }
            catch (Exception ex)
            {
                DebugLog.DebugTraceLog("Error reading color frame with skeleton " + ex.ToString(), true);
                return null;
            }
        }

        /// <summary>
        /// Draw feedback DepthFrame in a correct pixel format (Pbgra32)
        /// </summary>
        /// <param name="depthFrame">Byte array corresponding to the pixel data</param>
        /// <param name="nWidthImage">Image Width</param>
        /// <param name="nHeightImage">Image Height</param>
        /// <returns></returns>
        public static byte[] FeedbackDepthFrame(short[] depthFrame, int nWidthImage, int nHeightImage)
        {
            try
            {
                // Create BitmapSource with the data pixel (pixel format : Gray 16)
                BitmapSource sourceColor = BitmapSource.Create(nWidthImage, nHeightImage, 96, 96,
                                                   PixelFormats.Gray16, null, depthFrame, nWidthImage * 2);

                DrawingVisual refDrawingVisual = new DrawingVisual();

                // Draw the BitmapSource in DrawingVisual
                using (DrawingContext dc = refDrawingVisual.RenderOpen())
                {
                    dc.DrawImage(sourceColor, new Rect(0.0, 0.0, nWidthImage, nHeightImage));
                    DrawKinectModeOnStream(dc);
                }

                // Convert the pixel format to the Pbgra32
                var rtb = new RenderTargetBitmap(nWidthImage, nHeightImage, 96d, 96d, PixelFormats.Pbgra32);
                rtb.Render(refDrawingVisual);

                // return a byte array with a correct pixel format
                return ConvertBitmapToByteArray(rtb, nWidthImage, nHeightImage);
            }
            catch (Exception ex)
            {
                DebugLog.DebugTraceLog("Error reading depth frame " + ex.ToString(), true);
                return null;
            }
        }

        /// <summary>
        /// Draw feedback DepthFrame with the skeleton in a correct pixel format (Pbgra32)
        /// </summary>
        /// <param name="refKinect">the kinect sensor reference</param>
        /// <param name="refUsers">List of User Data (for the skeleton)</param>
        /// <param name="depthFrame">Byte array corresponding to the pixel data</param>
        /// <param name="nWidthImage">Image Width</param>
        /// <param name="nHeightImage">Image Height</param>
        /// <returns></returns>
        public static byte[] FeedbackDepthFrameWithSkeleton(KinectSensor refKinect, List<UserData> refUsers, short[] depthFrame, int nWidthImage, int nHeightImage)
        {
            try
            {
                m_refKinectSensor = refKinect;

                // Create BitmapSource with the data pixel (pixel format : Gray 16)
                BitmapSource sourceDepth = BitmapSource.Create(nWidthImage, nHeightImage, 96, 96,
                                                           PixelFormats.Gray16, null, depthFrame, nWidthImage * 2);

                DrawingVisual refDrawingVisual = new DrawingVisual();

                // Draw the BitmapSource in DrawingVisual
                using (DrawingContext dc = refDrawingVisual.RenderOpen())
                {
                    dc.DrawImage(sourceDepth, new Rect(0.0, 0.0, nWidthImage, nHeightImage));

                    if (refUsers.Count > 0)
                    {
                        // Draw skeletons
                        foreach (UserData user in refUsers)
                        {
                            DrawBonesAndJoints(user.UserSkeleton.refSkeleton, dc);
                            //DrawHand(user, dc);
                        }
                    }
                    DrawKinectModeOnStream(dc);
                }

                // Convert the pixel format to the Pbgra32
                var rtb = new RenderTargetBitmap(nWidthImage, nHeightImage, 96d, 96d, PixelFormats.Pbgra32);
                rtb.Render(refDrawingVisual);

                // return a byte array with a correct pixel format
                return ConvertBitmapToByteArray(rtb, nWidthImage, nHeightImage);
            }
            catch (Exception ex)
            {
                DebugLog.DebugTraceLog("Error reading depth frame with skeleton " + ex.ToString(), true);
                return null;
            }
        }

        /// <summary>
        /// Draw feedback SkeletonFrame in a correct pixel format (Pbgra32)
        /// </summary>
        /// <param name="refKinect">the kinect sensor reference</param>
        /// <param name="refUsers">List of User Data (for the skeleton)</param>
        /// <param name="nWidthImage">Image Width</param>
        /// <param name="nHeightImage">Image Height</param>
        /// <returns></returns>
        public static byte[] FeedbackSkeletonFrameAlone(KinectSensor refKinect, List<UserData> refUsers, int nWidthImage, int nHeightImage)
        {
            try
            {
                m_refKinectSensor = refKinect;

                DrawingVisual refDrawingVisual = new DrawingVisual();

                using (DrawingContext dc = refDrawingVisual.RenderOpen())
                {
                    // Create the background of the image
                    dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, nWidthImage, nHeightImage));

                    if (refUsers.Count > 0)
                    {
                        // Draw skeletons
                        foreach (UserData user in refUsers)
                        {
                            DrawBonesAndJoints(user.UserSkeleton.refSkeleton, dc);
                            //DrawHand(user, dc);
                        }
                    }
                    DrawKinectModeOnStream(dc);
                }

                // Convert the pixel format to the Pbgra32
                var rtb = new RenderTargetBitmap(nWidthImage, nHeightImage, 96d, 96d, PixelFormats.Pbgra32);
                rtb.Render(refDrawingVisual);

                // return a byte array with a correct pixel format
                return ConvertBitmapToByteArray(rtb, nWidthImage, nHeightImage);
            }
            catch (Exception ex)
            {
                DebugLog.DebugTraceLog("Error creating skeleton frame " + ex.ToString(), true);
                return null;
            }
        }

        /// <summary>
        /// Draw feedback system Locked in a correct pixel format (Pbgra32)
        /// </summary>
        /// <param name="nWidthImage">Image Width</param>
        /// <param name="nHeightImage">Image Height</param>
        /// <returns></returns>
        public static byte[] FeedbackSystemLocked(int nWidthImage, int nHeightImage)
        {
            try
            {
                DrawingVisual refDrawingVisual = new DrawingVisual();

                using (DrawingContext dc = refDrawingVisual.RenderOpen())
                {
                    // Get the path of directory execution
                    string pathDirResources = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Resources\\";
                    string[] temp = pathDirResources.Split(new string[] { "file:\\"}, StringSplitOptions.RemoveEmptyEntries);
                    pathDirResources = temp[0];

                    System.Drawing.Bitmap bitmap;
                    IntPtr hbitmap;
                    ImageSource imageSource;
                
                    // If the directory Resources and the file kinect_lock.png exists, use this file for the feedback
                    if (Directory.Exists(pathDirResources) && File.Exists(pathDirResources + "kinect_lock.png"))
                    {
                        bitmap = new System.Drawing.Bitmap(pathDirResources + "kinect_lock.png");
                        hbitmap = bitmap.GetHbitmap();
                        imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
                    }
                    else // else use the file in resources dll for the feedback
                    {
                        bitmap = IntuiLab.Kinect.Properties.Resources.kinect_lock;
                        hbitmap = bitmap.GetHbitmap();
                        imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
                    }

                    dc.DrawImage(imageSource, new Rect(0.0, 0.0, 640, 480));
                }

                // Convert the pixel format to the Pbgra32
                var rtb = new RenderTargetBitmap(nWidthImage, nHeightImage, 96d, 96d, PixelFormats.Pbgra32);
                rtb.Render(refDrawingVisual);

                // return a byte array with a correct pixel format
                return ConvertBitmapToByteArray(rtb, nWidthImage, nHeightImage);
            }
            catch (Exception ex)
            {
                DebugLog.DebugTraceLog("Error creating locked feedback " + ex.ToString(), true);
                return null;
            }
        }

        /// <summary>
        /// Convert a BitmapSource in a byte array
        /// </summary>
        /// <param name="source">the BitmapSource</param>
        /// <param name="width">Image width</param>
        /// <param name="height">Image Height</param>
        /// <returns></returns>
        private static byte[] ConvertBitmapToByteArray(BitmapSource source, int width, int height)
        {
            int bytePerPixel = source.Format.BitsPerPixel / 8;
            int stride = width * bytePerPixel;

            byte[] imageArray = new byte[stride * height];
            source.CopyPixels(imageArray, stride, 0);

            return imageArray;
        }

        /// <summary>
        /// Draw the skeleton bones and joints 
        /// </summary>
        /// <param name="refSkeleton">the skeleton</param>
        /// <param name="refDrawingContext">the drawing context where the skeleton is draw</param>
        private static void DrawBonesAndJoints(Skeleton refSkeleton, DrawingContext refDrawingContext)
        {
            // Render Torso
            DrawBone(refSkeleton, refDrawingContext, JointType.Head, JointType.ShoulderCenter);
            DrawBone(refSkeleton, refDrawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            DrawBone(refSkeleton, refDrawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            DrawBone(refSkeleton, refDrawingContext, JointType.ShoulderCenter, JointType.Spine);
            DrawBone(refSkeleton, refDrawingContext, JointType.Spine, JointType.HipCenter);
            DrawBone(refSkeleton, refDrawingContext, JointType.HipCenter, JointType.HipLeft);
            DrawBone(refSkeleton, refDrawingContext, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            DrawBone(refSkeleton, refDrawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            DrawBone(refSkeleton, refDrawingContext, JointType.ElbowLeft, JointType.WristLeft);
            DrawBone(refSkeleton, refDrawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            DrawBone(refSkeleton, refDrawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            DrawBone(refSkeleton, refDrawingContext, JointType.ElbowRight, JointType.WristRight);
            DrawBone(refSkeleton, refDrawingContext, JointType.WristRight, JointType.HandRight);

            // Left Leg
            DrawBone(refSkeleton, refDrawingContext, JointType.HipLeft, JointType.KneeLeft);
            DrawBone(refSkeleton, refDrawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            DrawBone(refSkeleton, refDrawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            DrawBone(refSkeleton, refDrawingContext, JointType.HipRight, JointType.KneeRight);
            DrawBone(refSkeleton, refDrawingContext, JointType.KneeRight, JointType.AnkleRight);
            DrawBone(refSkeleton, refDrawingContext, JointType.AnkleRight, JointType.FootRight);

            // Render Joints
            foreach (Joint joint in refSkeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = Brushes.Yellow;
                }

                if (drawBrush != null)
                {
                    refDrawingContext.DrawEllipse(drawBrush, null, SkeletonPointToScreen(joint.Position), 3, 3);
                }
            }

        }

        /// <summary>
        /// Convert position 3D in position 2D(pixel)
        /// </summary>
        /// <param name="skelpoint">Position 3D</param>
        /// <returns></returns>
        public static Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = m_refKinectSensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        /// <summary>
        /// Draw a bone
        /// </summary>
        /// <param name="skeleton">Skeleton</param>
        /// <param name="drawingContext">the drawing context where the bone is draw</param>
        /// <param name="jointType0">First joint</param>
        /// <param name="jointType1">Second joint</param>
        private static void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = new Pen(Brushes.Gray, 1);
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = new Pen(Brushes.Green, 6);
            }

            drawingContext.DrawLine(drawPen, SkeletonPointToScreen(joint0.Position), SkeletonPointToScreen(joint1.Position));
        }

        private static void DrawKinectModeOnStream(DrawingContext drawingContext)
        {
            Brush brush = null;
            if (PropertiesPluginKinect.Instance.KinectPointingModeEnabled)
            {
                brush = m_refBrushPointingMode;
            }
            else
            {
                brush = m_refBrushGesturesMode;
            }

            drawingContext.DrawText(new FormattedText(PropertiesPluginKinect.Instance.KinectModeDisplayOnstream + " mode",
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brush),
                new Point(10, 445));
        }

    }
}

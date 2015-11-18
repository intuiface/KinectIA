using IntuiLab.Kinect.Enums;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntuiLab.Kinect.Utils
{
    internal static class SkeletonMath
    {
        public const double MedianTolerance = 0.01;
        public const double MedianCorrectNeeded = 0.66666666;

        public static double DistanceBetweenPoints(SkeletonPoint refPoint1, SkeletonPoint refPoint2)
        {
            double dx = Math.Abs(refPoint2.X - refPoint1.X);
            double dy = Math.Abs(refPoint2.Y - refPoint1.Y);
            double dz = Math.Abs(refPoint2.Z - refPoint1.Z);

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static SkeletonPoint SubstractPoints(SkeletonPoint refPoint1, SkeletonPoint refPoint2)
        {
            SkeletonPoint res = new SkeletonPoint();
            res.X = refPoint1.X - refPoint2.X;
            res.Y = refPoint1.Y - refPoint2.Y;
            res.Z = refPoint1.Z - refPoint2.Z;
            return res;
        }

        public static SkeletonPoint AddPoints(SkeletonPoint refPoint1, SkeletonPoint refPoint2)
        {
            SkeletonPoint res = new SkeletonPoint();
            res.X = refPoint1.X + refPoint2.X;
            res.Y = refPoint1.Y + refPoint2.Y;
            res.Z = refPoint1.Z + refPoint2.Z;
            return res;
        }

        public static double Median(double d1, double d2, double d3)
        {
            // more performance than copying and sorting
            if ((d1 > d2 && d1 < d3) || (d1 < d2 && d1 > d3))
            {
                return d1;
            }
            if ((d2 > d1 && d2 < d3) || (d2 < d1 && d2 > d3))
            {
                return d2;
            }
            return d3;
        }

        public static double Median(IEnumerable<double> values)
        {
            List<double> d = values.ToList();
            d.Sort();
            return d[d.Count / 2];
        }

        /// <summary>
        /// The median of the direction between points
        /// </summary>
        /// <param name="from">Source joints</param>
        /// <param name="to">Target joints</param>
        /// <returns></returns>
        public static IEnumerable<EnumKinectDirectionGesture> SteadyDirectionTo(IEnumerable<SkeletonPoint> from, IEnumerable<SkeletonPoint> to)
        {
            List<List<EnumKinectDirectionGesture>> directions = new List<List<EnumKinectDirectionGesture>>();
            var origin = from.ToList();
            var target = to.ToList();
            if (from.Count() != to.Count())
            {
                throw new ArgumentException("Length not identical");
            }
            for (int i = 0; i < from.Count(); i++)
            {
                directions.Add(new List<EnumKinectDirectionGesture>());
                double dx = target[i].X - origin[i].X;
                double dy = target[i].Y - origin[i].Y;
                double dz = target[i].Z - origin[i].Z;
                if (dx > MedianTolerance)
                {
                    directions[i].Add(EnumKinectDirectionGesture.KINECT_DIRECTION_RIGHT);
                }
                else if (dx < -MedianTolerance)
                {
                    directions[i].Add(EnumKinectDirectionGesture.KINECT_DIRECTION_LEFT);
                }
                if (dy > MedianTolerance)
                {
                    directions[i].Add(EnumKinectDirectionGesture.KINECT_DIRECTION_UPWARD);
                }
                else if (dy < -MedianTolerance)
                {
                    directions[i].Add(EnumKinectDirectionGesture.KINECT_DIRECTION_DOWNWARD);
                }
                if (dz > MedianTolerance)
                {
                    directions[i].Add(EnumKinectDirectionGesture.KINECT_DIRECTION_BACKWARD);
                }
                else if (dz < -MedianTolerance)
                {
                    directions[i].Add(EnumKinectDirectionGesture.KINECT_DIRECTION_FORWARD);
                }

            }
            List<EnumKinectDirectionGesture> res = new List<EnumKinectDirectionGesture>();
            foreach (EnumKinectDirectionGesture item in System.Enum.GetValues(typeof(EnumKinectDirectionGesture)))
            {
                // found enough times in lists
                if (directions.Count(x => x.Contains(item)) > origin.Count * MedianCorrectNeeded)
                {
                    res.Add(item);
                }
            }
            if (res.Count == 0)
            {
                res.Add(EnumKinectDirectionGesture.KINECT_DIRECTION_NONE);
            }
            return res;
        }

        /// <summary>
        /// Get an abstract direction type between two skeleton points</summary>
        /// <param name="from">
        /// Source Point</param>
        /// <param name="to">
        /// Target Point</param>
        /// <returns>
        /// Returns a list of three directions (for each axis)</returns>
        public static IEnumerable<EnumKinectDirectionGesture> DirectionTo(SkeletonPoint from, SkeletonPoint to, double tolerance)
        {
            List<EnumKinectDirectionGesture> res = new List<EnumKinectDirectionGesture>();
            double dx = to.X - from.X;
            double dy = to.Y - from.Y;
            double dz = to.Z - from.Z;
            if (dx > tolerance)
            {
                res.Add(EnumKinectDirectionGesture.KINECT_DIRECTION_RIGHT);
            }
            else if (dx < -tolerance)
            {
                res.Add(EnumKinectDirectionGesture.KINECT_DIRECTION_LEFT);
            }
            if (dy > tolerance)
            {
                res.Add(EnumKinectDirectionGesture.KINECT_DIRECTION_UPWARD);
            }
            else if (dy < -tolerance)
            {
                res.Add(EnumKinectDirectionGesture.KINECT_DIRECTION_DOWNWARD);
            }
            if (dz > tolerance)
            {
                res.Add(EnumKinectDirectionGesture.KINECT_DIRECTION_BACKWARD);
            }
            else if (dz < -tolerance)
            {
                res.Add(EnumKinectDirectionGesture.KINECT_DIRECTION_FORWARD);
            }
            if (res.Count == 0)
            {
                res.Add(EnumKinectDirectionGesture.KINECT_DIRECTION_NONE);
            }
            return res;
        }
    }
}

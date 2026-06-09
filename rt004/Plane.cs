using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    public class Plane : Solids
    {
        private const double threshold = 1e-6;
        public Vector3d Normal { get; set; }


        public override Intersection Intersect(Ray ray)
        {
            if (Math.Abs(Vector3d.Dot(Normal, ray.Direction)) < threshold)
                return Intersection.False;

            double point = Vector3d.Dot(Position - ray.Position, Normal) / Vector3d.Dot(Normal, ray.Direction);

            if (point < 0)
                return Intersection.False;

            Vector3d intersectionPoint = ray.LookAt(point);

            //return new Intersection(true, point, Shading, -1 * Math.Sign(Vector3d.Dot(Normal, ray.Direction)) * Normal);
            return new Intersection(
                true,
                point,
                Shading,
                -1 * Math.Sign(Vector3d.Dot(Normal, ray.Direction)) * Normal,
                intersectionPoint,
                Id
            );

        }
    }
}
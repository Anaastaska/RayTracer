using OpenTK.Mathematics;
using rt004;


namespace rt004;
public class Sphere : Solids
{

    public double Radius { get; set; }

    public override Intersection Intersect(Ray ray)
    {

        // Coefficients
        Vector3d oc = ray.Position - Position;
        double a = Vector3d.Dot(ray.Direction, ray.Direction);
        double b = 2.0 * Vector3d.Dot(oc, ray.Direction);
        double c = Vector3d.Dot(oc, oc) - Radius * Radius;

        // Discriminant
        double discriminant = b * b - 4 * a * c;

        // If the discriminant is less than zero => no intersection
        if (discriminant < 0)
            return Intersection.False;

        // 2 points of intersection
        double sqrtDiscriminant = Math.Sqrt(discriminant);
        double t1 = (-b - sqrtDiscriminant) / (2.0 * a);
        double t2 = (-b + sqrtDiscriminant) / (2.0 * a);

        // If both t1 and t2 are negative, both intersections are behind the ray
        if (t1 < 0 && t2 < 0)
            return Intersection.False;

        // If t1 is negative, use t2
        double t = (t1 < 0) ? t2 : t1;

        // If t2 is negative, use t1
        if (t2 < 0) t = t1;

        // Calculate the intersection point and normal
        Vector3d intersectionPoint = ray.LookAt(t);
        Vector3d normal = (intersectionPoint - Position).Normalized();

        return new Intersection(true, t, Shading, normal, intersectionPoint, Id);

    }
}
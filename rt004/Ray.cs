using OpenTK.Mathematics;

namespace rt004;
//parametric ray representation
//Vector3d P0 is ray's origin and Vector3d p1 is its direction vector.
//P0 is a point in 3D space, p1 is a 3D vector/direction.
public struct Ray
{
    //P0, p1
    public Vector3d Position, Direction;

    //Ray = p0 + t*p1
    public Vector3d LookAt(double t) => Position + t * Direction;
}
using OpenTK.Mathematics;
//Class for ray intersection with solids

namespace rt004;

public class Intersection
{

    //if there is no intersection
    public readonly static Intersection False = new(
        false,
        double.PositiveInfinity,
        null,
        Vector3d.Zero,
        Vector3d.Zero,
        0
    );


    public bool Intersected { get; private set; }
    public double Distance { get; private set; }
    public string Shading { get; private set; }
    public Vector3d Normal { get; private set; }

    public Vector3d Position { get; private set; }

    public int SolidIdx { get; private set; }


    public Intersection(bool intersected, double distance, string shading, Vector3d normal, Vector3d position, int solidIdx)
    {
        Intersected = intersected;
        Distance = distance;
        Shading = shading;
        Normal = normal;
        Position = position;
        SolidIdx = solidIdx;
    }
}
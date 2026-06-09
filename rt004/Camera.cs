using OpenTK.Mathematics;
//perspective camera

namespace rt004;
public class Camera
{
    private Vector2d projPlane;
    private double fieldOfView, aspectRatio;
    private Vector2d resolution;
    public Vector3d Position;
    public Vector3d Direction = Vector3d.UnitZ;
    public Vector3d LookUp = Vector3d.UnitY;
    public double DistanceToProjPlane = 1;


    public Vector2d Resolution
    {
        get => resolution;
        set
        {
            resolution = value;
            aspectRatio = resolution.X / resolution.Y;
        }
    }

    public double FOV
    {
        get => fieldOfView;
        set
        {
            fieldOfView = value;
            projPlane.Y = 2 * Math.Tan(MathHelper.DegreesToRadians(fieldOfView) / 2);
            projPlane.X = aspectRatio * projPlane.Y;
        }
    }

    public Camera(Vector2d resolution)
    {
        //Width = ImWidth;
        //Height = ImHeight;
        Resolution = resolution;
        //fieldOfView = 45;
        //FOV = 45;
    }

    public Camera(double width, double height)
    {
        Resolution = new Vector2d(width, height);
    }

    public Ray CreateRay(Vector2d position)
    {
        // Normalized viewport position
        var normalizedPosition = position / (resolution - Vector2d.One);

        // Scale according to the projection plane size and center it
        var positionViewport = normalizedPosition * projPlane - projPlane / 2;


        // Midpoint of the projection plane
        var midpoint = Position + Direction * DistanceToProjPlane;

        // Right vector of the camera
        var rightCameraVec = Vector3d.Cross(LookUp, Direction);

        // Position of the pixel
        var positionPlane = midpoint + positionViewport.X * rightCameraVec - positionViewport.Y * LookUp;

        var computedDirection = positionPlane - Position;

        computedDirection.Normalize();

        // Create and return the ray
        return new Ray()
        {
            Position = Position,
            Direction = computedDirection,
        };
    }
}
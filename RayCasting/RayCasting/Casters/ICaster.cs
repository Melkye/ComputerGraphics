using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting.Casters;
internal interface ICaster
{
    // TODO: shouldn't it take pos + dir as parameters? not from scene
    // to be able to cast camera and light??
    //byte Cast(Scene scene, Point3D screenPoint);
    byte Cast(Scene scene, (float, float, float) pixelAngles);
}

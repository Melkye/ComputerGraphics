using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting.Casters;
public interface ICaster
{
    byte Cast(Scene scene, (float alpha, float beta, float gamma) pixelAngles);
}

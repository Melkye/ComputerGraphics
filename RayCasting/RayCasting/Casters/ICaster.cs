using ImageConverter;
using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting.Casters;
public interface ICaster
{
    Pixel Cast(Scene scene, Vector3D rayDirection);
}

using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Cameras;
using RayCasting.Casters;
using RayCasting.Transformations;

namespace RayCasting.Scenes;
public class Scene
{
    // TODO consider removing public setters 
    public Scene(string name, ICamera camera, ILighting[] lightings, IIntersectable[] figures, int maxFiguresInBox = 10)
    {
        Name = name;
        Camera = camera;
        Lightings = lightings;
        MaxFiguresInBox = maxFiguresInBox;
        Figures = figures;
    }

    private IIntersectable[] _figures;

    private IIntersectable[] _figuresInBoxes;

    public string Name { get; set; }

    public int MaxFiguresInBox { get; set; }

    public ICamera Camera { get; set; }

    public ILighting[] Lightings { get; set; }

    public IIntersectable[] Figures
    {
        get => _figures;
        set
        {
            _figuresInBoxes = BoundingBoxes.GetBoundingBoxes(value, MaxFiguresInBox);
            _figures = value;
        }
    }

    public IIntersectable[] FiguresInBoxes => _figuresInBoxes;

    public void Transform(TransformationMatrix4x4 transformation)
    {
        foreach (var figure in Figures)
        {
            figure.Transform(transformation);
        }

        _figuresInBoxes = BoundingBoxes.GetBoundingBoxes(Figures, MaxFiguresInBox);
    }
}

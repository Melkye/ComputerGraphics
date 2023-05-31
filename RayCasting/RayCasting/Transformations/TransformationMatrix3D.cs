namespace RayCasting.Transformations;

public class TransformationMatrix3D
{
    public TransformationMatrix3D()
    { }

    public TransformationMatrix3D(float[,] matrix)
    {
        Array.Copy(matrix, _matrix, matrix.Length);
    }

    private float[,] _matrix =
    {
        {1, 0, 0, 0 },
        {0, 1, 0, 0 },
        {0, 0, 1, 0 },
        {0, 0, 0, 1 }
    };

    public float this[int i, int j]
    {
        // TODO: add incorrect index handling
        get => _matrix[i, j];
        set => _matrix[i, j] = value;
    }
}

using RayCasting.Objects;

namespace RayCasting.Transformations;

public class TransformationMatrix4x4
{
    public TransformationMatrix4x4()
    { }

    public TransformationMatrix4x4(float[,] matrix)
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

    public TransformationMatrix4x4 Multiply(TransformationMatrix4x4 matrix)
    {
        TransformationMatrix4x4 resultMatrix = new(
            new float[,] 
            {
                {
                    this[0,0]*matrix[0,0] + this[0,1]*matrix[1,0] + this[0,2]*matrix[2,0] + this[0,3]*matrix[3,0],
                    this[0,0]*matrix[0,1] + this[0,1]*matrix[1,1] + this[0,2]*matrix[2,1] + this[0,3]*matrix[3,1],
                    this[0,0]*matrix[0,2] + this[0,1]*matrix[1,2] + this[0,2]*matrix[2,2] + this[0,3]*matrix[3,2],
                    this[0,0]*matrix[0,3] + this[0,1]*matrix[1,3] + this[0,2]*matrix[2,3] + this[0,3]*matrix[3,3]
                },
                {
                    this[1,0]*matrix[0,0] + this[1,1]*matrix[1,0] + this[1,2]*matrix[2,0] + this[1,3]*matrix[3,0],
                    this[1,0]*matrix[0,1] + this[1,1]*matrix[1,1] + this[1,2]*matrix[2,1] + this[1,3]*matrix[3,1],
                    this[1,0]*matrix[0,2] + this[1,1]*matrix[1,2] + this[1,2]*matrix[2,2] + this[1,3]*matrix[3,2],
                    this[1,0]*matrix[0,3] + this[1,1]*matrix[1,3] + this[1,2]*matrix[2,3] + this[1,3]*matrix[3,3]
                },
                {
                    this[2,0]*matrix[0,0] + this[2,1]*matrix[1,0] + this[2,2]*matrix[2,0] + this[2,3]*matrix[3,0],
                    this[2,0]*matrix[0,1] + this[2,1]*matrix[1,1] + this[2,2]*matrix[2,1] + this[2,3]*matrix[3,1],
                    this[2,0]*matrix[0,2] + this[2,1]*matrix[1,2] + this[2,2]*matrix[2,2] + this[2,3]*matrix[3,2],
                    this[2,0]*matrix[0,3] + this[2,1]*matrix[1,3] + this[2,2]*matrix[2,3] + this[2,3]*matrix[3,3],
                },
                {
                    this[3,0]*matrix[0,0] + this[3,1]*matrix[1,0] + this[3,2]*matrix[2,0] + this[3,3]*matrix[3,0],
                    this[3,0]*matrix[0,1] + this[3,1]*matrix[1,1] + this[3,2]*matrix[2,1] + this[3,3]*matrix[3,1],
                    this[3,0]*matrix[0,2] + this[3,1]*matrix[1,2] + this[3,2]*matrix[2,2] + this[3,3]*matrix[3,2],
                    this[3,0]*matrix[0,3] + this[3,1]*matrix[1,3] + this[3,2]*matrix[2,3] + this[3,3]*matrix[3,3],
                }
            }
            );

        return resultMatrix;
    }

    public Vector3D Multiply(Vector3D vector)
    {
        float[] vector4D = new float[] { vector.X, vector.Y, vector.Z, 1 };

        float[] resultVector4D = new float[]
        {
            this[0, 0]*vector4D[0] + this[0, 1]*vector4D[1] + this[0, 2]*vector4D[2] + this[0, 3]*vector4D[3],
            this[1, 0]*vector4D[0] + this[1, 1]*vector4D[1] + this[1, 2]*vector4D[2] + this[1, 3]*vector4D[3],
            this[2, 0]*vector4D[0] + this[2, 1]*vector4D[1] + this[2, 2]*vector4D[2] + this[2, 3]*vector4D[3],
            this[2, 0]*vector4D[0] + this[3, 1]*vector4D[1] + this[3, 2]*vector4D[2] + this[3, 3]*vector4D[3]
        };

        Vector3D resultVector3D = new(resultVector4D[0], resultVector4D[1], resultVector4D[2]);

        return resultVector3D;
    }

    public Point3D Multiply(Point3D point)
    {
        float[] point4D = new float[] { point.X, point.Y, point.Z, 1 };

        float[] resultPoint4D = new float[]
        {
            this[0, 0]*point4D[0] + this[0, 1]*point4D[1] + this[0, 2]*point4D[2] + this[0, 3]*point4D[3],
            this[1, 0]*point4D[0] + this[1, 1]*point4D[1] + this[1, 2]*point4D[2] + this[1, 3]*point4D[3],
            this[2, 0]*point4D[0] + this[2, 1]*point4D[1] + this[2, 2]*point4D[2] + this[2, 3]*point4D[3],
            this[3, 0]*point4D[0] + this[3, 1]*point4D[1] + this[3, 2]*point4D[2] + this[3, 3]*point4D[3]
        };

        Point3D resultPoint3D = new(resultPoint4D[0], resultPoint4D[1], resultPoint4D[2]);

        return resultPoint3D;
    }
}

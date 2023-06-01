namespace RayCasting.Transformations;

public enum Directions
{
    // positive is anticlockwise
    // when axis points toward observer

    // negative is clockwise
    // when axis points toward observer

    // x axis
    PositiveRoll,
    NegativeRoll,

    // y axis
    PositivePitch,
    NegativePitch,

    // z axis
    PositiveYaw,
    NegativeYaw,

    Left = PositiveYaw,
    Right = NegativeYaw,

    Up = PositiveRoll,
    Down = NegativeRoll,
}

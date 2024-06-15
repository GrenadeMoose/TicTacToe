using Unity.VisualScripting;

[System.Serializable, Inspectable]
public class NeighboringSpace
{
    public enum DirectionEnum
    {
        Left,
        Right,
        Top,
        Bottom,
        UpperLeft,
        UpperRight,
        LowerLeft,
        LowerRight,
        Undefined
    }
    public DirectionEnum Direction { get; private set; }
    public BoardSpace Space { get; private set; }

    public NeighboringSpace(BoardSpace space, DirectionEnum d)
    {
        this.Direction = d;
        this.Space = space;
    }
}

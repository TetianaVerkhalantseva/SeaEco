namespace SeaEco.Abstractions.ValueObjects.Half;

public struct HalfValue
{
    public readonly HalfType Key { get; }
    public readonly float Value { get; }

    public HalfValue(HalfType type)
    {
        Key = type;
        Value = type switch
        {
            HalfType.x => 0.5f,
            HalfType.X => 1,
            _ => 0
        };
    }

    public HalfValue(float value)
    {
        switch (value)
        {
            case 0.5f:
            {
                Key = HalfType.x;
                Value = value;
                break;
            }
            case 1:
            {
                Key = HalfType.X;
                Value = value;
                break;
            }
            default:
            {
                Key = HalfType.None;
                Value = 0;
                break;
            }
        }
    }
}
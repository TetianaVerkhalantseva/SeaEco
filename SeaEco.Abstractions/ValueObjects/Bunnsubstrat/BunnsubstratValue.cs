namespace SeaEco.Abstractions.ValueObjects.Bunnsubstrat;

public struct BunnsubstratValue
{
    public readonly BunnsubstratType Key { get; }
    public readonly float Value { get; }

    public BunnsubstratValue(BunnsubstratType type)
    {
        Key = type;
        Value = type switch
        {
            BunnsubstratType.x => 0.5f,
            BunnsubstratType.X => 1,
            _ => 0
        };
    }

    public BunnsubstratValue(float value)
    {
        switch (value)
        {
            case 0.5f:
            {
                Key = BunnsubstratType.x;
                Value = value;
                break;
            }
            case 1:
            {
                Key = BunnsubstratType.X;
                Value = value;
                break;
            }
            default:
            {
                Key = BunnsubstratType.None;
                Value = 0;
                break;
            }
        }
    }
}
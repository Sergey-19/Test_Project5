namespace EQX.Core.InOut
{
    public interface IDOutput
    {
        int Id { get; init; }
        string Name { get; init; }
        bool Value { get; set; }
    }
}

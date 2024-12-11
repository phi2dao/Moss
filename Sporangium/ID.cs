namespace Sporangium;
public readonly record struct ID(int Index, int Version)
{
    public static readonly ID None = new();
    public bool IsValid => Index > 0 && Version >= 0;
}

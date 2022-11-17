namespace TestsUtility.Hashids;

public class HashidsBuilder
{
    private static HashidsBuilder _instance;
    private readonly HashidsNet.Hashids _encripter;

    private HashidsBuilder()
    {
        _encripter ??= new HashidsNet.Hashids("IUHAOIUAH897AH", 3);
    }

    public static HashidsBuilder Instance()
    {
        _instance = new HashidsBuilder();
        return _instance;
    }
    public HashidsNet.Hashids Build()
    {
        return _encripter;
    }
}
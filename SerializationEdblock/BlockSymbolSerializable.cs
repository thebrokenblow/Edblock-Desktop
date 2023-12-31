﻿namespace SerializationEdblock;

public class BlockSymbolSerializable
{
    public string? Id { get; init; }
    public string? NameSymbol { get; init; }
    public TextFieldSerializable? TextFieldSerializable { get; init; }
    public double Width { get; init; }
    public double Height { get; init; }
    public double XCoordinate { get; init; }
    public double YCoordinate { get; init; }
}
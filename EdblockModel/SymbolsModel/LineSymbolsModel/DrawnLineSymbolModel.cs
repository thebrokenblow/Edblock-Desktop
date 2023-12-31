﻿using EdblockModel.SymbolsModel.Enum;

namespace EdblockModel.SymbolsModel.LineSymbolsModel;

public class DrawnLineSymbolModel
{
    public List<LineSymbolModel> LinesSymbolModel { get; set; }
    public BlockSymbolModel? SymbolOutgoingLine { get; set; }
    public BlockSymbolModel? SymbolIncomingLine { get; set; }
    public CoordinateLineModel CoordinateLineModel { get; set; }
    public PositionConnectionPoint OutgoingPosition { get; set; }
    public PositionConnectionPoint IncomingPosition { get; set; }
    public string? Color { get; set; }
    public string? Text { get; set; }

    private readonly int offsetLine = 10;

    public DrawnLineSymbolModel()
    {
        LinesSymbolModel = new();
        CoordinateLineModel = new(LinesSymbolModel);
    }

    public (double x, double y) RoundingCoordinatesLines((double x, double y) startCoordinate, (double x, double y) currentCoordinate)
    {
        if (OutgoingPosition == PositionConnectionPoint.Left || OutgoingPosition == PositionConnectionPoint.Right)
        {
            currentCoordinate = HorizontalRounding(startCoordinate, currentCoordinate);
        }
        else
        {
            currentCoordinate = VerticallyRounding(startCoordinate, currentCoordinate);
        }

        return currentCoordinate;
    }

    public void ChangeCoordinateLine((double x, double y) currentCoordinte)
    {
        if (OutgoingPosition == PositionConnectionPoint.Bottom || OutgoingPosition == PositionConnectionPoint.Top)
        {
            CoordinateLineModel.ChangeCoordinatesVerticalLines(currentCoordinte);
        }
        else
        {
            CoordinateLineModel.ChangeCoordinatesHorizontalLines(currentCoordinte);
        }
    }

    public void AddFirstLine()
    {
        if (SymbolOutgoingLine == null)
        {
            return;
        }

        var (x, y) = SymbolOutgoingLine.GetBorderCoordinate(OutgoingPosition);

        var firstLineSymbolModel = new LineSymbolModel
        {
            X1 = x,
            Y1 = y,
            X2 = x,
            Y2 = y,
        };

        LinesSymbolModel.Add(firstLineSymbolModel);
    }

    public LineSymbolModel GetNewLine()
    {
        var lastLineSymbol = LinesSymbolModel[^1];
        var newLineSymbolModel = FactoryLineSymbolModel.CreateLineByPreviousLine(lastLineSymbol);

        LinesSymbolModel.Add(newLineSymbolModel);

        return newLineSymbolModel;
    }

    private (double x, double y) HorizontalRounding((double x, double y) startCoordinate, (double x, double y) currentCoordinate)
    {
        if (LinesSymbolModel.Count % 2 == 1)
        {
            if (startCoordinate.x > currentCoordinate.x)
            {
                currentCoordinate.x += offsetLine;
            }
        }
        else
        {
            if (startCoordinate.y - offsetLine > currentCoordinate.y)
            {
                currentCoordinate.y += offsetLine;
            }
        }

        return currentCoordinate;
    }

    private (double x, double y) VerticallyRounding((double x, double y) startCoordinate, (double x, double y) currentCoordinate)
    {
        if (LinesSymbolModel.Count % 2 == 1)
        {
            if (startCoordinate.y > currentCoordinate.y)
            {
                currentCoordinate.y += offsetLine;
            }
        }
        else
        {
            if (startCoordinate.x - offsetLine > currentCoordinate.x)
            {
                currentCoordinate.x += offsetLine;
            }
        }

        return currentCoordinate;
    }
}
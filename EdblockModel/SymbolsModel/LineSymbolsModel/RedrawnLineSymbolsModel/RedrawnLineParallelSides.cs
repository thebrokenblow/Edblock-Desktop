﻿using EdblockModel.EnumsModel;
using EdblockModel.SymbolsModel.LineSymbolsModel.DecoratorLineSymbolsModel;

namespace EdblockModel.SymbolsModel.LineSymbolsModel.RedrawnLineSymbolsModel;

internal class RedrawnLineParallelSides
{
    private readonly RedrawnLine _redrawLine;
    private readonly List<CoordinateLine> _decoratedCoordinatesLines;
    private readonly BuilderCoordinateDecorator _builderCoordinateDecorator;
    private readonly SideSymbol? _positionOutgoing;
    private readonly SideSymbol? _positionIncoming;
    private readonly int _baseLineOffset;
    private const int linesSamePositions = 1;
    private const int linesOneDifferentPositions = 3;

    public RedrawnLineParallelSides(DrawnLineSymbolModel drawnLineSymbolModel, RedrawnLine redrawLine, int baseLineOffset)
    {
        _positionOutgoing = drawnLineSymbolModel.OutgoingPosition;
        _positionIncoming = drawnLineSymbolModel.IncomingPosition;

        _decoratedCoordinatesLines = redrawLine.DecoratedCoordinatesLines;

        _redrawLine = redrawLine;

        _baseLineOffset = baseLineOffset;

        _builderCoordinateDecorator = SetBuilderCoordinateDecorator();
    }

    private BuilderCoordinateDecorator SetBuilderCoordinateDecorator()
    {
        var builderCoordinateDecorator = new BuilderCoordinateDecorator();

        if (_positionOutgoing == SideSymbol.Right && _positionIncoming == SideSymbol.Left ||
            _positionOutgoing == SideSymbol.Left && _positionIncoming == SideSymbol.Right)
        {
            builderCoordinateDecorator = builderCoordinateDecorator.SetSwap();

            return builderCoordinateDecorator;
        }
        else if (_positionOutgoing == SideSymbol.Bottom && _positionIncoming == SideSymbol.Bottom)
        {
            builderCoordinateDecorator = builderCoordinateDecorator.SetInversionYCoordinate();

            return builderCoordinateDecorator;
        }
        else if (_positionOutgoing == SideSymbol.Left && _positionIncoming == SideSymbol.Left)
        {
            builderCoordinateDecorator = builderCoordinateDecorator.SetInversionXCoordinate();

            return builderCoordinateDecorator;
        }

        return builderCoordinateDecorator;
    }

    public void RedrawLine((double x, double y) borderCoordinateOutgoingSymbol, (double x, double y) borderCoordinateIncomingSymbol)
    {
        ICoordinateDecorator coordinateSymbolOutgoing = new CoordinateDecorator(borderCoordinateOutgoingSymbol);
        ICoordinateDecorator coordinateSymbolIncoming = new CoordinateDecorator(borderCoordinateIncomingSymbol);

        (coordinateSymbolOutgoing, coordinateSymbolIncoming) =
                RedrawnLine.SetBuilderCoordinate(coordinateSymbolOutgoing, coordinateSymbolIncoming, _builderCoordinateDecorator);

        if (_positionOutgoing == SideSymbol.Bottom && _positionIncoming == SideSymbol.Top ||
            _positionOutgoing == SideSymbol.Top && _positionIncoming == SideSymbol.Bottom ||
            _positionOutgoing == SideSymbol.Right && _positionIncoming == SideSymbol.Left ||
            _positionOutgoing == SideSymbol.Left && _positionIncoming == SideSymbol.Right)
        {
            ChooseWayRedrawDifferentSides(coordinateSymbolOutgoing, coordinateSymbolIncoming, borderCoordinateOutgoingSymbol, borderCoordinateIncomingSymbol);
        }
        else
        {
            _redrawLine.ChangeCountLines(linesOneDifferentPositions, _builderCoordinateDecorator);

            (var firstCoordinateLineIncrement, var secondCoordinateLineIncrement) =
                GetVerticalCoordinateLineIncrement(coordinateSymbolOutgoing, coordinateSymbolIncoming);

            if (_positionOutgoing == SideSymbol.Left && _positionIncoming == SideSymbol.Left ||
                _positionOutgoing == SideSymbol.Right && _positionIncoming == SideSymbol.Right)
            {
                (firstCoordinateLineIncrement, secondCoordinateLineIncrement) =
                    GetHorizontalCoordinateLineIncrement(coordinateSymbolOutgoing, coordinateSymbolIncoming);
            }

            SetCoordinatesIdenticalSides(coordinateSymbolOutgoing, coordinateSymbolIncoming, firstCoordinateLineIncrement, secondCoordinateLineIncrement);
        }
    }

    private ((double x, double y), (double x, double y)) GetVerticalCoordinateLineIncrement(ICoordinateDecorator coordinateSymbolOutgoing, ICoordinateDecorator coordinateSymbolIncoming)
    {
        if (coordinateSymbolOutgoing.Y >= coordinateSymbolIncoming.Y)
        {
            var lineCoordinateIncrement = coordinateSymbolIncoming.Y - _baseLineOffset;

            var firstCoordinateLineIncrement = (coordinateSymbolOutgoing.X, lineCoordinateIncrement);
            var secondCoordinateLineIncrement = (coordinateSymbolIncoming.X, lineCoordinateIncrement);

            return (firstCoordinateLineIncrement, secondCoordinateLineIncrement);
        }
        else
        {
            var lineCoordinateIncrement = coordinateSymbolOutgoing.Y - _baseLineOffset;

            var firstCoordinateLineIncrement = (coordinateSymbolOutgoing.X, lineCoordinateIncrement);
            var secondCoordinateLineIncrement = (coordinateSymbolIncoming.X, lineCoordinateIncrement);

            return (firstCoordinateLineIncrement, secondCoordinateLineIncrement);
        }
    }

    private ((double x, double y), (double x, double y)) GetHorizontalCoordinateLineIncrement(ICoordinateDecorator coordinateSymbolOutgoing, ICoordinateDecorator coordinateSymbolIncoming)
    {
        if (coordinateSymbolOutgoing.X >= coordinateSymbolIncoming.X)
        {
            var lineCoordinateIncrement = coordinateSymbolOutgoing.X + _baseLineOffset;

            var firstCoordinateLineIncrement = (lineCoordinateIncrement, coordinateSymbolOutgoing.Y);
            var secondCoordinateLineIncrement = (lineCoordinateIncrement, coordinateSymbolIncoming.Y);

            return (firstCoordinateLineIncrement, secondCoordinateLineIncrement);
        }
        else
        {
            var lineCoordinateIncrement = coordinateSymbolIncoming.X + _baseLineOffset;

            var firstCoordinateLineIncrement = (lineCoordinateIncrement, coordinateSymbolOutgoing.Y);
            var secondCoordinateLineIncrement = (lineCoordinateIncrement, coordinateSymbolIncoming.Y);

            return (firstCoordinateLineIncrement, secondCoordinateLineIncrement);
        }
    }

    private void ChooseWayRedrawDifferentSides(
        ICoordinateDecorator coordinateSymbolOutgoing,
        ICoordinateDecorator coordinateSymbolIncoming,
        (double x, double y) borderCoordinateOutgoingSymbol,
        (double x, double y) borderCoordinateIncomingSymbol)
    {
        if (coordinateSymbolOutgoing.X == coordinateSymbolIncoming.X && coordinateSymbolOutgoing.Y < coordinateSymbolIncoming.Y)
        {
            _redrawLine.ChangeCountLines(linesSamePositions);
            SetCoordinatesLastLine(borderCoordinateOutgoingSymbol, borderCoordinateIncomingSymbol);
        }
        else
        {
            _redrawLine.ChangeCountLines(linesOneDifferentPositions, _builderCoordinateDecorator);
            SetCoordinatesOneDifferentPositions(coordinateSymbolOutgoing, coordinateSymbolIncoming);
        }
    }

    private void SetCoordinatesLastLine(
        (double x, double y) borderCoordinateOutgoingSymbol,
        (double x, double y) borderCoordinateIncomingSymbol)
    {
        var firstLine = _decoratedCoordinatesLines[^1];

        firstLine.CoordinateSymbolOutgoing.X = borderCoordinateOutgoingSymbol.x;
        firstLine.CoordinateSymbolOutgoing.Y = borderCoordinateOutgoingSymbol.y;

        firstLine.CoordinateSymbolIncoming.X = borderCoordinateIncomingSymbol.x;
        firstLine.CoordinateSymbolIncoming.Y = borderCoordinateIncomingSymbol.y;
    }

    private void SetCoordinatesOneDifferentPositions(
        ICoordinateDecorator coordinateSymbolOutgoing,
        ICoordinateDecorator coordinateSymbolIncoming)
    {
        var firstLine = _decoratedCoordinatesLines[0];

        firstLine.CoordinateSymbolOutgoing.X = coordinateSymbolOutgoing.X;
        firstLine.CoordinateSymbolOutgoing.Y = coordinateSymbolOutgoing.Y;

        firstLine.CoordinateSymbolIncoming.X = coordinateSymbolOutgoing.X;
        firstLine.CoordinateSymbolIncoming.Y = coordinateSymbolOutgoing.Y + (coordinateSymbolIncoming.Y - coordinateSymbolOutgoing.Y) / 2;

        var secondLine = _decoratedCoordinatesLines[1];

        secondLine.CoordinateSymbolOutgoing.X = firstLine.CoordinateSymbolIncoming.X;
        secondLine.CoordinateSymbolOutgoing.Y = firstLine.CoordinateSymbolIncoming.Y;

        secondLine.CoordinateSymbolIncoming.X = coordinateSymbolIncoming.X;
        secondLine.CoordinateSymbolIncoming.Y = firstLine.CoordinateSymbolIncoming.Y;

        var thirdLine = _decoratedCoordinatesLines[2];

        thirdLine.CoordinateSymbolOutgoing.X = secondLine.CoordinateSymbolIncoming.X;
        thirdLine.CoordinateSymbolOutgoing.Y = secondLine.CoordinateSymbolIncoming.Y;

        thirdLine.CoordinateSymbolIncoming.X = coordinateSymbolIncoming.X;
        thirdLine.CoordinateSymbolIncoming.Y = coordinateSymbolIncoming.Y;
    }

    private void SetCoordinatesIdenticalSides(
        ICoordinateDecorator coordinateSymbolOutgoing,
        ICoordinateDecorator coordinateSymbolIncoming,
        (double x, double y) coordinateFirstLine,
        (double x, double y) coordinateSecondLine)
    {
        var firstLine = _decoratedCoordinatesLines[0];

        firstLine.CoordinateSymbolOutgoing.X = coordinateSymbolOutgoing.X;
        firstLine.CoordinateSymbolOutgoing.Y = coordinateSymbolOutgoing.Y;

        firstLine.CoordinateSymbolIncoming.X = coordinateFirstLine.x;
        firstLine.CoordinateSymbolIncoming.Y = coordinateFirstLine.y;

        var secondLine = _decoratedCoordinatesLines[1];

        secondLine.CoordinateSymbolOutgoing.X = firstLine.CoordinateSymbolIncoming.X;
        secondLine.CoordinateSymbolOutgoing.Y = firstLine.CoordinateSymbolIncoming.Y;

        secondLine.CoordinateSymbolIncoming.X = coordinateSecondLine.x;
        secondLine.CoordinateSymbolIncoming.Y = coordinateSecondLine.y;

        var thirdLine = _decoratedCoordinatesLines[2];

        thirdLine.CoordinateSymbolOutgoing.X = secondLine.CoordinateSymbolIncoming.X;
        thirdLine.CoordinateSymbolOutgoing.Y = secondLine.CoordinateSymbolIncoming.Y;

        thirdLine.CoordinateSymbolIncoming.X = coordinateSymbolIncoming.X;
        thirdLine.CoordinateSymbolIncoming.Y = coordinateSymbolIncoming.Y;
    }
}
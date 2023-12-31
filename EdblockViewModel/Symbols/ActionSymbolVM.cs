﻿using EdblockModel.SymbolsModel;
using EdblockViewModel.Symbols.Abstraction;
using EdblockViewModel.Symbols.CommentSymbolVMComponents;

namespace EdblockViewModel.Symbols;

public class ActionSymbolVM : BlockSymbolVM, IHaveCommentVM
{
    private const int defaultWidth = 140;
    private const int defaultHeigth = 60;

    private const string defaultText = "Действие";
    private const string defaultColor = "#FF52C0AA";

    private CommentSymbolVM? commentSymbolVM;
    public CommentSymbolVM? CommentSymbolVM
    {
        get => commentSymbolVM;
        set
        {
            if (value is null)
            {
                if (commentSymbolVM is not null)
                {
                    commentSymbolVM.ConnectionPointVM.IsHasConnectingLine = false;
                }
            }

            commentSymbolVM = value;
            commentSymbolVM?.AddСomment(this);
        }
    }

    public ActionSymbolVM(EdblockVM edblockVM) : base(edblockVM)
    {
        ScaleRectangles = _builderScaleRectangles
                        .AddMiddleTopRectangle()
                        .AddRightTopRectangle()
                        .AddRightMiddleRectangle()
                        .AddRightBottomRectangle()
                        .AddMiddleBottomRectangle()
                        .AddLeftBottomRectangle()
                        .AddLeftMiddleRectangle()
                        .AddLeftTopRectangle()
                        .Build();

        Color = defaultColor;

        TextFieldVM.Text = defaultText;

        Width = defaultWidth;
        Height = defaultHeigth;

        SetWidth(Width);
        SetHeight(Height);
    }

    public override void SetWidth(double width)
    {
        Width = width;

        var textFieldWidth = BlockSymbolModel.GetTextFieldWidth();
        var textFieldLeftOffset = BlockSymbolModel.GetTextFieldLeftOffset();

        TextFieldVM.Width = textFieldWidth;
        TextFieldVM.LeftOffset = textFieldLeftOffset;

        ChangeCoordinateAuxiliaryElements();

        CommentSymbolVM?.SetCoordinateBlockSymbol();
    }

    public override void SetHeight(double height)
    {
        Height = height;

        var textFieldHeight = BlockSymbolModel.GetTextFieldHeight();
        var textFieldTopOffset = BlockSymbolModel.GetTextFieldTopOffset();

        TextFieldVM.Height = textFieldHeight;
        TextFieldVM.TopOffset = textFieldTopOffset;

        ChangeCoordinateAuxiliaryElements();

        CommentSymbolVM?.SetCoordinateBlockSymbol();
    }

    public override BlockSymbolModel CreateBlockSymbolModel()
    {
        var nameBlockSymbolVM = GetType().BaseType?.ToString();

        var actionSymbolModel = new ActionSymbolModel
        {
            Id = Id,
            NameSymbol = nameBlockSymbolVM,
            Color = Color
        };

        return actionSymbolModel;
    }
}
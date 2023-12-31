﻿namespace EdblockModel.SymbolsModel;

public class ConditionSymbolModel : BlockSymbolModel
{
    public override double GetTextFieldWidth()
    {
        return Width / 2;
    }

    public override double GetTextFieldHeight()
    {
        return Height / 2;
    }

    public override double GetTextFieldLeftOffset()
    {
        return Width / 4;
    }

    public override double GetTextFieldTopOffset()
    {
        return Height / 4;
    }
}

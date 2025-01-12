using System.ComponentModel;

namespace ClassLibrary.Enums;

public enum RectangleParameter
{
    [Description("Width")]
    Width,
    [Description("Height")]
    Height
}

public enum ParallelogramParameter
{
    [Description("Base")]
    Base,
    [Description("Height")]
    Height,
    [Description("Side")]
    Side
}

public enum TriangleParameter
{
    [Description("Side A")]
    SideA,
    [Description("Side B")]
    SideB,
    [Description("Side C")]
    SideC,
    [Description("Height")]
    Height
}

public enum RhombusParameter
{
    [Description("Side")]
    Side,
    [Description("Height")]
    Height
}
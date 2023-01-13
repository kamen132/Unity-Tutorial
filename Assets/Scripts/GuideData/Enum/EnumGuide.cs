namespace Guide
{
    public enum ArrowDirection
    {
        UpToDown = 0,
        DownToUp = 1,
        LeftToRight = 2,
        RightToLeft = 3,
        TopRight = 4,
        BottomRight = 5,
        TopLeft = 6,
        BottomLeft = 7,
    }

    public enum GuideState
    {
        NotStart,
        Running,
        End
    }
}
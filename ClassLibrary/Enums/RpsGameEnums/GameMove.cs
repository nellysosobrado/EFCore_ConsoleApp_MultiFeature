namespace ClassLibrary.Enums.RpsGameEnums;

public enum GameMove
{
    [AsciiArt(@"
    _______
---'   ____)
      (_____)
      (_____)
      (____)
---.__(___)
")]
    Rock,

    [AsciiArt(@"
     _______
---'    ____)____
           ______)
          _______)
         _______)
---.__________)
")]
    Paper,

    [AsciiArt(@"
    _______
---'   ____)____
          ______)
       __________)
      (____)
---.__(___)
")]
    Scissors
}

[AttributeUsage(AttributeTargets.Field)]
public class AsciiArtAttribute : Attribute
{
    public string Art { get; }

    public AsciiArtAttribute(string art)
    {
        Art = art;
    }
}

public static class GameMoveExtensions
{
    public static string GetAsciiArt(this GameMove move)
    {
        var memberInfo = typeof(GameMove).GetMember(move.ToString())[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(AsciiArtAttribute), false)
            .FirstOrDefault() as AsciiArtAttribute;

        return attribute?.Art ?? string.Empty;
    }
}
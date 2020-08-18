namespace RobWest.Functional
{
    public static class EnumExtensions
    {
        public static Option<T> Parse<T>(this string input) where T : struct
            => System.Enum.TryParse(input, out T result) ? F.Some(result) : F.None;
    }
}
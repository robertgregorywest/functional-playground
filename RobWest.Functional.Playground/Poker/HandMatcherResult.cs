namespace RobWest.Functional.Playground.Poker
{
    public readonly struct HandMatcherResult
    {
        public bool IsMatch { get; }
        public string Name { get; }

        private HandMatcherResult(bool isMatch, string name) => (IsMatch, Name) = (isMatch, name);

        public static HandMatcherResult Match(string name) => new HandMatcherResult(true, name);

        public static HandMatcherResult NoMatch() => new HandMatcherResult(false, "No match");
    }
}
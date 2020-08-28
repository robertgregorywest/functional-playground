namespace RobWest.Functional.Playground.Poker
{
    public readonly struct HandResult
    {
        public bool IsMatch { get; }
        public string Name { get; }
        public int Rank { get; }

        private HandResult(bool isMatch, string name, int rank) => (IsMatch, Name, Rank) = (isMatch, name, rank);

        public static HandResult Match(string name, int rank) => new HandResult(true, name, rank);

        public static HandResult NoMatch(string name) => new HandResult(false, name, 0);
    }
}
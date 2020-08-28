namespace RobWest.Functional.Playground.Poker
{
    public readonly struct HandRankerResult
    {
        public string Name { get; }
        public int Rank { get; }

        public HandRankerResult(string name, int rank) => (Name, Rank) = (name, rank);

        public override string ToString()
        {
            return $"{Name}{Rank.ToString()}";
        }
    }
}
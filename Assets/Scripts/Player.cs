namespace Assets.Scripts
{
    public class Player
    {
        public Player(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
        public int Score { get; private set; }

        public void AddPoints(int points) => this.Score += points;
    }
}

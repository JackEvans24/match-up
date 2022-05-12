namespace Assets.Scripts
{
    public static class Score
    {
        public static ScoreType GetScoreType(int points)
        {
            if (points > 0)
                return ScoreType.Positive;
            else if (points < 0)
                return ScoreType.Negative;
            else
                return ScoreType.Neutral;
        }
    }
}

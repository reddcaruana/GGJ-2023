public class ScoreEntry
{
    /// <summary>
    /// The team name.
    /// </summary>
    public string Name;
    
    /// <summary>
    /// The score value.
    /// </summary>
    public int Value;

    public ScoreEntry(string name, int value)
    {
        Name = name;
        Value = value;
    }
}
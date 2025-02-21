namespace game2048cs.model;

using System;

public class GameScore
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string PlayerName { get; set; }
    public int TotalScore { get; set; }
    public int MaxNumber { get; set; }
}

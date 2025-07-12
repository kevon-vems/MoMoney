namespace Money.Domain;

public class BracketChange
{
    public int StartAge { get; set; }
    public List<TaxBracket> Brackets { get; set; } = new();
}

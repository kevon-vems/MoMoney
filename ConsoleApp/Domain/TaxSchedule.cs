using System.Collections.Generic;

namespace Money.Domain;

public class TaxSchedule
{
    public List<TaxBracket>? Brackets { get; set; }
    public decimal? FlatRate { get; set; }
    public List<BracketChange>? BracketChanges { get; set; }
    public List<RateChange>? RateChanges { get; set; }
}

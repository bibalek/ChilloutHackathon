public class PTPHandshake
{
    public bool isDealer;
    public bool isBigBlind;
    public int money;
    public string cards;

    public PTPHandshake(bool isDealer, bool isBigBlind, int money, string cards)
    {
        this.isDealer = isDealer;
        this.isBigBlind = isBigBlind;
        this.money = money;
        this.cards = cards;
    }
}

public class PTPHeader
{
    public int integer;
    public bool call;
    public bool raise;
    public bool check;
    public bool fold;      
    public string str;

    public PTPHeader(int integer, bool call, bool raise, bool check, bool fold, string str)
    {
        this.integer = integer;
        this.call = call;
        this.raise = raise;
        this.check = check;
        this.fold = fold;
        this.str = str;
    }
}

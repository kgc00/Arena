public class BaseStats {
    public Stat MovementSpeed { get; private set; }
    public Stat Attack { get; private set; }
    public Stat Defense { get; private set; }
    public Stat Health { get; private set; }
    public Stat Bounty { get; private set; }
    public BaseStats(Stats.Stats initStatValues) {
        MovementSpeed = new Stat (initStatValues.MovementSpeed);
        Attack = new Stat (initStatValues.Attack);
        Defense = new Stat (initStatValues.Defense);
        Health = new Stat (initStatValues.Health);
        Bounty = new Stat (initStatValues.Bounty);
    }
}
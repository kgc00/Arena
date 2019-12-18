public class BaseStats {
    public Stat MovementSpeed { get; private set; }
    public Stat Attack { get; private set; }
    public Stat Defense { get; private set; }
    public Stat Health { get; private set; }
    public BaseStats () {
        MovementSpeed = new Stat (1f);
        Attack = new Stat (1f);
        Defense = new Stat (1f);
        Health = new Stat (3f);
    }
}
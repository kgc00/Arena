namespace UI.InGameShop {
    public class PurchaseEvent {
        public readonly int Cost;
        public readonly string Name;
        public PurchaseEvent(int cost, string name) {
            Cost = cost;
            Name = name;
        }
    }
}
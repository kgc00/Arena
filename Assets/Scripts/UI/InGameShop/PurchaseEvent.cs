namespace UI.InGameShop {
    public class PurchaseEvent {
        public readonly int Cost;
        public readonly string Name;
        public enum PurchaseType {
            Modifier,
            SkillUnlock,
            Item
        }
        public readonly PurchaseType Type;
        public PurchaseEvent(int cost, string name, PurchaseType type) {
            Cost = cost;
            Name = name;
            Type = type;
        }
    }
}
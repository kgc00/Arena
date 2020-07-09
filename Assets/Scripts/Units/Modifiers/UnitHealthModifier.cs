namespace Units.Modifiers {
    public class UnitHealthModifier : UnitDataModifier {
        public override void Handle() {
            Model.health.maxHp += 99;
            base.Handle();
        }
    }
}
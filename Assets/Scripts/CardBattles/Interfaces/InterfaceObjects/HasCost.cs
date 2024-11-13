using UnityEngine;

namespace CardBattles.Interfaces.InterfaceObjects {
    public class HasCost : IHasCost {
        [Min(0)]
        private int cost;

        public HasCost(int cost) {
            this.cost = cost;
        }
        public int GetCost() {
            return cost;
        }
    }
}
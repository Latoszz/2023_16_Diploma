using UnityEngine;

namespace Interfaces {
    public interface IWalkable {
        void SetTargetPoint();
        void Walk(Vector3 target);
    }
}

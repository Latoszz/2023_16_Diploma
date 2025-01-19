using Interaction.Objects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Debugging {
    public class RemoveObstacle : MonoBehaviour, IPointerClickHandler {
        public Obstacle obstacle;


        public void OnPointerClick(PointerEventData eventData) {
            obstacle.SetObstacle(false);
        }
    }
}

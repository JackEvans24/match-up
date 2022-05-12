using UnityEngine;

namespace Assets.Scripts
{
    public class Draggable : MonoBehaviour
    {
        [SerializeField] protected float smoothing = 0.01f;

        protected Vector3 target;
        protected Vector3 currentVelocity;

        protected bool canDrag = true;

        protected virtual void Update()
        {
            var targetPosition = target;
            SmoothToTargetPosition(targetPosition);
        }

        protected virtual void OnMouseDrag()
        {
            if (!canDrag)
                return;

            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        protected void SmoothToTargetPosition(Vector3 target)
        {
            var targetPos = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, smoothing);
            transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
        }
    }
}

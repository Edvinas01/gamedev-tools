using UnityEngine;

namespace GamedevTools.CsharpEvents
{
    [AddComponentMenu("Gamedev Tools/CSharp Events/Mouse Clicker")]
    public class MouseClicker : MonoBehaviour
    {
        #region Editor Fields

        [Header("Dependencies")]
        [Tooltip("Camera which performs the clicks")]
        [SerializeField]
        private Camera mainCamera;

        [Header("Raycast")]
        [Tooltip("Click layer (only objects on this layer will be clicked)")]
        [SerializeField]
        private LayerMask raycastLayerMask;

        [Tooltip("Click distance")]
        [Range(0f, 100f)]
        [SerializeField]
        private float raycastDistance = 100f;

        [Header("Behaviour")]
        [Tooltip("Click power")]
        [Range(0, 100)]
        [SerializeField]
        private int clickDamage = 1;

        [Header("Input")]
        [Tooltip("Name of the button which is used to trigger clicks")]
        [SerializeField]
        private string clickButton = "Fire1";

        #endregion

        #region Unity Lifecycle

        private void Update()
        {
            if (!IsClicked())
            {
                return;
            }

            var ray = CreateRay();
            if (!IsRaycast(ray, out var hit))
            {
                return;
            }

            var clickable = GetClickable(hit);
            if (clickable)
            {
                clickable.OnClick(clickDamage);
            }
        }

        #endregion

        #region Private Methods

        private bool IsClicked()
        {
            return Input.GetButtonDown(clickButton);
        }

        private Ray CreateRay()
        {
            var position = Input.mousePosition;
            var newRay = mainCamera.ScreenPointToRay(position);

            return newRay;
        }

        private bool IsRaycast(Ray ray, out RaycastHit hit)
        {
            return Physics.Raycast(
                ray,
                out hit,
                raycastDistance,
                raycastLayerMask,
                QueryTriggerInteraction.Ignore
            );
        }

        private static Clickable GetClickable(RaycastHit hit)
        {
            return hit.collider.GetComponentInParent<Clickable>();
        }

        #endregion
    }
}

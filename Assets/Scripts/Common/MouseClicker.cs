using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamedevTools.Common
{
    [AddComponentMenu("Gamedev Tools/Common/Mouse Clicker")]
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

        [FormerlySerializedAs("clickButton")]
        [Header("Input")]
        [Tooltip("Name of the button which is used to trigger clicks")]
        [InputAxis]
        [SerializeField]
        private string damageClickButton = "Fire1";

        [Tooltip("Name of the button which is used to trigger info context clicks")]
        [InputAxis]
        [SerializeField]
        private string infoClickButton = "Fire2";

        #endregion

        #region Unity Lifecycle

        private void Update()
        {
            if (IsDamageClicked())
            {
                UpdateDamage();
                return;
            }

            if (IsInfoClicked())
            {
                UpdateInfo();
            }
        }

        #endregion

        #region Private Methods

        private bool IsDamageClicked()
        {
            return Input.GetButtonDown(damageClickButton);
        }

        private bool IsInfoClicked()
        {
            return Input.GetButtonDown(infoClickButton);
        }

        private void UpdateDamage()
        {
            if (TryRaycastClickable(out var clickable))
            {
                clickable.OnClick(clickDamage);
            }
        }

        private void UpdateInfo()
        {
#if UNITY_EDITOR
            if (TryRaycastClickable(out var clickable) && clickable is Object clickableObject)
            {
                UnityEditor.Selection.activeObject = clickableObject;
            }
#endif
        }


        private bool TryRaycastClickable(out IClickable clickable)
        {
            var ray = CreateRay();
            clickable = null;

            if (!IsRaycast(ray, out var hit))
            {
                return false;
            }

            clickable = GetClickable(hit);

            return clickable != null;
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

        private static IClickable GetClickable(RaycastHit hit)
        {
            return hit.collider.GetComponentInParent<IClickable>();
        }

        #endregion
    }
}

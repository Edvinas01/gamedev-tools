using System.Linq;
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

        [Header("Pointer")]
        [Tooltip("Arrow used to indicate currently selected object")]
        [SerializeField]
        private GameObject selectionPointerPrefab;

        [Min(0f)]
        [Tooltip("Offset on Y coordinate for the selection pointer")]
        [SerializeField]
        private float pointerYOffset = 1.5f;

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

        #region Private Fields

        private GameObject selectionArrow;

        #endregion

        #region Private Properties

        private GameObject SelectionPointer
        {
            get
            {
                if (selectionArrow)
                {
                    return selectionArrow;
                }

                selectionArrow = Instantiate(selectionPointerPrefab);

                return selectionArrow;
            }
        }

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
            if (TryRaycastClickable(out var clickable) && clickable is Component component)
            {
                ShowPointer(component);
                UnityEditor.Selection.activeObject = component;
            }
            else
            {
                HidePointer();
                UnityEditor.Selection.activeObject = null;
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

        private void ShowPointer(Component destination)
        {
            var destinationTransform = destination.transform;
            var destinationPosition = destinationTransform.position;

            var pointer = SelectionPointer;
            var pointerTransform = pointer.transform;
            var pointerPosition = destinationPosition;

            pointerPosition.y += GetYOffset(destination);

            pointerTransform.position = pointerPosition;
            pointerTransform.parent = destinationTransform;

            pointer.SetActive(true);
        }

        private void HidePointer()
        {
            SelectionPointer.SetActive(false);
        }

        private float GetYOffset(Component component)
        {
            var maxY = component
                .GetComponentsInChildren<Collider>()
                .Max(componentCollider =>
                {
                    var bounds = componentCollider.bounds;
                    var extents = bounds.extents;

                    return extents.y;
                });

            return maxY + pointerYOffset;
        }

        #endregion
    }
}

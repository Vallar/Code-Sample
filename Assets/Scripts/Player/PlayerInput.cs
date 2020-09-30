using System.Collections;
using System.Collections.Generic;
using ThirteenPixels.Soda;
using UnityEngine;
using UnityEngine.EventSystems;

public enum InputState { BASE_BUILDING, MANAGING }
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GlobalVector2 chosenBasePosition;
    [SerializeField] private GlobalInteractable currentActiveInteractable;
    [SerializeField] private GameEvent switchToInteract;
    [SerializeField] private InputState state;
    private AvatarAgentMove currentSelectedUnit;
    private EventSystem eventSystem;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        eventSystem = EventSystem.current;
    }

    private void OnEnable()
    {
        switchToInteract.onRaise.AddResponse(() => { state = InputState.MANAGING; });
    }

    private void Update()
    {

        switch (state)
        {
            case InputState.BASE_BUILDING:
                BaseBuildingInput();
                break;
            case InputState.MANAGING:
                InteractableManagement();
                break;
        }

    }


    private void BaseBuildingInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(origin: position, direction: Vector2.down, distance: Mathf.Infinity, layerMask: groundMask);

            if (hit.transform != null)
            {
                Collider2D col = Physics2D.OverlapBox(hit.point, Vector2.one, 0);

                if (col.transform == null)
                    chosenBasePosition.value = hit.point;
                else
                    Debug.Log("Can't build here because something blocking your way");
            }
        }
    }

    private void InteractableManagement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (eventSystem.IsPointerOverGameObject() == false)
            {
                Vector2 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, Mathf.Infinity);

                Interactable interactable = hit.transform?.GetComponent<Interactable>();

                if (currentActiveInteractable.value != null && currentActiveInteractable.value is Unit)
                {
                    currentSelectedUnit = currentActiveInteractable.value.GetComponent<AvatarAgentMove>();

                    currentSelectedUnit.MoveToDestination(position);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (currentActiveInteractable.value != null)
            {
                currentActiveInteractable.value.ToggleSelectionEffect();
                currentActiveInteractable.value = null;
                currentSelectedUnit = null;
            }
        }
    }

}

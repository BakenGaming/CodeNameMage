using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using System.Collections;

public class DynamicEventSystemHandler : MonoBehaviour
{
    [Header("References")]
    public List<Selectable> selectables = new List<Selectable>();

    [Header("Controls")]
    [SerializeField] protected InputActionReference navigateReference;

    [Header("Animations")]
    [SerializeField] protected float selectedAnimationScale = 1.1f;
    [SerializeField] protected float scaleDuration = .25f;
    protected Dictionary<Selectable, Vector3> scales = new Dictionary<Selectable, Vector3>();
    protected Tween scaleUpTween, scaleDownTween;
    protected Selectable lastSelected;
    public virtual void OnEnable()
    {
        navigateReference.action.performed += OnNavigate;
    }
    protected virtual IEnumerator SelectAfterDelay()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
    }
    public virtual void OnDisable()
    {
        navigateReference.action.performed -= OnNavigate;
        scaleUpTween.Kill(true);
        scaleDownTween.Kill(true);
    }

    protected virtual void AddSelectionListeners(Selectable selectable)
    {
        EventTrigger trigger = selectable.gameObject.GetComponent<EventTrigger>();
        if(trigger == null) trigger = selectable.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry selectEntry = new EventTrigger.Entry {eventID = EventTriggerType.Select};
        selectEntry.callback.AddListener(OnSelect);
        trigger.triggers.Add(selectEntry);

        EventTrigger.Entry deselectEntry = new EventTrigger.Entry {eventID = EventTriggerType.Deselect};
        deselectEntry.callback.AddListener(OnDeselect);
        trigger.triggers.Add(deselectEntry);

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry{ eventID = EventTriggerType.PointerEnter};
        pointerEnter.callback.AddListener(OnPointerEnter);
        trigger.triggers.Add(pointerEnter);

        EventTrigger.Entry pointerExit = new EventTrigger.Entry{ eventID = EventTriggerType.PointerExit};
        pointerExit.callback.AddListener(OnPointerExit);
        trigger.triggers.Add(pointerExit);
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        SoundManager.PlaySound(SoundManager.Sound.uiClick);

        lastSelected = eventData.selectedObject.GetComponent<Selectable>();
        Vector3 newScale = eventData.selectedObject.transform.localScale * selectedAnimationScale;
        scaleUpTween = eventData.selectedObject.transform.DOScale(newScale, scaleDuration);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        Selectable sel = eventData.selectedObject.GetComponent<Selectable>();
        scaleDownTween = eventData.selectedObject.transform.DOScale(scales[sel], scaleDuration);
    }
    public virtual void OnPointerEnter(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if(pointerEventData != null)
        {
            Selectable sel = pointerEventData.pointerEnter.GetComponentInParent<Selectable>();
            if(sel == null)
            {
                sel = pointerEventData.pointerEnter.GetComponentInChildren<Selectable>();
            }
            pointerEventData.selectedObject = sel.gameObject;
        } 
    }
    public virtual void OnPointerExit(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if(pointerEventData != null) pointerEventData.selectedObject = null;
    }
    protected virtual void OnNavigate(InputAction.CallbackContext context)
    {
        if(EventSystem.current.currentSelectedGameObject == null && lastSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
        }
    }

    #region Helper Methods
    public void AddSelectable(Selectable selectable)
    {
        selectables.Add(selectable);
    }

    public void InitializeSelectables()
    {
        foreach(var selectable in selectables)
        {
            AddSelectionListeners(selectable);
            scales.TryAdd(selectable, selectable.transform.localScale);
        }
    }
    public void SetFirstSelected()
    {
        StartCoroutine(SelectAfterDelay());
    }
    #endregion
}

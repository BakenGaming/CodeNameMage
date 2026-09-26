using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using System.Collections;

public class MenuEventSystemHandler : MonoBehaviour
{
    [Header("References")]
    public List<Selectable> selectables = new List<Selectable>();
    [SerializeField] protected Selectable firstSelected;

    [Header("Controls")]
    [SerializeField] protected InputActionReference navigateReference;

    [Header("Animations")]
    [SerializeField] protected float selectedAnimationScale = 1.1f;
    [SerializeField] protected float scaleDuration = .25f;
    [SerializeField] protected List<GameObject> animationExclusions = new List<GameObject>();
    protected Dictionary<Selectable, Vector3> scales = new Dictionary<Selectable, Vector3>();
    protected Tween scaleUpTween, scaleDownTween;
    protected Selectable lastSelected;
    public virtual void Awake()
    {
        foreach(var selectable in selectables)
        {
            AddSelectionListeners(selectable);
            scales.Add(selectable, selectable.transform.localScale);

        }
    }
    public virtual void OnEnable()
    {
        navigateReference.action.performed += OnNavigate;

        for (int i = 0; i < selectables.Count; i++)
        {
            selectables[i].transform.localScale = scales[selectables[i]];
        }
        StartCoroutine(SelectAfterDelay());
    }
    protected virtual IEnumerator SelectAfterDelay()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
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

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.PlaySound(SoundManager.Sound.uiClick);
        if(animationExclusions.Contains(eventData.selectedObject)) return;

        lastSelected = eventData.selectedObject.GetComponent<Selectable>();
        Vector3 newScale = eventData.selectedObject.transform.localScale * selectedAnimationScale;
        scaleUpTween = eventData.selectedObject.transform.DOScale(newScale, scaleDuration);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(animationExclusions.Contains(eventData.selectedObject)) return;

        Selectable sel = eventData.selectedObject.GetComponent<Selectable>();
        scaleDownTween = eventData.selectedObject.transform.DOScale(scales[sel], scaleDuration);
    }
    public void OnPointerEnter(BaseEventData eventData)
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
    public void OnPointerExit(BaseEventData eventData)
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
}

using TMPro;
using UnityEngine;

public class WhatAmILookingAt : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] float detectionRadius = .1f;
    [SerializeField] LayerMask detectionLayer;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI objectNameText;
    [SerializeField] GameObject[] uiPanels;          // przypisz w Inspectorze
                                                     // (lub usuñ [SerializeField] – patrz Start)

    void Start()
    {
        if (objectNameText == null)
            Debug.LogError("Brak referencji do TextMeshProUGUI");

        // jeœli nic nie przypisano rêcznie – pobierz panele z tagiem
        if (uiPanels == null || uiPanels.Length == 0)
            uiPanels = GameObject.FindGameObjectsWithTag("UIPanel");
    }

    void Update()
    {
        if (objectNameText == null) return;

        bool uiVisible = AnyPanelActive();
        objectNameText.gameObject.SetActive(!uiVisible);

        if (uiVisible) return;

        string objName = "";
        foreach (var hit in Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer))
            if (hit.GetComponent<Interactable>())
            {
                objName = hit.name;
                break;
            }

        objectNameText.text = objName;
    }

    bool AnyPanelActive()
    {
        foreach (var p in uiPanels)
            if (p != null && p.activeSelf) return true;
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

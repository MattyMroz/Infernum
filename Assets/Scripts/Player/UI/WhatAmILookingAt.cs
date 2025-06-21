using UnityEngine;
using TMPro;

public class WhatAmILookingAt : MonoBehaviour
{
    public float detectionRadius = .1f;  // Promieñ wokó³ gracza, w którym sprawdzamy obiekty
    public LayerMask detectionLayer;    // Warstwa, w której bêd¹ obiekty, które chcemy wykrywaæ (np. NPC, przedmioty)

    public TextMeshProUGUI objectNameText;  // Referencja do TextMeshProUGUI (UI Text), gdzie wyœwietlimy nazwê obiektu

    void Update()
    {
        // Upewnij siê, ¿e objectNameText jest przypisane w Inspektorze, zanim u¿yjesz go
        if (objectNameText == null)
        {
            Debug.LogError("objectNameText is not assigned in the Inspector.");
            return;  // Zatrzymaj dalsze wykonywanie kodu, jeœli objectNameText jest null
        }

        // Sprawdzamy, czy którykolwiek panel UI z tagiem "UIPanel" jest aktywowany
        if (IsAnyUIPanelActive())
        {
            objectNameText.gameObject.SetActive(false);  // Ukrywamy tekst, jeœli jakikolwiek panel UI jest aktywowany
            return;  // Zatrzymujemy dalsze przetwarzanie, poniewa¿ ekran UI jest aktywowany
        }

        objectNameText.gameObject.SetActive(true);  // Upewniamy siê, ¿e tekst jest w³¹czony, gdy nie ma aktywnego UI

        // Sprawdzamy obiekty wokó³ gracza, jeœli ¿aden panel UI nie jest aktywowany
        string objectName = "";
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Interactable>() != null)
            {
                objectName = hitCollider.gameObject.name;
                break;  // Zatrzymujemy po znalezieniu pierwszego trafionego obiektu
            }
        }

        // Wyœwietlamy nazwê obiektu, jeœli ¿aden panel UI nie jest aktywowany
        objectNameText.text = objectName;
    }

    // Funkcja sprawdzaj¹ca, czy jakikolwiek panel UI z tagiem "UIPanel" jest aktywowany
    bool IsAnyUIPanelActive()
    {
        // W tej funkcji sprawdzamy, czy jakikolwiek panel UI (z tagiem "UIPanel") jest aktywowany
        foreach (GameObject panel in GameObject.FindGameObjectsWithTag("UIPanel"))
        {
            if (panel.activeSelf)  // Sprawdzamy, czy panel jest aktywowany
            {
                return true;  // Zwracamy true, jeœli jakikolwiek panel jest aktywowany
            }
        }

        return false;  // Zwracamy false, jeœli ¿aden panel UI nie jest aktywowany
    }

    // Funkcja rysuj¹ca okr¹g w edytorze (zasiêg detekcji)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // Rysujemy okr¹g wokó³ modelu gracza
    }
}

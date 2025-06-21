using UnityEngine;
using TMPro;

public class WhatAmILookingAt : MonoBehaviour
{
    public float detectionRadius = .1f;  // Promie� wok� gracza, w kt�rym sprawdzamy obiekty
    public LayerMask detectionLayer;    // Warstwa, w kt�rej b�d� obiekty, kt�re chcemy wykrywa� (np. NPC, przedmioty)

    public TextMeshProUGUI objectNameText;  // Referencja do TextMeshProUGUI (UI Text), gdzie wy�wietlimy nazw� obiektu

    void Update()
    {
        // Upewnij si�, �e objectNameText jest przypisane w Inspektorze, zanim u�yjesz go
        if (objectNameText == null)
        {
            Debug.LogError("objectNameText is not assigned in the Inspector.");
            return;  // Zatrzymaj dalsze wykonywanie kodu, je�li objectNameText jest null
        }

        // Sprawdzamy, czy kt�rykolwiek panel UI z tagiem "UIPanel" jest aktywowany
        if (IsAnyUIPanelActive())
        {
            objectNameText.gameObject.SetActive(false);  // Ukrywamy tekst, je�li jakikolwiek panel UI jest aktywowany
            return;  // Zatrzymujemy dalsze przetwarzanie, poniewa� ekran UI jest aktywowany
        }

        objectNameText.gameObject.SetActive(true);  // Upewniamy si�, �e tekst jest w��czony, gdy nie ma aktywnego UI

        // Sprawdzamy obiekty wok� gracza, je�li �aden panel UI nie jest aktywowany
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

        // Wy�wietlamy nazw� obiektu, je�li �aden panel UI nie jest aktywowany
        objectNameText.text = objectName;
    }

    // Funkcja sprawdzaj�ca, czy jakikolwiek panel UI z tagiem "UIPanel" jest aktywowany
    bool IsAnyUIPanelActive()
    {
        // W tej funkcji sprawdzamy, czy jakikolwiek panel UI (z tagiem "UIPanel") jest aktywowany
        foreach (GameObject panel in GameObject.FindGameObjectsWithTag("UIPanel"))
        {
            if (panel.activeSelf)  // Sprawdzamy, czy panel jest aktywowany
            {
                return true;  // Zwracamy true, je�li jakikolwiek panel jest aktywowany
            }
        }

        return false;  // Zwracamy false, je�li �aden panel UI nie jest aktywowany
    }

    // Funkcja rysuj�ca okr�g w edytorze (zasi�g detekcji)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // Rysujemy okr�g wok� modelu gracza
    }
}

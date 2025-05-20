using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickMinigame : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI displayLvl;
    [SerializeField] private TextMeshProUGUI displayExp;
    [SerializeField] private TextMeshProUGUI displayClicks;  


    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private int keyGain = 1;                

    private const KeyCode CURRENT_KEY = KeyCode.Space;       
    private int _clicks = 0;                              
    private Player _playerScript;


    private IEnumerator Start()
    {
        _playerScript = _player.GetComponent<Player>();

        yield return null;

        UpdateHud();      // EXP / Level
        UpdateClicks();   // Klikniêcia
    }

    private void Update()
    {
        if (Input.GetKeyDown(CURRENT_KEY))
        {
            _playerScript.exams_knowledge[2] += keyGain; // programming - id : 2
            _clicks++;

            UpdateHud();
            UpdateClicks();
          
        }

        if (Input.GetKeyDown(KeyCode.X))         // wyjœcie z minigry
            _panel.SetActive(false);
    }



    private void UpdateHud()
    {
        int lvl = _playerScript.exams_knowledge[2] / 100;
        displayLvl.text = $"{lvl}";
        int exp = _playerScript.exams_knowledge[2] % 100;
        displayExp.text = $"{exp} / 100";
    }

    private void UpdateClicks()
    {
        displayClicks.text = $"Klikniêcia: {_clicks}";
    }
}

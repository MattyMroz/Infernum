using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class GraphMinigame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI displayKeys;
    [SerializeField] private TextMeshProUGUI displayLvl;
    [SerializeField] private TextMeshProUGUI displayExp;

    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private int comboGain = 15;
    [SerializeField] private int comboLength = 3;

    private readonly KeyCode[] _possibleKeys =
    {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
        KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
    };

    private KeyCode[] _currentCombo;
    private Player _playerScript;


    private IEnumerator Start()
    {
        _playerScript = _player.GetComponent<Player>();

        yield return null;

        GenerateCombo();
        UpdateHud();
    }


    private void Update()
    {
        if (_currentCombo == null) return;            

        if (Input.GetKeyDown(KeyCode.X))            
        {
            _panel.SetActive(false);
            enabled = false;                          
            return;
        }

        bool allPressed = _currentCombo.All(Input.GetKey);
        bool anyPressedNow = _currentCombo.Any(Input.GetKeyDown);

        if (allPressed && anyPressedNow)
        {
            _playerScript.exams_knowledge[3] += comboGain;
            UpdateHud();
            GenerateCombo();
        }
    }

  
    private void GenerateCombo()
    {
        _currentCombo = _possibleKeys
            .OrderBy(_ => Random.value)
            .Take(comboLength)
            .ToArray();

        displayKeys.text = string.Join(" + ",
            _currentCombo.Select(k => k.ToString().Replace("Alpha", "")));
    }

    private void UpdateHud()
    {
        int totalExp = _playerScript.exams_knowledge[3];
        displayLvl.text = $"{totalExp / 100}";
        displayExp.text = $"{totalExp % 100} / 100";
    }

    public void FocusInput() { }
}

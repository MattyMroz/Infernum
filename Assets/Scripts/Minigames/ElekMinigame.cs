using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class ElekMinigame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image indicator; 
    [SerializeField] private TextMeshProUGUI label; 
    [SerializeField] private TextMeshProUGUI displayLvl;
    [SerializeField] private TextMeshProUGUI displayExp;

    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private int baseGain = 25;  
    [SerializeField] private float minWait = 1f;  
    [SerializeField] private float maxWait = 3f;

    private Player _playerScript;
    private bool _ready;    
    private float _goTime;


    private IEnumerator Start()
    {
        _playerScript = _player.GetComponent<Player>();
        
        yield return null;
        
        indicator.color = Color.red;
        label.text = "REAGUJ 'SPACE'";

        UpdateHud();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            _panel.SetActive(false);
            enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_ready)
            {
                float reaction = UnityEngine.Time.time - _goTime;
                int gain = Mathf.Max(1, baseGain - Mathf.RoundToInt(reaction * baseGain));

                _playerScript.exams_knowledge[4] += gain;
                UpdateHud();
                StartCoroutine(WaitAndGo());     // kolejny cykl
            }
            else                                 // przedwczesny klik
            {
                StopAllCoroutines();
                StartCoroutine(WaitAndGo());
            }
        }
    }


    private IEnumerator WaitAndGo()
    {
        _ready = false;
        indicator.color = Color.red;
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));

        _goTime = UnityEngine.Time.time;
        _ready = true;
        indicator.color = Color.green;
    }


    private void UpdateHud()
    {
        int totalExp = _playerScript.exams_knowledge[4];
        displayLvl.text = $"{totalExp / 100}";
        displayExp.text = $"{totalExp % 100} / 100";
    }
}

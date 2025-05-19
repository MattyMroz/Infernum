using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfMinigame : MonoBehaviour
{
    /* ----------  UI ---------- */
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI displayLvl;
    [SerializeField] private TextMeshProUGUI displayExp;
    [SerializeField] private RectTransform playArea;

    /* ---------- Gameplay ---------- */
    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Image ball;
    [SerializeField] private float liftForce = 1800f;
    [SerializeField] private float gravity = 900f;
    [SerializeField] private int expPerSec = 5;
    [SerializeField] private int knowledgeIndex = 1;   // slot w exams_knowledge

    /* ---------- Internal ---------- */
    private Player _playerScript;
    private Vector2 _velocity;
    private float _secTimer;

    /* ---------- Start (IEnumerator) ---------- */
    private IEnumerator Start()
    {
     
        _playerScript = _player.GetComponent<Player>();

        yield return null;

        CenterBall();
        UpdateHud();
    }

    /* ---------- Update ---------- */
    private void Update()
    {
        /* wyjście z minigry */
        if (Input.GetKeyDown(KeyCode.X))
        {
            _panel.SetActive(false);
            return;
        }

        /* sterowanie piłką */
        if (Input.GetKey(KeyCode.Space))
            _velocity.y += liftForce * UnityEngine.Time.deltaTime;

        _velocity.y -= gravity * UnityEngine.Time.deltaTime;
        ball.rectTransform.anchoredPosition += _velocity * UnityEngine.Time.deltaTime;

        /* sprawdzanie, czy piłka jest w playArea */
        bool inside = RectTransformUtility.RectangleContainsScreenPoint(
            playArea,
            ball.rectTransform.position,
            null);

        if (inside)
        {
            _secTimer += UnityEngine.Time.deltaTime;
            if (_secTimer >= 1f)
            {
                _playerScript.exams_knowledge[knowledgeIndex] += expPerSec;
                _secTimer -= 1f;
                UpdateHud();
            }
        }
        else
        {
            _secTimer = 0f;
            _velocity = Vector2.zero;
        }
    }

    /* ---------- Helpers ---------- */
    private void CenterBall()
    {
        ball.rectTransform.anchoredPosition = playArea.rect.center;
        _velocity = Vector2.zero;
    }

    private void UpdateHud()
    {
        int total = _playerScript.exams_knowledge[knowledgeIndex];
        displayLvl.text = $"{total / 100}";
        displayExp.text = $"{total % 100} / 100";
    }

    public void FocusInput() { }
}

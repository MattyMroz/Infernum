using System.Collections;
using TMPro;
using UnityEngine;

public class MathMinigame : BaseMinigame
{
    [System.Serializable]
    public class PlayerSlot
    {
        public Player player;
        public GameObject panel;
        public KeyCode exitKey;
    }

    [Header("Players")]
    [SerializeField] private PlayerSlot player1;
    [SerializeField] private PlayerSlot player2;

    private TextMeshProUGUI displayNumber;
    private TMP_InputField inputField;
    private TextMeshProUGUI lvlTxt;
    private TextMeshProUGUI expTxt;
    private TextMeshProUGUI timeTxt;
    private TextMeshProUGUI nameTxt;

    [Header("Gameplay")]
    [SerializeField] private int mathGain = 10;
    [SerializeField] private int knowledgeIndex = 0;   // Math

    private int randomNumber;
    private PlayerSlot cur;            // aktualnie grający slot - chodzi o gracza

    /* ───── Interakcja z obiektem w świecie ───── */
    public override void React(GameObject playerGO)
    {
        cur = playerGO == player1.player.gameObject ? player1 : player2;

        // boot bazowej logiki (blokada ruchu + pokazanie panelu) - CHAT UGOTOWAŁ - CZEMU MUSIAŁEM NAPISAĆ SPECJALNIE Boot()? WTF
        Boot(playerGO, new MinigameConfig
        {
            panel = cur.panel,
            exitKey = cur.exitKey,
            actionKeys = null
        });
    }

    // Błagam zostawcie to w spokoju. // To wywołuje bazowa klasa przy bootowaniu minigry.
    protected override void OnBoot(MinigameConfig _)
    {
        BindUI(cur.panel);
        nameTxt.text = "Matematyka";

        inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        inputField.characterValidation = TMP_InputField.CharacterValidation.Digit;
        inputField.onValidateInput += OnlyDigits;
        inputField.onEndEdit.AddListener(CheckInput);

        GenerateNumber();
        UpdateHUD();
        FocusInput();
    }

    protected override void OnOpen()
    {
        BindUI(cur.panel);
        FocusInput();
    }

    protected override void Update()
    {
        if (player == null)
            return;

        UpdateHUD();

        base.Update();                      //  Exit
        if (!active) return;

        if (inputField.isFocused &&
            (Input.GetKeyDown(KeyCode.LeftArrow) ||
             Input.GetKeyDown(KeyCode.RightArrow) ||
             Input.GetKeyDown(KeyCode.UpArrow) ||
             Input.GetKeyDown(KeyCode.DownArrow)))
        {
            int len = inputField.text.Length;
            inputField.caretPosition =
            inputField.selectionAnchorPosition =
            inputField.selectionFocusPosition = len;
        }
    }

    private void BindUI(GameObject panel)
    {
        // znajdowanie obietków po nazwach
        displayNumber = panel.transform.Find("DisplayNumber").GetComponent<TextMeshProUGUI>();
        inputField = panel.transform.Find("InputNumber").GetComponent<TMP_InputField>();
        lvlTxt = panel.transform.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        expTxt = panel.transform.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        timeTxt = panel.transform.Find("Time").GetComponent<TextMeshProUGUI>();
        nameTxt = panel.transform.Find("MinigameName").GetComponent<TextMeshProUGUI>();
    }

    private void GenerateNumber()
    {
        randomNumber = Random.Range(1_000_000, 10_000_000);
        displayNumber.text = randomNumber.ToString();
        inputField.text = string.Empty;
    }

    private void CheckInput(string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput)) return;

        if (int.TryParse(userInput, out int n) && n == randomNumber)
        {
            player.exams_knowledge[knowledgeIndex] += mathGain;
            UpdateHUD();
        }

        GenerateNumber();
        FocusInput();
    }

    private char OnlyDigits(string _, int __, char c) => char.IsDigit(c) ? c : '\0';

    private void UpdateHUD()
    {
        var r = player.LvlIncrease(player.exams_knowledge[knowledgeIndex]);
        lvlTxt.text = r.lvl.ToString();
        expTxt.text = $"{r.exp} / {r.divide}";
        timeTxt.text = Time.Time_now;
    }

    private void FocusInput() => inputField.ActivateInputField();
}

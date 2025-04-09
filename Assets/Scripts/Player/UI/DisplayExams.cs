using TMPro;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class DisplayExams : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI grade;

    [SerializeField] private GameObject panel;

    // player script
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject exam;

    private Player _player_script;
    private Rigidbody2D _rb;

    public KeyCode display;

    private void Start()
    {
        _player_script = player.GetComponent<Player>();
        _rb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Display();
    }



    private void Display()
    {
        if (Input.GetKey(display))
        {
            player.GetComponent<Movement>().ResetVelocity();
            player.GetComponent<Movement>().enabled = false;
            _rb.bodyType = RigidbodyType2D.Static;

            panel.SetActive(true);

            Exams exam_script = exam.GetComponent<Exams>();

            grade.text = "";

            for (int i = 0; i < exam_script.exams.Count; i++)
            {
                int score = exam_script.exams[i].score[_player_script.id];
                Debug.Log(score);
                float ocena;
                score /= 10;
                switch (score)
                {
                    case 9: ocena = 5.0f; break;
                    case 8: ocena = 4.5f; break;
                    case 7: ocena = 4.0f; break;
                    case 6: ocena = 3.5f; break;
                    case 5: ocena = 3.0f; break;
                    default: ocena = 2.0f; break;
                }
                if (score == 0 && !exam_script.exams[i].failed[_player_script.id])
                {
                    grade.text += "\n";
                }
                else
                {
                    grade.text += (ocena % 1 == 0 ? ((int)ocena).ToString() : ocena.ToString("0.0")) + "\n";
                }
            }
        }
        else
        {
            player.GetComponent<Movement>().enabled = true;
            _rb.bodyType = RigidbodyType2D.Dynamic;

            panel.SetActive(false);
        }
    }
}

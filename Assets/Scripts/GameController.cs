using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    //public GameObject CorrectImage;
    //public GameObject ButImage;
    //public Transform Canvace;

    public Text questionText;
    public Text ScoreText;
    public Text LScoreText;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;
    public GameObject QuestionDisPlay;
    public GameObject RoundOverDisPlay;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive;
    private int questionIndex;
    private int PlayerScore;
    private List<GameObject> answerButtonObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.questions;

        ScoreText.text = "SCORE:" + PlayerScore.ToString();

        questionIndex = 0;
        PlayerScore = 0;

        isRoundActive = true;
        ShowQuestion();
    }

    private void ShowQuestion()
    {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        questionText.text = questionData.questiontext;

        for (int i = 0; i < questionData.answer.Length; i++)
        {
            GameObject answerButtonObject = answerButtonObjectPool.GetObject();
            answerButtonObject.transform.SetParent(answerButtonParent);
            answerButtonObjects.Add(answerButtonObject);

            AnswerButton answerbutton = answerButtonObject.GetComponent<AnswerButton>();
            answerbutton.Setup(questionData.answer[i]);
        }
    }

    private void RemoveAnswerButtons()
    {
        while(answerButtonObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonObjects[0]);
            answerButtonObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (isCorrect)
        {
            PlayerScore += 10;
            ScoreText.text = "SCORE:" + PlayerScore.ToString();
        }
       
       
        if (questionPool.Length > questionIndex +1)
        {
            questionIndex++;
            ShowQuestion();
        }
        else
        {
            EndRound();
        }
    }

    public void EndRound()
    {
        isRoundActive = false;
        QuestionDisPlay.SetActive(false);
        RoundOverDisPlay.SetActive(true);
        LScoreText.text = "SCORE:" + PlayerScore.ToString();
    }

   public void ReturnMenuScene()
    {
        SceneManager.LoadScene("MenuScreen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

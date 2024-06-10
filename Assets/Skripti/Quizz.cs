using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers;
        public int[] correctAnswers; // Indeksi pareizajām atbildēm
    }

    public List<Question> questions;
    public Text questionText;
    public Toggle[] answerToggles; // UI elementiem, kur lietotājs izvēlas atbildes
    public Button submitButton;
    public Text resultText;

    private int currentQuestionIndex = 0;
    private int correctAnswerCount = 0;
    private List<int> incorrectQuestions = new List<int>();

    void Start()
    {
        DisplayQuestion();
        submitButton.onClick.AddListener(CheckAnswer);
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            Question currentQuestion = questions[currentQuestionIndex];
            questionText.text = currentQuestion.questionText;
            for (int i = 0; i < answerToggles.Length; i++)
            {
                answerToggles[i].gameObject.SetActive(i < currentQuestion.answers.Length);
                if (i < currentQuestion.answers.Length)
                {
                    answerToggles[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];
                    answerToggles[i].isOn = false; // Atiestatīt iepriekšējās atbildes
                }
            }
        }
        else
        {
            ShowResults();
        }
    }

    void CheckAnswer()
    {
        Question currentQuestion = questions[currentQuestionIndex];
        List<int> selectedAnswers = new List<int>();

        for (int i = 0; i < answerToggles.Length; i++)
        {
            if (answerToggles[i].isOn)
            {
                selectedAnswers.Add(i);
            }
        }

        bool isCorrect = true;
        foreach (int correctAnswer in currentQuestion.correctAnswers)
        {
            if (!selectedAnswers.Contains(correctAnswer))
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            correctAnswerCount++;
        }
        else
        {
            incorrectQuestions.Add(currentQuestionIndex);
        }

        currentQuestionIndex++;
        DisplayQuestion();
    }

    void ShowResults()
    {
        resultText.text = "You answered " + correctAnswerCount + " questions correctly.\n";

        if (incorrectQuestions.Count > 0)
        {
            resultText.text += "Incorrectly answered questions:\n";
            foreach (int index in incorrectQuestions)
            {
                resultText.text += (index + 1) + ". " + questions[index].questionText + "\n";
            }
        }
    }
}

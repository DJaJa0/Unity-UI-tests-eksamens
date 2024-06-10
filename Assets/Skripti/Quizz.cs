using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quizz : MonoBehaviour
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
        InitializeQuestions();
        DisplayQuestion();
        submitButton.onClick.AddListener(CheckAnswer);
    }

    void InitializeQuestions()
    {
        questions = new List<Question>
        {
            new Question
            {
                questionText = "Which components are necessary for a Unity UI Button to function?",
                answers = new string[] { "Button Component", "Text Component", "Canvas Component", "EventSystem Component" },
                correctAnswers = new int[] { 0, 2, 3 }
            },
            new Question
            {
                questionText = "How can you make a UI Text bold in Unity?",
                answers = new string[] { "Using the Bold tag", "Using the Rich Text feature", "Using the Text component's style settings", "Using a Bold Font" },
                correctAnswers = new int[] { 0, 1, 3 }
            },
            // Pievienojiet vēl 8 jautājumus
            new Question
            {
                questionText = "What is the default layer for UI elements in Unity?",
                answers = new string[] { "Default", "UI", "User Interface", "Overlay" },
                correctAnswers = new int[] { 1 }
            },
            new Question
            {
                questionText = "Which component is used to handle user input in Unity?",
                answers = new string[] { "Input Manager", "Event System", "Canvas", "Graphic Raycaster" },
                correctAnswers = new int[] { 1, 3 }
            },
            new Question
            {
                questionText = "How do you anchor a UI element to the center of the screen?",
                answers = new string[] { "Set anchor presets to middle center", "Set pivot to (0.5, 0.5)", "Set position to (0, 0)", "Use RectTransform component" },
                correctAnswers = new int[] { 0, 1, 3 }
            },
            new Question
            {
                questionText = "Which component is essential for rendering UI elements in Unity?",
                answers = new string[] { "Canvas", "Event System", "RectTransform", "Graphic Raycaster" },
                correctAnswers = new int[] { 0, 2, 3 }
            },
            new Question
            {
                questionText = "What property is used to change the text color of a UI Text component?",
                answers = new string[] { "Color", "FontStyle", "TextColor", "Font" },
                correctAnswers = new int[] { 0 }
            },
            new Question
            {
                questionText = "Which of the following allows for responsive UI design in Unity?",
                answers = new string[] { "Canvas Scaler", "Anchor Presets", "RectTransform", "Layout Groups" },
                correctAnswers = new int[] { 0, 1, 3 }
            },
            new Question
            {
                questionText = "How can you create a drop-down menu in Unity UI?",
                answers = new string[] { "Using the Dropdown component", "Using the Canvas component", "Using the EventSystem component", "Using the Input Field component" },
                correctAnswers = new int[] { 0 }
            },
            new Question
            {
                questionText = "What is the best way to handle multiple resolutions for UI in Unity?",
                answers = new string[] { "Using Canvas Scaler", "Using multiple Canvases", "Using fixed sizes", "Using dynamic anchors" },
                correctAnswers = new int[] { 0, 3 }
            }
        };
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


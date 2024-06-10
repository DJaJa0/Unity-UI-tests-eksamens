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
    public GameObject ResultPanel;
    public Button retryButton;

    private int currentQuestionIndex = 0;
    private int correctAnswerCount = 0;
    private List<int> incorrectQuestions = new List<int>();

    void Start()
    {
        InitializeQuestions();
        DisplayQuestion();
        submitButton.onClick.AddListener(CheckAnswer);
        retryButton.onClick.AddListener(RestartQuiz);
        ResultPanel.SetActive(false);
    }

    void InitializeQuestions()
    {
        questions = new List<Question>
        {
            new Question
            {
                questionText = "Ko var skaitīt kā Unity UI elementu?",
                answers = new string[] { "Pogu", "Teksta lauku", "Bīdjoslu", "Zīmulis" },
                correctAnswers = new int[] { 0, 1, 2 }
            },
            new Question
            {
                questionText = "Kas ir Unity UI?",
                answers = new string[] { "Lietotāja saskarne", "Videospēle", "User Interface", "Mājaslapa" },
                correctAnswers = new int[] { 0, 2 }
            },
            
            new Question
            {
                questionText = "Ko dara Input lauks?",
                answers = new string[] { "nodrošina teksta ievadi", "Ļauj aizvērt Unity", "To pašu ko poga", 
                                        "nodrošina ciparu ievadi" },
                correctAnswers = new int[] { 0, 3 }
            },
            new Question
            {
                questionText = "Ko dara Dropdown UI elements?",
                answers = new string[] { "Izdēš GitBash", "Nodrošina izkrītošo sarakstu attēliem", 
                                       "Ļauj lietotājiem izvēlēties vērtības no iepriekš definētām opcijām.", 
                                       "Nodrošina teksta ievadi"},
                correctAnswers = new int[] { 1, 2 }
            },
            new Question
            {
                questionText = "Vai veidojot spēli ir obligāti jāizmanto skripti?",
                answers = new string[] { "Var neizmantot", "Jā, ir", "Skripti var uzlabot spēles funkcionalitāti, bet nav obligāti",
                                        "Skriptus vispār neizmanto" },
                correctAnswers = new int[] { 0, 2 }
            },
            new Question
            {
                questionText = "Ko parasta dara ar Slider UI elementu?",
                answers = new string[] { "Pievieno bildes, pavelkot", "Ievada tekstu", 
                                        "Maina objekta rotāciju", "Maina objekta izmēru" },
                correctAnswers = new int[] { 2, 3 }
            },
            new Question
            {
                questionText = "Vai parasti norāda skriptu references uz nepieciešamajiem objektiem?",
                answers = new string[] { "Nē, tas notiek automātiski", "Jā", "Nenorāda", "Var nenorādīt, ja neizmanto" },
                correctAnswers = new int[] { 1, 3 }
            },
            new Question
            {
                questionText = "Kā var mainīt UI elementu dizainu?",
                answers = new string[] { "Caur skriptiem", "Caur Unity vizuālo redaktoru", "Caur skriptiem un Unity vizuālo redaktoru",
                                        "Java programmēšanas valodā" },
                correctAnswers = new int[] { 0, 1, 2 }
            },
            new Question
            {
                questionText = "Ko nodrošina toggle poga?",
                answers = new string[] { "Tikai vizuālam izskatam", 
                               "Ļauj lietotājam viegli ieslēgt vai izslēgt noteiktus funkcionalitātes aspektus", 
                               "Nodrošina iespēju veikt izvēli starp vairākiem izkrītošiem variantiem", 
                               "Var izmantot, lai izvēlētos starp diviem alternatīviem stāvokļiem, piemēram, ieslēgt/izslēgt skaņu" },
                correctAnswers = new int[] { 1, 3 }
            },
            new Question
            {
                questionText = "Ko dara Panel elements?",
                answers = new string[] { "Padara vizuāli skaistāku programmu", "Tas var kalpot kā konteineris priekš organizēšanas", 
                                        "Aprēķina Skriptu skaitu", "Izmantoto, lai radītu fiziskus 3D modeļu" },
                correctAnswers = new int[] { 0, 1 }
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
        resultText.text = "Atbildēji uz " + correctAnswerCount + " jautājumiem pareizi.\n";

        if (incorrectQuestions.Count > 0)
        {
            resultText.text += "Nepareizi atbildētie jautājumi:\n";
            foreach (int index in incorrectQuestions)
            {
                resultText.text += (index + 1) + ". " + questions[index].questionText + "\n";
            }
        }

        ResultPanel.SetActive(true);

        }

        void RestartQuiz()
    {
        correctAnswerCount = 0;
        currentQuestionIndex = 0;
        incorrectQuestions.Clear();
        ResultPanel.SetActive(false);
        DisplayQuestion();

    }
}


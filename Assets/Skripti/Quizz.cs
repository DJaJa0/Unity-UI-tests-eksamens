using System.Collections.Generic; //Nodrošina kolekciju klases kā List
using UnityEngine; //Nodrošina galveno Unity funkcionalitāti
using UnityEngine.UI; //Nodrošina Unity UI elementus

public class Quizz : MonoBehaviour
{
    //Definē jautājumu klasi, ko var sēriālizēt, lai to varētu redzēt Unity Inspector
    [System.Serializable]
    public class Question
    {
        public string questionText; //Jautājuma teksts
        public string[] answers; //Atbilžu varianti
        public int[] correctAnswers; //Pareizo atbilžu indeksi
    }

    public List<Question> questions; //Jautājumu saraksts
    public Text questionText; //UI teksts jautājumam
    public Toggle[] answerToggles; //UI elementi atbilžu izvēlei
    public Button submitButton; //Poga atbildes apstiprināšanai
    public Text resultText; //UI teksts rezultātu parādīšanai
    public GameObject ResultPanel; //Panelis rezultātu parādīšanai
    public Button retryButton; //Poga, lai sāktu testu no jauna

    private int currentQuestionIndex = 0; //Pašreizējā jautājuma indekss
    private int correctAnswerCount = 0; //Pareizo atbilžu skaits
    private List<int> incorrectQuestions = new List<int>(); //Saraksts ar nepareizi atbildētajiem jautājumiem

    void Start()
    {
        InitializeQuestions(); //Inicializē jautājumus
        DisplayQuestion(); //Parāda pirmo jautājumu
        submitButton.onClick.AddListener(CheckAnswer); //Pievieno klausītāju submit pogai
        retryButton.onClick.AddListener(RestartQuiz); //Pievieno klausītāju retry pogai
        ResultPanel.SetActive(false); //Paslēpj rezultātu paneli sākumā
    }

    //Inicializē jautājumus ar piemēriem
    void InitializeQuestions()
    {
        questions = new List<Question>
        {
            new Question
            {
                questionText = "Ko var skaitīt kā Unity UI elementu?",
                answers = new string[] {"Pogu", "Teksta lauku", "Bīdjoslu", "Zīmulis"},
                correctAnswers = new int[] {0, 1, 2}
            },
            new Question
            {
                questionText = "Kas ir Unity UI?",
                answers = new string[] {"Lietotāja saskarne", "Videospēle", "User Interface", "Mājaslapa"},
                correctAnswers = new int[] {0, 2}
            },
            new Question
            {
                questionText = "Ko dara Input lauks?",
                answers = new string[] {"nodrošina teksta ievadi", "Ļauj aizvērt Unity", "To pašu ko poga", "nodrošina ciparu ievadi"},
                correctAnswers = new int[] {0, 3}
            },
            new Question
            {
                questionText = "Ko dara Dropdown UI elements?",
                answers = new string[] {"Izdēš GitBash", "Nodrošina izkrītošo sarakstu attēliem",
                "Ļauj lietotājiem izvēlēties vērtības no iepriekš definētām opcijām.", "Nodrošina teksta ievadi"},
                correctAnswers = new int[] {1, 2}
            },
            new Question
            {
                questionText = "Vai veidojot spēli ir obligāti jāizmanto skripti?",
                answers = new string[] {"Var neizmantot", "Jā, ir", "Skripti var uzlabot spēles funkcionalitāti, bet nav obligāti",
                "Skriptus vispār neizmanto"},
                correctAnswers = new int[] {0, 2}
            },
            new Question
            {
                questionText = "Ko parasta dara ar Slider UI elementu?",
                answers = new string[] {"Pievieno bildes, pavelkot", "Ievada tekstu", "Maina objekta rotāciju",
                "Maina objekta izmēru"},
                correctAnswers = new int[] {2, 3}
            },
            new Question
            {
                questionText = "Vai parasti norāda skriptu references uz nepieciešamajiem objektiem?",
                answers = new string[] {"Nē, tas notiek automātiski", "Jā", "Nenorāda", "Var nenorādīt, ja neizmanto"},
                correctAnswers = new int[] {1, 3}
            },
            new Question
            {
                questionText = "Kā var mainīt UI elementu dizainu?",
                answers = new string[] {"Caur skriptiem", "Caur Unity vizuālo redaktoru",
                "Caur skriptiem un Unity vizuālo redaktoru", "Java programmēšanas valodā"},
                correctAnswers = new int[] {0, 1, 2}
            },
            new Question
            {
                questionText = "Ko nodrošina toggle poga?",
                answers = new string[] {"Tikai vizuālam izskatam",
                "Ļauj lietotājam viegli ieslēgt vai izslēgt noteiktus funkcionalitātes aspektus",
                "Nodrošina iespēju veikt izvēli starp vairākiem izkrītošiem variantiem",
                "Var izmantot, lai izvēlētos starp diviem alternatīviem stāvokļiem, piemēram, ieslēgt/izslēgt skaņu"},
                correctAnswers = new int[] {1, 3}
            },
            new Question
            {
                questionText = "Ko dara Panel elements?",
                answers = new string[] {"Padara vizuāli skaistāku programmu", "Tas var kalpot kā konteineris priekš organizēšanas",
                "Aprēķina Skriptu skaitu", "Izmantoto, lai radītu fiziskus 3D modeļu"},
                correctAnswers = new int[] {0, 1}
            }
        };
    }

    //Parāda pašreizējo jautājumu
    void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count) //Pārbauda, vai ir vēl jautājumi
        {
            Question currentQuestion = questions[currentQuestionIndex];
            questionText.text = currentQuestion.questionText;
            for (int i=0; i<answerToggles.Length; i++)
            {
                answerToggles[i].gameObject.SetActive(i<currentQuestion.answers.Length); //Aktivizē tikai nepieciešamos toggle
                if (i<currentQuestion.answers.Length)
                {
                    answerToggles[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i]; //Iestata atbilžu tekstu
                    answerToggles[i].isOn = false; //Atiestata iepriekšējās atbildes
                }
            }
        }
        else
        {
            ShowResults(); //Parāda rezultātus, ja nav vairāk jautājumu
        }
    }

    //Pārbauda atbildi un pāriet pie nākamā jautājuma
    void CheckAnswer()
    {
        Question currentQuestion = questions[currentQuestionIndex];
        List<int> selectedAnswers = new List<int>();

        for (int i=0; i<answerToggles.Length; i++)
        {
            if (answerToggles[i].isOn) //Ja toggle ir ieslēgts, pievieno izvēlēto atbildi
            {
                selectedAnswers.Add(i);
            }
        }

        bool isCorrect = true;
        foreach (int correctAnswer in currentQuestion.correctAnswers)
        {
            if (!selectedAnswers.Contains(correctAnswer)) //Ja nav visas pareizās atbildes, atbilde ir nepareiza
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            correctAnswerCount++; //Palielina pareizo atbilžu skaitu
        }
        else
        {
            incorrectQuestions.Add(currentQuestionIndex); //Pievieno jautājumu nepareizo jautājumu sarakstam
        }

        currentQuestionIndex++; //Pāriet uz nākamo jautājumu
        DisplayQuestion(); //Parāda nākamo jautājumu
    }

    //Parāda rezultātus
    void ShowResults()
    {
        resultText.text = "Atbildēji uz "+correctAnswerCount+" jautājumiem pareizi.\n"; //Parāda pareizo atbilžu skaitu

        if (incorrectQuestions.Count>0) //Ja ir nepareizas atbildes, parāda tās
        {
            resultText.text += "Nepareizi atbildētie jautājumi:\n";
            foreach (int index in incorrectQuestions)
            {
                resultText.text += (index+1)+". "+questions[index].questionText+"\n"; //Parāda nepareizo jautājumu tekstu
            }
        }

        ResultPanel.SetActive(true); //Parāda rezultātu paneli

        }

        void RestartQuiz() //Restartē spēli
    {
        correctAnswerCount = 0;
        currentQuestionIndex = 0;
        incorrectQuestions.Clear();
        ResultPanel.SetActive(false);
        DisplayQuestion();

    }
}


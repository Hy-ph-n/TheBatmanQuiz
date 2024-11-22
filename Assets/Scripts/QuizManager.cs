using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Using Unity Video Player
using UnityEngine.Video;
// Using Unity UI Engine
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    // Screen Transformation Variables
    // A link to to the camera's cameraMove
    public cameraMove cameraMove;

    // The video screen anchor so that screen can be moved to
    public Transform studyScreen;

    // The quiz screen anchor so that screen can be moved to
    public Transform quizScreen;

    // The fail screen anchor so that screen can be moved to
    public Transform failScreen;

    // The pass screen anchor so that screen can be moved to
    public Transform passScreen;

    // The confirm screen anchor so that screen can be moved to
    public Transform confirmScreen;

    // The thank you screen anchor so that screen can be moved to
    public Transform thankYouScreen;

    // The main menu screen anchor so that we can return to it
    public Transform mainMenuScreen;



    // Main Menu Button Methods
    // A method that sends the camera to the study screen
    public void StudyScreen()
    {
        // Passes the study screen anchor to the camera's move script
        cameraMove.setAnchor(studyScreen);
    }

    // A method that sends the camera to the quiz screen
    public void QuizScreen()
    {
        // Passes the quiz screen anchor to the camera's move script
        cameraMove.setAnchor(quizScreen);
    }

    // Quits out of the application
    public void QuitApp()
    {
        // Reloads the main menu
        cameraMove.setAnchor(mainMenuScreen);

        // Logs a message to the console
        Debug.Log("Quit Application");

        // Quits the application
        Application.Quit();
    }



    // Study Screen Variables
    // A link to the video player
    public VideoPlayer videoPlayer;

    // Study Screen Button Methods
    // Plays the video
    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    // Pauses the video
    public void PauseVideo()
    {
        videoPlayer.Pause();
    }

    // Rewinds the video
    public void RewindVideo()
    {
        // Pauses the video
        videoPlayer.Pause();

        // Rewinds the video to frame zero
        videoPlayer.frame = 0;
    }

    // Returns to the main menu
    public void ReturnFromVideo()
    {
        // Pauses the video
        videoPlayer.Pause();

        // Rewinds the video to frame zero
        videoPlayer.frame = 0;

        // Passes the main menu anchor to the camera's move script
        cameraMove.setAnchor(mainMenuScreen);
    }



    // Quiz Variables
    // A string that holds all the questions as the file is read from the disk
    string allQuestions;

    // A list of lists that will hold all the questions, including the
    // question itself, all four possible answers, and the correct answer
    public List<List<string>> questionsList = new List<List<string>>();

    // An integer holding the current question
    int currentQuestion = 0;

    // A link to the quiz text fields
    public Text titleText, questionText, answer1Text, answer2Text, answer3Text, answer4Text;

    // A string holding the correct answer
    public string correctAnswerText;

    // An integer holding the total number of questions
    public int totalNumberOfQuestions;

    // Creates a new random object using system.random
    public System.Random rnd = new System.Random();



    // Start is called before the first frame update
    void Start()
    {
        // Loads the questions from the Resources folder
        TextAsset readInQuestionsFile = Resources.Load("Questions for The Batman (2004)") as TextAsset;

        // Places the questions in a string variable
        string allQuestions = readInQuestionsFile.text;

        // Splits the text into individual question strings using ';' as
        // a delimiter. Each string within the array will then contain
        // the question, all answers, and the correct answer
        string[] questions = allQuestions.Split(';');

        // Sets the total question count
        totalNumberOfQuestions = questions.Length;

        // A for loop that cycles through each string line
        foreach (string question in questions)
        {
            // Splits the text into individual question components using ','
            // as a delimiter. Each string within the array will then contain
            // either the question, one of four answers, or the correct answer
            string[] questionParts = question.Split(',');

            // Creates a temporary string list to hold the individual question parts
            List<string> tempList = new List<string>();

            // Adds the question
            tempList.Add(questionParts[0].Replace("\n", ""));

            // Adds answer1
            tempList.Add(questionParts[1].Replace("\n", ""));

            // Adds answer2
            tempList.Add(questionParts[2].Replace("\n", ""));

            // Adds answer3
            tempList.Add(questionParts[3].Replace("\n", ""));

            // Adds answer4
            tempList.Add(questionParts[4].Replace("\n", ""));

            // Adds correct answer
            tempList.Add(questionParts[5].Replace("\n", ""));

            // Adds the question to the questions list after
            // it has been broken down into individual parts
            questionsList.Add(tempList);
        }

        // Shuffles the questions in the list
        questionsList = ShuffleQuestions(questionsList);

        // Loads in the first question
        // Shows the user what question they are on out of the total questions
        titleText.text = "Question " + (currentQuestion + 1) + " of " + totalNumberOfQuestions;

        // Shows the question
        questionText.text = questionsList[currentQuestion][0];

        // Creates a temporary list to hold all possible question answers
        List<string> tempAnswersList = new List<string>();

        // Grabs all possible answers
        tempAnswersList.Add(questionsList[currentQuestion][1]);
        tempAnswersList.Add(questionsList[currentQuestion][2]);
        tempAnswersList.Add(questionsList[currentQuestion][3]);
        tempAnswersList.Add(questionsList[currentQuestion][4]);

        // Shuffles the possible answers
        tempAnswersList = ShuffleAnswers(tempAnswersList);

        // Displays text in answer1
        answer1Text.text = tempAnswersList[0];

        // Displays text in answer2
        answer2Text.text = tempAnswersList[1];

        // Displays text in answer3
        answer3Text.text = tempAnswersList[2];

        // Displays text in answer4
        answer4Text.text = tempAnswersList[3];

        // Stores the correct answer to this question
        correctAnswerText = questionsList[currentQuestion][5];
    }

    // A method that shuffles the questions, the individual string lists
    // that reside in a list of lists and returns a newly shuffled list
    // It accepts the list of lists as a parameter, shuffles, and returns
    public List<List<string>> ShuffleQuestions(List<List<string>> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;

            int k = rnd.Next(n + 1);

            List<string> value = list[k];

            list[k] = list[n];

            list[n] = value;
        }

        return list;
    }

    // A method to shuffle answers in a list, returning a shuffled list
    // It accepts the list as a parameter, shuffles, and returns
    public List<string> ShuffleAnswers(List<string> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;

            int k = rnd.Next(n + 1);

            string value = list[k];

            list[k] = list[n];

            list[n] = value;
        }

        return list;
    }



    // An integer holding the total number of questions the user got right
    public int totalAnswersRight = 0;

    // A link to the check answer button
    public GameObject checkAnswerButton;

    // The user's answer is filled in whenever a toggle box is checked
    public string userAnswer;

    // A link to the Correct and Incorrect GameObject Panels
    public GameObject correctAnswerPanel, incorrectAnswerPanel;



    // Quiz Button Methods
    // A method called every time a toggle is clicked
    public void ToggleChecked()
    {
        // Number of toggles checked, automatically set to zero
        int togglesChecked = 0;

        // Grabs all objects tagged "Toggle" and places them in an array
        GameObject[] toggles = GameObject.FindGameObjectsWithTag("Toggle");

        // A for loop that cycles through the toggle objects
        foreach (GameObject toggle in toggles)
        {
            // An if statement for if the toggle is checked
            if (toggle.GetComponent<Toggle>().isOn)
            {
                // Adds one to the counter
                togglesChecked++;

                // Since the toggle is checked, shows the check answer button
                checkAnswerButton.SetActive(true);

                // Gets text from checked toggle and places it in the userAnswer variable
                userAnswer = toggle.transform.Find("Label").GetComponent<Text>().text;
            }
        }

        // Checks if no toggles are checked
        if (togglesChecked == 0)
        {
            // No answer is given, so the check answer button is turned off
            checkAnswerButton.SetActive(false);

            // Empties the userAnswer variable
            userAnswer = "";
        }
    }



    // A method called from the Check Answer button to
    // compare the user's answer to the correct answer
    public void CheckAnswer()
    {
        checkAnswerButton.SetActive(false); ;

        if (string.Equals(userAnswer, correctAnswerText))
        {
            // Adds one to the total number of correct answers
            totalAnswersRight++;

            // Shows the correct answer feedback panel
            correctAnswerPanel.SetActive(true);
        }
        else
        {
            // Shows the incorrect answer feedback panel
            incorrectAnswerPanel.SetActive(true);
        }
    }



    // Closes the correct/incorrect pnael based on which is active
    public void CloseAnswerPanel()
    {
        GameObject[] toggles = GameObject.FindGameObjectsWithTag("Toggle");

        // Unchecks all toggles
        foreach (GameObject toggle in toggles)
        {
            toggle.GetComponent<Toggle>().isOn = false;
        }

        // Closes the correct answer panel
        correctAnswerPanel.SetActive(false);

        // Closes the incorrect answer panel
        incorrectAnswerPanel.SetActive(false);

        // Loads the next question
        NextQuestion();
    }



    // The grade for passing
    public int passingGrade;

    // A link to the fail screen text so
    // the the final grade can be given
    public Text failText;

    // A link to the past screen text so
    // the the final grade can be given
    public Text passText;



    // A method to load the next question
    public void NextQuestion()
    {
        // Increases the current question variable
        currentQuestion++;

        // Checks to see if we are on the last question
        if (currentQuestion > (questionsList.Count - 1))
        {
            // Calculates the user's score
            float userScore = ((float)totalAnswersRight / (float)questionsList.Count) * 100;

            // Goes to the end quiz screen
            if (userScore > passingGrade)
            {
                // Write's the user's score to the
                // passing text on the pass screen
                passText.text = totalAnswersRight + " out of " + totalNumberOfQuestions
                    + "\n" + " Score of: " + userScore + "\nRequired: " + passingGrade;

                // Goes to the pass screen
                cameraMove.setAnchor(passScreen);

                // Does not continue with the rest of the method
                return;
            }
            else
            {
                // Write's the user's score to the
                // failing text on the fail screen
                failText.text = totalAnswersRight + " out of " + totalNumberOfQuestions
                    + "\n" + " Score of: " + userScore + "\nRequires: " + passingGrade;

                // Goes to the fail screen
                cameraMove.setAnchor(failScreen);

                // Does not continue with the rest of the method
                return;
            }
        }

        // Loads in the next question
        // Shows the user what question they are on out of the total questions
        titleText.text = "Question " + (currentQuestion + 1) + " of " + totalNumberOfQuestions;

        // Shows the question
        questionText.text = questionsList[currentQuestion][0];

        // Creates a temporary list to hold all possible question answers
        List<string> tempAnswersList = new List<string>();

        // Grabs all possible answers
        tempAnswersList.Add(questionsList[currentQuestion][1]);
        tempAnswersList.Add(questionsList[currentQuestion][2]);
        tempAnswersList.Add(questionsList[currentQuestion][3]);
        tempAnswersList.Add(questionsList[currentQuestion][4]);

        // Shuffles the possible answers
        tempAnswersList = ShuffleAnswers(tempAnswersList);

        // Displays text in answer1
        answer1Text.text = tempAnswersList[0];

        // Displays text in answer2
        answer2Text.text = tempAnswersList[1];

        // Displays text in answer3
        answer3Text.text = tempAnswersList[2];

        // Displays text in answer4
        answer4Text.text = tempAnswersList[3];

        // Stores the correct answer to this question
        correctAnswerText = questionsList[currentQuestion][5];
    }



    // The input field on the pass screen so the user can add their signature
    public InputField userSignature;

    // The text on the confirm screen to update the certificate's signature
    public Text certificateSignature;

    // The text field for date on the confirm screen
    public Text certificateDate;

    // A link to the certificate
    public RectTransform certificate;



    // A method that goes from the pass screen to the confirm signature
    // screen while setting the user's signature and completion date
    public void EnterSignature()
    {
        // Gets the user's signature from the signature input
        // field and places it onto the certifcate's text
        certificateSignature.text = userSignature.text;

        // Sets the date's text to the current date
        // and places it onto the certificate
        certificateDate.text = System.DateTime.Now.ToShortDateString();

        // Passes the confirm signature screen
        // anchor to the camera's move script
        cameraMove.setAnchor(confirmScreen);
    }



    // A method to go from the confirm screen to the pass screen
    public void RedoSignature()
    {
        // Empties signature text
        userSignature.text = "";

        // Empties date text
        certificateDate.text = "";

        // Passes the pass screen anchor to the camera's move script
        cameraMove.setAnchor(passScreen);
    }

    // A method to go from the confirm screen to the
    // thank you screen and save the certificate
    public void ConfirmSignature()
    {
        // Calls a method to take the screen capture
        StartCoroutine(TakeScreenshotAndSave());
    }

    // Takes a screenshot of the certificate
    private IEnumerator TakeScreenshotAndSave()
    {
        // Waits for the frame to process
        yield return new WaitForEndOfFrame();

        // Sets the width x height for screen and captures a 1920 x 1080 screen
        float widthFactor = Screen.width / 1920f;
        float heightFactor = Screen.height / 1080f;
        int width = Mathf.FloorToInt(certificate.rect.width * widthFactor);
        int height = Mathf.FloorToInt(certificate.rect.height * heightFactor);

        // Gets the X and Y of the certificate
        Vector2 certPos = certificate.anchoredPosition;

        // Creates a new texture
        var ss = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Reads pixels into the new texture from the screen
        ss.ReadPixels(new Rect(400, 100, 1100, 800), 0, 0);

        // Applies pixels to the texture
        ss.Apply();

#if UNITY_EDITOR

        // If in the editor, writes the data to the application's folder
        System.IO.File.WriteAllBytes(Application.dataPath + ".png", ss.EncodeToPNG());

        Debug.Log("Unity Editor");

#endif

#if UNITY_ANDROID

        // Saves the screenshot to gallery
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "GalleryText", "Image.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));

        Debug.Log("Permission result: " + permission);

#endif

        // Destroys the image to avoid any memory leaks
        Destroy(ss);

        // Passes the thank you screen anchor to the camera's move script
        cameraMove.setAnchor(thankYouScreen);
    }



    // A method for clicking the button to return to the Mein Menu
    public void MainMenu()
    {
        // Calls restart to reset variables
        Restart();

        // Passes the main menu anchor to the camera's move script
        cameraMove.setAnchor(mainMenuScreen);
    }

    // A method that resets variables and calls the start method to reload the quiz
    public void Restart()
    {
        // Empties all questions text
        allQuestions = "";

        // Empties questionsList
        questionsList = new List<List<string>>();

        // Sets the current question to zero
        currentQuestion = 0;

        // Sets the total number of right answers to zero
        totalAnswersRight = 0;

        // Calls start to reload questions, randomize them, and prepare the quiz
        Start();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro; // UI ke liye zaroori

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI ScoreText;
    public GameObject PausePanel;

    [Header("Game Over UI")]
    public GameObject GameOverPanel;      // Game Over wala panel
    public TextMeshProUGUI ResultTitle;   // "You Won" ya "Try Again" likhne ke liye
    public GameObject NextLevelButton;    // Next Level button (Haarne par chupane ke liye)

    [Header("Game References")]
    public CameraFollow cameraFollow;
    public SlingShot slingshot;

    // --- SCORING SYSTEM ---
    public static int Score = 0;

    // Game State variables
    int currentBirdIndex;
    [HideInInspector]
    public static GameState CurrentGameState = GameState.Playing;

    private List<GameObject> Bricks;
    private List<GameObject> Birds;
    private List<GameObject> Pigs;

    void Start()
    {
        // 1. Reset Time & Score
        Time.timeScale = 1;
        Score = 0;

        // 2. Hide all Panels initially
        if (PausePanel != null) PausePanel.SetActive(false);
        if (GameOverPanel != null) GameOverPanel.SetActive(false);

        CurrentGameState = GameState.Playing;
        slingshot.enabled = false;

        // 3. Find Game Objects
        Bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
        Birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird"));
        Pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pig"));

        // 4. Subscribe Events
        slingshot.BirdThrown -= Slingshot_BirdThrown;
        slingshot.BirdThrown += Slingshot_BirdThrown;

        // 5. Start Game Animation
        AnimateBirdToSlingshot();
    }

    void Update()
    {
        // Update Score UI
        if (ScoreText != null) ScoreText.text = "Score: " + Score;

        switch (CurrentGameState)
        {
            case GameState.Playing:
                // Check if bird has stopped or timeout
                if (slingshot.slingshotState == SlingshotState.BirdFlying &&
                    (BricksBirdsPigsStoppedMoving() || Time.time - slingshot.TimeSinceThrown > 5f))
                {
                    slingshot.enabled = false;
                    AnimateCameraToStartPosition();
                    CurrentGameState = GameState.BirdMovingToSlingshot;
                }
                break;
            default:
                break;
        }
    }

    // --- BUTTON FUNCTIONS (Inspector mein connect karein) ---

    public void PauseGame()
    {
        if (PausePanel != null) PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (PausePanel != null) PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Game Finished! Wapis Menu par ja rahe hain.");
            SceneManager.LoadScene("MainMenu");
        }
    }

    // --- LOGIC HELPER FUNCTIONS ---

    private bool AllPigsDestroyed()
    {
        return Pigs.All(x => x == null);
    }

    private void AnimateCameraToStartPosition()
    {
        float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.StartingPosition) / 10f;
        if (duration == 0.0f) duration = 0.1f;

        Camera.main.transform.DOMove(cameraFollow.StartingPosition, duration).
            OnComplete(() =>
            {
                cameraFollow.IsFollowing = false;

                // --- JEET AUR HAAR KA FAISLA ---
                if (AllPigsDestroyed())
                {
                    CurrentGameState = GameState.Won;
                    ShowGameOverUI(true); // Player Won
                }
                else if (currentBirdIndex == Birds.Count - 1)
                {
                    CurrentGameState = GameState.Lost;
                    ShowGameOverUI(false); // Player Lost
                }
                else
                {
                    slingshot.slingshotState = SlingshotState.Idle;
                    currentBirdIndex++;
                    AnimateBirdToSlingshot();
                }
            });
    }

    // --- UI & SAVE SYSTEM LOGIC ---
    void ShowGameOverUI(bool isWin)
    {
        if (GameOverPanel != null)
        {
            GameOverPanel.SetActive(true);

            if (isWin)
            {
                if (ResultTitle != null) ResultTitle.text = "LEVEL CLEARED!";
                if (NextLevelButton != null) NextLevelButton.SetActive(true);

                // --- SAVE SYSTEM: UNLOCK NEXT LEVEL ---
                int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
                int nextLevelToUnlock = currentLevelIndex + 1;

                // Agar agla level pehle se unlocked nahi hai, tabhi save karo
                if (nextLevelToUnlock > PlayerPrefs.GetInt("LevelsUnlocked", 1))
                {
                    PlayerPrefs.SetInt("LevelsUnlocked", nextLevelToUnlock);
                    PlayerPrefs.Save();
                    Debug.Log("Level " + nextLevelToUnlock + " Unlocked!");
                }
                // -------------------------------------
            }
            else
            {
                if (ResultTitle != null) ResultTitle.text = "TRY AGAIN";
                if (NextLevelButton != null) NextLevelButton.SetActive(false); // Haar gaye to Next button mat dikhao
            }
        }
    }

    void AnimateBirdToSlingshot()
    {
        CurrentGameState = GameState.BirdMovingToSlingshot;
        Birds[currentBirdIndex].transform.DOMove
            (slingshot.BirdWaitPosition.transform.position,
            Vector2.Distance(Birds[currentBirdIndex].transform.position / 10,
            slingshot.BirdWaitPosition.transform.position) / 10).
                OnComplete(() =>
                {
                    CurrentGameState = GameState.Playing;
                    slingshot.enabled = true;
                    slingshot.BirdToThrow = Birds[currentBirdIndex];
                });
    }

    private void Slingshot_BirdThrown(object sender, System.EventArgs e)
    {
        cameraFollow.BirdToFollow = Birds[currentBirdIndex].transform;
        cameraFollow.IsFollowing = true;
    }

    bool BricksBirdsPigsStoppedMoving()
    {
        foreach (var item in Bricks.Union(Birds).Union(Pigs))
        {
            if (item != null && item.GetComponent<Rigidbody2D>().linearVelocity.sqrMagnitude > Constants.MinVelocity)
            {
                return false;
            }
        }
        return true;
    }

    public static void AutoResize(int screenWidth, int screenHeight)
    {
        Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
    }
}
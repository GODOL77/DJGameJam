using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI elements

public class TutoManager : MonoBehaviour
{
    public GameObject tutorialPanel; // The UI panel that holds tutorial elements
    public Text tutorialText; // Text component to display instructions
    public Button continueButton; // Button to proceed to next step (if manual)

    public GameObject prefabToSpawn; // The prefab to spawn after step 2
    public float spawnDistance = 5f; // Distance from player to spawn the prefab
    public GameObject spwanner;

    [System.Serializable]
    public class TutorialStep
    {
        [TextArea(3, 10)]
        public string instruction; // The text instruction for this step
        public bool waitForInput; // True if we wait for specific input/action
        public KeyCode requiredKey; // Key to press for this step (e.g., KeyCode.W)
        // Add more fields for other types of required actions (e.g., "collect XP", "kill enemy")
    }

    public List<TutorialStep> tutorialSteps;
    private int currentStepIndex = 0;

    void Start()
    {
        // Ensure tutorial panel is initially hidden
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }

       
        // Start the tutorial
        StartTutorial();
        StartCoroutine(StartSet());
    }
    IEnumerator StartSet()
    {
        yield return new WaitForSeconds(0.1f); // 0.1초 대기
         Time.timeScale = 1f;
    }
    void Update()
    {
        if (tutorialPanel.activeSelf && currentStepIndex < tutorialSteps.Count)
        {
            TutorialStep currentStep = tutorialSteps[currentStepIndex];

            if (currentStep.waitForInput)
            {
                // Example: Check for movement input
                if (currentStep.requiredKey != KeyCode.None && Input.GetKeyDown(currentStep.requiredKey))
                {
                    // For movement, we might want to check if any movement key is pressed
                    // For now, just check the specific key
                    ProceedToNextStep();
                }
                // Add more checks for other types of input/actions here
            }
        }
    }

    public void StartTutorial()
    {
        Time.timeScale = 1f; // Ensure time is flowing
        currentStepIndex = 0;
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
        DisplayCurrentStep();

        // Temporarily disable game elements (e.g., enemy spawner, player attack if not part of tutorial step)
        // Example: FindObjectOfType<EnemySpawner>()?.gameObject.SetActive(false);
        // You'll need to add references to your game managers here.
    }

    void DisplayCurrentStep()
    {
        if (currentStepIndex < tutorialSteps.Count)
        {
            TutorialStep currentStep = tutorialSteps[currentStepIndex];
            if (tutorialText != null)
            {
                tutorialText.text = currentStep.instruction;
            }

            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(!currentStep.waitForInput); // Show continue button if not waiting for input
                continueButton.onClick.RemoveAllListeners();
                if (!currentStep.waitForInput)
                {
                    continueButton.onClick.AddListener(ProceedToNextStep);
                }
            }
        }
        else
        {
            EndTutorial();
        }
    }

    public void ProceedToNextStep()
    {
        
        // Check if we are moving from step 2 (index 1) to step 3 (index 2)
        if (currentStepIndex == 2 && prefabToSpawn != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                Vector3 spawnPosition = player.transform.position + (Vector3)randomDirection * spawnDistance;
                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                Debug.Log($"Spawned {prefabToSpawn.name} at {spawnPosition}");
            }
            else
            {
                Debug.LogWarning("Player object not found with 'Player' tag. Cannot spawn prefab.");
            }
        }
        if (currentStepIndex == 5)
        {
            if (spwanner != null)
            {
                spwanner.SetActive(true);
            }
        }

        currentStepIndex++;
        DisplayCurrentStep();
    }

    void EndTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        Debug.Log("Tutorial Finished!");
        // Re-enable game elements
        // Example: FindObjectOfType<EnemySpawner>()?.gameObject.SetActive(true);
    }
}
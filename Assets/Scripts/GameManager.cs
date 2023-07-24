using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{


    public GameObject colliderAsset;
    private GameObject colliderRef;

    public int ColliderID;
    
    public GameObject numberAsset;
    public GameObject parent;
    public Vector3 startPos;

    private int Height = 40;
    private int Length = 54;

    //by the player
    public Queue<int> selectedNumbersQueue = new Queue<int>();

    //by the game
    private int[] pickedNumbersArray;

    //number of matches a player gets in a game
    private int matchedAmount = 0;
    private int hatchedEggs = 0;

    private Dictionary<int, GameObject> prefabDictionary = new Dictionary<int, GameObject>();

    private Coroutine functionCallCoroutine;

    public float interval = 0.2f;

    private int currentIndex = 0;

    private GameState gameState;

    public Button increaseButton;
    public Button decreaseButton;
    public Button playButton;
    public TMP_Dropdown m_Dropdown;
    public Button infoButton;
    private bool buttonSwitch = false;

    private List<int> selectedEggList = new List<int>();


    void Awake()
    {
        
        SpawnGrid(Height, Length);

    }
    // Start is called before the first frame update
    void Start()
    {
        
        gameState = FindObjectOfType<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnGrid(int height, int length)
    {

        
        Vector3 startPoint = startPos;
        startPoint.x += length / 2;
        startPoint.y -= height / 2;

        int textNumber = 1;
        
        

        for (int i = 1; i <= 8; i++ )
        {
            
            for (int j = 1; j <= 10; j++)
            {
                GameObject temp = Instantiate(numberAsset, parent.transform);
                temp.transform.localScale = Vector3.one;
                temp.transform.localPosition = startPoint;
                
                TextMeshProUGUI textMeshPro = temp.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPro.text = textNumber.ToString();

                int instanceID = temp.GetInstanceID();
                prefabDictionary.Add(instanceID, temp);
                
                startPoint.x += length;
                textNumber++;

            }
            startPoint.y -= height;
            startPoint.x = startPos.x;
            startPoint.x += length / 2;

        }

        
        
    }

    public void NumberClicked(int instanceID, int currentState)
    {
        
        

        int removeId;

        
        
          if(currentState == 1)
          {
              if(selectedNumbersQueue.Count < 10) 
              { 
                 selectedNumbersQueue.Enqueue(instanceID);
                 
              }
              else if (selectedNumbersQueue.Count >= 10) 
              { 
                 removeId = selectedNumbersQueue.Dequeue();
                     
                 QueueOverflow(removeId);
                 selectedNumbersQueue.Enqueue(instanceID);
                 
              }
          }
          else if (currentState == 2) 
          {
            Queue<int> tempQueue = new Queue<int>();

            while(selectedNumbersQueue.Count > 0) 
            {
                int currentElement = selectedNumbersQueue.Dequeue();

                // Check if the current element is the one to remove
                if (currentElement == instanceID)
                {
                    // Skip adding the element to the temporary queue
                    continue;
                }

                // Add the current element to the temporary queue
                tempQueue.Enqueue(currentElement);
            }

            selectedNumbersQueue = tempQueue;
          }
        gameState.PayoutManager.SetText(selectedNumbersQueue.Count);
    }

    public void QueueOverflow(int RemovedId)
    {
        if (prefabDictionary.ContainsKey(RemovedId))
        {
            
            GameObject prefabInstance = prefabDictionary[RemovedId];

            
            numberButtonScript scriptComponent = prefabInstance.GetComponent<numberButtonScript>();

            
            if (scriptComponent != null)
            {
                
                scriptComponent.ColourChange();

                Debug.Log("color change called");
            }
            else
            {
                Debug.LogError("Script component not found on the GameObject: " + RemovedId);
            }
        }
        else
        {
            Debug.LogError("GameObject not found: " + RemovedId);
        }
        
    }

    public void GameStart()
    {

        //Cursor.lockState = CursorLockMode.Locked;
        gameState.MouseSwitch();
        ButtonSwitch();

        SpawnEggs();

        gameState.PayoutManager.GameStart();

        
        
        pickedNumbersArray = CreateArrayFromDictionary(prefabDictionary, 20);

        matchedAmount = CompareArrayAndQueue(pickedNumbersArray, selectedNumbersQueue);
        hatchedEggs = CompareArrayAndList(pickedNumbersArray, selectedEggList);

        functionCallCoroutine = StartCoroutine(CallFunctionPeriodically());

        //move this to after coroutine stops

        gameState.PayoutManager.DecidePayout(selectedNumbersQueue.Count, matchedAmount -1, hatchedEggs -1);
        Debug.Log(matchedAmount.ToString());
    }

    private int[] CreateArrayFromDictionary(Dictionary<int, GameObject> dict, int size)
    {
        List<int> keys = new List<int>(dict.Keys);

        if (size > keys.Count)
        {
            Debug.LogWarning("Array size exceeds the number of unique keys in the dictionary.");
            size = keys.Count;
        }

        int[] newArray = new int[size];
        HashSet<int> usedKeys = new HashSet<int>();

        for (int i = 0; i < size; i++)
        {
            int index = Random.Range(0, keys.Count);

            while (usedKeys.Contains(keys[index]))
            {
                index = Random.Range(0, keys.Count);
            }

            newArray[i] = keys[index];
            usedKeys.Add(keys[index]);
        }

        return newArray;
    }

    private int CompareArrayAndQueue(int[] arr, Queue<int> q)
    {
        int commonElements = 0;

        foreach (int element in q)
        {
            if (System.Array.IndexOf(arr, element) >= 0)
            {
                commonElements++;
            }
        }

        return commonElements;
    }

    private int CompareArrayAndList(int[] arr, List<int> q)
    {
        int commonElements = 0;

        foreach (int element in q)
        {
            if (System.Array.IndexOf(arr, element) >= 0)
            {
                commonElements++;
            }
        }

        return commonElements;
    }

    private IEnumerator CallFunctionPeriodically()
    {
        // Check if there are instance IDs and the dictionary is populated
        if (pickedNumbersArray.Length == 0 || prefabDictionary.Count == 0)
        {
            Debug.LogWarning("Instance ID array or dictionary is empty.");
            yield break;
        }

        while (currentIndex < pickedNumbersArray.Length)
        {
            int currentInstanceID = pickedNumbersArray[currentIndex];
            if (prefabDictionary.TryGetValue(currentInstanceID, out GameObject gameObject))
            {
                // Call a function on the game object using the instance ID
                gameObject.GetComponent<numberButtonScript>().MatchOrNot();
            }

            currentIndex++;

            if(currentIndex >= pickedNumbersArray.Length)
            {
                StopFunctionCallCoroutine();
            }
            // Wait for the specified interval before processing the next element
            yield return new WaitForSeconds(interval);
        }
    }

    // Stop the coroutine when needed
    private void StopFunctionCallCoroutine()
    {
        if (functionCallCoroutine != null)
        {
            StopCoroutine(functionCallCoroutine);
            Debug.Log("Courutine sstopped");

            SpawnCollider();
            
        }

    }

    public void ResetGame()
    {
        int index = 0;
        int EggDex = 0;
        while (index < pickedNumbersArray.Length)
        {
            int currentInstanceID = pickedNumbersArray[index];
            if (prefabDictionary.TryGetValue(currentInstanceID, out GameObject gameObject))
            {
                // Call a function on the game object using the instance ID
                gameObject.GetComponent<numberButtonScript>().Reset();
            }

            index++;
        }
        while (EggDex < selectedEggList.Count)
        {
            int currentInstanceID = selectedEggList[EggDex];
            if (prefabDictionary.TryGetValue(currentInstanceID, out GameObject gameObject))
            {
                // Call a function on the game object using the instance ID
                gameObject.GetComponent<numberButtonScript>().Reset();
            }

            EggDex++;
        }



        currentIndex = 0;
        pickedNumbersArray= null;
        //Cursor.lockState = CursorLockMode.None;
        gameState.MouseSwitch();
        ButtonSwitch();
        
        Destroy(colliderRef.gameObject);
        
    }

    private void ButtonSwitch()
    {
        if (!buttonSwitch)
        {
            increaseButton.enabled = false;
            decreaseButton.enabled = false;
            playButton.enabled = false;
            m_Dropdown.enabled = false;
            infoButton.enabled = false;
            buttonSwitch= true;
        }
        else
        {
            increaseButton.enabled = true;
            decreaseButton.enabled = true;
            playButton.enabled = true;
            m_Dropdown.enabled = true;
            infoButton.enabled = true;
            buttonSwitch = false;
        }
        

    }

    private void SpawnCollider()
    {
        colliderRef = Instantiate(colliderAsset);
        

        

    }

    private void SpawnEggs()
    {

        selectedEggList.Clear();

        // Get all the instance IDs from the prefab dictionary
        var allInstanceIDs = prefabDictionary.Keys.ToList();

        // Exclude the instance IDs present in the selectedNumbersQueue
        var availableInstanceIDs = allInstanceIDs.Except(selectedNumbersQueue).ToList();

        // Check if we have enough available instance IDs to spawn 3 eggs
        if (availableInstanceIDs.Count >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                int randomIndex = Random.Range(0, availableInstanceIDs.Count);
                int selectedEggInstanceID = availableInstanceIDs[randomIndex];
                selectedEggList.Add(selectedEggInstanceID);
                availableInstanceIDs.RemoveAt(randomIndex);
            }

            // Change the color of the numberPrefabs corresponding to the selected egg instance IDs
            foreach (int selectedEggInstanceID in selectedEggList)
            {
                if (prefabDictionary.TryGetValue(selectedEggInstanceID, out GameObject eggGameObject))
                {
                    numberButtonScript scriptComponent = eggGameObject.GetComponent<numberButtonScript>();
                    if (scriptComponent != null)
                    {
                        scriptComponent.SetEgg();
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Not enough available instance IDs to spawn 3 eggs!");
        }
    }
    
}

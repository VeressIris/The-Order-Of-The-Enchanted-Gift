using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Transactions;

public class GameManager : MonoBehaviour
{
    [Header("Customers:")]
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform initCustomerPos;
    private int customersServed = 0;
    private Queue<Customer> customers = new Queue<Customer>();
    private Vector3[] customersPos;
    private bool handlingCustomer = false;
    [Header("Gifts:")]
    [SerializeField] private GameObject[] giftOptions;
    public Transform objectPosition;
    [SerializeField] private Image wrappingPaperImg;
    [SerializeField] private Image stickerImg;
    public GameObject objectToWrap;
    [SerializeField] private Sprite closedBoxSprite;
    [Header("Game:")]
    [SerializeField] private TMP_Text timerAndWaveText;
    private bool gameOver = false;
    private int wave = 0;
    private float timeRemaining;
    public bool addedCorrectBox = false;
    public GameObject addedBox;
    public bool wrappedBox = false;
    public bool closedBox = false;

    void Start()
    {
        wrappingPaperImg.sprite = null;
        stickerImg.sprite = null;

        timeRemaining = 35f;
        StartNewWave(3);
    }

    void Update()
    {
        if (!gameOver)
        {
            //timer
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimer(timeRemaining);
            }
            else
            {
                Debug.Log($"Wave {wave} over!");
                timeRemaining = Random.Range(30f, 120f); //reset timer
                StartNewWave(Random.Range(2, 13));
            }

            //handle customers
            if (!handlingCustomer)
            {
                //setup customer handling
                handlingCustomer = true;
                objectToWrap = Instantiate(giftOptions[Random.Range(0, giftOptions.Length)], objectPosition.position, Quaternion.identity);
                Customer currentCustomer = customers.Peek();
                DisplayRequirements(currentCustomer);
            }
            else
            {
                //handle customer
                if (addedCorrectBox)
                {
                    //close box
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (BoxClicked())
                        {
                            addedBox.GetComponent<SpriteRenderer>().sprite = closedBoxSprite;
                            closedBox = true;
                        }
                    }
                }
            }
        }
    }

    //handles box wrapping evolution
    bool BoxClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.layer == LayerMask.NameToLayer("UnwrappedBox"))
            {
                objectHit.layer = LayerMask.NameToLayer("ClosedBox");
                return true;
            }
        }

        return false;
    }

    void UpdateTimer(float time)
    {
        time++;
        float minutes = Mathf.FloorToInt(time / 60f);
        float seconds = Mathf.FloorToInt(time % 60f);
        timerAndWaveText.text = string.Format($"Wave {wave}\n {minutes:00}:{seconds:00}");
    }

    void StartNewWave(int numCustomers)
    {
        wave++;
        Debug.Log($"Starting wave {wave}!");
        customersPos = new Vector3[numCustomers];

        for (int i = 0; i < numCustomers; i++)
        {
            Customer customer;
            if (customers.Count == 0)
            {
                customer = Instantiate(customerPrefab, initCustomerPos.position, Quaternion.identity).GetComponent<Customer>();
                customersPos[0] = initCustomerPos.position;
            }
            else
            {
                //instantiate customers behind each other
                customersPos[i] = customersPos[i - 1] - new Vector3(1.825f, 0f, 0f);
                customer = Instantiate(customerPrefab, customersPos[i], Quaternion.identity).GetComponent<Customer>(); 
            }
            customers.Enqueue(customer);
        }
    }

    void DisplayRequirements(Customer currentCustomer)
    {
        wrappingPaperImg.sprite = currentCustomer.wrappingPaperImg;
        stickerImg.sprite = currentCustomer.stickerImg;
    }
}

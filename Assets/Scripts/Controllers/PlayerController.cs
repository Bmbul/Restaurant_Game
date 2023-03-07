using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [SerializeField] Camera mainCamera;
    Vector3 destination;
    Animator animator;
    NavMeshAgent agent;
    float terminatingDistance = 0.01f;
    internal List<ID> foodIDs;
    internal float currentMoney;
    internal bool canMove;

    private void Awake()
    {
        canMove = true;
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        foodIDs = new List<ID>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && canMove)
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if(hitInfo.transform.gameObject.tag == "Ground")
                    SetDestination(hitInfo.point);
                if (hitInfo.transform.gameObject.tag == "Food")
                    TakeFood(hitInfo.transform.gameObject);
            }

        }
    }

    void TakeFood(GameObject food)
    {
        if (Vector3.Distance(gameObject.transform.position, food.transform.position) < 1.5f)
        {
            food.SetActive(false);
            food.GetComponent<Collider>().enabled = false;
            foodIDs.Add(food.GetComponent<ID>());
            Order currentOrder = OrderController.Instance.GetOrder(food.GetComponent<ID>().id);
            UI_Controller.Instance.CreateFoodImage(currentOrder.orderSprite, currentOrder.orderID);
        }
    }
    public bool GiveFood(int _orderID, CustomerController customer)
    {
        for (int i = 0; i < foodIDs.Count; i++)
        {
            if (foodIDs[i].id == _orderID)
            {
                customer.TakeCookedOrder(foodIDs[i]);
                foodIDs.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    
    public void SetDestination(Vector3 _destination)
    {
        destination = _destination;
        agent.SetDestination(destination);
        animator.SetBool("Walk", true);
        StartCoroutine(CheckRemainingDistance());
    }

    IEnumerator CheckRemainingDistance()
    {
        yield return new WaitForEndOfFrame();
        if (Vector3.Distance(destination, transform.position) <= agent.stoppingDistance)
        {
            animator.SetBool("Walk", false);
        }
        else
        {
            StartCoroutine(CheckRemainingDistance());
        }
    }

    public void GetMoney(float _increment)
    {
        StartCoroutine(UI_Controller.Instance.EarnMoney(currentMoney, _increment));
        currentMoney = currentMoney + _increment;
    }
}

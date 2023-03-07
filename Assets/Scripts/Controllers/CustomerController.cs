using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class CustomerController : MonoBehaviour
{
    [SerializeField] Image orderImage;
    [SerializeField] Image smallOrderImage;

    [SerializeField] GameObject bubbles_Popup;
    [SerializeField] RectTransform[] bubblesTranforms;
    [Range(1, 10)] [SerializeField] float bubble_movement_speed;

    Animator animator;
    NavMeshAgent agent;
    float terminatingDistance = 0.6f;
    Transform destination;
    Vector3 beforeSittingPos;
    Vector3 StartPosition;
    Transform tableFoodSpawnPosition;
    int tableNumber;
    Order currentOrder;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartPosition = agent.transform.position;
    }


    public void SetDestination(Transform _destination, int _tableNumber, Transform _tableFoodSpawnPoint)
    {
        destination = _destination;
        tableNumber = _tableNumber;
        agent.SetDestination(destination.position);
        tableFoodSpawnPosition = _tableFoodSpawnPoint;
        animator.SetBool("Walk", true);
        StartCoroutine(CheckRemainingDistance());
    }

    IEnumerator CheckRemainingDistance()
    {
        yield return new WaitForEndOfFrame();
        if (Vector3.Distance(agent.transform.position, destination.position) >= terminatingDistance)
        {
            StartCoroutine(CheckRemainingDistance());
        }
        else
        {
            agent.enabled = false;
            beforeSittingPos = transform.position;
            transform.position = destination.position;
            transform.rotation = destination.rotation;

            if(transform.rotation.eulerAngles.y == 90)
            {
                Vector3 rotation360 = new Vector3(0,360,0);
                orderImage.transform.eulerAngles = rotation360;
                smallOrderImage.transform.eulerAngles = rotation360;
            }
            animator.SetBool("Walk", false);
            animator.SetBool("Sitting", true);

            yield return new WaitForSeconds(5);
            Order();
            //Leave();
        }
    }

    public void Order()
    {
        currentOrder = OrderController.Instance.GetRandomOrder();
        orderImage.sprite = currentOrder.orderSprite;
        orderImage.gameObject.SetActive(true);
    }

    internal IEnumerator WaitforOrder()
    {
        yield return new WaitForSeconds(1.5f);

        bubbles_Popup.SetActive(true);
        float waitTime = Random.Range(7f, 15f);

        while (waitTime > 0)
        {
            foreach (var bubble in bubblesTranforms)
            {
                bubble.DOAnchorPos(bubble.anchoredPosition + Vector2.up * 10, 1 / bubble_movement_speed);

                yield return new WaitForSeconds(1 / bubble_movement_speed);

                waitTime -= 1 / bubble_movement_speed;
                bubble.DOAnchorPos(bubble.anchoredPosition - Vector2.up * 10, 1 / bubble_movement_speed);
            }
        }

        
        foreach (var bubble in bubblesTranforms)
        {
            bubble.gameObject.SetActive(false);
        }

        bubbles_Popup.transform.GetChild(0).gameObject.SetActive(true);
        bubbles_Popup.transform.GetChild(0).GetComponent<Image>().sprite = orderImage.sprite;
        OrderController.Instance.Cook(currentOrder);
    }

    public IEnumerator Leave(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transform.position = beforeSittingPos;
        agent.enabled = true;
        animator.SetBool("Sitting", false);
        animator.SetBool("Eat", false);
        animator.SetBool("Walk", true);
        agent.SetDestination(StartPosition);
        StartCoroutine(CustomerSpawnController.Instance.WaitandCreateCustomer(tableNumber));
        Destroy(gameObject, 7);
    }

    public IEnumerator StartEating()
    {
        animator.SetBool("Sitting", false);
        animator.SetBool("Eat", true);
        yield return new WaitForSeconds(5f);
        PlayerController.Instance.GetMoney(currentOrder.orderPrice);
        StartCoroutine(Leave(0));
    }

    public void TurnOffOrder()
    {
        orderImage.gameObject.SetActive(false);
    }

    public void OnImageClick()
    {
        UI_Controller.Instance.OnClickOnOrder(this);
    }

    public void OnCookedOrderClick()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < 1.5f)
        {
            if (PlayerController.Instance.GiveFood(currentOrder.orderID, this))
            {
                UI_Controller.Instance.RemoveFromList(currentOrder.orderID);
                StartCoroutine(StartEating());
            }
        }
    }

    //tableFoodSpawnPosition
    public void TakeCookedOrder(ID _gameObjectID)
    {
        _gameObjectID.gameObject.transform.position = tableFoodSpawnPosition.position;
        _gameObjectID.gameObject.SetActive(true);
        Destroy(_gameObjectID.gameObject, 4f);
        bubbles_Popup.SetActive(false);
    }
}

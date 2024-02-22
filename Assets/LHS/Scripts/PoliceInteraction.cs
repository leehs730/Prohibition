using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 경찰NPC를 위한 상호작용 스크립트
public class PoliceInteraction : MonoBehaviour
{
    // 경찰이 식당 내부에 들어왔을 때 나타나는 감정표현 스프라이트
    public GameObject SearchingSprite;

    // 경찰 프리팹이 가게 점검을 실]
    private bool checkingComplete = false;
    public bool CheckingComplete { get { return checkingComplete; } }
    private bool checkingStarted = false;
    public bool CheckingStarted { get { return checkingStarted; } }

    // 경찰이 가게 내부에 대기하는데에 걸리는 시간
    public float CheckingTime = 5f;

    // Update is called once per frame
    void Update()
    {
        if(checkingStarted && !checkingComplete)
        {
            // 입구에 들어왔다면 가게 내부를 점검하는 메서드 실행
            if(CheckingTime > 0f)
            {
                // 시간 제한을 5초로 두고 0이 될때까지 메서드 실행
                CheckingTime -= Time.deltaTime;
                CheckingRestaurant();
            }
            else if(CheckingTime == 0f)
            {
                SearchingComplete();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 식당 내부의 특정 위치(이 위치는 경찰이 식당 내부를 점검하는 위치에 "SearchLocation" 태그가 달린 오브젝트를 배치)와 충돌 시에 발생하도록 함
        if (other.CompareTag("SearchLocation") && !checkingStarted && !checkingComplete)
        {
            //Debug.Log("StartInteraction");
            SeachingStart();
        }
    }

    // 서칭 상태를 시작하는 bool 값 시작
    private void SeachingStart()
    {
        checkingStarted = true;
        SearchingSprite.SetActive(true);
    }

    // 서칭 상태를 종료하는 bool 값들 수정 및 타겟 조정
    private void SearchingComplete()
    {
        checkingComplete = true;
        checkingStarted = false;
        SearchingSprite.SetActive(false);
        ExitToInsideEntrance();
    }

    private void CheckingRestaurant()
    {
        // 레스토랑이 위장 상태가 아니면 플레이어한테 패널티 부여 및 스테이지 종료
        PlayerStatus undercover = GameObject.Find("Player").GetComponent<PlayerStatus>();


        if(!undercover.isUndercover)
        {
            // 중첩 패널티를 넣기 위한 패널티
            GetPenalty();
            SearchingComplete();
        }
    }

    private void GetPenalty()
    {
        // 게임 매니저 오브젝트에 있는 패널티 컴포넌트를 이용해 조건에 따른 
        Penalties PPenalty = GameObject.Find("GameManager").GetComponent<Penalties>();
        Debug.Log("Penalties");
        
    }

    // 자가 파괴 지정
    private void ExitToInsideEntrance()
    {
        if (checkingComplete)
        {
            NPCController controller = GetComponent<NPCController>();
            controller.SetTarget(controller.nextTarget);
        }
    }
}

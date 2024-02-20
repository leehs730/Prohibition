using System.Collections.Generic;
using UnityEngine;


public class InteractionManager : MonoBehaviour
{
    #region Singleton
    public static InteractionManager interactionInstance;

    private void Awake()
    {
        if (interactionInstance == null)
        {
            interactionInstance = this;
        }
        else if (interactionInstance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    string objectName;
    [SerializeField] GameObject[] panels; 
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] GameObject alcoholPanel;
    private Dictionary<string, GameObject> storePanels = new Dictionary<string, GameObject>();

    private void Start()
    {
        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(false);
            storePanels.Add(panel.name, panel);
        }
    }
    public void SetValue(string name)
    {
        objectName = name;
    }


    void OnInteraction()
    {
        if (objectName != "None")
        {
            if (storePanels.TryGetValue(objectName+"Panel", out GameObject panel))
            {
                // 패널을 활성화합니다.
                panel.SetActive(true);
            }
            else if (objectName == "Counter")
            {
                //음식
                playerStatus.IsServed("Food");
            }
            else if (objectName == "Booze")
            {
                //술
                alcoholPanel.SetActive(true);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class Datas
{
    public PlayerData Playerinfo;
    public List<Item> items;
    public List<PlayerInventory> inventory;
    public List<MissionData> missions;
}

[System.Serializable]
public class Item
{
    public string Classification;
    public string Name;
    public int Quantity;
    public int PurchasePrice;
    public int SellingPrice;
    public int RiseScale;
    public int EnhancementValue;
    public string spritePath;
}

[System.Serializable]
public class PlayerInventory
{
    public string Classification;
    public string Name;
    public int Quantity;
    public int PurchasePrice;
    public int SellingPrice;
    public int RiseScale;
    public int EnhancementValue;
    public string spritePath;
}
[System.Serializable]
public class InventoryWrapper
{
    public List<PlayerInventory> inventory;
}
[System.Serializable]
public class ItemWrapper
{
    public List<Item> items;
}
public class MissionWrapper
{
    public List<MissionData> missions;
}
[System.Serializable]
public class PlayerData
{
    public int Gold = 100;
    public int Debt = 50000;
    public int Day = 1;
    public bool IsDay = true;
}
[System.Serializable]
public class MissionData
{
    public string Name;
    public string Description;
    public bool DidMission;
}
public class DataManager : MonoBehaviour
{
    internal string playerName;
    public Datas nowPlayer = new Datas();

    string path;

    #region Singleton
    public static DataManager instance;
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        if (transform.parent != null)
        {
            DontDestroyOnLoad(transform.parent.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        path = Application.persistentDataPath;
    }
    #endregion
    /*
    #region Save
    public void SetValue()
    {
        WWWForm form = new WWWForm();

        string goldValue = nowPlayer.Playerinfo.Gold.ToString();
        if (goldValue == null) goldValue = "";
        Debug.Log(goldValue);

        string debtValue = nowPlayer.Playerinfo.Debt.ToString();
        if (debtValue == null) debtValue = "";
        Debug.Log(debtValue);

        string tutorialValue = nowPlayer.Playerinfo.DidTutorial.ToString();
        if (tutorialValue == null) tutorialValue = "";
        Debug.Log(tutorialValue);

        string dayValue = nowPlayer.Playerinfo.Day.ToString();
        if (dayValue == null) dayValue = "";
        Debug.Log(dayValue);

        InventoryWrapper invenWrapper = new InventoryWrapper { inventory = nowPlayer.inventory };
        string invenValue = JsonUtility.ToJson(invenWrapper);
        if (invenValue == null) invenValue = "";

        ItemWrapper itemWrapper = new ItemWrapper { items = nowPlayer.items};
        string itemValue = JsonUtility.ToJson(itemWrapper);
        if (itemValue == null) itemValue = "";

        Debug.Log(invenValue);

        form.AddField("order", "setValue");
        form.AddField("gold", goldValue);
        form.AddField("debt", debtValue);
        form.AddField("tutorial", tutorialValue);
        form.AddField("inven", invenValue);
        form.AddField("item", itemValue);
        form.AddField("day", dayValue);

        StartCoroutine(LoginManager.loginInstance.Post(form));
    }
    #endregion
    */

    #region SaveDatas
    public void SaveAllData()
    {
        SavePlayerData();
        SaveInventoryData();
        SaveItemData();
        SaveMissionData();
    }

    void SavePlayerData()
    {
        string playerData = JsonUtility.ToJson(nowPlayer.Playerinfo, true);
        File.WriteAllText(path + "/playerData.json", playerData);
    }
    void SaveInventoryData()
    {
        InventoryWrapper invenWrapper = new InventoryWrapper { inventory = nowPlayer.inventory };
        string InventoryData = JsonUtility.ToJson(invenWrapper, true);
        File.WriteAllText(path + "/inventoryData.json", InventoryData);
    }
    void SaveItemData()
    {
        ItemWrapper itemWrapper = new ItemWrapper { items = nowPlayer.items };
        string ItemData = JsonUtility.ToJson(itemWrapper, true);
        File.WriteAllText(path + "/itemData.json", ItemData);
    }
    void SaveMissionData()
    {
        MissionWrapper missionWrapper = new MissionWrapper { missions = nowPlayer.missions };
        string missionData = JsonUtility.ToJson(missionWrapper, true);
        File.WriteAllText(path + "/missionData.json", missionData);
    }
    #endregion

    #region LoadDatas
    public void LoadAllData()
    {
        try
        {
            bool result = CheckDatas();
            #region ItemData Load
            if(result)
            {
                Debug.Log("Local Item Data ");
                Debug.Log("Local Inventory Data ");
                Debug.Log("Local Player Data ");
                Debug.Log("Local Mission Data");

                string ItemData = File.ReadAllText(path + "/itemData.json");
                string InventoryData = File.ReadAllText(path + "/inventoryData.json");
                string PlayerData = File.ReadAllText(path + "/playerData.json");
                string MissionData = File.ReadAllText(path + "/missionData.json");

                ItemWrapper itemWrapper = JsonUtility.FromJson<ItemWrapper>(ItemData);
                InventoryWrapper inventoryWrapper = JsonUtility.FromJson<InventoryWrapper>(InventoryData);
                MissionWrapper missionWrapper = JsonUtility.FromJson<MissionWrapper>(MissionData);

                nowPlayer.items = itemWrapper.items;
                nowPlayer.inventory = inventoryWrapper.inventory;
                nowPlayer.missions = missionWrapper.missions;
                nowPlayer.Playerinfo = JsonUtility.FromJson<PlayerData>(PlayerData);
            }
            else 
            {
                DeleteAllData();

                Debug.Log("Base Item Data ");
                Debug.Log("Base Inventory Data ");
                Debug.Log("Base Mission Data");
                Debug.Log("Base Player Data ");

                TextAsset itemFile = Resources.Load<TextAsset>("Datas/ItemData");
                TextAsset inventoryFile = Resources.Load<TextAsset>("Datas/InventoryData");
                TextAsset missionFile = Resources.Load<TextAsset>("Datas/MissionData");

                if (itemFile == null) throw new Exception("아이템 데이터를 찾을 수 없습니다.");
                if (inventoryFile == null) throw new Exception("인벤토리 데이터를 찾을 수 없습니다.");
                if (missionFile == null) throw new Exception("미션 데이터를 찾을 수 없습니다.");

                Datas itemData = JsonUtility.FromJson<Datas>(itemFile.text);
                Datas inventoryData = JsonUtility.FromJson<Datas>(inventoryFile.text);
                Datas MissionData = JsonUtility.FromJson<Datas>(missionFile.text);

                nowPlayer.items = itemData.items;
                nowPlayer.inventory = inventoryData.inventory;
                nowPlayer.missions = MissionData.missions;
                nowPlayer.Playerinfo = new PlayerData();
            }
            /*
            if (File.Exists(path + "/itemData.json"))
            {
                Debug.Log("Local Item Data ");
                string ItemData = File.ReadAllText(path + "/itemData.json");
                ItemWrapper itemWrapper = JsonUtility.FromJson<ItemWrapper>(ItemData);
                nowPlayer.items = itemWrapper.items;
            }
            else
            {
                Debug.Log("Base Item Data ");
                TextAsset itemFile = Resources.Load<TextAsset>("Datas/ItemData");
                if (itemFile == null) throw new Exception("아이템 데이터를 찾을 수 없습니다.");
                Datas itemData = JsonUtility.FromJson<Datas>(itemFile.text);
                nowPlayer.items = itemData.items;
            }
            Debug.Log("아이템 데이터 성공");
            #endregion

            #region PlayerData Load
            if (File.Exists(path + "/playerData.json"))
            {
                Debug.Log("Local Player Data ");
                string PlayerData = File.ReadAllText(path + "/playerData.json");
                nowPlayer.Playerinfo = JsonUtility.FromJson<PlayerData>(PlayerData);
            }
            else
            {
                Debug.Log("Base Player Data ");
                nowPlayer.Playerinfo = new PlayerData();
            }
            #endregion
            
            #region InventoryData Load
            if (File.Exists(path + "/inventoryData.json"))
            {
                Debug.Log("Local Inventory Data ");
                string InventoryData = File.ReadAllText(path + "/inventoryData.json");
                InventoryWrapper inventoryWrapper = JsonUtility.FromJson<InventoryWrapper>(InventoryData);
                nowPlayer.inventory = inventoryWrapper.inventory;
            }
            else
            {
                Debug.Log("Base Inventory Data ");
                TextAsset inventoryFile = Resources.Load<TextAsset>("Datas/InventoryData");
                if (inventoryFile == null) throw new Exception("인벤토리 데이터를 찾을 수 없습니다.");
                Datas inventoryData = JsonUtility.FromJson<Datas>(inventoryFile.text);
                nowPlayer.inventory = inventoryData.inventory;
            }
            */
            #endregion
        }
        catch (Exception e)
        {
            Debug.LogError("데이터를 로드하는데 실패했습니다.: " + e.Message);
        }
    }
    #endregion

    #region DeleteDatas
    public void DeleteAllData()
    {
        File.Delete(path + "/playerData.json");
        File.Delete(path + "/inventoryData.json");
        File.Delete(path + "/itemData.json");
    }
    #endregion

    #region CheckDatas
    public bool CheckDatas()
    {
        if (File.Exists(path + "/playerData.json") && File.Exists(path + "/itemData.json") && File.Exists(path + "/inventoryData.json")) { return true; }
        else { return false; }
    }
    #endregion

}

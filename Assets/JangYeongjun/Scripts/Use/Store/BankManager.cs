using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class BankManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI debtText;
    [SerializeField] TextMeshProUGUI warningText;
    [SerializeField] TextMeshProUGUI mafiaText;
    #region �̱���
    public static BankManager BankInstance;
    private void Awake()
    {
        
        if (BankInstance == null)
        {
            BankInstance = this;
        }
        else if (BankInstance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    private void Start()
    {
        TextFix();
    }
    public void Repayment(int goldValue)
    {
        if (goldValue <= TemporaryDataManager.instance.nowPlayer.Playerinfo.Gold)
        {
            TemporaryDataManager.instance.nowPlayer.Playerinfo.Debt -= goldValue;
            TemporaryDataManager.instance.nowPlayer.Playerinfo.Gold -= goldValue;
            TextFix();
        }
        else 
        {
            warningText.text = String.Format("{0:N0}", goldValue - TemporaryDataManager.instance.nowPlayer.Playerinfo.Gold) + "��ŭ �������� �����մϴ�.";
            mafiaText.color = new Color(1, 0, 0, 1);
            mafiaText.text = "���� �������ݾ�!!!";
        }
    }
    public void Loan(int goldValue)
    {
        if (TemporaryDataManager.instance.nowPlayer.Playerinfo.Debt + goldValue <= 3000000)
        {
            TemporaryDataManager.instance.nowPlayer.Playerinfo.Debt += goldValue;
            TemporaryDataManager.instance.nowPlayer.Playerinfo.Gold += goldValue;
            TextFix();
        }
        else 
        {
            warningText.text = "�ѵ� 3,000,000 Gold�� �ѽ��ϴ�.";
            mafiaText.color = new Color(1, 0, 0, 1);
            mafiaText.text = "�󸶳� ���� ������ �ž�?!!";
        }
    }
    void TextFix()
    {
        int playerGold = TemporaryDataManager.instance.nowPlayer.Playerinfo.Gold;
        int playerDebt = TemporaryDataManager.instance.nowPlayer.Playerinfo.Debt;
        goldText.text = String.Format("{0:N0}", playerGold);
        debtText.text = String.Format("{0:N0}", playerDebt);
    }
}
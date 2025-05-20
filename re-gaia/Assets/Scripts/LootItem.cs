using UnityEngine;

[System.Serializable]

public class LootItem 
{
    public GameObject lootPrefab;
    [Range(0,100)] public float dropRate;

}
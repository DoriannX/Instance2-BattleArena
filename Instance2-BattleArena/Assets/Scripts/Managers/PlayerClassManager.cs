using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerClassManager : MonoBehaviour
{
    [System.Serializable]
    public class CharacterClass
    {
        public string ClassName;
        public Sprite BaseSprite;
        public int BaseAttack;
        public int BaseHeal;
        public float BaseAttackSpeed;

        public void ApplyClassStats(PlayerStats stats)
        {
            stats.SetStats(BaseAttack, BaseHeal, BaseAttackSpeed);
        }
    }

    public CharacterClass[] CharacterClasses;
    private PlayerStats _playerStats;
    public GameObject PlayerPrefabShield;
    public GameObject PlayerPrefabSoldier;
    public GameObject PlayerPrefabCarrier;
    public GameObject PanelSelectClass;
    public GameObject PanelSelectSkin;

    public int selectedClassIndex = -1;
    public CharacterClass selectedClass;

    void Start()
    {
        _playerStats = PlayerPrefabShield.GetComponent<PlayerStats>();
    }

    public void SelectClass(int classIndex)
    {
        if (classIndex >= 0 && classIndex < CharacterClasses.Length)
        {
            selectedClassIndex = classIndex;
            selectedClass = CharacterClasses[classIndex];
            PlayerPrefabShield.GetComponent<SpriteRenderer>().sprite = selectedClass.BaseSprite;
            selectedClass.ApplyClassStats(_playerStats);
            PanelSelectClass.SetActive(false);
            PanelSelectSkin.SetActive(true);
        }
    }
}
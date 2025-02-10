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
    public GameObject PlayerPrefab;
    public GameObject PlayerPrefabAlternate;
    public GameObject PanelSelectClass;
    public GameObject PanelSelectSkin;

    public int selectedClassIndex = -1;
    public CharacterClass selectedClass;

    void Start()
    {
        _playerStats = PlayerPrefab.GetComponent<PlayerStats>();
    }

    public void SelectClass(int classIndex)
    {
        if (classIndex >= 0 && classIndex < CharacterClasses.Length)
        {
            selectedClassIndex = classIndex;
            selectedClass = CharacterClasses[classIndex];
            PlayerPrefab.GetComponent<SpriteRenderer>().sprite = selectedClass.BaseSprite;
            selectedClass.ApplyClassStats(_playerStats);
            PanelSelectClass.SetActive(false);
            PanelSelectSkin.SetActive(true);
        }
    }
}
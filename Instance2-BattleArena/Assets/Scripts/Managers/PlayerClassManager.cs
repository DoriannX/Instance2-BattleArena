using UnityEngine;

public class PLayerClassManager : MonoBehaviour
{
    [System.Serializable]
    public class CharacterClass
    {
        public string ClassName;            
        public Sprite BaseSprite;         
        public Sprite Level10Sprite;
        public Sprite Level20Sprite;
        public int BaseAttack;              
        public int BaseSpeed;
        public void ApplyClassStats(PlayerStats stats)
        {
            stats.SetStats(BaseAttack, BaseSpeed);
        }
    }
    public CharacterClass[] CharacterClasses;
    private PlayerStats _playerStats;
    public GameObject PlayerSprite;
    public GameObject PanelSelectClass;

    [SerializeField] private Transform _playerSpawn;

    void Start()
    {
        _playerStats = PlayerSprite.GetComponent<PlayerStats>();
    }

    public void SelectClass(int classIndex)
    {
        if (classIndex >= 0 && classIndex < CharacterClasses.Length)
        {
            CharacterClass selectedClass = CharacterClasses[classIndex];
            PlayerSprite.GetComponent<SpriteRenderer>().sprite = selectedClass.BaseSprite;
            selectedClass.ApplyClassStats(_playerStats);
            PanelSelectClass.gameObject.SetActive(false);
            Instantiate(PlayerSprite, _playerSpawn);
        }
    }
}

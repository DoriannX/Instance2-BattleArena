using UnityEngine;

public class PlayerClassManager : MonoBehaviour
{
    [System.Serializable]
    public class CharacterClass
    {
        public string ClassName;
        public Sprite BaseSprite;
        public Sprite Level10Sprite;
        public Sprite Level20Sprite;
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
    public GameObject PanelSelectClass;
    public ExpManager expManager;

    [SerializeField] private Transform _playerSpawn;
    [SerializeField] private GameObject PlayerPrefabAlternate;  

    private int _selectedClassIndex = -1;

    void Start()
    {
        _playerStats = PlayerPrefab.GetComponent<PlayerStats>();
    }

    public void SelectClass(int classIndex)
    {
        if (classIndex >= 0 && classIndex < CharacterClasses.Length)
        {
            _selectedClassIndex = classIndex;
            CharacterClass selectedClass = CharacterClasses[classIndex];
            PlayerPrefab.GetComponent<SpriteRenderer>().sprite = selectedClass.BaseSprite;
            selectedClass.ApplyClassStats(_playerStats);
            PanelSelectClass.SetActive(false);
            GameObject playerInstance;

            if (classIndex == 0)  
            {
                playerInstance = Instantiate(PlayerPrefabAlternate, _playerSpawn.position, Quaternion.identity);
            }
            else
            {
                playerInstance = Instantiate(PlayerPrefab, _playerSpawn.position, Quaternion.identity);
            }
            if (expManager != null)
            {
                expManager.Initialize(selectedClass, playerInstance);
            }
        }
    }
}

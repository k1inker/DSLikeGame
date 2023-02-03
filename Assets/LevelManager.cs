using DS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentWave = 0;
    public UIBossHealthBar bossHealthBar;
    
    [SerializeField] private int countWave = 4;
    [SerializeField] private int enemiesPerWave = 1;
    [SerializeField] private List<GameObject> enemyType;
    [SerializeField] private GameObject boss;
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private float timeBetweenEnemies = 2f;
    [SerializeField] private float wallRiseSpeed = 2f;


    [SerializeField] private Transform wall;
    [SerializeField] private Vector3 volume;
    [SerializeField] private Vector3 spawnPoint;

    private int countEnemy = 0;
    private float _wallEndY;
    private float _timeSinceLastWave;
    private bool _isWallRising;
    private void Awake()
    {
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();

    }
    private void Start()
    {
        spawnPoint = gameObject.transform.GetChild(0).position;
        _wallEndY = wall.position.y * 2.5f;
        _timeSinceLastWave = timeBetweenWaves;
    }

    private void Update()
    {
        if(currentWave == countWave && countEnemy == 0)
        {
            StopCoroutine(nameof(SpawnEnemies));
            ActiveBossFight();
        }

        _timeSinceLastWave += Time.deltaTime;


        if (_timeSinceLastWave >= timeBetweenWaves || countEnemy == 0)
        {
            StartNewWave();
        }

        if (_isWallRising)
        {
            RaiseWall();
        }
    }
    public List<GameObject> ChoosingEnemyTypeSpawn()
    {
        List<GameObject> enemyTypeSpawn = new List<GameObject>();
        for (int i = 0; i < enemiesPerWave; i++)
        {
            foreach (GameObject enemy in enemyType)
            {
                EnemyManager manager = enemy.GetComponent<EnemyManager>();
                int randomChance = Random.Range(0, 100);
                if (randomChance <= manager.chanceSpawn)
                {
                    enemyTypeSpawn.Add(enemy);
                }
            }
        }
        return enemyTypeSpawn;
    }
    private void StartNewWave()
    {
        _timeSinceLastWave = 0f;
        _isWallRising = true;
        StartCoroutine(SpawnEnemies(ChoosingEnemyTypeSpawn()));
    }

    private IEnumerator SpawnEnemies(List<GameObject> enemyPrefab)
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            countEnemy++;
            Vector3 possition = new Vector3(Random.Range(spawnPoint.x - volume.x, spawnPoint.x + volume.x),
                spawnPoint.y,
                Random.Range(spawnPoint.z - volume.z, spawnPoint.z + volume.z));
            Instantiate(enemyPrefab[i], possition, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenEnemies);
        }

        currentWave++;
        enemiesPerWave += 1;
    }

    private void RaiseWall()
    {
        wall.position = new Vector3(wall.position.x, wall.position.y + (wallRiseSpeed * Time.deltaTime), wall.position.z);

        if (wall.position.y >= _wallEndY)
        {
            _isWallRising = false;
        }
    }
    public void ActiveBossFight()
    {
        Instantiate(boss, spawnPoint, Quaternion.identity);
        bossHealthBar.SetHealthBarToActive();
        countEnemy++;
    }
    public void BossHasDefeated()
    {   
        SceneManager.LoadScene("Level1");
    }
    public void DefeatEnemy()
    {
        countEnemy--;
        //add chance item droping
    }
}

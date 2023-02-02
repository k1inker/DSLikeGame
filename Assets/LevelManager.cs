using DS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> enemyType;
    
    public int currentWave = 0;

    [SerializeField] private int enemiesPerWave = 1;
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private float timeBetweenEnemies = 2f;
    [SerializeField] private float wallRiseSpeed = 2f;
    [SerializeField] private float wallLowerSpeed = 2f;
    [SerializeField] private Transform wall;
    [SerializeField] private Vector3 volume;

    private int countEnemy = 0;
    private float _wallStartY;
    private float _wallEndY;
    private float _timeSinceLastWave;
    private bool _isWallRising;
    private bool _isWallLowering;
    private void Start()
    {
        _wallStartY = wall.position.y;
        _wallEndY = wall.position.y * 2f;
        _timeSinceLastWave = timeBetweenWaves;
    }

    private void Update()
    {
        _timeSinceLastWave += Time.deltaTime;

        if (_timeSinceLastWave >= timeBetweenWaves)
        {
            StartNewWave();
        }

        if (_isWallRising)
        {
            RaiseWall();
        }

        if (_isWallLowering)
        {
            LowerWall();
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
            Vector3 possition = new Vector3(Random.Range(gameObject.transform.position.x - volume.x, gameObject.transform.position.x + volume.x), 
                Random.Range(gameObject.transform.position.y - volume.y, gameObject.transform.position.y + volume.y),
                Random.Range(gameObject.transform.position.z - volume.z, gameObject.transform.position.z + volume.z));
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

    private void LowerWall()
    {
        wall.position = new Vector3(wall.position.x, wall.position.y - (wallLowerSpeed * Time.deltaTime), wall.position.z);

        if (wall.position.y <= _wallStartY)
        {
            _isWallLowering = false;
        }
    }
    public void DefeatEnemy()
    {
        countEnemy--;
        //add chance item droping
        if (countEnemy == 0)
        {
            //instantiet transition next level
            SceneManager.LoadScene("Level1");
        }
    }
}

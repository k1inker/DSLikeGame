using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace DS
{
    public class LevelManager : MonoBehaviour
    {
        public int currentWave = 0;

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

        private UIManager _UIManager;

        private int countEnemy = 0;
        private float _wallEndY;
        private float _timeSinceLastWave;
        private bool _isWallRising;
        private void Awake()
        {
            _UIManager = FindObjectOfType<UIManager>();
        }
        private void Start()
        {
            spawnPoint = gameObject.transform.GetChild(0).position;
            _wallEndY = wall.position.y;
            _timeSinceLastWave = timeBetweenWaves;
        }
        private void Update()
        {
            if (currentWave == countWave && countEnemy == 0)
            {
                StopCoroutine(nameof(SpawnEnemies));
                spawnPoint = Vector3.zero;
                ActiveBossFight();
            }

            _timeSinceLastWave += Time.deltaTime;


            if (_timeSinceLastWave >= timeBetweenWaves || countEnemy == 0)
            {
                StartNewWave();
                _UIManager.ShowWaveIndicator(currentWave);
            }

            if (_isWallRising)
            {
                RaiseWall();
            }
        }
        public List<GameObject> ChoosingEnemyTypeSpawn()
        {
            List<GameObject> enemyTypeSpawn = new List<GameObject>();
            do
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
            } while (enemyTypeSpawn.Count < enemiesPerWave);
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

            _UIManager.ShowWaveIndicator(currentWave);
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
            countEnemy++;
            Instantiate(boss, spawnPoint, Quaternion.identity);
            _UIManager.bossHealthBar.SetHealthBarToActive();
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
}

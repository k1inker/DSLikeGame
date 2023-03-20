using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace DS
{
    public class LevelManager : MonoBehaviour
    {
        public int currentWave = 0;

        [Header("Settings wave")]
        [SerializeField] private int _countWave = 4;
        [SerializeField] private int _enemiesPerWave = 1;
        [SerializeField] private List<GameObject> _enemyType;
        [SerializeField] private GameObject _boss;
        [SerializeField] private float _timeBetweenEnemies = 2f;
        [SerializeField] private Animator _animatorGate;

        [Header("Settings spawn area")]
        [SerializeField] private Vector3 _volume;
        [SerializeField] private Vector3 _spawnPoint;

        private UIManager _UIManager;

        private int _countEnemy = 0;
        private bool _isBossFight = false;
        private void Awake()
        {
            _UIManager = FindObjectOfType<UIManager>();
        }
        public void StartLevel()
        {
            _spawnPoint = gameObject.transform.GetChild(0).position;
            _animatorGate.Play("UpGate");
            StartNewWave();
        }
        public List<GameObject> ChoosingEnemyTypeSpawn()
        {
            List<GameObject> enemyTypeSpawn = new List<GameObject>();
            do
            {
                foreach (GameObject enemy in _enemyType)
                {
                    EnemyManager manager = enemy.GetComponent<EnemyManager>();
                    int randomChance = Random.Range(0, 100);
                    if (randomChance <= manager.chanceSpawn)
                    {
                        enemyTypeSpawn.Add(enemy);
                    }
                }
            } while (enemyTypeSpawn.Count < _enemiesPerWave);
            return enemyTypeSpawn;
        }
        private void StartNewWave()
        {
            StartCoroutine(SpawnEnemies(ChoosingEnemyTypeSpawn()));
        }
        private IEnumerator SpawnEnemies(List<GameObject> enemyPrefab)
        {
            for (int i = 0; i < _enemiesPerWave; i++)
            {
                _countEnemy++;
                Vector3 possition = new Vector3(Random.Range(_spawnPoint.x - _volume.x, _spawnPoint.x + _volume.x),
                    _spawnPoint.y,
                    Random.Range(_spawnPoint.z - _volume.z, _spawnPoint.z + _volume.z));
                Instantiate(enemyPrefab[i], possition, Quaternion.identity);
                yield return new WaitForSeconds(_timeBetweenEnemies);
            }

            currentWave++;
            _enemiesPerWave += 1;
        }
        public void ActiveBossFight()
        {
            _countEnemy++;
            Instantiate(_boss, _spawnPoint, Quaternion.identity);
            _UIManager.bossHealthBar.SetHealthBarToActive();
        }
        public void BossHasDefeated()
        {
            SceneManager.LoadScene(0);
        }
        public void DefeatEnemy()
        {
            _countEnemy--;

            HandleActiveNewWave();
            //add chance item droping
        }
        private void HandleActiveNewWave()
        {
            if (_countEnemy > 0)
                return;

            if (currentWave == _countWave)
            {
                _spawnPoint = Vector3.zero;
                ActiveBossFight();
                _isBossFight = true;
            }

            if (_isBossFight)
                return;

            StartNewWave();
            _UIManager.ShowWaveIndicator(currentWave);
        }
    }
}

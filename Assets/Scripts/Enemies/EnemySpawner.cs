using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public int enemyQuantity;

        private List<GameObject> _enemyList;
        private BoxCollider _rangeBox;
    
        void Start()
        {
            _rangeBox = GetComponent<BoxCollider>();
            _enemyList = new List<GameObject>();

            SpawnEnemies();
        }

        private void RandomizePosition(GameObject obj)
        {
            float randomX = Random.Range(_rangeBox.bounds.min.x, _rangeBox.bounds.max.x);
            float randomZ = Random.Range(_rangeBox.bounds.min.z, _rangeBox.bounds.max.z);

            obj.transform.position = new Vector3(randomX, 0.5f, randomZ);
        }
    
        private void AddEnemyToList()
        {
            var temp = Instantiate(enemyPrefab, transform);
            RandomizePosition(temp);
            _enemyList.Add(temp);
        }
    
        private void SpawnEnemies()
        {
            for (int i = 0; i < enemyQuantity; i++)
            {
                AddEnemyToList();
            }
        }

        public List<GameObject> GetEnemyList()
        {
            return _enemyList;
        }
    }
}

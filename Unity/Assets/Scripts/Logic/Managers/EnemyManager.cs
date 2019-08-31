using System.Collections.Generic;
using Lockstep.Game;
using Lockstep.Math;
using UnityEngine;
using Debug = Lockstep.Logging.Debug;

namespace LockstepTutorial {
    public class EnemyManager : BaseLogicManager {
        public List<Spawner> spawners;
        public static EnemyManager Instance { get; private set; }
        public List<Enemy> allEnemy = new List<Enemy>();

        public static int maxCount = 1;
        private static int curCount = 0;
        private static int enmeyID = 0;

        public override void DoAwake(IServiceContainer services){
            Instance = this;
            var config = Resources.Load<GameConfig>("GameConfig");
            spawners = config.SpawnerConfig.spawners;
        }

        public override void DoStart(){
            foreach (var spawner in spawners) {
                spawner.OnSpawnEvent += OnSpawn;
                spawner.DoStart();
            }
        }

        public override void DoUpdate(LFloat deltaTime){
            foreach (var spawner in spawners) {
                spawner.DoUpdate(deltaTime);
            }

            foreach (var enemy in allEnemy) {
                enemy.DoUpdate(deltaTime);
            }
        }

        private void OnSpawn(int prefabId, LVector3 position){
            if (curCount >= maxCount) {
                return;
            }

            curCount++;
            var entity = InstantiateEntity(prefabId, position);
            Instance.AddEnemy(entity as Enemy);
        }


        public void AddEnemy(Enemy enemy){
            allEnemy.Add(enemy);
        }

        public void RemoveEnemy(Enemy enemy){
            allEnemy.Remove(enemy);
        }
        
        
        public static BaseEntity InstantiateEntity(int prefabId, LVector3 position){
            var prefab = ResourceGameService.LoadPrefab(prefabId);
            var config = ResourceGameService.GetEnemyConfig(prefabId);
            Debug.Trace("CreateEnemy");
            var entity = new Enemy();
            var obj = UnityEntityService.CreateEntity(entity, prefabId, position, prefab, config);
            obj.name = obj.name + enmeyID++;
            return entity;
        }
    }
}
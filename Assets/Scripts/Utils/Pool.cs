using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Stairs.Utils
{
    public sealed class Pool : Singleton<Pool>
    {
        public Dictionary<GameObject, Step> GoToStepDictionary = new Dictionary<GameObject, Step>();
        public Stairs StairController = null;

        private readonly Dictionary<string, Queue<GameObject>> _pools = new Dictionary<string, Queue<GameObject>>();
        private readonly Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

        private Transform _parent = null;

        public bool AddToPool(GameObject prefab, int count, Transform parent = null)
        {
            if (prefab == null || count < 1) return false;
            _parent = parent;

            if (!_prefabs.ContainsKey(prefab.name))
            {
                _prefabs.Add(prefab.name, prefab);
                _pools.Add(prefab.name, new Queue<GameObject>());
            }

            for (var i = 0; i < count; i++)
            {
                var go = GetObject(prefab, true);
                if (_parent != null) go.transform.parent = _parent;
                if (go != null) ReturnObject(ref go);
            }

            return true;
        }

        public GameObject GetObject(GameObject prefab, bool forceNew = false)
        {
            if (!_prefabs.ContainsKey(prefab.name)) return null;

            if (!forceNew && _pools[prefab.name].Count >= 1)
            {
                var rv = _pools[prefab.name].Dequeue();
                rv.SetActive(true);
                return rv;
            }

            var go = Instantiate(prefab);
            go.transform.parent = _parent;
            go.name = prefab.name;
            return go;
        }

        public void ReturnObject(ref GameObject obj, bool destroyObject = false)
        {
            if (obj == null) return;

            if (destroyObject || !_prefabs.ContainsKey(obj.name))
            {
                Destroy(obj);
                return;
            }

            obj.SetActive(false);
            _pools[obj.name].Enqueue(obj);
        }

        public void DestroyPool(GameObject prefab)
        {
            if (!_prefabs.ContainsKey(prefab.name)) return;

            _prefabs.Remove(prefab.name);

            for (var i = 0; i < _pools[prefab.name].Count; i++)
            {
                var go = _pools[prefab.name].Dequeue();
                Destroy(go);
            }

            _pools.Remove(prefab.name);
        }

        public void DestroyAllPools()
        {
            for (var i = 0; i < _pools.Count; i++)
            {
                var t = _prefabs.First();
                DestroyPool(t.Value);
            }
        }

        public void Awake()
        {
            StairController = FindObjectOfType<Stairs>();
        }
    }
}

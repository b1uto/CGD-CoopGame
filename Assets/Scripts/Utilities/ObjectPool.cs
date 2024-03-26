using System.Collections.Generic;
using UnityEngine;

namespace CGD.Utilities
{
    public class ObjectPool<T> : MonoBehaviour where T : Component
    {
        public bool addChildrenOnAwake = true;
        public GameObject prefab;

        protected List<T> pool = new List<T>();

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                var comp = child.GetComponent<T>();
                
                if(comp != null)
                   pool.Add(comp);
            }
        }

        private void OnDisable()
        {
            ReturnAllObjects();
        }

        private void ReturnAllObjects()
        {
            foreach (var obj in pool)
            {
                obj.gameObject.SetActive(false);
            }
        }

        public void ResetUnusedObjects(int lastIndex)
        {
            for (int i = lastIndex; i < pool.Count; i++)
            {
                pool[i].gameObject.SetActive(false);
            }
        }

        public T GetObject()
        {
            foreach (var obj in pool)
            {
                if (obj.gameObject.activeSelf == false)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            var newObj = Instantiate(prefab, transform).GetComponent<T>();
            newObj.gameObject.SetActive(true);
            pool.Add(newObj);
            return newObj;
        }
    }
}

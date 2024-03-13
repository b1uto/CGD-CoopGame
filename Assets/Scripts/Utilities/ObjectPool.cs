using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGD.Utilities
{
    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        public bool addChildrenOnAwake = true;
        public GameObject prefab;

        private List<T> pool = new List<T>();

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                pool.Add(child.GetComponent<T>());
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

using System.Collections.Generic;
using UnityEngine;

public class GenericPoolFactory<T> where T : MonoBehaviour {
    private T prefab;
    private bool isExpandable;
    private Queue<T> pooledItems;
    private List<T> itemsInUse;
    private GameObject poolContainer;
    private int initialCount;

    public GenericPoolFactory(T prefab, int initialCount = 10, bool isExpandable = true, Transform parent = null) {
        this.prefab = prefab;
        this.initialCount = initialCount;
        this.isExpandable = isExpandable;
        Initialize(parent);
    }

    private void Initialize(Transform parent) {
        pooledItems = new Queue<T>();
        itemsInUse = new List<T>();
        poolContainer = new GameObject($"{typeof(T).Name} Pool");
        poolContainer.transform.parent = parent;

        for (int i = 0; i < initialCount; i++) {
            T newItem = Object.Instantiate(prefab, poolContainer.transform);
            newItem.gameObject.SetActive(false);
            pooledItems.Enqueue(newItem);
        }
    }

    public T GetNewInstance() {
        if (pooledItems.Count > 0) {
            T item = pooledItems.Dequeue();
            itemsInUse.Add(item);
            item.gameObject.SetActive(true);
            return item;
        } else if (isExpandable) {
            T newItem = Object.Instantiate(prefab, poolContainer.transform);
            itemsInUse.Add(newItem);
            return newItem;
        } else {
            throw new System.Exception("No items available in the pool and the pool is not expandable.");
        }
    }

    public void ReturnInstance(T item) {
        if (itemsInUse.Contains(item)) {
            item.gameObject.SetActive(false);
            itemsInUse.Remove(item);
            pooledItems.Enqueue(item);
        } else {
            Debug.LogWarning($"Trying to return an item that is not managed by this pool: {item}");
        }
    }
}
q
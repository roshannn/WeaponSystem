using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class GenericPoolFactory<T> where T : MonoBehaviour {
    [InjectOptional] private DiContainer _container;
    
    [SerializeField]
    private T prefab;
    [SerializeField]
    private bool isExpandable;
    [SerializeField] 
    private int initialCount;
    
    
    private Queue<T> pooledItems;
    private List<T> itemsInUse;
    private GameObject poolContainer;

    /// <summary>
    /// Call This To Initialize The Pool
    /// </summary>
    /// <param name="parent"></param>
    public void Initialize(Transform parent) {
        pooledItems = new Queue<T>();
        itemsInUse = new List<T>();
        poolContainer = new GameObject($"{typeof(T).Name} Pool");
        poolContainer.transform.parent = parent;
        _container.Inject(this);for (int i = 0; i < initialCount; i++) {
            T newItem = CreateNewItem();
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
            T newItem = CreateNewItem();
            itemsInUse.Add(newItem);
            return newItem;
        } else {
            throw new System.Exception("No items available in the pool and the pool is not expandable.");
        }
    }
    private T CreateNewItem() {
        T newItem = UnityEngine.Object.Instantiate(prefab, poolContainer.transform);
        _container.Inject(newItem);
        return newItem;
    }


    public void ReturnInstance(T item) {
        if (itemsInUse.Contains(item)) {
            item.gameObject.SetActive(false);
            item.transform.SetParent(poolContainer.transform,false);
            itemsInUse.Remove(item);
            pooledItems.Enqueue(item);
        } else {
            Debug.LogWarning($"Trying to return an item that is not managed by this pool: {item}");
        }
    }

    public void ReturnAllInstances() {
        foreach (var item in itemsInUse) {
            item.gameObject.SetActive(false);
            item.transform.SetParent(poolContainer.transform,false);
            pooledItems.Enqueue(item);
        }
        itemsInUse.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerManager : MonoBehaviour
{
    public int index = 1;
    public List<Transform> cityList;
    public List<Transform> emptyCity;

    public static PoolerManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public void Shuffle()
    {
        var count = cityList.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = cityList[i];
            cityList[i] = cityList[r];
            cityList[r] = tmp;
        }
    }

    //public Transform GetFromPool()
    //{
    //    for (int i = 0; i < cityList.Count; i++)
    //    {
    //        if (!cityList[i].gameObject.activeInHierarchy)
    //        {
    //            return cityList[i];
    //        }
    //    }
    //    return null;
    //}

    public Transform GetFromPool()
    {
        if (index == cityList.Count - 1)
        {
            Shuffle();
            index = -1;
        }

        index++;

        while (cityList[index].gameObject.activeInHierarchy)
        {
            index++;

            if (index > 3)
            {
                index = 0;
            }
        }

        return cityList[index];
    }

    public Transform GetEmptyChunk()
    {
        for (int i = 0; i < cityList.Count; i++)
        {
            if (!emptyCity[i].gameObject.activeInHierarchy)
            {
                return emptyCity[i];
            }
        }
        return null;
    }
}

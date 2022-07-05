using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerManager : MonoBehaviour
{
    public List<Transform> cityList;

    public Transform GetFromPool()
    {
        for (int i = 0; i < cityList.Count; i++)
        {
            if (!cityList[i].gameObject.activeInHierarchy)
            {
                return cityList[i];
            }
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wise.ScrollViewPooling
{
    public static class SPHelper
    {
        public static void GetScrollViewObject<T>(ScrollViewPooling scrollViewPooling, T[][] targetArr, int i) where T : class
        {
            var pooledObject = scrollViewPooling.GetGameObjects(i);
            var objectLength = pooledObject.Length;

            targetArr[i] = new T[objectLength];
            for (int j = 0; j < objectLength; j++)
            {
                targetArr[i][j] = pooledObject[j].GetComponent<T>();
            }
        }
    }
}
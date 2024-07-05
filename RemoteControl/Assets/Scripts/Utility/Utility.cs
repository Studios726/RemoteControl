using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class Utility
    {
        // Start is called before the first frame update
        public static T FindComponent<T>(this Transform transform, string name) where T : MonoBehaviour
        {
            return transform.Find(name)?.GetComponent<T>();
        }
    }

}

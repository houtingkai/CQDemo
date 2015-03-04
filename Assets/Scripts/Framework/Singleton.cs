using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Singleton<T> : UnityEngine.Object where T : UnityEngine.Object
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) +
                       " is needed in the scene, but there is none.");
                }
            }

            return instance;
        }
    }

}


public class SingletonBehaviour<T> : UnityEngine.MonoBehaviour where T : UnityEngine.MonoBehaviour
{
    static T instance;

    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                /*
                if (instance == null)
                {

                    Debug.LogError("An instance of " + typeof(T) +
                       " is needed in the scene, but there is none.");
                }*/
            }
  
            return instance; 
        }
    }

}


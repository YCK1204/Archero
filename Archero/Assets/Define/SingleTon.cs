using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> where T : class, new()
{

    protected static T instance;
    public static T GetInstance
    {
        get
        {
            if (instance == null) instance = new T();
            return instance;
        }
    }
    public virtual void Init()
    {
        //Reset�ؾ��ϴ°͵�(���� ������Ʈor�迭��) ����ٰ�
    }
}

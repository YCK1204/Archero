using UnityEngine;

public class BulletGroup : MonoBehaviour
{
    [Header("���� ����ü ����")]
    public int projectileCount = 1;         // �Ѿ� ��

    public void ApplyBulletUpgrade(int additionalCount)
    {
        projectileCount += additionalCount;
    }
}

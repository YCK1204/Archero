using UnityEngine;

public class BulletGroup : MonoBehaviour
{
    [Header("공통 투사체 설정")]
    public int projectileCount = 1;         // 총알 수

    public void ApplyBulletUpgrade(int additionalCount)
    {
        projectileCount += additionalCount;
    }
}

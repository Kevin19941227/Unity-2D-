    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Rifle : Gun
    {

    protected override void Fire()
    {
        //  播放音效（手動補上）
        if (audioSource != null && gunFireSound != null)
            audioSource.PlayOneShot(gunFireSound);

        //  播放動畫
        if (animator != null)
            animator.SetTrigger("Shoot");

        // 射線偵測
        RaycastHit2D hit2D = Physics2D.Raycast(muzzlePos.position, direction, 30);

        //  建立子彈與特效
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        LineRenderer tracer = bullet.GetComponent<LineRenderer>();
        tracer.SetPosition(0, muzzlePos.position);
        tracer.SetPosition(1, hit2D.point);

        //  建立彈殼
        GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;

        //  命中敵人處理
        if (hit2D.collider != null)
        {
            Debug.Log("Hit " + hit2D.collider.name);
            Enemy dog = hit2D.collider.GetComponent<Enemy>();
            if (dog != null)
            {
                dog.TakeDamage(10);
            }
        }
    }

}

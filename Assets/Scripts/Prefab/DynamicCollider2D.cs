using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DynamicCollider2D : MonoBehaviour
{
    PolygonCollider2D poly;
    SpriteRenderer sr;
    Sprite prevSprite;

    void Awake()
    {
        poly = GetComponent<PolygonCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        prevSprite = null;
    }

    void LateUpdate()
    {
        // 如果當前的 sprite 換了，才重新設定 collider
        if (sr.sprite != prevSprite)
        {
            UpdateColliderShape();
            prevSprite = sr.sprite;
        }
    }

    void UpdateColliderShape()
    {
        Sprite s = sr.sprite;
        int shapeCount = s.GetPhysicsShapeCount();
        poly.pathCount = shapeCount;

        // Unity API: 每個 shapePath 都是一組 Vector2
        for (int i = 0; i < shapeCount; i++)
        {
            List<Vector2> shape = new List<Vector2>();
            s.GetPhysicsShape(i, shape);
            poly.SetPath(i, shape.ToArray());
        }
    }
}

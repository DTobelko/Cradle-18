using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    private const float DOT_THRESHOLD = -0.5f;

    public static bool IsFacingTarget(this Transform transform, Transform target) // проверяем, чтобы камера смотрела на лицевую часть объекта, а не сзади
    {

        float dot = Vector3.Dot(transform.forward, target.forward);

        return dot <= DOT_THRESHOLD;
    }
}

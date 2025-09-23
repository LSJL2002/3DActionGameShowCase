using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    [field: SerializeField] public float DodgeStrength { get; private set; } = 6f;
}

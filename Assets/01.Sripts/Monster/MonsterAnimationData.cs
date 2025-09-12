using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterAnimationData
{
    public enum MonsterAnimationType
    {
        Idle,
        Walk,
        Run,
        Attack,
        BaseAttack,
        Death
    }

    [Serializable]
    public class MonsterAnimationEntry
    {
        public MonsterAnimationType type;
        public string parameterName;
    }

    [SerializeField] private List<MonsterAnimationEntry> animationEntries;

    private Dictionary<MonsterAnimationType, int> parameterHashes;

    public void Initialize()
    {
        parameterHashes = new Dictionary<MonsterAnimationType, int>();

        foreach (MonsterAnimationEntry entry in animationEntries)
        {
            if (!string.IsNullOrEmpty(entry.parameterName))
            {
                int hash = Animator.StringToHash(entry.parameterName);
                parameterHashes[entry.type] = hash;
            }
        }
    }

    public int GetHash(MonsterAnimationType type)
    {
        if (parameterHashes != null && parameterHashes.TryGetValue(type, out var hash))
            return hash;

        Debug.LogWarning($"Animation Hash not found for {type}");
        return -1;
    }
}

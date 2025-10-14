using UnityEngine;

public class PartyManager : Singleton<PartyManager>
{
    public static PartyManager Instance { get; private set; }

    [SerializeField] private PlayerManager[] partyMembers; // 3명 등록
    [SerializeField] private int currentIndex = 0;

    public PlayerManager ActiveCharacter => partyMembers[currentIndex];

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        for (int i = 0; i < partyMembers.Length; i++)
            partyMembers[i].gameObject.SetActive(i == currentIndex);

        ActiveCharacter.EnableInput(true);
        //CameraManager.Instance.FollowTarget(ActiveCharacter.transform);
    }

    public void SwapTo(int index)
    {
        if (index == currentIndex || index < 0 || index >= partyMembers.Length)
            return;

        var oldChar = ActiveCharacter;
        var newChar = partyMembers[index];

        // 상태 동기화 (위치, 속도 등)
        newChar.transform.position = oldChar.transform.position;

        // 활성 전환
        oldChar.EnableInput(false); 
        oldChar.gameObject.SetActive(false);

        newChar.gameObject.SetActive(true);
        newChar.EnableInput(true);

        //CameraManager.Instance.FollowTarget(newChar.transform);

        currentIndex = index;
    }
}

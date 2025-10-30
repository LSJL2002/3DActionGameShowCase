using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class InvenSecondSequence : MonoBehaviour
{
    [SerializeField] private UISceneManager manager;

    [Header("Zoom Positions")]
    [SerializeField] private Transform characterRoot; // 캐릭터 중심 (이걸 회전시킬 거임)
    [SerializeField] private float zoomOffsetX = -1f; // 줌 시 x축 이동량
    [SerializeField] private float moveSmooth = 5f;
    [SerializeField] private float verticalMoveRange = 0.5f; // 위아래 이동 허용 범위
    [SerializeField] private float rotateSpeed = 300f; // 캐릭터 회전 속도

    [Header("UI")]
    [SerializeField] private Button zoomToggleButton;
    [SerializeField] private Slider verticalSlider;

    private Transform camTransform;

    private bool isZoomed = false;
    private bool isDragging = false;
    private bool isActive = false; // charCam 활성화 여부

    private Vector3 defaultCamPos;
    private float baseY;
    private Vector3 currentTargetPos;

    void Start()
    {
        camTransform = manager.charCam.transform;
        defaultCamPos = camTransform.position;
        currentTargetPos = defaultCamPos;
        baseY = camTransform.position.y;

        if (zoomToggleButton != null) zoomToggleButton.onClick.AddListener(ToggleZoom);
        if (verticalSlider != null) verticalSlider.onValueChanged.AddListener(OnSliderValueChanged);
        manager.charEvent.CameraActivatedEvent.AddListener(OnCharCamBlendFinished2);
        manager.charEvent.CameraDeactivatedEvent.AddListener(OnCharCamDeactivated);
    }
    void OnDestroy()
    {
        // 구독 해제
        manager.charEvent.CameraActivatedEvent.RemoveListener(OnCharCamBlendFinished2);
        manager.charEvent.CameraDeactivatedEvent.RemoveListener(OnCharCamDeactivated);
    }

    void Update()
    {
        if (!isActive) return; // charCam 활성화 상태에서만 실행

        // --- 카메라 이동 ---
        camTransform.position = Vector3.Lerp(camTransform.position, currentTargetPos, Time.unscaledDeltaTime * moveSmooth);
        // --- 캐릭터 회전 ---
        if (Input.GetMouseButtonDown(0)) isDragging = true;
        if (Input.GetMouseButtonUp(0)) isDragging = false;
        if (isDragging)
        {
            float mouseX = Input.GetAxis("Mouse X");
            characterRoot.Rotate(Vector3.up, -mouseX * rotateSpeed * Time.unscaledDeltaTime, Space.World);
        }
    }

    private void ToggleZoom()
    {
        isZoomed = !isZoomed;
        verticalSlider.gameObject.SetActive(isZoomed);

        if (isZoomed)
        {
            Vector3 zoomPos = defaultCamPos;
            zoomPos.x += zoomOffsetX;
            currentTargetPos = zoomPos;

            if (verticalSlider != null)
                verticalSlider.value = 0.5f;
        }
        else
        {
            currentTargetPos = defaultCamPos; // 원래 위치로 복귀
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (!isZoomed) return;

        float offset = (value - 0.7f) * 2f * verticalMoveRange;
        Vector3 newPos = currentTargetPos;
        newPos.y = baseY + offset;
        currentTargetPos = newPos;
    }

    private void OnCharCamBlendFinished2(ICinemachineMixer mixer, ICinemachineCamera cam)
    {
        if (cam == manager.charCam)
        {
            isActive = true;
        }
    }

    private void OnCharCamDeactivated(ICinemachineMixer mixer, ICinemachineCamera cam)
    {
        if (cam == manager.charCam)
        {
            // 캐릭터 회전 초기화
            characterRoot.localRotation = Quaternion.Euler(0f, 90f, 0f);
            isDragging = false;

            // 줌 초기화
            if (isZoomed)
            {
                isZoomed = false;
                currentTargetPos = defaultCamPos;
                if (verticalSlider != null)
                {
                    verticalSlider.value = 0.5f;
                    verticalSlider.gameObject.SetActive(false);
                }
            }

            // UI 숨기기
            manager.charUI.SetActive(false);
            isActive = false;
            manager.seqCam1.enabled = true;
            manager.charCam.enabled = false;
        }
    }
}
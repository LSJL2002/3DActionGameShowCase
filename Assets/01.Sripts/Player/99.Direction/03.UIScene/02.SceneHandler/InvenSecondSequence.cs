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

    private CinemachineCamera cam;
    private Transform camTransform;

    private bool isZoomed = false;
    private bool isDragging = false;

    private Vector3 defaultCamPos;
    private float baseY;
    private Vector3 currentTargetPos;

    void Start()
    {
        cam = manager.CharCam;
        camTransform = cam.transform;

        defaultCamPos = camTransform.position;
        currentTargetPos = defaultCamPos;
        baseY = camTransform.position.y;

        if (zoomToggleButton != null) zoomToggleButton.onClick.AddListener(ToggleZoom);
        if (verticalSlider != null) verticalSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Update()
    {
        // --- VCam 라이브 체크 ---
        bool isLive = CinemachineCore.IsLive(cam);
        bool isBlending = manager.brain.IsBlending;

        if (isLive && !isBlending)
        {
            // --- 카메라 이동 ---
            camTransform.position = Vector3.Lerp(camTransform.position, currentTargetPos, Time.deltaTime * moveSmooth);
            // --- 캐릭터 회전 ---
            if (Input.GetMouseButtonDown(0)) isDragging = true;
            if (Input.GetMouseButtonUp(0)) isDragging = false;
            if (isDragging)
            {
                float mouseX = Input.GetAxis("Mouse X");
                characterRoot.Rotate(Vector3.up, -mouseX * rotateSpeed * Time.deltaTime, Space.World);
            }

            manager.ShowCharUI();
        }
        else
        {
            // --- CharUI 숨김 시 캐릭터 회전 초기화 ---
            characterRoot.localRotation = Quaternion.Euler(0f, 90f, 0f); // 초기 Y축 90도로 고정
            isDragging = false; // 드래그 해제

            // --- 줌 상태 초기화 ---
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

            manager.HideCharUI();
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
}
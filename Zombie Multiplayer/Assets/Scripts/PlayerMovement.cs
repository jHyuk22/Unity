using Photon.Pun;
using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviourPun {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    //public float rotateSpeed = 180f; // 좌우 회전 속도

    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 로컬 플레이어만 직접 위치와 회전을 변경 가능
        if (!photonView.IsMine)
        {
            return;
        }

        // 회전 실행
        Rotate();
        // 움직임 실행
        Move();

        // 입력값에 따라 애니메이터의 Move 파라미터 값을 변경
        playerAnimator.SetFloat("Move", Mathf.Abs(playerInput.move) + Mathf.Abs(playerInput.rotate));
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move() {
        //Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        // Vector3 strafeDistance = playerInput.rotate * transform.right * moveSpeed * Time.deltaTime;
        //playerRigidbody.MovePosition(playerRigidbody.position + moveDistance + strafeDistance);
        Vector3 moveDirection =
        (playerInput.move * Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)) +
        playerInput.rotate * Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1))).normalized;

        Vector3 moveDistance = moveDirection * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate() {
        //float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;

        //playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;

        if (GroupPlane.Raycast(cameraRay, out rayLength))

        {

            Vector3 pointTolook = cameraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));

        }
    }
}

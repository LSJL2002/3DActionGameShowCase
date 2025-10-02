## Project 8
<img width="2368" height="1328" alt="image" src="https://github.com/user-attachments/assets/17ce600e-2fc3-402e-b8b4-5afcf39c67d1" />

## 목차
- [게임 소개](#게임-소개)
- [주요 기능](#주요-기능)
- [설치 및 실행 방법](#설치-및-실행-방법)
- [조작법](#조작법)
- [프로젝트 구조](#프로젝트-구조)
- [알려진 문제](#알려진-문제)
- [향후 계획](#향후-계획)
- [라이선스](#라이선스)

---

## 게임 소개
이 프로젝트는 3D 캐리커처 액션 로그라이크 게임으로, 블레이드 앤 소울, 젠레스 존 제로와 같은 작품들에서 영감을 받았습니다.
게임의 핵심은 높은 난이도를 통한 긴장감과 몰입감입니다. 플레이어는 반복되는 도전 속에서 성장과 성취를 경험하며, 치밀한 전투와 전략적인 선택을 통해 짜릿한 액션을 즐길 수 있습니다.

## 주요 기능
- 고난이도 전투 시스템: 적의 공격 패턴을 분석하고, 회피와 반격을 통해 긴장감 넘치는 전투를 경험할 수 있습니다.
- 로그라이크 진행 방식: 캐릭터가 쓰러지면 처음부터 다시 시작하지만, 일부 성장 요소는 유지되어 점차 강해지는 재미를 줍니다.
- 캐리커처 아트 스타일: 독특한 3D 캐릭터 디자인과 개성 있는 비주얼로 차별화된 분위기를 연출합니다.
- 보스 전투: 강력한 보스들이 각 층의 끝에서 플레이어를 기다리며, 특별한 패턴과 도전 과제를 제공합니다.

## 설치 및 실행 방법


## 조작법
이동: W / A / S / D
회피: 스페이스 바
공격: 마우스 좌클릭
강공격 / 스킬: 마우스 우클릭
상호작용: E
인벤토리 열기: Tab
메뉴 열기 / 일시정지: Esc

## 프로젝트 구조
Assets/ 
├── 00.Scenes # 게임 씬 파일 모음 
├── 01.Sripts # 게임 스크립트 (C# 코드) 
├── 02.Prefabs # 프리팹 오브젝트 
├── 03.Animations # 애니메이션 클립 및 컨트롤러 
├── 04.Particlas # 파티클 효과
├── 05.Materials # 머티리얼 리소스 
├── 06.ScriptableObjects # 스크립터블 오브젝트 데이터 
├── 07.System # 시스템 관련 스크립트 및 매니저 
├── 08.AddressableAssetsData # Addressables 관련 데이터 
├── 09.Plugins # 외부 플러그인 
├── 10.Resources # 리소스 폴더 (런타임 로딩용) 
├── 10.TimeLines # Timeline 관련 애셋 
├── 99.Externals # 외부 리소스 
├── 99.MAvatar # 아바타 관련 리소스 
├── AddressableAssetsData # Addressables 기본 데이터 
├── Editor # 에디터 확장 스크립트 
├── JMO Assets # 외부 에셋 (JMO 패키지) 
├── MA_ToolBox # 툴박스 관련 에셋 
├── Resources # 추가 리소스 폴더 
├── StreamingAssets # 스트리밍 에셋 (런타임 로딩) 
├── TextMesh Pro # TextMeshPro 관련 리소스 
└── Unity-Logs-Viewer # 로그 뷰어 관련 리소스

## 알려진 문제
- 일부 버그 존재
- 몬스터 패턴 결정 로직이 정제되지 않음
- 밸런스 문제
- 씬 전환 시 이슈 발생
- 힐링 포션 사용 시 데미지 이펙트가 출력되는 문제
- 전투 구역(Battlezone)이 게임 리셋 시 초기화되지 않음 (이전 게임의 참조 문제 추적 중)

## 향후 계획
- Addressable 서버 구축
- 새로운 몬스터 추가
- 로그라이크 요소 확장
- 기타 콘텐츠 및 시스템 개선

## 라이선스
이 프로젝트는 개인/팀 학습 및 포트폴리오 목적으로 제작되었습니다.




## 프로토타입 개발정보 확인 (중간발표 PPT 자료)

https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Ffile.notion.so%2Ff%2Ff%2F83c75a39-3aba-4ba4-a792-7aefe4b07895%2F6b303b5a-b267-4b2e-8108-7ba31a9d4b79%2F3D%25EC%2595%25A1%25EC%2585%2598%25EA%25B2%258C%25EC%259E%2584_%25EB%25B0%259C%25ED%2591%259C%25EC%259E%2590%25EB%25A3%258C.pptx%3Ftable%3Dblock%26id%3D27f2dc3e-f514-802e-8cd1-e9b2ac09bf25%26spaceId%3D83c75a39-3aba-4ba4-a792-7aefe4b07895%26expirationTimestamp%3D1759392000000%26signature%3D80TSKrz9BRdXU9d2uL_3nxqUhNboN4E9LOHDkEd4zTA%26downloadName%3D3D%25EC%2595%25A1%25EC%2585%2598%25EA%25B2%258C%25EC%259E%2584%2B%25EB%25B0%259C%25ED%2591%259C%25EC%259E%2590%25EB%25A3%258C.pptx&wdOrigin=BROWSELINK

## WorkFlow
<details><summary>접기/펼치기</summary>
    
워크플로우
1. 이슈 작성하기
<img width="2848" height="1192" alt="image" src="https://github.com/user-attachments/assets/80763439-c5ad-4e94-900d-08f25433bfa7" />
<img width="798" height="570" alt="image" src="https://github.com/user-attachments/assets/f3ec2d77-55d6-4387-b901-f9e4021e829e" />



2. 프로젝트 입력하기
<img width="1626" height="726" alt="image" src="https://github.com/user-attachments/assets/f00d21df-d7b0-4a49-b3c6-db32f6036b7f" />
- Add Item
<img width="596" height="414" alt="image" src="https://github.com/user-attachments/assets/fbe45e8c-2404-4a1a-a03d-0156fba3988a" />

<img width="685" height="273" alt="image" src="https://github.com/user-attachments/assets/0a3c640c-9960-4ac3-b1bb-2d87ed1ea609" />
<img width="1346" height="453" alt="image" src="https://github.com/user-attachments/assets/20555ccd-e817-4fa3-8fb3-267f29f3579d" />

StartDate DeadLine입력하기

3. 이슈 번호 확인하기
<img width="541" height="86" alt="image" src="https://github.com/user-attachments/assets/cca97813-0f63-437c-a673-a79fd2b13be6" />

4. Branch 생성
<img width="402" height="385" alt="image" src="https://github.com/user-attachments/assets/d90d2a53-814e-4842-8c62-5ed6cdf2d482" />

- 종류가 feat 이슈번호가 40이었으면 feat#40으로 생성

5. 작업 종료 후 Commit
<img width="1221" height="687" alt="image" src="https://github.com/user-attachments/assets/e3537fcf-9096-45e8-883c-a44452c6ef19" />
<img width="697" height="136" alt="image" src="https://github.com/user-attachments/assets/4587f39d-db6a-49e1-ae4b-862b7b0d26bb" />

- 웹에서 내용 복사 후 수정사항이 있으면 수정하여 Commit내용 작성
   
6. 승인 단계를 건너뛰기 때문에 로컬에서 직접 Dev로 Merge 후 웹으로 Push

7. 포로젝트에서 state를 Done으로 수정, End Date 입력

8. 매일 7시 30분 코드리뷰
</details>

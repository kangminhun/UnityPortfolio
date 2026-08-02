# 어린이 영어학원 학습 콘텐츠 게임 (Unity / C#)

영어로 영어 · 과학 · 수학을 함께 배우는 어린이 대상 학원을 위해 개발한
Unity 기반 학습 콘텐츠 애플리케이션의 클라이언트 스크립트입니다.

챕터별 미니게임과 상호작용 영상으로 아이들의 개념 이해를 돕고, 학습 성과에 따라
지급되는 포인트로 카드를 구매할 수 있는 보상 시스템을 통해 아이들이 스스로 즐겁게,
꾸준히 학습에 참여하도록 설계했습니다.

> 이 저장소는 실제 서비스에 사용된 C# 스크립트 97개를 정리해 올린 것으로,
> 학습 목적 및 포트폴리오 열람용입니다. Unity 프로젝트 전체(씬, 에셋, 프리팹 등)는
> 포함하지 않았습니다. 서버 주소 등 민감 정보는 `your-server-domain.com`으로 치환했습니다.

## 주요 기능

- **로그인 / 회원 관리** — 일반회원·관리자 로그인, 회원가입, 아이디/비밀번호 찾기
- **포인트 · 카드 보상 시스템** — 학습 성과에 따른 포인트 지급, 포인트 상점, 카드 구매/보유 관리
- **카드 배틀 미니게임** — 보유 카드를 활용한 턴제 전투 (공격/방어, HP, 라운드 진행)
- **챕터별 학습 미니게임** — 단어·이미지 매칭, 문장 완성, 조각 퍼즐, 복습 퀴즈 등 15종 이상
- **학습 영상 시스템** — 영상 다운로드/재생, 구간 반복, 진행률 표시
- **랭킹 / 우편함 / 출석 체크**
- **관리자 콘솔** — 명령어 기반 공지/카드 등록, 포인트 지급

## 기술 스택

- Unity Engine / C#
- UnityWebRequest 기반 REST API 통신 (JWT Bearer Token 인증)
- Coroutine 기반 비동기 처리
- ScriptableObject 기반 데이터 설계
- TextMeshPro, Unity Video Player

## 폴더 구조

```
Scripts/
├── Server/         # 서버 통신 · 로그인 · 결제 · 랭킹 · 관리자 콘솔 (14개)
├── Manager/         # 각 미니게임 진행 상태 · 데이터 총괄 매니저 (11개)
├── Movement/         # 오브젝트 이동 · 애니메이션 연출 유틸리티 (8개)
├── OneCard/          # 원카드 미니게임 로직 및 데이터
├── Infomation/       # 게임별 정답/문제 등 ScriptableObject 데이터 정의
└── (루트)             # 챕터별 미니게임, UI 내비게이션, 타이핑/보카 학습, 영상·오디오 등
```

## 아키텍처 개요

- `DataBase.cs`가 싱글톤 서비스 로케이터 역할을 하며, 각 매니저(로그인/결제/우편함/랭킹 등)에
  대한 접근을 중계합니다.
- `WebRequestManager.cs`가 모든 REST API 요청(로그인, 회원가입, 카드 구매, Zoom 연동 등)을
  전담하고, 각 기능별 매니저가 UI와 서버 응답을 연결합니다.
- `Uichage.cs`가 여러 화면(패널)의 오픈/클로즈와 전환 사운드를 제어하는 UI 허브 역할을 합니다.

## 데모 영상

- https://youtu.be/rFp6rwlDrgs

## 스크린샷 각 종 미니게임 중 일부 사진

- <img width="854" height="481" alt="플레이 사진 5" src="https://github.com/user-attachments/assets/00cf4ac1-dcb1-4d25-bf2c-37a9701029a7" />
<img width="857" height="485" alt="플레이 사진 4" src="https://github.com/user-attachments/assets/e706a566-0830-4e5b-b211-e3e626a3eac2" />
<img width="853" height="483" alt="플레이 사진 3" src="https://github.com/user-attachments/assets/402b8435-8298-4d9c-8193-2d9f9c1d680e" />
<img width="856" height="480" alt="플레이 사진 2" src="https://github.com/user-attachments/assets/c2b16e5f-8b6e-4fda-9c7a-d4adfce47860" />
<img width="859" height="480" alt="플레이 사진 1" src="https://github.com/user-attachments/assets/ac7c8d62-7d1b-46b9-873a-225b3d3ec033" />

## 개발 기간
- 약 6개월 가량

## 담당 역할
- 기획 참여부터 클라이언트/서버 통신, 게임 로직 전체 개발

# ProjectVS 팀 프로젝트 

Unity Version: 2022.3.61f1

## 컨벤션

### 커밋 탬플릿
| 타입 (type) | 설명 |
|-------------|------|
| `feat`      | 새로운 기능 추가, 기존의 기능을 요구 사항에 맞추어 수정 |
| `fix`       | 기능에 대한 버그 수정 |
| `set`       | 단순 파일 추가 |
| `build`     | 빌드 관련 수정 |
| `chore`     | 그 외 기타 수정 |
| `ci`        | CI 관련 설정 수정 |
| `docs`      | 문서(주석) 수정 |
| `style`     | 코드 스타일, 포맷팅에 대한 수정 |
| `refactor`  | 기능의 변화가 아닌 코드 리팩터링 |
| `test`      | 테스트 코드 추가/수정 |
| `release`   | 버전 릴리즈 |

### 브랜치 규칙
1. `main` 브랜치 - Dev 브랜치에서 하루량의 커밋된 것들이 충돌 되지 않는 것들을 매지 하여 관리하는 브랜치치
2. `dev` 브랜치 - 오전, 오후 작업한 것들을 머지하여 관리하는 브랜치치
3. `담당자(이니셜)/기능 명 등` - 개발 중인 기능을 따로 브랜치로 관리한다. (담당중인 기능 이름/담당자(개발자 본인 영어 앞글자 스팰링만))

## 코드 컨벤션
프로젝트는 아래의 코드 컨벤션을 따라 작성됩니다. 
https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/coding-style/identifier-names
https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/coding-style/coding-conventions
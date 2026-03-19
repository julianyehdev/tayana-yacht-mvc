# Git 規範

## Branch 命名

格式：`<category>-<branch-name>`

| Category | 用途 |
|----------|------|
| `feature` | 新增功能 |
| `update` | 更新、優化 |
| `fix` | 修復 Bug |
| `hotfix` | 修復重大 Bug |

範例：
```
feature-message-board
feature-sign-up-api
update-user-profile-fields
update-filter-logic
fix-api-response
hotfix-login-error
```

---

## Commit 命名

格式：`<type>: <verb> <subject>`

| Type | 用途 |
|------|------|
| `feat` | 新增功能 |
| `update` | 修改既有項目或功能 |
| `fix` / `resolve` | 修復 Bug |
| `style` | 格式、風格（不影響程式碼運行的變動） |
| `perf` | 改善效能 |
| `chore` | 建構程序或輔助工具的變動 |
| `refactor` | 重構 |

範例：
```
feat: add delete logic
update: adjust user data
update: adjust for loop logic
perf: optimize for loop logic
fix: resolve field mapping error
style: format code writing
refactor: adjust filter logic
```

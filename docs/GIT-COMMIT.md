# Git 規範

## Branch 命名

格式：`<category>/<branch-name>`

| Category | 用途 |
|----------|------|
| `feature` | 新增功能 |
| `update` | 更新、優化 |
| `fix` | 修復 Bug |
| `hotfix` | 修復重大 Bug |

範例：
```
feature/message-board
feature/sign-up-api
update/user-profile-fields
update/filter-logic
fix/api-response
hotfix/login-error
```

---

## Commit 命名

格式：`<type>(<scope>): <繁體中文簡短描述>`

| Type | 用途 |
|------|------|
| `feat` | 新增功能、元件、頁面 |
| `update` | 修改既有項目或功能 |
| `fix` | 修復 Bug |
| `style` | 格式、風格（不影響程式碼運行的變動） |
| `perf` | 改善效能 |
| `refactor` | 重構（不改變行為） |
| `chore` | 建構程序或輔助工具的變動 |
| `docs` | 文件更新 |
| `test` | 測試相關 |

**subject 規則：**
- 使用繁體中文
- 不超過 50 字
- 不以句號結尾
- 動詞開頭：新增、調整、修正、移除、重構

範例：
```
feat(product): 新增商品列表頁面
update(member): 調整會員資料欄位
fix(auth): 修正登入錯誤問題
style(layout): 調整首頁排版格式
perf(query): 優化商品查詢邏輯
refactor(filter): 重構篩選條件邏輯
chore(project): 更新 NuGet 套件設定
docs(readme): 更新開發環境說明
test(product): 新增商品 CRUD 測試
```

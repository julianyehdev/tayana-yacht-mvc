# TayanaYachtMVC

[![.NET Framework](https://img.shields.io/badge/.NET-Framework%204.8-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL-Server-CC2927?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server/)
[![Entity Framework](https://img.shields.io/badge/ORM-EF%206%20Code%20First-512BD4)](https://docs.microsoft.com/ef/ef6/)
[![GCP](https://img.shields.io/badge/Cloud-Google%20Cloud-4285F4?logo=googlecloud)](https://cloud.google.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**大洋遊艇官網改版** — 單人開發的 ASP.NET MVC 5 全棧專案，包含前台展示、後台管理與 GCP 生產部署。展示了 EF Code First 資料建模、MVC Areas 架構分層、Anti-Forgery Token 安全加固、以及雲端上線的完整工程能力。

| | 連結 | 帳號 | 密碼 |
|--|------|------|------|
| 前台 | [前台網站](http://35.201.172.158/) | — | — |
| 後台 | [後台管理](http://35.201.172.158/Admin/Login) | `Admin1234` | `Admin1234` |

## 技術棧

| 層級 | 技術 |
|-----|------|
| 框架 | ASP.NET MVC 5 (.NET Framework 4.8) |
| ORM | Entity Framework 6 (Code First) |
| 資料庫 | SQL Server |
| 前端 | Bootstrap 5.2.3, jQuery 3.7.0 |
| 部署 | GCP Compute Engine + Windows Server + IIS |

## 核心功能

**後台管理**：遊艇、新聞、經銷商、地區、會員 CRUD  
**前台展示**：產品規格、新聞中心、經銷商查詢、客戶聯繫  
**自動郵件**：聯繫表單寄信給管理員 & HTML 回信給客戶

### 自動郵件流程
![SMTP 郵件系統展示](docs/img/smtp-demo.gif)

## 架構亮點

| 設計 | 說明 |
|-----|------|
| **MVC Areas** | 前台 & 後台路由完全分離。業務邏輯清晰隔離，降低耦合風險。 |
| **EF Code First + Migration** | 資料模型即程式碼，版本完整追蹤。從本地到生產，schema 一致性有保證，部署風險小。 |
| **Anti-Forgery Token** | 登入頁禁快取、CSRF Token 驗證、Session + Forms Auth 雙層保護，防止常見 Web 攻擊。 |
| **自訂 AdminAuthorizeAttribute** | 繼承 AuthorizeAttribute 實作後台專屬驗證邏輯，SHA256 密碼加密，存取控制集中管理。 |
| **SMTP 郵件系統** | 聯繫表單觸發自動寄信，HTML 模板分別通知管理員與回信客戶，提升業務回應效率。 |

## 快速開始

**環境**：Visual Studio 2019、.NET Framework 4.8、SQL Server  

```bash
# 1. 複製 & 還原套件
git clone https://github.com/julianyehdev/tayana-yacht-mvc.git
cd TayanaYachtMVC
# NuGet Package Manager Console: Update-Package -Reinstall

# 2. 資料庫連線 (Web.config)
<add name="TayanaDB" 
     connectionString="Server=YOUR_SERVER;Database=TayanaDB;Integrated Security=true;" />

# 3. 建立資料庫
# Package Manager Console: Update-Database

# 4. 啟動
# Visual Studio: F5 → IIS Express
```

**後台登入**：執行下方 SQL 新增管理員帳號密碼，瀏覽 `/Admin/Login`
> 預設帳號：`admin` ／ 預設密碼：`admin`
```sql
INSERT INTO AdminUsers (Username, PasswordHash, DisplayName, IsActive, CreatedAt, UpdatedAt)
VALUES ('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Administrator', 1, GETDATE(), GETDATE())
-- 預設密碼：admin（SHA256 加密）
```

## 詳細文件

- **[部署指南](docs/DEPLOYMENT.md)** — GCP Compute Engine + Windows Server + IIS 上線步驟
- **[資料模型](docs/DATABASE.md)** — EF Entity、Migration、schema 設計
- **[開發指南](docs/DEVELOPMENT.md)** — Commit 規範、功能流程、FAQ、後續計畫

## License

MIT © 2024-2025 Julian Ye

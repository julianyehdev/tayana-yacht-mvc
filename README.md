# Tayana Yacht MVC

大洋遊艇（Tayana Yacht）官網改版專案，採用 ASP.NET MVC 5 架構開發。

## 專案概述

本專案為大洋遊艇官網的全面改版，包含前台展示網站與後台管理系統。前台提供遊艇產品展示、新聞資訊、經銷商查詢及客戶聯繫等功能；後台提供完整的 CRUD 管理介面，支援遊艇、新聞、經銷商、地區及使用者管理。

## 技術棧

| 層次 | 技術 |
|------|------|
| 框架 | ASP.NET MVC 5 (.NET Framework 4.8) |
| ORM | Entity Framework 6 (Code First) |
| 資料庫 | SQL Server |
| 前端 | Bootstrap 5.2.3, jQuery 3.7.0 |
| 套件管理 | NuGet |
| 認證 | Forms Authentication + Session |

## 核心功能

### 🌐 前台功能
- **遊艇展示**：產品詳細規格、尺寸、技術指標及相片展示
- **新聞中心**：分類新聞文章、附件下載
- **經銷商查詢**：全球經銷商位置搜索、分頁查詢
- **聯繫表單**：客戶查詢表單，支援雙向郵件通知
  - 自動寄信給管理員
  - 自動回信給客戶（HTML 格式）
  - SMTP 配置及錯誤處理

### 🔐 後台管理系統
- **認證系統**
  - SHA256 密碼加密
  - Session + FormsAuthentication Cookie 雙重管理
  - 自訂 AdminAuthorizeAttribute Filter 權限控制
  
- **模組管理**
  - Dashboard 儀表板
  - Yachts（遊艇 CRUD）
  - News Articles（新聞文章 CRUD）
  - Dealers（經銷商 CRUD + 分頁）
  - Regions（地區 CRUD）
  - Login（登入/登出）

## 資料夾結構

```
TayanaYachtMVC/
├── Areas/Admin/
│   ├── Controllers/          # 後台控制器
│   ├── Models/               # 後台 ViewModel
│   └── Views/                # 後台視圖
├── Controllers/              # 前台控制器
├── Data/
│   └── TayanaYachtDBContext.cs
├── Models/
│   ├── Domain/               # EF 實體類
│   │   ├── Yacht.cs
│   │   ├── NewsArticle.cs
│   │   ├── Dealer.cs
│   │   ├── AdminUser.cs
│   │   ├── Country.cs
│   │   └── ...
│   └── ViewModels/           # 前台 ViewModel
│       ├── ProductViewModel.cs
│       ├── ContactViewModel.cs
│       └── ...
├── Views/                    # 前台視圖
├── Filters/
│   └── AdminAuthorizeAttribute.cs
├── Helpers/
│   └── HtmlHelpers.cs
├── Content/                  # CSS、圖片
├── Scripts/                  # JavaScript
├── Migrations/               # EF Code First Migrations
└── App_Start/                # 應用程式啟動配置
```

## 安裝與設置

### 前置需求
- Visual Studio 2019 或更新版本
- SQL Server 2019 或 LocalDB
- .NET Framework 4.8
- NuGet Package Manager

### 安裝步驟

1. **複製專案**
   ```bash
   git clone <repository-url>
   cd TayanaYachtMVC
   ```

2. **安裝依賴套件**
   ```
   NuGet Package Manager Console > Update-Package
   ```

3. **配置資料庫連線**
   編輯 `Web.config`：
   ```xml
   <connectionStrings>
       <add name="TayanaDB" 
            connectionString="Server=YOUR_SERVER;Database=TayanaDB;Integrated Security=true;" 
            providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

4. **執行 Entity Framework Migration**
   ```
   Package Manager Console > Update-Database
   ```

5. **配置 SMTP（用於聯繫表單郵件）**
   編輯 `Web.config` 的 `appSettings`：
   ```xml
   <add key="SmtpHost" value="your-smtp-host" />
   <add key="SmtpPort" value="587" />
   <add key="SmtpUser" value="your-email@example.com" />
   <add key="SmtpPass" value="your-password" />
   <add key="ToEmail" value="admin@tayanaYacht.com" />
   ```

6. **啟動應用程式**
   - 在 Visual Studio 中按 F5 或 Ctrl+F5 啟動

## 使用指南

### 前台訪問
- 主頁：`http://localhost:port/`
- 遊艇展示：`http://localhost:port/Yacht`
- 新聞中心：`http://localhost:port/News`
- 經銷商：`http://localhost:port/Dealer`
- 聯繫我們：`http://localhost:port/Contact`

### 後台訪問
- 登入：`http://localhost:port/Admin/Login`
- Dashboard：`http://localhost:port/Admin/Dashboard`
- 需要先在資料庫新增 AdminUser 記錄

### 後台初始化（首次登入）
需在 `AdminUser` 表新增管理員帳號：
```sql
INSERT INTO AdminUsers (Username, PasswordHash, DisplayName, IsActive, CreatedAt, UpdatedAt)
VALUES ('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Administrator', 1, GETDATE(), GETDATE())
-- 預設密碼：123456（SHA256）
```

## 開發指南

### 命名慣例
- Controller：`PascalCase` + `Controller` 後綴
- Model：`PascalCase`
- ViewModel：`PascalCase` + `ViewModel` 後綴
- View：對應 Controller Action 名稱

### Commit 規範
遵循 Conventional Commits 格式：
- `feat(module): 功能描述`
- `fix(module): 修復說明`
- `chore: 雜務更新`
- `style: 樣式修改`

### 新增功能流程
1. 在 `Models/Domain/` 新增實體類
2. 在 `DbContext` 新增 DbSet
3. 建立 Migration：`Add-Migration FeatureName`
4. 更新資料庫：`Update-Database`
5. 建立 Controller 和 View
6. 測試並 commit

## 常見問題

**Q: 如何新增新的管理員帳號？**
A: 在 AdminUser 表新增記錄，PasswordHash 需要使用 SHA256 加密。

**Q: 聯繫表單郵件無法發送？**
A: 檢查 Web.config 的 SMTP 配置是否正確，確認防火牆允許 SMTP 連接。

**Q: 如何更改後台登入密碼？**
A: 更新 AdminUser 表的 PasswordHash 欄位，使用 SHA256 加密新密碼。

## License

此專案為私人專案，未公開發行。

## 聯繫方式

- 官網：https://www.tayanaYacht.com
- 電話：+886(7)641 2422
- 郵箱：9ulianye@gmail.com

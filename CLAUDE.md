# TayanaYachtMVC — 專案指引

## 專案概述

大洋遊艇（Tayana Yacht）官網改版專案，ASP.NET MVC 5 + Entity Framework + SQL Server。

- **前台**：遊艇展示網站（老師提供切版 HTML，需整合進 MVC）
- **後台**：管理系統（商品、新聞、會員管理等 CRUD）
- **開發者**：單人開發

## 技術棧

| 層次 | 技術 |
|------|------|
| 框架 | ASP.NET MVC 5 (.NET Framework 4.8) |
| ORM | Entity Framework 6（Code First） |
| 資料庫 | SQL Server |
| 前端 | Bootstrap 5.2.3, jQuery 3.7.0 |
| 套件管理 | NuGet |

## 架構規範

### 命名慣例
- Controller：`PascalCase` + `Controller` 後綴，e.g. `ProductController`
- Model：`PascalCase`，e.g. `Product`, `NewsArticle`
- ViewModel：`PascalCase` + `ViewModel` 後綴，e.g. `ProductViewModel`
- View：對應 Controller Action 名稱
- 後台 Controller 放在 `Areas/Admin/Controllers/`，Area 名稱為 `Admin`

### 資料夾結構（目標狀態）
```
TayanaYachtMVC/
├── Areas/
│   └── Admin/                  # 後台管理區域
│       ├── Controllers/        # 後台 Controller
│       ├── Models/             # 後台專用 ViewModel（只有後台用的才放這）
│       │   ├── ProductEditViewModel.cs
│       │   └── NewsEditViewModel.cs
│       └── Views/
├── Controllers/                # 前台 Controller
├── Data/                       # DbContext
│   └── TayanaDbContext.cs
├── Models/
│   ├── Domain/                 # EF 實體類（前後台共用）
│   │   ├── Product.cs
│   │   ├── NewsArticle.cs
│   │   └── Member.cs
│   └── ViewModels/             # 前台專用 ViewModel
│       └── ProductViewModel.cs
├── Views/                      # 前台 View
├── Content/                    # CSS、圖片
├── Scripts/                    # JS
├── Migrations/                 # EF Migration 檔案
└── App_Data/                   # DB 檔案（若用 LocalDB）
```

### ViewModel 放置原則
- **只有後台用** → `Areas/Admin/Models/`
- **只有前台用** → `Models/ViewModels/`
- **前後台共用** → `Models/ViewModels/`（少見，盡量避免）

### Entity Framework
- 使用 **Code First**
- DbContext 類名：`TayanaDbContext`，放在 `Data/` 資料夾
- Connection string 名稱：`TayanaDB`
- Migration 放在 `Migrations/` 資料夾
- Controller 直接注入 `TayanaDbContext`，不加 Service Layer（CRUD 邏輯不複雜）

### 前後台路由
- 前台：`/{controller}/{action}/{id}` (預設)
- 後台：`/Admin/{controller}/{action}/{id}` (Area)
- 後台需要登入驗證（`[Authorize]`）

## 開發順序建議

1. 建立 `Data/TayanaDbContext.cs` + `Models/Domain/` 基本實體
2. 建立後台 Admin Area + 實作各模組 CRUD（商品 → 新聞 → 會員）
3. 整合老師提供的前台切版 HTML → MVC Layout
4. 前台串接資料庫資料

## 專案重要文件

- [Commit 規範說明](docs/GIT-COMMIT.md)
- [開發風格說明](docs/GUIDELINE.md)

## 注意事項

- 這是 ASP.NET MVC 5，**不是** ASP.NET Core，語法和套件有差異
- Razor 語法用 `.cshtml`
- DI 不是內建的，若需要請用 Unity 或 Autofac
- 前台 HTML 由老師提供，整合時保持原始設計不要自行改動 CSS 結構

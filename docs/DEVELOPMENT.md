# 開發指南

## 命名慣例

遵循 ASP.NET MVC 5 標準與專案風格規範。

| 類型 | 命名 | 範例 |
|------|------|------|
| Controller | `PascalCase` + `Controller` | `YachtController`, `AdminNewsController` |
| Model (Entity) | `PascalCase` | `Yacht`, `NewsArticle`, `AdminUser` |
| ViewModel | `PascalCase` + `ViewModel` | `YachtViewModel`, `DealerListViewModel` |
| View 檔案 | 對應 Action | `Details.cshtml` (for `Details` action) |
| Method | `PascalCase` | `GetYachtsByRegion()`, `CreateAdminUser()` |
| Property | `PascalCase` | `Name`, `CreatedAt`, `IsActive` |
| Private Field | `_camelCase` | `_dbContext`, `_smtpService` |
| Constant | `UPPER_CASE` | `MAX_PAGE_SIZE`, `DEFAULT_TIMEOUT` |

## 資料夾結構

```
TayanaYachtMVC/
├── Areas/Admin/                      # 後台管理區域
│   ├── Controllers/
│   │   ├── DashboardController.cs
│   │   ├── YachtController.cs
│   │   ├── NewsController.cs
│   │   ├── DealerController.cs
│   │   ├── LoginController.cs
│   │   └── RegionController.cs
│   ├── Models/                       # 後台專用 ViewModel
│   │   ├── YachtEditViewModel.cs
│   │   ├── NewsEditViewModel.cs
│   │   └── DealerListViewModel.cs
│   └── Views/                        # Razor 檢視
│       ├── Dashboard/
│       ├── Yacht/
│       ├── News/
│       ├── Dealer/
│       ├── Login/
│       └── Shared/
├── Controllers/                      # 前台 Controller
│   ├── HomeController.cs
│   ├── YachtController.cs
│   ├── NewsController.cs
│   ├── DealerController.cs
│   └── ContactController.cs
├── Data/                             # ORM
│   ├── TayanaYachtDBContext.cs
│   └── Migrations/
├── Models/
│   ├── Domain/                       # EF 實體（前後台共用）
│   │   ├── Yacht.cs
│   │   ├── NewsArticle.cs
│   │   ├── Dealer.cs
│   │   ├── Region.cs
│   │   └── AdminUser.cs
│   └── ViewModels/                   # 前台 ViewModel
│       ├── YachtViewModel.cs
│       ├── ContactViewModel.cs
│       └── DealerSearchViewModel.cs
├── Views/                            # 前台 Razor
│   ├── Home/
│   ├── Yacht/
│   ├── News/
│   ├── Dealer/
│   ├── Contact/
│   └── Shared/
│       └── _Layout.cshtml
├── Filters/
│   └── AdminAuthorizeAttribute.cs    # 自訂授權篩選器
├── Helpers/
│   └── PasswordHasher.cs             # 密碼加密輔助
├── Content/                          # CSS、圖片
│   ├── css/
│   │   ├── bootstrap.min.css
│   │   └── site.css
│   └── images/
├── Scripts/                          # JavaScript
│   ├── jquery-3.7.0.min.js
│   └── site.js
├── App_Start/
│   ├── RouteConfig.cs
│   └── FilterConfig.cs
├── Web.config                        # 應用程式配置（含連線字串、SMTP）
├── Web.Release.config                # 發佈環境轉換
├── Global.asax
└── README.md
```

## 新增功能流程

### 1. 定義資料模型

在 `Models/Domain/` 建立新的 Entity 類：

```csharp
// Models/Domain/Review.cs
public class Review
{
    public int Id { get; set; }
    public int YachtId { get; set; }
    public string CustomerName { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }  // 1-5
    public DateTime CreatedAt { get; set; }

    public virtual Yacht Yacht { get; set; }
}
```

### 2. 更新 DbContext

在 `Data/TayanaYachtDBContext.cs` 新增 DbSet：

```csharp
public DbSet<Review> Reviews { get; set; }

// OnModelCreating 中定義關聯
modelBuilder.Entity<Review>()
    .HasRequired(r => r.Yacht)
    .WithMany(y => y.Reviews)
    .HasForeignKey(r => r.YachtId)
    .WillCascadeOnDelete(true);
```

### 3. 建立 Migration

```powershell
# Package Manager Console
Add-Migration AddReviewEntity
# 檢查自動生成的 Migration 檔案
Update-Database
```

### 4. 建立 ViewModel（如需要）

```csharp
// Models/ViewModels/ReviewViewModel.cs
public class ReviewViewModel
{
    public int Id { get; set; }
    public int YachtId { get; set; }
    public string CustomerName { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### 5. 建立 Controller

```csharp
// Controllers/ReviewController.cs
public class ReviewController : Controller
{
    private TayanaYachtDBContext db = new TayanaYachtDBContext();

    // GET: /Review
    public ActionResult Index(int yachtId)
    {
        var reviews = db.Reviews
            .Where(r => r.YachtId == yachtId)
            .OrderByDescending(r => r.CreatedAt)
            .ToList();
        return View(reviews);
    }

    // GET: /Review/Create
    public ActionResult Create(int yachtId)
    {
        ViewBag.YachtId = yachtId;
        return View();
    }

    // POST: /Review/Create
    [HttpPost]
    public ActionResult Create(ReviewViewModel model)
    {
        if (ModelState.IsValid)
        {
            var review = new Review
            {
                YachtId = model.YachtId,
                CustomerName = model.CustomerName,
                Content = model.Content,
                Rating = model.Rating,
                CreatedAt = DateTime.Now
            };
            db.Reviews.Add(review);
            db.SaveChanges();
            return RedirectToAction("Index", new { yachtId = model.YachtId });
        }
        return View(model);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) db.Dispose();
        base.Dispose(disposing);
    }
}
```

### 6. 建立 View

```html
<!-- Views/Review/Index.cshtml -->
@model IEnumerable<TayanaYachtMVC.Models.Domain.Review>

<div class="container">
    <h2>遊艇評論</h2>
    
    @foreach (var review in Model)
    {
        <div class="card mb-3">
            <div class="card-body">
                <h5>@review.CustomerName</h5>
                <p>評分：@String.Concat(Enumerable.Repeat("⭐", review.Rating))</p>
                <p>@review.Content</p>
                <small class="text-muted">@review.CreatedAt.ToString("yyyy-MM-dd HH:mm")</small>
            </div>
        </div>
    }
</div>
```

## Commit 規範

遵循 **Conventional Commits** 格式，便於自動化發版與變更日誌。

### 格式

```
<type>(<scope>): <subject>

<body>

<footer>
```

### 類型

| 類型 | 說明 | 範例 |
|------|------|------|
| `feat` | 新功能 | `feat(admin-yacht): 新增遊艇刪除功能` |
| `fix` | Bug 修復 | `fix(login): 修復 Anti-Forgery Token 驗證失敗` |
| `refactor` | 程式碼重構（無功能改變） | `refactor(dealer): 簡化分頁邏輯` |
| `style` | 程式碼風格調整（不影響功能） | `style: 統一縮排為 4 空格` |
| `chore` | 雜務更新（套件、配置等） | `chore: 升級 Bootstrap 至 5.3.0` |
| `docs` | 文件更新 | `docs: 更新 README 部署說明` |
| `test` | 測試相關 | `test: 新增 YachtController 單元測試` |

### 範例 Commit

```
feat(admin-news): 新增新聞批次刪除功能

- 後台 News 管理頁面加入 checkbox
- 實作 Delete (multiple IDs) Action
- 前端確認對話框

Closes #42
```

### 好的 Commit Message

✅ 清楚說明「做了什麼」與「為什麼」  
✅ 使用英文 scope，中文說明  
✅ 第一行保持在 50 字以內  
✅ Body 說明實作細節（可選）

### 不好的 Commit Message

❌ 太籠統：`update code`, `fix bug`  
❌ 無關：`wip`, `temp`  
❌ 不清楚：`asdfgh`, `lorem ipsum`

## FAQ

### Q: 如何重置後台密碼？

```csharp
// 使用 PasswordHasher 產生 SHA256 hash
using System.Security.Cryptography;
using System.Text;

public static string HashPassword(string password)
{
    using (var sha256 = SHA256.Create())
    {
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}

// 產生密碼 hash：HashPassword("123456")
// → 8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918

// 更新資料庫
UPDATE AdminUsers SET PasswordHash = '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918'
WHERE Username = 'admin';
```

### Q: 聯繫表單郵件無法發送？

**檢查清單：**

1. **Web.config SMTP 配置**
   ```xml
   <add key="SmtpHost" value="smtp.gmail.com" />
   <add key="SmtpPort" value="587" />
   <add key="SmtpUser" value="your-email@gmail.com" />
   <add key="SmtpPass" value="your-app-password" />  <!-- 不是常用密碼 -->
   <add key="SmtpEnableSsl" value="true" />
   ```

2. **防火牆允許 SMTP 連接**
   ```powershell
   # 測試連線
   Test-NetConnection -ComputerName smtp.gmail.com -Port 587
   ```

3. **郵件服務程式碼**
   ```csharp
   public void SendEmail(string toEmail, string subject, string body)
   {
       try
       {
           using (var client = new SmtpClient(SmtpHost, SmtpPort))
           {
               client.EnableSsl = true;
               client.Credentials = new NetworkCredential(SmtpUser, SmtpPass);
               client.Send(FromEmail, toEmail, subject, body);
           }
       }
       catch (Exception ex)
       {
           // 記錄錯誤到 Event Log
           System.Diagnostics.EventLog.WriteEntry("TayanaYachtMVC", ex.Message);
       }
   }
   ```

### Q: 如何新增一個新的後台 CRUD 模組？

參考上方「新增功能流程」章節，按順序執行 6 個步驟即可。

### Q: 如何在多個開發者間同步資料庫變更？

1. **開發者 A** 新增功能 → `Add-Migration` → Commit
2. **開發者 B** Pull 最新代碼 → `Update-Database` 自動套用 Migration
3. 無須手動執行 SQL，EF 自動處理

### Q: 本地開發時可以用 LocalDB 嗎？

可以，修改 `Web.config`：

```xml
<connectionStrings>
    <add name="TayanaDB"
         connectionString="Data Source=(localdb)\mssqllocaldb;Initial Catalog=TayanaDB;Integrated Security=true;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

LocalDB 是 SQL Server 輕量版，適合本地開發。

## 後續改進計畫

- [ ] **升級至 ASP.NET Core**（降低維護成本，性能提升）
- [ ] **API 層分離**（RESTful API 支援行動端）
- [ ] **非同步 I/O 優化**（async/await 改寫 Controller/Service）
- [ ] **快取機制**（Redis 或 In-Memory Cache）
- [ ] **單元測試與整合測試**（NUnit + Moq）
- [ ] **CI/CD 自動化**（GitHub Actions 或 Azure Pipelines）
- [ ] **容器化部署**（Docker + Kubernetes）

## 參考資源

- [ASP.NET MVC 5 官方文件](https://docs.microsoft.com/aspnet/mvc/)
- [Entity Framework 6 Code First](https://docs.microsoft.com/ef/ef6/modeling/code-first/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [GCP Compute Engine 最佳實踐](https://cloud.google.com/compute/docs/instances/best-practices)

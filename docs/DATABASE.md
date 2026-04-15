# 資料模型與資料庫設計

使用 Entity Framework 6 Code First 開發，所有資料庫結構由 C# 實體類定義與自動生成。

## Entity 設計

### Yacht（遊艇）
```csharp
public class Yacht
{
    public int Id { get; set; }
    public string Name { get; set; }              // 遊艇名稱
    public string Model { get; set; }             // 型號
    public decimal Length { get; set; }           // 船長（公尺）
    public decimal BeamWidth { get; set; }        // 船寬
    public string Description { get; set; }       // 詳細說明
    public string ImagePath { get; set; }         // 主圖路徑
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    // 關聯
    public virtual ICollection<NewsArticle> RelatedNews { get; set; }
}
```

### NewsArticle（新聞文章）
```csharp
public class NewsArticle
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Category { get; set; }          // 新聞分類
    public int? YachtId { get; set; }             // 關聯遊艇（選擇）
    public DateTime PublishedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    // 導航屬性
    public virtual Yacht Yacht { get; set; }
}
```

### Dealer（經銷商）
```csharp
public class Dealer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int RegionId { get; set; }             // 地區外鍵
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    // 導航屬性
    public virtual Region Region { get; set; }
}
```

### Region（地區）
```csharp
public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }              // 地區名稱（如：亞洲、歐洲）
    public DateTime CreatedAt { get; set; }

    // 關聯
    public virtual ICollection<Dealer> Dealers { get; set; }
}
```

### AdminUser（管理員帳號）
```csharp
public class AdminUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }      // SHA256 加密
    public string DisplayName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
```

## DbContext 設定

```csharp
public class TayanaYachtDBContext : DbContext
{
    public TayanaYachtDBContext() : base("name=TayanaDB") { }

    public DbSet<Yacht> Yachts { get; set; }
    public DbSet<NewsArticle> NewsArticles { get; set; }
    public DbSet<Dealer> Dealers { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        // Yacht → NewsArticle: 一對多
        modelBuilder.Entity<NewsArticle>()
            .HasOptional(n => n.Yacht)
            .WithMany(y => y.RelatedNews)
            .HasForeignKey(n => n.YachtId)
            .WillCascadeOnDelete(true);

        // Region → Dealer: 一對多
        modelBuilder.Entity<Dealer>()
            .HasRequired(d => d.Region)
            .WithMany(r => r.Dealers)
            .HasForeignKey(d => d.RegionId)
            .WillCascadeOnDelete(false);

        // 索引最佳化
        modelBuilder.Entity<AdminUser>()
            .Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Yacht>()
            .Property(y => y.Name)
            .IsRequired()
            .HasMaxLength(100);

        base.OnModelCreating(modelBuilder);
    }
}
```

## Migration 工作流

### 建立新的 Migration

當資料模型有變更時（如新增 Entity 或欄位）：

```powershell
# Package Manager Console

# 1. 建立 Migration
Add-Migration AddYachtFeatures
# 產生檔案：Migrations/202412150930_AddYachtFeatures.cs

# 2. 預覽變更
Update-Database -Script
# 將 SQL 輸出到 console，無須執行

# 3. 更新資料庫
Update-Database
# 執行 Migration，變更會套用到資料庫
```

### Migration 檔案結構

```csharp
public partial class AddYachtFeatures : DbMigration
{
    public override void Up()
    {
        // 新增欄位、資料表等
        AddColumn("dbo.Yachts", "MaxSpeed", c => c.Int());
        AddIndex("dbo.Yachts", "Name");
    }

    public override void Down()
    {
        // 回復變更（用於 Rollback）
        DropIndex("dbo.Yachts", new[] { "Name" });
        DropColumn("dbo.Yachts", "MaxSpeed");
    }
}
```

### 常見操作

```powershell
# 查看所有 Migration 狀態
Get-Migrations

# 回復到上一個 Migration
Update-Database -TargetMigration:"AddYachtFeatures"

# 強制回復（危險操作，會遺失資料）
Update-Database -TargetMigration: -Force
```

## 資料庫初始化

### Seed 初始資料

在 Migration 中插入預設資料：

```csharp
public partial class AddInitialData : DbMigration
{
    public override void Up()
    {
        InsertData("dbo.Regions", new[] { "Name" }, 
            new object[] { "Asia" });
        
        InsertData("dbo.AdminUsers", 
            new[] { "Username", "PasswordHash", "DisplayName", "IsActive", "CreatedAt", "UpdatedAt" },
            new object[] { "admin", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", 
                          "Administrator", true, DateTime.Now, DateTime.Now });
    }

    public override void Down()
    {
        DeleteData("dbo.AdminUsers", "Username", "admin");
        DeleteData("dbo.Regions", "Name", "Asia");
    }
}
```

## 查詢最佳化

### 避免 N+1 查詢問題

```csharp
// 不好：會產生 N+1 查詢
var dealers = db.Dealers.ToList();
foreach (var dealer in dealers)
{
    var region = dealer.Region.Name;  // 每次迴圈都查一次資料庫
}

// 好：使用 Include 一次載入關聯資料
var dealers = db.Dealers.Include(d => d.Region).ToList();
foreach (var dealer in dealers)
{
    var region = dealer.Region.Name;  // 已在記憶體中
}
```

### 分頁查詢

```csharp
var pageNumber = 1;
var pageSize = 10;

var dealers = db.Dealers
    .Include(d => d.Region)
    .OrderBy(d => d.Country)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();

var totalCount = db.Dealers.Count();
var totalPages = (totalCount + pageSize - 1) / pageSize;
```

## 資料庫備份策略

### SQL Server 備份

```sql
-- 完整備份（Daily）
BACKUP DATABASE TayanaDB 
TO DISK = 'C:\Backups\TayanaDB_Full_20241215.bak'
WITH INIT, COMPRESSION;

-- 交易日誌備份（Hourly）
BACKUP LOG TayanaDB 
TO DISK = 'C:\Backups\TayanaDB_Log_20241215_1000.trn'
WITH COMPRESSION;

-- 復原備份
RESTORE DATABASE TayanaDB 
FROM DISK = 'C:\Backups\TayanaDB_Full_20241215.bak'
WITH REPLACE;
```

### 復原程序

```powershell
# 如果資料庫損毀或誤刪資料
# 1. 取得最新的完整備份檔案
# 2. 在 SQL Server Management Studio 執行 RESTORE
# 3. 驗證復原後的資料完整性
```

## 連線池管理

###  Web.config 連線字串優化

```xml
<connectionStrings>
    <add name="TayanaDB"
         connectionString="Server=localhost;Database=TayanaDB;
         Integrated Security=true;
         Connection Lifetime=300;
         Max Pool Size=100;
         Min Pool Size=5;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

- **Connection Lifetime=300**：連線 5 分鐘後重新建立（防止滯留連線）
- **Max Pool Size=100**：最多 100 個並行連線
- **Min Pool Size=5**：預先建立 5 個連線

## Schema 版本控制

所有 Migration 檔案儲存在 `Migrations/` 資料夾，Git 版本控制確保：
- 開發者同步進度不會 schema 衝突
- 生產環境能追蹤每次變更
- 發生問題時能快速回滾到穩定版本

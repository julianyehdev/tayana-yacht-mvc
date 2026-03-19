# 開發風格與原則

## 整體方向

- 這是學習型專案，**能用簡單寫法就用簡單寫法**，不追求工業級架構
- 沒有必要不抽象，不用 Interface、不用 Repository Pattern、不用 DI Container

## MVC 層次職責

| 層次 | 職責 |
|------|------|
| **Controller** | 只做三件事：接收 Request → 呼叫 Service → 回傳 View 或 Redirect |
| **Service** | 商業邏輯，放在頂層 `Services/`，一個功能一個 class |
| **Domain Entity** | EF 映射用，盡量乾淨，不寫商業邏輯 |
| **ViewModel** | View 專用資料結構，View 不直接吃 EF Entity |

## Service 寫法

不用 interface，不用 DI，直接 `new` 使用：

```csharp
// Services/ProductService.cs
public class ProductService
{
    private TayanaDbContext db = new TayanaDbContext();

    public List<Product> GetActiveProducts()
    {
        return db.Products.Where(p => p.IsActive).ToList();
    }
}

// Controller 用法
var service = new ProductService();
var products = service.GetActiveProducts();
```

## 撰碼規則

- **LINQ 優先**，不寫 raw SQL（除非效能有問題）
- **Validation 用 DataAnnotations**（`[Required]`, `[StringLength]` 等），不另外寫 Validator class
- Service 內部直接 `new TayanaDbContext()`，不傳入、不注入

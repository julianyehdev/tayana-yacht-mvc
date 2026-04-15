# GCP 部署指南

此專案部署在 Google Cloud Platform Compute Engine，展示完整的雲端上線工程流程。

## 部署架構

```
┌─────────────────────────────────────┐
│  GCP Compute Engine (Windows Server) │
├─────────────────────────────────────┤
│  IIS 10                             │
│  ├─ Application Pool (.NET 4.0)     │
│  └─ Website (TayanaYachtMVC)        │
├─────────────────────────────────────┤
│  SQL Server (遠端連線)               │
│  SMTP (聯繫表單郵件)                 │
└─────────────────────────────────────┘
```

## 1. VM 與 IIS 配置

### 建立 Compute Engine 實例

- **Image**：Windows Server 2019 Standard
- **Machine Type**：e2-medium (2 vCPU, 4GB RAM)
- **磁碟**：100GB Standard Persistent Disk
- **防火牆規則**：允許 HTTP (80)、HTTPS (443)

### 安裝 Runtime

1. **下載並安裝 .NET Framework 4.8**
   ```powershell
   # 從 Microsoft 官方下載
   # 或使用 Web Platform Installer 自動安裝
   ```

2. **啟用 IIS 與角色**
   ```powershell
   # Windows Server Manager → Add Roles and Features
   # 勾選：Web Server (IIS) → Application Development → ASP.NET 4.8
   ```

3. **建立 IIS 應用程式集區**
   - 名稱：`TayanaYachtMVC_Pool`
   - .NET Framework 版本：4.0
   - 託管管線模式：Integrated
   - 身份：ApplicationPoolIdentity

4. **建立 IIS 網站**
   - 網站名稱：`TayanaYachtMVC`
   - 應用程式集區：`TayanaYachtMVC_Pool`
   - 實體路徑：`C:\inetpub\TayanaYachtMVC`
   - 繫結：`http://<公開 IP>:80`（或自訂域名）

## 2. 資料庫連線

### SQL Server 遠端連線

假設 SQL Server 執行在另一台機器或 Cloud SQL

1. **設定連線字串** (Web.config)
   ```xml
   <connectionStrings>
       <add name="TayanaDB" 
            connectionString="Server=<DB_SERVER_IP>;Database=TayanaDB;User Id=<USER>;Password=<PASSWORD>;" 
            providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

2. **防火牆規則**
   - GCP：Cloud SQL 白名單加入 Compute Engine 實例 IP
   - SQL Server：允許 TCP 1433 連接埠

3. **建立資料庫與初始化**
   ```powershell
   # 在 VM 上使用 EF Migration 初始化
   # Visual Studio → Package Manager Console
   # 執行：Update-Database -StartupProjectName TayanaYachtMVC
   ```

## 3. SMTP 與郵件配置

聯繫表單透過 SMTP 自動寄信給管理員，並回信給客戶。

1. **配置 SMTP 服務** (Web.config)
   ```xml
   <appSettings>
       <add key="SmtpHost" value="smtp.gmail.com" />
       <add key="SmtpPort" value="587" />
       <add key="SmtpUser" value="your-email@gmail.com" />
       <add key="SmtpPass" value="your-app-password" />
       <add key="ToEmail" value="admin@taianaYacht.com" />
       <add key="SmtpEnableSsl" value="true" />
   </appSettings>
   ```

2. **如果使用 Gmail**
   - 啟用 2FA
   - 產生應用程式密碼：https://myaccount.google.com/apppasswords
   - 使用應用程式密碼作為 SmtpPass

3. **測試郵件**
   - 在前台聯繫表單測試
   - 檢查 IIS Application Event Log（若郵件失敗）

## 4. 應用程式部署

### 準備發佈

1. **建立 Publish Profile**（Visual Studio）
   ```
   右鍵 Project → Publish
   → New Profile → IIS/FTP
   → 輸入 Server IP 與 FTP 憑證
   ```

2. **設定 Web.config Transform**
   ```xml
   <!-- Web.Release.config -->
   <configuration xmlns:xdt="...">
       <appSettings>
           <add key="SmtpHost" xdt:Transform="SetAttributes" value="生產 SMTP" />
       </appSettings>
       <connectionStrings>
           <add name="TayanaDB" xdt:Transform="SetAttributes" 
                connectionString="生產資料庫連線字串" />
       </connectionStrings>
   </configuration>
   ```

3. **發佈應用程式**
   ```
   Visual Studio → Publish Profile → Publish
   → 選擇 Release 組態
   ```

### 部署後檢查

1. **驗證應用程式**
   ```
   http://<公開 IP>/
   http://<公開 IP>/Admin/Login
   ```

2. **檢查 IIS Log**
   ```
   C:\inetpub\logs\LogFiles\W3SVC1\
   ```

3. **檢查 Application Event Log**
   ```powershell
   # PowerShell (管理員)
   Get-EventLog -LogName Application -Newest 20
   ```

## 5. 安全加固

### HTTPS 憑證

1. **使用 Let's Encrypt（免費）**
   ```
   IIS Manager → Server Certificates
   → 透過 certbot 或 Certify The Web 自動續約
   ```

2. **配置重新導向**
   ```xml
   <!-- Web.config -->
   <system.webServer>
       <rewrite>
           <rules>
               <rule name="HTTPS Redirect" stopProcessing="true">
                   <match url="(.*)" />
                   <conditions>
                       <add input="{HTTPS}" pattern="^OFF$" />
                   </conditions>
                   <action type="Redirect" url="https://{HTTP_HOST}{REQUEST_URI}" />
               </rule>
           </rules>
       </rewrite>
   </system.webServer>
   ```

### 應用程式安全

- 定期更新 NuGet 套件（含安全補丁）
- 啟用 .NET Framework 自動更新（Windows Update）
- 定期檢查 IIS Log 尋找異常 HTTP 請求

## 6. 監控與維護

### 效能監控

```powershell
# 檢查應用程式集區回收（重啟）
# IIS Manager → Application Pools → Advanced Settings
# → Recycle Worker Process (勾選自動回收)
```

### 備份策略

- **資料庫備份**：每天自動備份 (SQL Server)
- **應用程式備份**：部署前備份 `C:\inetpub\TayanaYachtMVC`
- **Web.config 備份**：環境敏感設定（密碼、Key）單獨備份

## 7. 故障排除

### 應用程式無法啟動

1. **檢查事件日誌**
   ```powershell
   Get-EventLog -LogName Application -Source "ASP.NET" -Newest 10
   ```

2. **檢查 IIS Application Pool 狀態**
   ```
   IIS Manager → Application Pools → TayanaYachtMVC_Pool
   → 若狀態為 Stopped，點擊 Start
   ```

3. **檢查 Web.config 語法**
   ```
   IIS Manager → Configuration Editor（檢查紅色警告）
   ```

### 資料庫連線失敗

```powershell
# 測試連線
# PowerShell：Test-NetConnection -ComputerName <DB_IP> -Port 1433
# sqlcmd -S <DB_IP> -U <USER> -P <PASSWORD> -d TayanaDB -Q "SELECT 1"
```

### 郵件無法寄送

- 檢查 SMTP 憑證正確性
- 檢查防火牆允許 587 (SMTP) 連接埠
- 檢查 IIS Log 中的 SMTP 相關錯誤

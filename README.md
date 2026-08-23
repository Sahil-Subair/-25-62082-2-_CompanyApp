# -25-62082-2-_CompanyApp

## System Transformation: Before and After
* **Before:** The project was split across two completely separate C# WinForms apps. The first handled login and user accounts using a Microsoft Access database (`.mdb`) over legacy `OleDb` connections. The second managed employee data in a totally isolated setup with no user tracking or links between the two systems.
* **After:** I merged both codebases into a single unified WinForms app (`25-62082-2_CompanyApp`). Everything now runs off one SQL Server LocalDB database (`dbCompanyApp`), giving us a single source of truth with built-in session tracking and full relational integrity.

---

## Fixing the Six Core Integration Conflicts
1. **Database Divergence:** I dumped Microsoft Access (`.mdb`) entirely and pointed the whole application to a single SQL Server LocalDB database (`dbCompanyApp`).
2. **Namespace Mismatches:** Cleaned up and updated every single file imported from the old projects to use one shared namespace: `_25_62082_2_CompanyApp`.
3. **Legacy Data Drivers:** Removed all `System.Data.OleDb` references (`OleDbConnection`, `OleDbCommand`, etc.) and replaced them with modern `System.Data.SqlClient` tools across all forms.
4. **Session & Identity Tracking:** Built a static `Session` class to store `UserID` and `Username` in memory after login, letting child forms access active user details without passing parameters around.
5. **Form File Decoupling:** To stop Visual Studio from breaking the Form Designer, I followed the Three-File Rule when bringing over `frmEmployee`, copying the `.cs`, `.Designer.cs`, and `.resx` files together.
6. **Relational Link:** Linked employee records directly to user accounts by adding a `CreatedBy` foreign key in the employee table that points to `Users(UserID)`.

---

## Database Architecture & Migration
The unified `dbCompanyApp` SQL Server LocalDB instance combines user authentication and employee management in one place:

* **`Users` Table:** Stores user IDs (auto-increment primary key), usernames (unique), passwords, emails, full names, and account creation dates. Old Access accounts were migrated straight into this table.
* **`Emp_details` Table:** Stores `EmpId` (Primary Key), `EmpName`, `EmpAge`, `EmpContact`, `EmpGender`, and `CreatedBy`. The `CreatedBy` field acts as a foreign key pointing to `Users(UserID)` with `ON DELETE SET NULL` so employee data stays safe even if a user account gets deleted.
* **Database Script:** You can find the full setup script in `Schema.sql` in the project root.

---

## Form Porting & The Three-File Rule
To bring over `frmEmployee` without breaking the Visual Studio designer or messing up UI elements, I brought over all three essential files as a unit:
* **`frmEmployee.cs`** (Code-behind and business logic)
* **`frmEmployee.Designer.cs`** (Form controls and auto-generated layout code)
* **`frmEmployee.resx`** (Form resources and UI metadata)

After copying them into the project, I updated the namespace at the top of each file to `_25_62082_2_CompanyApp` so everything compiled smoothly.

---

## Upgrading to SqlClient & Class Structure
I re-wrote all database operations to use `System.Data.SqlClient` instead of `OleDb`.

* **Files Updated:** Updated `LoginForm.cs`, `HomeForm.cs`, and `frmEmployee.cs` to run parameterized SQL queries using `SqlConnection` and `SqlCommand`.
* **`DatabaseHelper.cs`:** Holds connection string management and reusable helper methods to run queries cleanly.
* **`Session.cs`:** A simple static class that holds `UserID` and `Username` once a user logs in.
* **`User.cs`:** Represents the user object model across the app.

---

## Application Workflow & Relational Links
1. **Login:** The app starts on `LoginForm.cs`. Credentials get verified through `DatabaseHelper`, and valid logins save the active `UserID` into `Session.cs`.
2. **Dashboard:** `LoginForm` opens `HomeForm.cs`, showing a welcome message using `Session.Username`.
3. **Employee CRUD:** Clicking **Manage Employees** opens `frmEmployee.cs`. Whenever you add an employee record, the app grabs `Session.UserID` and writes it to the `CreatedBy` column.
4. **Data Grid Query:** The data grid pulls employee records using a join query with the `Users` table to show who created each entry.

### Why Use LEFT JOIN Instead of INNER JOIN?
I used a `LEFT JOIN` so that all employee records stay visible in the grid even if the user account that created them was deleted or unassigned (`CreatedBy` is NULL). An `INNER JOIN` would completely hide those employee records from the screen, which would cause data loss in the UI. A `LEFT JOIN` keeps the records on screen and lets us show a default fallback like `System/Unassigned`.

---

## Real Build Error & How I Fixed It
* **The Problem:** I ran into a `CS1061` build error ("'frmEmployee' does not contain a definition for 'txtEmpId_TextChanged'") alongside file lock errors (`CS1519`/`CS1513`) during a rebuild.
* **The Cause:** Accidental double-clicking in the Form Designer created broken event references in `frmEmployee.Designer.cs` pointing to deleted methods. At the same time, an old `25-62082-2_dbCompanyApp.exe` process was still running in the background, locking the binary file in `bin/Debug`.
* **The Fix:**
  1. Deleted the orphan event bindings in `frmEmployee.Designer.cs`.
  2. Added simple stub event methods in `frmEmployee.cs` to keep the designer happy.
  3. Opened Task Manager, found `25-62082-2_dbCompanyApp.exe`, and killed the process.
  4. Ran **Clean Solution** followed by a successful **Rebuild Solution**.

---

## Why One Database Is Better Than Two
Merging two separate databases into a single SQL Server LocalDB setup makes the entire system faster, cleaner, and much easier to maintain. Running two separate databases creates massive sync headaches, risks orphaned data, and requires juggling different database drivers (like Access and SQL Server). Moving everything into one relational database lets us enforce actual foreign key rules, keep transaction processing secure, and run simple queries—like our `LEFT JOIN` mapping employees to their creators—without messy cross-database hacks.

---

## Submission Screenshots Checklist
The submission package includes visual verification for:
1. **Object Explorer:** Both `Users` and `Emp_details` tables listed under `dbCompanyApp`.
2. **Users Data View:** Rows of user data inside the table editor.
3. **Solution Explorer:** The nested three-file layout (`.cs`, `.Designer.cs`, `.resx`) for `frmEmployee`.
4. **App Flow:** Step-by-step screenshots from `LoginForm` to `HomeForm` to `frmEmployee`.
5. **Data Grid Audit:** The employee table showing record creators populated via `LEFT JOIN`.

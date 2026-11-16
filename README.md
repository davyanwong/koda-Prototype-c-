# Koda AI Assistant: prototype (C# / ASP.NET Core)

Two tools in one web app:
- **6b Invoice checker**: reads invoice PDFs, compares with PO and goods received, flags differences, a person approves or rejects.
- **6c Production planner**: explodes the multi-level BOM, checks stock and incoming POs per order due date, suggests a priority list with reasons.

Code does the maths and matching. AI is optional and only reads messy invoices or explains results.
The app never writes to the ERP; it exports CSV files.

## Run it
1. Install the .NET SDK (8 or newer) from dotnet.microsoft.com.
2. Open a terminal in this folder and run:  `dotnet run`
   (the first run downloads one package, PdfPig, so you need internet once)
3. Open the address it prints, usually http://localhost:5000 or http://localhost:5xxx
4. In Visual Studio: open `KodaPrototype.csproj` and press F5.

Go to **6b**, press "Check the 12 sample invoices", then open **6c**.

## Files
| File | What it does |
|---|---|
| Program.cs | Starts the web app |
| Pages/*.cshtml (+ .cs) | The screens and their buttons |
| Services/InvoiceService.cs | Invoice reading rules and matching rules (6b) |
| Services/PlannerService.cs | BOM explosion, shortage check, priority list (6c) |
| Services/PdfTextReader.cs | Pulls text out of invoice PDFs (PdfPig) |
| Services/DataStore.cs | Reads Koda's CSV exports. Change only this for an API later |
| Services/AiService.cs | Optional Claude calls |
| Models/Models.cs | The data shapes |
| data/ | Mock CSVs, 12 sample invoice PDFs (8 with planted errors) and one AI demo invoice |

## Real data later
Replace the CSVs in `data/` with Koda's exports using the same column names.
Sales orders and open POs can use real dates (`due_date`, `eta`) instead of `due_in_days` / `eta_in_days`.

## Turn on AI invoice reading
AI reading is built in but needs a key. Without one, the built-in rules read the invoices.
1. Create a key at console.anthropic.com (API billing is separate from a Claude chat account; a few invoices cost very little).
2. Set it, then **restart the app**:
   - Windows PowerShell:  `$env:ANTHROPIC_API_KEY="your-key"` then `dotnet run`
   - or once only: `dotnet user-secrets init` then `dotnet user-secrets set ANTHROPIC_API_KEY your-key`
   - Visual Studio: right-click the project, Manage User Secrets, add `{ "ANTHROPIC_API_KEY": "your-key" }`
3. On the Check invoices page you should see "AI ready". Press **Test the AI connection**, tick **Use AI to read the invoices**, then try **the AI demo invoice**.
If AI fails for an invoice, the app falls back to the rules and says why on that invoice's page.
Use fake or anonymised data only until Koda says cloud AI is allowed. Never put the key in GitHub.

## Put it online
Streamlit hosting cannot run C#. Use a host for .NET, for example Azure App Service (Azure for Students) or Render using the included `Dockerfile`.

## Not in the prototype yet
- Scanned invoices (needs OCR or AI vision)
- Real supplier layouts (rule-based reading knows the two mock layouts; AI reading handles more)
- Machine capacity and labour limits in planning
- Several users at once (state is held in memory for one user)

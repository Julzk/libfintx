using System.Text.RegularExpressions;
using libfintx.FinTS;
using libfintx.FinTS.Camt;
using libfintx.FinTS.Data;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000); // Setzen Sie den gewünschten Port hier
});
var app = builder.Build();



// app.MapGet("/GetTransactions", async ([FromBody] RequestData requestData) => await GetTransactions(requestData));
app.MapPost("/GetTransactions", async ([FromBody] LoginData loginData) => await GetTransactions(loginData));
app.MapPost("/Transaction", async ([FromBody] TransactionData transactionData) => await Transaction(transactionData));


app.Run();

async Task<string> GetTransactions(LoginData loginData)
{
    try
    {
        Console.WriteLine("START GetTransactions");
        if (loginData.secret != "nxtlvlink")
        {
            throw new Exception("wrong secret");
        }
        var client = new FinTsClient(GetConnectionDetails(loginData));
        var startDate = DateTime.Parse(loginData.startDate);
        var endDate = DateTime.Parse(loginData.endDate);
        // var transactions = await client.Transactions_camtNxt(new TANDialog(WaitForTanAsync), CamtVersion.Camt052, startDate, endDate);
        // var transactions = await client.Transactions(new TANDialog(WaitForTanAsync), startDate, endDate);
        var transactions = await client.Transactions_camt(new TANDialog(WaitForTanAsync), CamtVersion.Camt052, startDate, endDate);
        return transactions.RawData;
    }
    catch (Exception err)
    {
        return err.Message;
    }
}

async Task<String> Transaction(TransactionData transactionData)
{
    try
    {
        Console.WriteLine("START GetTransactions");
        if (transactionData.loginData.secret != "nxtlvlink")
        {
            throw new Exception("wrong secret");
        }
        var client = new FinTsClient(GetConnectionDetails(transactionData.loginData));
        var sync = await client.Synchronization();
        client.ConnectionDetails.AccountHolder = "NXTLVLINK";



        if (sync.IsSuccess)
        {
            // TAN-Verfahren
            client.HIRMS = ""; // txt_tanverfahren.Text;

            await InitTANMedium(client);

            var transfer = await client.Transfer(CreateTANDialog(client), transactionData.name, Regex.Replace(transactionData.iban, @"\s+", ""), transactionData.bic,
                transactionData.value, transactionData.text, client.HIRMS);

            // Out image is needed e. g. for photoTAN
            //var transfer = Main.Transfer(connectionDetails, txt_empfängername.Text, txt_empfängeriban.Text, txt_empfängerbic.Text,
            //    decimal.Parse(txt_betrag.Text), txt_verwendungszweck.Text, Segment.HIRMS, pBox_tan, false);

            return string.Join('\n', transfer.Messages.Select(m => m.Message));
        }
        throw new Exception("IsSuccess false");





        /*Console.WriteLine("START GetTransactions");
        if (transactionData.loginData.secret != "nxtlvlink")
        {
            throw new Exception("wrong secret");
        }
        var client = new FinTsClient(GetConnectionDetails(transactionData.loginData));
        var startDate = DateTime.Parse(transactionData.loginData.startDate);
        var endDate = DateTime.Parse(transactionData.loginData.endDate);
        // var transactions = await client.Transactions_camtNxt(new TANDialog(WaitForTanAsync), CamtVersion.Camt052, startDate, endDate);
        var transactions = await client.Transactions(new TANDialog(WaitForTanAsync), startDate, endDate);
        return transactions.RawData;*/
    }
    catch (Exception err)
    {
        return err.Message;
    }
}

async Task<bool> InitTANMedium(FinTsClient client)
{
    // TAN-Medium-Name
    client.HITAB = ""; // txt_tan_medium.Text;
    var accounts = await client.Accounts(CreateTANDialog(client));
    if (!accounts.IsSuccess)
    {
        return false;
    }
    var conn = client.ConnectionDetails;
    AccountInformation accountInfo = UPD.GetAccountInformations(conn.Account, conn.Blz.ToString());
    if (accountInfo != null && accountInfo.IsSegmentPermitted("HKTAB"))
    {
        client.HITAB = ""; // txt_tan_medium.Text;
    }

    return true;
}

static async Task<string> WaitForTanAsync(TANDialog tanDialog)
{
    Console.WriteLine("Starte WaitForTanAsync");
    foreach (var msg in tanDialog.DialogResult.Messages)
    {
        Console.WriteLine(msg);
    }
    Console.WriteLine("RAW: " + tanDialog.DialogResult.RawData);
    if (tanDialog.DialogResult.RawData.IndexOf("Bitte Auftrag in Ihrer App freigeben") > -1)
    {
        Console.WriteLine("Es wird nicht auf TAN gewartet, weil PUSHTAN");
        return "";
    }
    return await Task.FromResult(Console.ReadLine());
}

ConnectionDetails GetConnectionDetails(LoginData loginData)
{
    var result = new ConnectionDetails()
    {
        Account = loginData.account,
        Blz = loginData.blz,
        Bic = loginData.bic,
        Iban = Regex.Replace(loginData.iban, @"\s+", ""),
        Url = loginData.url,
        HbciVersion = 300,
        UserId = loginData.userId,
        Pin = loginData.pin
    };

    return result;
}

TANDialog CreateTANDialog(FinTsClient client)
{
    var dialog = new TANDialog(WaitForTanAsync, null);
    if (client.HIRMS == "922")
        dialog.IsDecoupled = true;

    return dialog;
}

class LoginData
{
    public string? account { get; set; }
    public int blz { get; set; }
    public string? bic { get; set; }
    public string? iban { get; set; }
    public string? url { get; set; }
    public string? userId { get; set; }
    public string? pin { get; set; }
    public string? secret { get; set; }
    public string? startDate { get; set; }
    public string? endDate { get; set; }

}

class TransactionData
{
    public LoginData? loginData { get; set; }
    public string? name { get; set; }
    public int blz { get; set; }
    public string? bic { get; set; }
    public string? iban { get; set; }
    // public string? url { get; set; }
    // public string? pin { get; set; }
    public string? text { get; set; }
    public decimal value { get; set; }
}



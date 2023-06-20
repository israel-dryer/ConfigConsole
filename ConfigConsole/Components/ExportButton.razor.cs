using CsvHelper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.Globalization;
using System.Text;

namespace ConfigConsole.Components;

public partial class ExportButton : ComponentBase
{

    [Inject] IJSRuntime JS { get; set; } = default!;
    [Parameter] public string FileName { get; set; } = "data.csv";
    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public ButtonStyle ButtonStyle { get; set; } = ButtonStyle.Primary;
    [Parameter] public EventCallback Click { get; set; }
    [Parameter] public ButtonSize ButtonSize { get; set; } = ButtonSize.Small;
    [Parameter] public string ButtonText { get; set; } = string.Empty;

    private static async Task<byte[]> DataToBytes<T>(IEnumerable<T> data)
    {
        using var writer = new StringWriter();
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteHeader<T>();
        csv.NextRecord();
        await csv.WriteRecordsAsync(data);
        return Encoding.UTF8.GetBytes(writer.ToString());
    }

    public async Task DownloadFileFromStreamAsync<T>(IEnumerable<T> data)
    {
        var bytestring = await DataToBytes(data);
        using var stream = new MemoryStream(bytestring);
        using var streamRef = new DotNetStreamReference(stream);
        await JS.InvokeVoidAsync("downloadFileFromStream", FileName, streamRef);
    }

}

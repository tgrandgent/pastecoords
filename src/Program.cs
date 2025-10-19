using PasteCoords;
using TextCopy;

//CoordConverter.GetApacheCoords(":");
//CoordConverter.GetApacheCoords("MGRS GRID: 38 T KM 82644 70332");
//CoordConverter.GetApacheCoords("4QFJ 12345 67890");
//CoordConverter.GetApacheCoords("4QFJ 12345 678901");
//CoordConverter.GetApacheCoords("4QFJ 1234 5678");
//CoordConverter.GetApacheCoords("4QFJ 123 456");
//CoordConverter.GetApacheCoords("4QFJ 12");
//CoordConverter.GetApacheCoords("4QFJ 1");
//CoordConverter.GetApacheCoords("4QFJ");
//CoordConverter.GetApacheCoords("4QF");
//return;

var clipText = await ClipboardService.GetTextAsync();
if (clipText == null)
    return;

var output = CoordConverter.GetApacheCoords(clipText);
if (string.IsNullOrEmpty(output))
    return;

await CoordSender.SendCoords(output);

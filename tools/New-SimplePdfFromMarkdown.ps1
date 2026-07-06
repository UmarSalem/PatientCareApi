param(
    [Parameter(Mandatory = $true)]
    [string]$InputPath,

    [Parameter(Mandatory = $true)]
    [string]$OutputPath,

    [string]$Title = "Document"
)

$ErrorActionPreference = "Stop"

function Escape-PdfText {
    param([string]$Text)

    return $Text.Replace("\", "\\").Replace("(", "\(").Replace(")", "\)")
}

function Wrap-Line {
    param(
        [string]$Text,
        [int]$MaxCharacters
    )

    if ([string]::IsNullOrWhiteSpace($Text)) {
        return @("")
    }

    $words = $Text -split "\s+"
    $lines = New-Object System.Collections.Generic.List[string]
    $current = ""

    foreach ($word in $words) {
        if ($current.Length -eq 0) {
            $current = $word
            continue
        }

        if (($current.Length + 1 + $word.Length) -le $MaxCharacters) {
            $current = "$current $word"
        }
        else {
            $lines.Add($current)
            $current = $word
        }
    }

    if ($current.Length -gt 0) {
        $lines.Add($current)
    }

    return $lines.ToArray()
}

function Add-PdfObject {
    param(
        [System.Collections.Generic.List[string]]$Objects,
        [string]$Content
    )

    $Objects.Add($Content)
    return $Objects.Count
}

$markdownLines = Get-Content -LiteralPath $InputPath
$pageWidth = 595
$pageHeight = 842
$leftMargin = 50
$topMargin = 795
$bottomMargin = 55
$normalSize = 10
$lineHeight = 14
$pages = New-Object System.Collections.Generic.List[object]
$currentLines = New-Object System.Collections.Generic.List[object]
$y = $topMargin

function New-PageIfNeeded {
    param([int]$NeededHeight)

    if (($script:y - $NeededHeight) -lt $script:bottomMargin) {
        $script:pages.Add($script:currentLines.ToArray())
        $script:currentLines = New-Object System.Collections.Generic.List[object]
        $script:y = $script:topMargin
    }
}

function Add-Line {
    param(
        [string]$Text,
        [string]$Font = "F1",
        [int]$Size = 10,
        [int]$Indent = 0,
        [int]$ExtraAfter = 0
    )

    New-PageIfNeeded -NeededHeight ($script:lineHeight + $ExtraAfter)
    $script:currentLines.Add([pscustomobject]@{
        Text = $Text
        Font = $Font
        Size = $Size
        X = $script:leftMargin + $Indent
        Y = $script:y
    })
    $script:y -= ($script:lineHeight + $ExtraAfter)
}

Add-Line -Text $Title -Font "F2" -Size 18 -ExtraAfter 10

foreach ($rawLine in $markdownLines) {
    $line = $rawLine.TrimEnd()

    if ($line.StartsWith("# ")) {
        $text = $line.Substring(2)
        New-PageIfNeeded -NeededHeight 40
        Add-Line -Text $text -Font "F2" -Size 18 -ExtraAfter 8
        continue
    }

    if ($line.StartsWith("## ")) {
        $text = $line.Substring(3)
        New-PageIfNeeded -NeededHeight 32
        Add-Line -Text $text -Font "F2" -Size 14 -ExtraAfter 5
        continue
    }

    if ($line.StartsWith("### ")) {
        $text = $line.Substring(4)
        New-PageIfNeeded -NeededHeight 26
        Add-Line -Text $text -Font "F2" -Size 12 -ExtraAfter 3
        continue
    }

    if ([string]::IsNullOrWhiteSpace($line)) {
        $script:y -= 6
        continue
    }

    $indent = 0
    $textToWrite = $line

    if ($line.StartsWith("- ")) {
        $indent = 14
        $textToWrite = "* " + $line.Substring(2)
    }

    if ($line.StartsWith("> ")) {
        $indent = 18
        $textToWrite = $line.Substring(2)
    }

    if ($line.StartsWith('```')) {
        continue
    }

    $maxChars = if ($indent -gt 0) { 82 } else { 88 }
    foreach ($wrapped in (Wrap-Line -Text $textToWrite -MaxCharacters $maxChars)) {
        Add-Line -Text $wrapped -Font "F1" -Size $normalSize -Indent $indent
    }
}

if ($currentLines.Count -gt 0) {
    $pages.Add($currentLines.ToArray())
}

$objects = New-Object System.Collections.Generic.List[string]
$catalogId = Add-PdfObject -Objects $objects -Content ""
$pagesId = Add-PdfObject -Objects $objects -Content ""
$fontRegularId = Add-PdfObject -Objects $objects -Content "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>"
$fontBoldId = Add-PdfObject -Objects $objects -Content "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold >>"
$pageObjectIds = New-Object System.Collections.Generic.List[int]
$contentObjectIds = New-Object System.Collections.Generic.List[int]

for ($i = 0; $i -lt $pages.Count; $i++) {
    $content = New-Object System.Text.StringBuilder
    [void]$content.AppendLine("BT")

    foreach ($item in $pages[$i]) {
        $escaped = Escape-PdfText -Text $item.Text
        [void]$content.AppendLine("/$($item.Font) $($item.Size) Tf")
        [void]$content.AppendLine("1 0 0 1 $($item.X) $($item.Y) Tm")
        [void]$content.AppendLine("($escaped) Tj")
    }

    [void]$content.AppendLine("ET")
    $streamText = $content.ToString()
    $streamLength = [System.Text.Encoding]::ASCII.GetByteCount($streamText)
    $contentId = Add-PdfObject -Objects $objects -Content "<< /Length $streamLength >>`nstream`n$streamText`nendstream"
    $pageId = Add-PdfObject -Objects $objects -Content ""
    $contentObjectIds.Add($contentId)
    $pageObjectIds.Add($pageId)
}

$kids = ($pageObjectIds | ForEach-Object { "$_ 0 R" }) -join " "
$objects[$catalogId - 1] = "<< /Type /Catalog /Pages $pagesId 0 R >>"
$objects[$pagesId - 1] = "<< /Type /Pages /Kids [ $kids ] /Count $($pageObjectIds.Count) >>"

for ($i = 0; $i -lt $pageObjectIds.Count; $i++) {
    $pageId = $pageObjectIds[$i]
    $contentId = $contentObjectIds[$i]
    $objects[$pageId - 1] = "<< /Type /Page /Parent $pagesId 0 R /MediaBox [0 0 $pageWidth $pageHeight] /Resources << /Font << /F1 $fontRegularId 0 R /F2 $fontBoldId 0 R >> >> /Contents $contentId 0 R >>"
}

$pdf = New-Object System.Text.StringBuilder
[void]$pdf.AppendLine("%PDF-1.4")
$offsets = New-Object System.Collections.Generic.List[int]
$encoding = [System.Text.Encoding]::ASCII

for ($i = 0; $i -lt $objects.Count; $i++) {
    $offsets.Add($encoding.GetByteCount($pdf.ToString()))
    [void]$pdf.AppendLine("$($i + 1) 0 obj")
    [void]$pdf.AppendLine($objects[$i])
    [void]$pdf.AppendLine("endobj")
}

$xrefOffset = $encoding.GetByteCount($pdf.ToString())
[void]$pdf.AppendLine("xref")
[void]$pdf.AppendLine("0 $($objects.Count + 1)")
[void]$pdf.AppendLine("0000000000 65535 f ")

foreach ($offset in $offsets) {
    [void]$pdf.AppendLine(("{0:D10} 00000 n " -f $offset))
}

[void]$pdf.AppendLine("trailer")
[void]$pdf.AppendLine("<< /Size $($objects.Count + 1) /Root $catalogId 0 R >>")
[void]$pdf.AppendLine("startxref")
[void]$pdf.AppendLine("$xrefOffset")
[void]$pdf.AppendLine("%%EOF")

$outputDirectory = Split-Path -Parent $OutputPath
if ($outputDirectory -and -not (Test-Path -LiteralPath $outputDirectory)) {
    New-Item -ItemType Directory -Force -Path $outputDirectory | Out-Null
}

[System.IO.File]::WriteAllText($OutputPath, $pdf.ToString(), $encoding)

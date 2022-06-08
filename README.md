# ReadmeToBBcode
Tool to assist in converting ReadMe to the NexusMods.com BBCode.


# Command Line Actions
```
Description:

Usage:
  ReadmeToBBcode [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  convert  Converts markdown to NexusMods format bbcode
  table    Creates an ASCII table from markdown table input
```

# Convert (README.md)

Converts a README.md to BBCode for use on NexusMods.com.  This is currently a very simple translation and does not convert tables, links, and images.

Use the [table](#table) command to convert the tables individually.

## Command Line

```
Description:
  Converts markdown to NexusMods format BBCode

Usage:
  ReadmeToBBcode convert [options]

Options:
  path <path>     The path to the file
  -?, -h, --help  Show help and usage information
```

 # Table
  
 Creates an ASCII table from a markdown table.
 
 General use is to copy a table from the markdown and take the output to replace the table in the nexus mod BBCode text.
 
 Input example:
 ```
|Setting|Description|
|-|-|
```isDefault ``` | True for the color set to default to .  If multiple are set then the first ```isDefault ``` set will be used.
```Description ``` | A friendly description used for user clarity when editing the mod.json .
```TickMarkOptimal ``` | Hex color for weapons in range.
```TickMarkNonOptimal ``` | Hex color for weapons in non-optimal range.
```TickMarkTargetedOptimal ``` | Hex color for weapons in range in first person  targeting mode.
```TickMarkTargetedNonOptimal ``` | Hex color for weapons in non-optimal range in first person targeting mode.
  
 ```
  
 Output example:
  ```
[font=Courier New] 
| Setting                    | Description                                                         |
| -------------------------- | ------------------------------------------------------------------- |
| isDefault                  | True for the color set to default to .  If multiple are set then    |
|                            | the first isDefault  set will be used.                              |
|                            |                                                                     |
| Description                | A friendly description used for user clarity when editing the       |
|                            | mod.json .                                                          |
|                            |                                                                     |
| TickMarkOptimal            | Hex color for weapons in range.                                     |
|                            |                                                                     |
| TickMarkNonOptimal         | Hex color for weapons in non-optimal range.                         |
|                            |                                                                     |
| TickMarkTargetedOptimal    | Hex color for weapons in range in first person  targeting mode.     |
|                            |                                                                     |
| TickMarkTargetedNonOptimal | Hex color for weapons in non-optimal range in first person          |
|                            | targeting mode.                                                     |
[/font]  
  ```

### NexusMods Editor Note

There is currently a bug in the raw BBCode mode of the NexusMods where spaces are not perserved correctly.

The workaround is to paste the ASCII table in the preview mode, highlight the table, and change the font to Courier New.

Until that issue is corrected, disable including the font with ```use-bbcode-font=false``` .

## Command Line
```
Description:
  Creates an ASCII table from markdown table input

Usage:
  ReadmeToBBcode table [options]

Options:
  path <path>                    The path to the file to read.  If not set, the program will use stdin
  maxTableWidth <maxTableWidth>  The maximum width the table.  Set to 0 to not limit. [default: 100]
  remove-code-block              Removes and mardown code blocks (```) in the text [default: True]
  use-bbcode-font                If true, wraps the table in the Courier New font [default: True]
  -?, -h, --help                 Show help and usage information
```

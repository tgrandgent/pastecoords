# PasteCoords
This program simulates keypresses to paste MGRS coordinates from the clipboard
into the DCS AH-64D Apache's KDU (keyboard display unit).  It's like a Ctrl+V for
your Apache, with some extra logic for conversion.

The program does not run continuously.  It pastes one set of coordinates and exits.
You must set up an AutoHotKey script or similar to run it when you want to paste.

## What it does when you run it
1. read MGRS coordinates from the clipboard (only MGRS is supported, not lat/long)
2. convert to a format suitable for the Apache, e.g. `MGRS GRID: 38 T KM 82644 70332` becomes `38TKM82647033`
3. simulate keypresses to input the coordinates into the Apache's KDU

## Setup
### AutoHotKey script
You need a way to trigger the program to run while you're in DCS.  Here's an example AutoHotKey 2.0 script that runs it when you press Win+Z:
```ahk
#z::Run("C:\PROGRAM_LOCATION\PasteCoords.exe", , "Hide")
```
### Keybindings
You need to have specific keybindings set up in DCS for the Apache's KDU:
- letters A-Z:
    - LCtrl+LShift+A = `KU Key - A`
    - ...
    - LCtrl+LShift+Z = `KU Key - Z`
- numbers 0-9:
    - LCtrl+LShift+Num0 = `KU Key - 0`
    - ...
    - LCtrl+LShift+Num9 = `KU Key - 9`
- CLR (clear button) - optional, bind if you want to auto-clear before pasting
    - LCtrl+LShift+= = `KU Key - CLR`

If you're comfortable working with text files, you can do that instead of using
the Controls interface in DCS to set up the keybindings:

- refer to example [Keyboard.diff.lua](Keyboard.diff.lua) (this was created by an Oct. 2025 version of DCS)
- this file goes in the following locations under `Saved Games\DCS`:
    - `Config\Input\AH-64D_BLK_II_PLT\keyboard`
    - `Config\Input\AH-64D_BLK_II_CPG\keyboard`
- if you already have non-default keybindings, manually merge this file with yours
- check controls in DCS and resolve any conflicts

## Usage
- copy MGRS coordinates to the clipboard (e.g. via F10 map alt+click + copy)
- manipulate the TSD/KDU to the point where you'd start typing in MGRS coordinates
    - on the TSD: Point, Add, WP
    - on the KDU: Enter, Enter, Clear
- press your PasteCoords hotkey (Win+Z in the above AutoHotKey example)
- confirm the coordinates were pasted correctly, then hit Enter on the KDU

## Troubleshooting
- make sure the keybindings work with the KDU when you press them yourself
- make sure you don't have some other global hotkey conflicting
- make sure you don't have any conflicting keybindings within DCS
- DCS may have changed in a way that breaks this program - there's no guarantee that it still works
- the keys available for simulated keypress input into DCS are limited:
    - LCtrl+LAlt+T doesn't work for some reason (all other LCtrl+LAlt keys work)
    - LWin doesn't work because it switches away from DCS
    - LCtrl+LShift seems to be safe and doesn't conflict with defaults
    - LCtrl+LShift+Del doesn't work for some reason

## Extras
Copied something to the clipboard?  Don't just paste it into your Apache.
Check out this text editor built into DCS where you can paste it and make notes:
- [rkusa/dcs-scratchpad: Resizable and movable DCS World ingame Scratchpad for quick notes - especially useful in VR.](https://github.com/rkusa/dcs-scratchpad)

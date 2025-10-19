namespace PasteCoords;

public static class CoordSender
{
    const byte VK_LWIN = 91;
    const byte VK_NUMPAD0 = 96;
    const byte VK_LSHIFT = 160;
    const byte VK_LCONTROL = 162;
    const byte VK_LALT = 164;
    const byte VK_OEM_PLUS = 187;

    const int DelayAfterFocus = 200;
    const int DelayBeforeInput = 500;
    const int DelayBetweenKeys = 150;

    static IntPtr GetDcsWindow()
    {
        return PInvoke.FindWindow("DCS", null);
    }

    static async Task EnsureDcsHasFocus()
    {
        // Restore focus to DCS in case this program was not run hidden
        var hwndDcs = GetDcsWindow();
        if (hwndDcs == IntPtr.Zero)
            return;
        var hwndFore = PInvoke.GetForegroundWindow();
        if (!hwndDcs.Equals(hwndFore))
        {
            PInvoke.SetForegroundWindow(hwndDcs);
            await Task.Delay(DelayAfterFocus);
        }
    }

    static byte? GetVirtualKeyCode(char c)
    {
        if (char.IsDigit(c))
            return (byte)(VK_NUMPAD0 + byte.Parse(c.ToString()));
        if (char.IsLetter(c))
            return (byte)c;
        return null;
    }

    static byte GetScanCode(byte vk)
    {
        return (byte)PInvoke.MapVirtualKey(vk, PInvoke.MAPVK_VK_TO_VSC);
    }

    static void SendKeyDown(byte vk)
    {
        var sc = GetScanCode(vk);
        PInvoke.keybd_event(vk, sc, 0, UIntPtr.Zero);
    }

    static void SendKeyUp(byte vk)
    {
        var sc = GetScanCode(vk);
        PInvoke.keybd_event(vk, sc, PInvoke.KEYEVENTF_KEYUP, UIntPtr.Zero);
    }

    static async Task SendClear()
    {
        SendKeyDown(VK_LCONTROL);
        SendKeyDown(VK_LSHIFT);
        SendKeyDown(VK_OEM_PLUS);
        await Task.Delay(DelayBetweenKeys);

        SendKeyUp(VK_OEM_PLUS);
        SendKeyUp(VK_LCONTROL);
        SendKeyUp(VK_LSHIFT);
        await Task.Delay(DelayBetweenKeys);
    }

    static async Task SendChar(char ch)
    {
        var vk = GetVirtualKeyCode(ch);
        if (vk == null)
            return;

        SendKeyDown(VK_LCONTROL);
        SendKeyDown(VK_LSHIFT);
        //SendKeyDown(VK_LWIN);
        //SendKeyDown(VK_LALT);
        SendKeyDown(vk.Value);

        await Task.Delay(DelayBetweenKeys);

        SendKeyUp(vk.Value);
        SendKeyUp(VK_LCONTROL);
        SendKeyUp(VK_LSHIFT);
        //SendKeyUp(VK_LWIN);
        //SendKeyUp(VK_LALT);

    }

    public static async Task SendCoords(string output)
    {
        await Task.Delay(DelayBeforeInput);

        await EnsureDcsHasFocus();

        await SendClear();

        foreach (var ch in output)
            await SendChar(ch);
    }
}

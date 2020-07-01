namespace ChocoTrayNotify
{
    public class CTNSettings
    {
        public static CTNSettings Inst = new CTNSettings();

        public int    ChocoCommandTimeout  = 60 * 1000;
        public bool   ShowPowershellWindow = false;
        public bool   PowershellElevate    = true;
        public string PowershellPath       = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe";
        public bool   SortUpdatesToTop     = false;
    }
}

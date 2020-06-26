using System;
using System.Collections.Generic;
using System.Text;

namespace ChocoTrayNotify
{
    public class CTNSettings
    {
        public static CTNSettings Inst = new CTNSettings();

        public int ChocoCommandTimeout   = 60 * 1000;
        public bool ShowPowershellWindow = false;
        public bool PowershellElevate    = true;

    }
}

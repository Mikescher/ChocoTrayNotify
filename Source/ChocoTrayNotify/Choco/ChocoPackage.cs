namespace ChocoTrayNotify.Choco
{
    public class ChocoPackage
    {

        public string PackageName      { get;}
        public string CurrentVersion   { get;}
        public string AvailableVersion { get;}
        public bool?  Pinned           { get;}

        public bool UpdateAvailable => CurrentVersion != AvailableVersion;

        public bool IsOutdatedAndUpdateable => UpdateAvailable && Pinned != true;

        public ChocoPackage(string name, string v1, string v2, bool? pinned)
        {
            this.PackageName      = name;
            this.CurrentVersion   = v1;
            this.AvailableVersion = v2;
            this.Pinned           = pinned;
        }
    }
}

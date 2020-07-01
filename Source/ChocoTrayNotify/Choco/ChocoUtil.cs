using System.Collections.Generic;
using System.Linq;

namespace ChocoTrayNotify.Choco
{
    public static class ChocoUtil
    {
        public static List<ChocoPackage> CleanPackageList(List<ChocoPackage> pkglist)
        {
            var result = pkglist.ToList();

            foreach (var pkg in pkglist.ToList())
            {
                if (pkg.PackageName.EndsWith(".install"))
                {
                    var basePackage = result.Where(p => p.PackageName == pkg.PackageName.Substring(0, pkg.PackageName.Length - ".install".Length)).ToList();
                    if (basePackage.Count != 1) continue;

                    if (basePackage.Single().CurrentVersion != pkg.CurrentVersion) continue;
                    if (basePackage.Single().AvailableVersion != pkg.AvailableVersion) continue;
                    if (basePackage.Single().Pinned != pkg.Pinned) continue;

                    result.Remove(pkg);
                    continue;
                }
                if (pkg.PackageName.EndsWith(".portable"))
                {
                    var basePackage = result.Where(p => p.PackageName == pkg.PackageName.Substring(0, pkg.PackageName.Length - ".install".Length)).ToList();
                    if (basePackage.Count != 1) continue;

                    if (basePackage.Single().CurrentVersion != pkg.CurrentVersion) continue;
                    if (basePackage.Single().AvailableVersion != pkg.AvailableVersion) continue;
                    if (basePackage.Single().Pinned != pkg.Pinned) continue;

                    result.Remove(pkg);
                    continue;
                }
            }

            return result;
        }
    }
}

// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Community.PowerToys.Run.Plugin.GitKraken.Helpers
{
    public class RepositoryQuery
    {
        private readonly string _profilesPath = Environment.ExpandEnvironmentVariables(@"%APPDATA%\.gitkraken\profiles");

        public IEnumerable<Repository> GetAll()
        {
            if (!Directory.Exists(_profilesPath))
            {
                yield break;
            }

            var profilesDirectories = Directory.GetDirectories(_profilesPath);
            if (profilesDirectories.Length == 0)
            {
                yield break;
            }

            // TODO the directory may contain more than one profile
            var localRepoCachePath = Path.Combine(profilesDirectories.First(), "localRepoCache");
            if (!File.Exists(localRepoCachePath))
            {
                yield break;
            }

            using var fs = new FileStream(localRepoCachePath, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(fs);
            var json = sr.ReadToEnd();
            var parsed = JsonDocument.Parse(json);
            parsed.RootElement.TryGetProperty("localRepoCache", out var localRepoCache);
            if (localRepoCache.ValueKind != JsonValueKind.Array)
            {
                yield break;
            }

            using var repoEnumerator = localRepoCache.EnumerateArray();
            foreach (var repo in repoEnumerator)
            {
                if (repo.ValueKind != JsonValueKind.String)
                {
                    continue;
                }

                var repoDirectory = new DirectoryInfo(repo.ToString());
                if (!repoDirectory.Exists)
                {
                    continue;
                }

                var parentDirectory = repoDirectory.Parent;
                if (parentDirectory == null)
                {
                    continue;
                }

                yield return new(parentDirectory.Name, parentDirectory.FullName);
            }
        }
    }
}

// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Community.PowerToys.Run.Plugin.GitKraken
{
    public record Repository
    {
        public string Name { get; }

        public string Path { get; }

        public Repository(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}

// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Community.PowerToys.Run.Plugin.GitKraken.Helpers;
using ManagedCommon;
using Wox.Infrastructure;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.GitKraken
{
    public class Main : IPlugin
    {
        private readonly RepositoryQuery _repositoryQuery;
        private PluginInitContext? _context;
        private string? _icoPath;

        public string Name => "GitKraken";

        public string Description => "Open GitKraken repositories.";

        public Main()
        {
            _repositoryQuery = new RepositoryQuery();
        }

        public void Init(PluginInitContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(_context.API.GetCurrentTheme());
        }

        public List<Result> Query(Query query)
        {
            var results = new List<Result>();

            foreach (var (name, path) in _repositoryQuery.GetAll())
            {
                var score = StringMatcher.FuzzySearch(query.Search, name);
                if (string.IsNullOrWhiteSpace(query.Search) || score.Score > 0)
                {
                    results.Add(new Result
                    {
                        Title = name,
                        SubTitle = path,
                        Score = score.Score,
                        TitleHighlightData = score.MatchData,
                        IcoPath = _icoPath,
                        Action = _ =>
                        {
                            Helper.OpenInShell("gitkraken", $"-p \"{path}\"", runWithHiddenWindow: true);
                            return true;
                        },
                    });
                }
            }

            return results.OrderBy(r => r.Title).ToList();
        }

        private void UpdateIconPath(Theme theme)
        {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                _icoPath = "Images/GitKraken.light.png";
            }
            else
            {
                _icoPath = "Images/GitKraken.dark.png";
            }
        }

        private void OnThemeChanged(Theme currentTheme, Theme newTheme)
        {
            UpdateIconPath(newTheme);
        }
    }
}

// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Community.PowerToys.Run.Plugin.GitKraken.Helpers;
using Community.PowerToys.Run.Plugin.GitKraken.Properties;
using ManagedCommon;
using Wox.Infrastructure;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.GitKraken
{
    public class Main : IPlugin, IPluginI18n, IContextMenu
    {
        private static readonly string _pluginName = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;

        public static string PluginID => "582B10E1E9144702B5E18C3EDC3FF466";

        private readonly RepositoryQuery _repositoryQuery;
        private PluginInitContext? _context;
        private string? _icoPath;

        public string Name => Resources.PluginName;

        public string Description => Resources.PluginDescription;

        public string GetTranslatedPluginTitle() => Resources.PluginName;

        public string GetTranslatedPluginDescription() => Resources.PluginDescription;

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

            foreach (var repository in _repositoryQuery.GetAll())
            {
                var score = StringMatcher.FuzzySearch(query.Search, repository.Name);
                if (string.IsNullOrWhiteSpace(query.Search) || score.Score > 0)
                {
                    results.Add(new Result
                    {
                        Title = repository.Name,
                        SubTitle = repository.Path,
                        Score = score.Score,
                        TitleHighlightData = score.MatchData,
                        IcoPath = _icoPath,
                        ContextData = repository,
                        Action = _ =>
                        {
                            Helper.OpenInShell("gitkraken", $"-p \"{repository.Path}\"", runWithHiddenWindow: true);
                            return true;
                        },
                    });
                }
            }

            return results.OrderBy(r => r.Title).ToList();
        }

        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            if (selectedResult.ContextData is not Repository repository)
            {
                return new List<ContextMenuResult>();
            }

            return new List<ContextMenuResult>
            {
                new ContextMenuResult
                {
                    Title = Resources.Action_OpenContainingFolder,
                    Glyph = "\xE838",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    AcceleratorKey = Key.E,
                    AcceleratorModifiers = ModifierKeys.Control | ModifierKeys.Shift,
                    PluginName = _pluginName,
                    Action = _ =>
                    {
                        Helper.OpenInShell(repository.Path);
                        return true;
                    },
                },
            };
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

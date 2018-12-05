@Html.DevExpress().GetStyleSheets(
    new StyleSheet { ExtensionSuite = ExtensionSuite.NavigationAndLayout },
    new StyleSheet { ExtensionSuite = ExtensionSuite.Editors },
    new StyleSheet { ExtensionSuite = ExtensionSuite.Scheduler }
)
@Html.DevExpress().GetScripts(
    new Script { ExtensionSuite = ExtensionSuite.NavigationAndLayout },
    new Script { ExtensionSuite = ExtensionSuite.Editors },
    new Script { ExtensionSuite = ExtensionSuite.Scheduler }
)

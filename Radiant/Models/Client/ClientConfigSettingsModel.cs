using YamlDotNet.RepresentationModel;

namespace Radiant.Models.Client
{
    public class ClientConfigSettingsModel
    {
        public YamlStream CreateSettings(UserData userData)
        {
            var settings = new YamlStream(
                new YamlDocument(
                    new YamlMappingNode(
                        new YamlScalarNode("install"), new YamlMappingNode(
                            new YamlScalarNode("cohorts"),
                            new YamlMappingNode(new YamlScalarNode("RC_15.new_lifecycle"), new YamlScalarNode($"betaEnable")),
                            new YamlScalarNode("globals"), new YamlMappingNode(
                                new YamlScalarNode("region"), new YamlScalarNode($"{userData.RiotRegion}") { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                new YamlScalarNode("locale"), new YamlScalarNode($"{userData.RiotUserData.PlayerLocale}") { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted }),
                            new YamlScalarNode("multigame-client"), new YamlMappingNode(
                                new YamlScalarNode("shortcut_created"), new YamlScalarNode("true")

                            )))));

            return settings;
        }
    }
}

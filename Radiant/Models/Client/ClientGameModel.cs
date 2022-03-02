using System.Net;
using YamlDotNet.RepresentationModel;

namespace Radiant.Models.Client
{
    public class ClientGameModel
    {
        public CookieValue cookieValues = new CookieValue();

        public struct CookieValue
        {
            public string tdid;
            public string ssid;
            public string clid;
            public string sub;
            public string csid;
        }


        public ClientGameModel(UserData userData)
        {
            foreach (Cookie cookie in userData.ClientHandler.CookieContainer.GetCookies(ApiURIs.URIs["AuthUri"]))
            {
                switch (cookie.Name)
                {
                    case "tdid":
                        cookieValues.tdid = cookie.Value;
                        break;
                    case "ssid":
                        cookieValues.ssid = cookie.Value;
                        break;
                    case "clid":
                        cookieValues.clid = cookie.Value;
                        break;
                    case "sub":
                        cookieValues.sub = cookie.Value;
                        break;
                    case "csid":
                        cookieValues.csid = cookie.Value;
                        break;
                }
            }
        }

        public YamlStream CreateFile()
        {
            var settings = new YamlStream(
                new YamlDocument(
                    new YamlMappingNode(
                        new YamlScalarNode("riot-login"), new YamlMappingNode(
                            new YamlScalarNode("persist"), new YamlMappingNode(
                                new YamlScalarNode("session"), new YamlMappingNode(
                                    new YamlScalarNode("cookies"), new YamlSequenceNode(
                                        // TDID Cookie Section
                                        new YamlMappingNode(
                                            new YamlScalarNode("domain"),
                                            new YamlScalarNode("auth.riotgames.com")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("hostOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("httpOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("name"),
                                            new YamlScalarNode("tdid")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("path"),
                                            new YamlScalarNode("/")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("persistent"), new YamlScalarNode("true"),
                                            new YamlScalarNode("secureOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("value"),
                                            new YamlScalarNode($"{cookieValues.tdid}")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted }
                                        ),
                                        // SSID Cookie Section
                                        new YamlMappingNode(
                                            new YamlScalarNode("domain"),
                                            new YamlScalarNode("auth.riotgames.com")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("hostOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("httpOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("name"),
                                            new YamlScalarNode("ssid")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("path"),
                                            new YamlScalarNode("/")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("persistent"), new YamlScalarNode("true"),
                                            new YamlScalarNode("secureOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("value"),
                                            new YamlScalarNode($"{cookieValues.ssid}")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted }
                                        ),
                                        // clid Cookie Section
                                        new YamlMappingNode(
                                            new YamlScalarNode("domain"),
                                            new YamlScalarNode("auth.riotgames.com")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("hostOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("httpOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("name"),
                                            new YamlScalarNode("clid")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("path"),
                                            new YamlScalarNode("/")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("persistent"), new YamlScalarNode("true"),
                                            new YamlScalarNode("secureOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("value"),
                                            new YamlScalarNode("ue1")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted }
                                        ),
                                        // sub Cookie Section
                                        new YamlMappingNode(
                                            new YamlScalarNode("domain"),
                                            new YamlScalarNode("auth.riotgames.com")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("hostOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("httpOnly"), new YamlScalarNode("false"),
                                            new YamlScalarNode("name"),
                                            new YamlScalarNode("sub")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("path"),
                                            new YamlScalarNode("/")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("persistent"), new YamlScalarNode("true"),
                                            new YamlScalarNode("secureOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("value"),
                                            new YamlScalarNode($"{cookieValues.sub}")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted }
                                        ),
                                        // csid Cookie Section
                                        new YamlMappingNode(
                                            new YamlScalarNode("domain"),
                                            new YamlScalarNode("auth.riotgames.com")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("hostOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("httpOnly"), new YamlScalarNode("false"),
                                            new YamlScalarNode("name"),
                                            new YamlScalarNode("csid")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("path"),
                                            new YamlScalarNode("/")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted },
                                            new YamlScalarNode("persistent"), new YamlScalarNode("true"),
                                            new YamlScalarNode("secureOnly"), new YamlScalarNode("true"),
                                            new YamlScalarNode("value"),
                                            new YamlScalarNode($"{cookieValues.csid}")
                                                { Style = YamlDotNet.Core.ScalarStyle.DoubleQuoted }
                                        )
                                    )))))));

            return settings;
        }
    }
}